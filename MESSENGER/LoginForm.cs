using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MESSENGER.Properties;

namespace MESSENGER
{
    public partial class LoginForm : Form
    {
        public TcpClient _client;
        public NetworkStream _stream;
        public ClientUserAccount _account;

        private string ip;

        private bool OK;

        public LoginForm(string ip)
        {
            InitializeComponent();

            this.ip = ip;

            Settings.Default.Reload();
            chbRemember.Checked = Settings.Default.rememberLogin;
            tbUN.Text = Settings.Default.loginName;

            if (chbRemember.Checked)
                tbPW.Text = Settings.Default.loginPassword;

            _client = new TcpClient();
        }

        void init()
        {
            new Thread(() =>
            {
                while (Visible)
                {
                    if (_stream != null && _stream.CanRead && _stream.DataAvailable)
                        receivedData();

                    Thread.Sleep(1);
                }
            })
            { IsBackground = true }.Start();
        }

        private void receivedData()
        {
            BinaryFormatter bf = new BinaryFormatter();
            object received = bf.Deserialize(_stream);

            if (received is ClientUserAccount acc)
            {
                _account = acc;
                OK = true;
                BeginInvoke(new MethodInvoker(Close));
            }
            else if (received is LoginFailedPacket lfp)
            {
                MessageBox.Show("Failed to log in:\n" + lfp.reason);
            }
            else if (received is RegisterFailedPacket)
            {
                MessageBox.Show("This username is not available.");
            }
        }

        private void tb_TextChanged(object sender, EventArgs e)
        {
            if (sender == tbUN)
            {
                Settings.Default.loginName = tbUN.Text;
            }
            else if (sender == tbPW && chbRemember.Checked && tbPW.Text != "")
            {
                Settings.Default.loginPassword = tbPW.Text;
            }

            Color backColor = DefaultBackColor;

            if (!String.IsNullOrWhiteSpace(tbPWA.Text) && tbPW.Text != tbPWA.Text)
                backColor = Color.Red;

            tbPW.BackColor = backColor;
            tbPWA.BackColor = backColor;

            Settings.Default.Save();

            bool detailsOK =
                !String.IsNullOrWhiteSpace(tbUN.Text) && tbUN.Text.Length >= 3 &&
                !String.IsNullOrWhiteSpace(tbPW.Text) && tbPW.Text.Length >= 3;

            btnRegister.Enabled = !String.IsNullOrWhiteSpace(tbPWA.Text) && detailsOK && tbPW.Text == tbPWA.Text;
            btnLogin.Enabled = detailsOK && String.IsNullOrWhiteSpace(tbPWA.Text);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            if (!tryConnect())
                return;

            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(_stream, new RegisterRequestPacket(tbUN.Text, tbPW.Text, tbUN.Text));
            }
            catch
            {
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OK)
                return;

            try
            {
                if (_account == null)
                {
                    if (_stream != null)
                    {
                        var bf = new BinaryFormatter();
                        bf.Serialize(_stream, new LogoutAckPacket());

                        Visible = false;

                        Thread.Sleep(1000);
                    }
                    _client.Close();
                }
            }
            catch { }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!tryConnect())
                return;

            try
            {
                var bf = new BinaryFormatter();

                bf.Serialize(_stream, new LoginRequestPacket(tbUN.Text, tbPW.Text));
            }
            catch { }
        }

        bool tryConnect()
        {
            if (!_client.Connected)
            {
                try
                {
                    var result = _client.BeginConnect(ip.Split(':')[0], int.Parse(ip.Split(':')[1]), null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(2));

                    if (success)
                    {
                        _stream = _client.GetStream();

                        init();
                    }
                    else
                        throw new Exception();
                }
                catch
                {
                    MessageBox.Show("Unable to connect to the server.", "Error", MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                    return false;
                }
            }

            return true;
        }

        private void chbRemember_CheckedChanged(object sender, EventArgs e)
        {
            if (chbRemember.Checked && tbPW.Text != "")
                Settings.Default.loginPassword = tbPW.Text;

            Settings.Default.rememberLogin = chbRemember.Checked;
            Settings.Default.Save();
        }

        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnLogin.Enabled)
                    btnLogin_Click(null, null);
                else if (btnRegister.Enabled)
                    btnRegister_Click(null, null);
            }
        }
    }
}
