using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MESSENGER
{
    public partial class ServerForm : Form
    {
        ServerHandler _server;

        private int port;

        private int lastSelected = -1;

        public ServerForm(int port)
        {
            InitializeComponent();

            this.port = port;
        }

        private void onUsersChanged(object sender, RefreshUsersEventArgs e)
        {
            BeginInvoke(new MethodInvoker(() =>
            {
                try
                {
                    lbOnline.Items.Clear();

                    for (int i = 0; i < e.nodes.Count; i++)
                    {
                        lbOnline.Items.Add(e.nodes[i]);
                    }

                    if (lastSelected < lbOnline.Items.Count)
                        lbOnline.SelectedIndex = lastSelected;

                    btnKickAll.Enabled = lbOnline.Items.Count > 0;
                    btnKickAllCM.Enabled = btnKickAll.Enabled && !String.IsNullOrEmpty(tbKickMessage.Text) &&
                                           !String.IsNullOrWhiteSpace(tbKickMessage.Text) &&
                                           tbKickMessage.Text.Length > 0;
                }
                catch
                {
                }
            }));
        }

        private void lbOnline_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbOnline.SelectedIndex == -1)
            {
                if (lastSelected != -1)
                    lbOnline.SelectedIndex = lastSelected;
                else
                    lbOnline.SelectedIndex = lastSelected = 0;
            }
            else
                lastSelected = lbOnline.SelectedIndex;

            btnRemoveAcc.Enabled = btnKick.Enabled = lbOnline.SelectedItem != null;

            if (lbOnline.SelectedItem is ClientUserAccount acc)
            {
                btnUnbanAcc.Enabled = !(btnBanAcc.Enabled = !acc.banned);
            }
            else
            {
                btnUnbanAcc.Enabled = btnBanAcc.Enabled = false;
            }
        }

        private void btnKick_Click(object sender, EventArgs e)
        {
            try
            {
                _server.Kick((lbOnline.SelectedItem as ClientUserAccount).UUID);
            }
            catch
            {
            }
        }

        private void btnKickCM_Click(object sender, EventArgs e)
        {
            try
            {
                _server.Kick((lbOnline.SelectedItem as ClientUserAccount).UUID, tbKickMessage.Text);

                tbKickMessage.Text = "";
            }
            catch
            {
            }
        }

        private void tbKickMessage_TextChanged(object sender, EventArgs e)
        {
            btnKickCM.Enabled = !String.IsNullOrEmpty(tbKickMessage.Text) &&
                                !String.IsNullOrWhiteSpace(tbKickMessage.Text) && tbKickMessage.Text.Length > 0 &&
                                btnKick.Enabled;

            btnKickAll.Enabled = lbOnline.Items.Count > 0;
            btnKickAllCM.Enabled = btnKickAll.Enabled && !String.IsNullOrEmpty(tbKickMessage.Text) &&
                                   !String.IsNullOrWhiteSpace(tbKickMessage.Text) &&
                                   tbKickMessage.Text.Length > 0;
        }

        private void lbOnline_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (lbOnline.Items.Count > 0 && e.Index != -1)
            {
                ClientUserAccount account = lbOnline.Items[e.Index] as ClientUserAccount;

                DrawingHelper.enableSmooth(e.Graphics, false);

                e.DrawBackground();

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e = new DrawItemEventArgs(e.Graphics,
                        e.Font,
                        e.Bounds,
                        e.Index,
                        e.State ^ DrawItemState.Selected,
                        e.ForeColor,
                        Color.FromArgb(0, 180, 255));

                    e.DrawBackground();
                    e.DrawFocusRectangle();
                }

                Brush brush = account.banned ? new SolidBrush(Color.FromArgb(255, 50, 100)) : new SolidBrush(account.Online ? Color.FromArgb(150, 255, 50) : Color.FromArgb(50, 50, 50));

                e.Graphics.FillRectangle(brush, new Rectangle(e.Bounds.Width - 5, e.Bounds.Y, 5, 50));
                DrawingHelper.enableSmooth(e.Graphics, true);

                Image img = ImageUtils.ResizeAndCrop(account.GetProfileImage(), 50, 50);

                e.Graphics.DrawImage(img, e.Bounds.X, e.Bounds.Y);

                if (account.isTyping)
                {
                    Font f = new Font(lbOnline.Font.FontFamily, 21, FontStyle.Bold);

                    string text = "...";

                    SizeF s = e.Graphics.MeasureString(text, f, new Size(50, 50));

                    e.Graphics.FillRectangle(new SolidBrush(Color.Black), 10, e.Bounds.Y + 50 - 10, 50 - 20, 10);

                    e.Graphics.DrawString(text, f, Brushes.Turquoise, 25 - s.Width / 2f + 1, e.Bounds.Y + 25 - s.Height / 2f + 14);
                }

                int i = (int)e.Graphics.MeasureString(account.ToString(), lbOnline.Font, lbOnline.Width - 60).Height;
                e.Graphics.DrawString(account.ToString(), e.Font, new SolidBrush(Color.Black),
                    new Rectangle(new Point(55, e.Bounds.Y + 25 - i / 2), new Size(e.Bounds.Width - 60, 50)));
            }
        }

        private void btnWriteLog_Click(object sender, EventArgs e)
        {
            _server.writeLog();
        }

        private void btnKickAllCM_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lbOnline.Items.Count; i++)
            {
                try
                {
                    _server.Kick((lbOnline.Items[i] as ClientUserAccount).UUID, tbKickMessage.Text);
                }
                catch
                {
                }
            }

            tbKickMessage.Text = "";
        }

        private void btnKickAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < lbOnline.Items.Count; i++)
            {
                try
                {
                    _server.Kick((lbOnline.Items[i] as ClientUserAccount).UUID);
                }
                catch
                {
                }
            }
        }

        private void btnRemoveAcc_Click(object sender, EventArgs e)
        {
            try
            {
                var result =
                    MessageBox.Show(
                        $"Are you sure you want to delete {(lbOnline.SelectedItem as ClientUserAccount).Nickname}'s account?",
                        "Delete account", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                    _server.unregisterAccount((lbOnline.SelectedItem as ClientUserAccount).UUID);
            }
            catch
            {
            }
        }

        private void MainServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _server.Dispose();
            }
            catch
            {
            }

            Visible = false;
            Thread.Sleep(1000);
        }

        private void btnBanAcc_Click(object sender, EventArgs e)
        {
            try
            {
                var result =
                    MessageBox.Show(
                        $"Are you sure you want to ban {(lbOnline.SelectedItem as ClientUserAccount).Nickname}?",
                        "Ban user", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                    _server.banAccount((lbOnline.SelectedItem as ClientUserAccount).UUID);
            }
            catch
            {
            }
        }

        private void btnUnbanAcc_Click(object sender, EventArgs e)
        {
            try
            {
                _server.unbanAccount((lbOnline.SelectedItem as ClientUserAccount).UUID);
            }
            catch
            {
            }
        }

        private void ServerForm_Shown(object sender, EventArgs e)
        {
            _server = new ServerHandler(port);
            _server.OnUsersChanged += onUsersChanged;
            _server.init();
        }
    }
}
