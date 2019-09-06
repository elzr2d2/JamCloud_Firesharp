using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Storage.HttpClient.Tasks;
using Firebase.Storage.Client;



namespace JamCloud_Firesharp
{
    public partial class UploadToCloudForm : Form
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

        OpenFileDialog ofd = new OpenFileDialog();
        string fileUrl = null;
        string fileName = null;
        public UploadToCloudForm()
        {
            InitializeComponent();
        }

        private void UploadToCloudForm_Load(object sender, EventArgs e)
        {
            ofd.Filter = "WAV File|*.wav";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileUrl = ofd.FileName;
            }

            UploadTask(fileUrl);
        }

        private void UploadTask(string fileUrl)
        {
            User user = new User();

            // Get any Stream - it can be FileStream, MemoryStream or any other type of Stream

            var stream = File.Open(fileUrl, FileMode.Open);
            fileName = ShortenPathName(stream);

            filenameLabel.Text = fileName + " is being uploaded.\n It might take a while...";
            // Construct FirebaseStorage, path to where you want to upload the file and Put it there

            var task = new FirebaseStorage("jamcloud-db-2aea9.appspot.com")
                .Child("jamcloud_audio")
                .Child(user.GetCurrentUser().Username)
                .Child(fileName)
                .PutAsync(stream);

            task.Progress.ProgressChanged += (s, per) =>
            {
                Console.WriteLine($"Progress: {per.Percentage} %");
                

                if (per.Percentage >= 0 && per.Percentage < 100)
                {
                    progressBar.Value = per.Percentage;
                    progressLabel.Text = per.Percentage + "%";

                }
                else
                { 
                    this.Close();
                }
            };

            var downloadUrl = GetTaskAsync(task);
        }

        private static string ShortenPathName(FileStream stream)
        {
            string[] streamSplit = stream.Name.Split('\\');
            return streamSplit[streamSplit.Length - 1];
        }

        private async Task<string> GetTaskAsync(FirebaseStorageTask task)
        {
            var downloadUrl = await task;
            return downloadUrl;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
