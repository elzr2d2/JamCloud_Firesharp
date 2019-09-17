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
        DownloadProgressForm downloadProgress = new DownloadProgressForm();

        string bucketName = "jamcloud-db-2aea9.appspot.com";
        string fileName = null;

        // A Json file with auths and information for accessing the cloud storage
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
            // Getting file information from the bucket of the cloud storage
            ListObjects(bucketName);
        }

        private void ListObjects(string bucketName)
        {
            // Firebase is a cloud platform which developed by Google, for getting access to the storage we need to create a path with the GoogleCredential
            string credPath = myJsonFile;
            var credential = GoogleCredential.FromStream(File.OpenRead(credPath));
            var storageClient = StorageClient.Create(credential);

            // creating two lists, one of the local storage of the current user and the second of the public storage of all the users
            var listAudioFiles = storageClient.ListObjects(bucketName);
            
            foreach (var audioFileLink in listAudioFiles)
            {
                if(audioFileLink.Name.EndsWith(".wav"))
                {

                    if (user.GetCurrentUser().Username == ShorthandOwnerName(audioFileLink.Name))
                    {
                        UserRepo.Items.Add(audioFileLink.Name);
                    }
                    else
                    {

                        PublicRepo.Items.Add(audioFileLink.Name);
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


        private static string ShorthandOwnerName(string ownerName)
        {
            string[] linkNameSplit = ownerName.Split('/');
            return linkNameSplit[linkNameSplit.Length - 2];
        }

        private void DownloadSelectedButton_Click(object sender, EventArgs e)
        {
            
            if (UserRepo.SelectedItem != null)
            {
                DownloadObject(bucketName, UserRepo.SelectedItem.ToString());
            }         
            else if (PublicRepo.SelectedItem != null)
            {
                DownloadObject(bucketName, PublicRepo.SelectedItem.ToString());
            }
            else
                MessageBox.Show("Please choose a file to download");
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
                downloadProgress.Show();
            }

            localPath = localPath ?? Path.GetFileName(objectName);
            using (var outputFile = File.OpenWrite(localPath))
            {
                storageClient.DownloadObject(bucketName, objectName, outputFile);
                
            }
            Console.WriteLine($"downloaded {objectName} to {localPath}.");
            downloadProgress.Close();
            var menu = new MenuForm();
            menu.Show();
            Close();
        }

        private void ClipboardButton_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(fileName);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            var menu = new MenuForm();
            menu.Show();
            Close();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {

            Close();
            Application.Exit();

        }

        private void PublicRepo_SelectedValueChanged(object sender, EventArgs e)
        {
            UserRepo.ClearSelected();
          
        }

        private void UserRepo_SelectedValueChanged(object sender, EventArgs e)
        {
            PublicRepo.ClearSelected();

        }
    }
}
