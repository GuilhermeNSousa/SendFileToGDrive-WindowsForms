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

        private void sendFileBut_Click(object sender, EventArgs e)
        {
            UploadDrive uploadDrive = new UploadDrive();

            string sIdPastaDestino = String.Empty;

            if (linkBox.Text.StartsWith("https://"))
            {
                string[] link = linkBox.Text.Split('/');
                sIdPastaDestino = link[5]; 
            }
            else if (linkBox.Text.StartsWith("drive"))
            {
                string[] link = linkBox.Text.Split('/');
                sIdPastaDestino = link[3];
            }
            else
            {
                MessageBox.Show("Link inválido! Tente novamente");
                linkBox.Text = "";
            }

            string caminhoDasCredenciais = "client_secret_790542996681-o1bs8eh4s1h2a8m8l4gu5j4adt5ihf2d.apps.googleusercontent.com.json";
            string caminhoDoArquivoLocal = fileBox.Text;

            UserCredential credential = uploadDrive.ObtemAutorização(caminhoDasCredenciais);
            DriveService service = uploadDrive.CriaDivreService(credential);

            string nomeDoArquivoNoDrive = "nomeArquivoNoDrive";

            var fileMetadata = uploadDrive.CriaArquivoNoDrive(nomeDoArquivoNoDrive, sIdPastaDestino);
            uploadDrive.UploadArquivoParaDrive(service, fileMetadata, caminhoDoArquivoLocal);

        }
    }
}
