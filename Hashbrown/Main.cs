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
            cmbHash.Items.Add("sha256");
            cmbHash.Items.Add("sha512");
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if(dlg.ShowDialog() == DialogResult.OK)
            {
                txtFile.Text = dlg.FileName;
                byte[] hash = Utility.IO.FileExt.ComputeSHA1(dlg.FileName);
                txtHash.Text = Utility.CryptoFunctions.FromBytesToHex(hash);
              
           }

         }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            if (txtHash.Text == txtVerify.Text)
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
