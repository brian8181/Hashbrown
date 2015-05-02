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
        public Main(string[] args)
        {
            InitializeComponent();
            cmbHash.Items.Add("md5");
            cmbHash.Items.Add("sha1");
            //cmbHash.Items.Add("sha256");
            //cmbHash.Items.Add("sha512");
            cmbHash.SelectedIndex = 1;

            if(args.Length > 0 && args[0] != null)
                LoadPath(args[0]);
        }

        private void LoadPath(string path)
        {
            byte[] hash = null;
            txtFile.Text = path;
            switch (cmbHash.SelectedIndex)
            {
                case 0:
                    {
                        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                        hash = md5.ComputeHash(File.ReadAllBytes(path));
                        break;
                    }
                case 1:
                    {
                        SHA1 sha1 = new SHA1CryptoServiceProvider();
                        hash = sha1.ComputeHash(File.ReadAllBytes(path));
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

            RefreshVerify();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if(dlg.ShowDialog() == DialogResult.OK)
            {
                LoadPath(dlg.FileName);
           }
       }

        private void RefreshVerify()
        {
            if (txtVerify.Text != string.Empty && txtHash.Text != string.Empty)
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
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            RefreshVerify();
        }

        private void cmbHash_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(txtFile.Text != string.Empty)
            {
                FileInfo fi = new FileInfo(txtFile.Text);
                if (fi.Exists)
                {
                    LoadPath(fi.FullName);
                }
            }
            RefreshVerify();
        }
    }
}
