using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hashbrown
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            cmbHash.Items.Add("md5");
            cmbHash.Items.Add("sha1");
            //cmbHash.Items.Add("sha256");
            //cmbHash.Items.Add("sha512");
            cmbHash.SelectedIndex = 1;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            byte[] hash = null;
            OpenFileDialog dlg = new OpenFileDialog();
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = dlg.FileName;
                switch (cmbHash.SelectedIndex)
                {
                    case 0:
                        {
                            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                            hash = md5.ComputeHash(File.ReadAllBytes(dlg.FileName));
                            break;
                        }
                    case 1:
                        {
                            SHA1 sha1 = new SHA1CryptoServiceProvider();
                            hash = sha1.ComputeHash(File.ReadAllBytes(dlg.FileName));
                            break;
                        }
                    default:
                        break;
                }

                if (hash != null)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hash)
                    {
                        sb.Append(b.ToString("X2"));
                    }
                    txtHash.Text = sb.ToString();
                }
                else
                {
                    txtHash.Text = string.Empty;
                }
           }
         }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (txtHash.Text.TrimEnd().ToLower() == txtVerify.Text.TrimEnd().ToLower())
            {
                txtOutput.ForeColor = Color.Green;
                txtOutput.Text = "Verified";
            }
            else
            {
                txtOutput.ForeColor = Color.Red;
                txtOutput.Text = "Unverified";
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }
    }
}
