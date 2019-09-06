using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Storage.Client;
using Google.Cloud.Storage.V1;
using Google.Apis.Auth.OAuth2;
using System.Collections.Generic;

namespace JamCloud_Firesharp
{
    public partial class DownloadFromCloudForm : Form
    {

        #region Moveable
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Drag_MouseDown(object sender, MouseEventArgs e)
        {

            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }
        #endregion

        User user = new User();
        string bucketName = "jamcloud-db-2aea9.appspot.com";
        static string fileName = null;
        string myJsonFile = "jamcloud-db-2aea9-2284733e8888.json";
        SaveFileDialog sfd = new SaveFileDialog();
        string localPath = null;
        List<string> urlsList = new List<string>();

        public DownloadFromCloudForm()
        {
            InitializeComponent();
        }

        private void DownloadFromCloudForm_Load(object sender, EventArgs e)
        {        
            ListObjects(bucketName);
        }

        private void ListObjects(string bucketName)
        {
            string credPath = myJsonFile;
            var credential = GoogleCredential.FromStream(File.OpenRead(credPath));
            var storageClient = StorageClient.Create(credential);

            var listAudioFiles = storageClient.ListObjects(bucketName);
            
            foreach (var audioFileLink in listAudioFiles)
            {
                if(audioFileLink.Name.EndsWith(".wav"))
                {
                    fileName = audioFileLink.Name;

                    if (user.GetCurrentUser().Username == ShortenOwnerName(audioFileLink.Name))
                    {
                        fileName = audioFileLink.Name;
                        UserRepo.Items.Add(ShortenLinkName(ShortenLinkName(audioFileLink.Name)));
                    }
                    else
                    {

                        PublicRepo.Items.Add(ShortenOwnerName(audioFileLink.Name) + " - " + ShortenLinkName(audioFileLink.Name));
                    }

                }
            }
        }

        private async Task<string> DownloadTask(string fileName)
        {
            var task = new FirebaseStorage("jamcloud-db-2aea9.appspot.com")
                .Child("jamcloud_audio")
                .Child(user.GetCurrentUser().Username)
                .Child(fileName);
            
            return await task.GetDownloadUrlAsync();
        }

        private async Task<string> UserTask(string fileName)
        {
            var task = new FirebaseStorage("jamcloud-db-2aea9.appspot.com")
                .Child("jamcloud_audio");

            return await task.GetDownloadUrlAsync();
        }

        private static string ShortenLinkName(string linkName)
        {
            string[] linkNameSplit = linkName.Split('/');
            return linkNameSplit[linkNameSplit.Length - 1];
        }

        private static string ShortenOwnerName(string ownerName)
        {
            string[] linkNameSplit = ownerName.Split('/');
            return linkNameSplit[linkNameSplit.Length - 2];
        }

        private void DownloadSelectedButton_Click(object sender, EventArgs e)
        {
            DownloadObject(bucketName, fileName);
        }

        private void DownloadObject(string bucketName, string objectName)
        {
            string credPath = myJsonFile;
            var credential = GoogleCredential.FromStream(File.OpenRead(credPath));
            var storageClient = StorageClient.Create(credential);

            sfd.Filter = "WAV File| *.wav";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                
                localPath = sfd.FileName;
            }

            localPath = localPath ?? Path.GetFileName(objectName);
            using (var outputFile = File.OpenWrite(localPath))
            {
                storageClient.DownloadObject(bucketName, objectName, outputFile);
            }
            Console.WriteLine($"downloaded {objectName} to {localPath}.");
        }

        private void ClipboardButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(fileName);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            MenuForm menu = new MenuForm();
            menu.Show();
            this.Close();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {

            this.Close();
            Application.Exit();

        }

    }
}
