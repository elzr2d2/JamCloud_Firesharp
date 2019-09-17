using System;
using System.Windows.Forms;

namespace JamCloud_Firesharp
{
    public partial class MenuForm : Form
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
        public MenuForm()
        {
            InitializeComponent();
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            var upload = new UploadToCloudForm();
            upload.Show();
            
        }

        private void SwitchAccountsButton_Click(object sender, EventArgs e)
        {
            var login = new LoginForm();
            login.Show();  
            Close();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }

        private void MenuForm_Load(object sender, EventArgs e)
        {

        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            var download = new DownloadFromCloudForm();
            download.Show(); 
            Close();
        }

    }
}
