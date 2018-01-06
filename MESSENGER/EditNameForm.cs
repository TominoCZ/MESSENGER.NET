using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MESSENGER
{
    public partial class EditNameForm : Form
    {
        public string newName;
        public string oldName;

        public EditNameForm(string currentName)
        {
            oldName = newName = currentName;

            InitializeComponent();

            tbNewName.Text = oldName;
        }

        private void tbNewName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && !String.IsNullOrWhiteSpace(tbNewName.Text) && tbNewName.Text.Length >= 3 && tbNewName.Text != oldName)
            {
                newName = tbNewName.Text;
                Close();
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            newName = tbNewName.Text;
            Close();
        }

        private void tbNewName_TextChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = !String.IsNullOrWhiteSpace(tbNewName.Text) && tbNewName.Text.Length >= 3 && tbNewName.Text != oldName;
        }
    }
}
