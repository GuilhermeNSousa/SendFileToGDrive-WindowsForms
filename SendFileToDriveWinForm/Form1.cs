using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2.Flows;
using System.Net;


namespace SendFileToDriveWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void chooseFileBut_Click(object sender, EventArgs e)
        {
            //Abre uma janela para a seleção do(s) arquivo(s)
            using(var ofd =  new OpenFileDialog())
            {
                ofd.InitialDirectory = @"C:\temp\";
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

            bool validaLink = !linkBox.Text.StartsWith("https://drive.google.com/drive/folders/") && !linkBox.Text.StartsWith("https://drive.google.com/drive/u/1/folders/") && !linkBox.Text.StartsWith("drive.google.com/drive/u/1/folders/") && !linkBox.Text.StartsWith("drive.google.com/drive/folders/") && linkBox.Text != "" ? false : true;

            //Verifica o formato do link da pasta
            if (linkBox.Text.StartsWith("https://drive.google.com/drive"))
            {
                string[] link = linkBox.Text.Split('/');
                sIdPastaDestino = link[5];
                
                if (linkBox.Text.Contains("/u/1"))
                {
                    sIdPastaDestino = link[7];
                }
            }

            //Verifica o formato do link da pasta
            else if (linkBox.Text.StartsWith("drive.google.com/drive"))
            {
                string[] link = linkBox.Text.Split('/');
                sIdPastaDestino = link[3];

                if (linkBox.Text.Contains("/u/1"))
                {
                    sIdPastaDestino = link[5];
                }
            }

            //Verifica o formato do link da pasta
            else if (!validaLink)
            {
                MessageBox.Show("Insira um link válido!", "Link inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Verifica se há arquivos inseridos
            if (fileBox.Text != "")
            {
                //Se o link da pasta e o arquivo forem válidos, envie para o Drive
                if (linkBox.Text != "" && System.IO.File.Exists(fileBox.Text) == true && validaLink)
                {
                    string caminhoDasCredenciais = "NomeDoArquivoDeCredenciais";
                    string caminhoDoArquivoLocal = fileBox.Text;

                    UserCredential credential = uploadDrive.ObtemAutorização(caminhoDasCredenciais);
                    DriveService service = uploadDrive.CriaDivreService(credential);

                    string nomeDoArquivoNoDrive = "Arquivo_Enviado";

                    var fileMetadata = uploadDrive.CriaArquivoNoDrive(nomeDoArquivoNoDrive, sIdPastaDestino);

                    uploadDrive.UploadArquivoParaDrive(service, fileMetadata, caminhoDoArquivoLocal);

                    MessageBox.Show("Arquivo enviado!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                //Se não houver link, exiba uma mensagem
                else if (linkBox.Text == "")
                {
                    MessageBox.Show("Insira o link da pasta no Drive!", "Link da pasta não inserido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //Se o arquivo não existir, exiba uma mensagem
                else if (System.IO.File.Exists(fileBox.Text) == false)
                {
                    MessageBox.Show("Caminho para o arquivo inválido!", "Insira um caminho existente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //Verifica se não há arquivos inseridos
            else if (fileBox.Text == "") 
            {
                if (linkBox.Text == "")
                {
                    MessageBox.Show("Insira o link e o(s) arquivo(s)!", "Faltam o link e o(s) arquivo(s)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Selecione o(s) arquivo(s)!", "Arquivo(s) não selecionado(s)", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }            
        }
    }
}
