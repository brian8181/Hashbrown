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
            cmbHash.Items.Add("sha256");
            cmbHash.SelectedIndex = 1;

            if(args.Length > 0 && args[0] != null)
                LoadPath(args[0]);

            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(Main_DragEnter);
            this.DragDrop += new DragEventHandler(Main_DragDrop);
        }
        
        private void LoadPath(string path)
        {
            byte[] hash = null;
            txtFile.Text = path;
            byte[] data = File.ReadAllBytes(path);
            switch (cmbHash.SelectedIndex)
            {
                case 0:
                    {
                        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
                        hash = md5.ComputeHash(data);
                        break;
                    }
                case 1:
                    {
                        SHA1 sha1 = new SHA1CryptoServiceProvider();
                        hash = sha1.ComputeHash(data);
                        break;
                    }
                case 2:
                    {
                        SHA256 sha256 = new SHA256CryptoServiceProvider();
                        hash = sha256.ComputeHash(data);
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

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            LoadPath(files[0]);
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
