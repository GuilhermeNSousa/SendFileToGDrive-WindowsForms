using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2.Flows;
using System.Net;


namespace SendFileToDriveWinForm
{
    public partial class Form1 : Form
    {
        //UploadDrive uploadDrive = new UploadDrive();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void chooseFileBut_Click(object sender, EventArgs e)
        {
            using(var ofd =  new OpenFileDialog())
            {
                ofd.InitialDirectory = @"Desktop\";
                ofd.Filter = "All Files (*.*) | *.*";
                ofd.RestoreDirectory = true;

                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    fileBox.Text = ofd.FileName;
                }
            }
        }
    }
}
