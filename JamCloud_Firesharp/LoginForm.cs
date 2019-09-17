using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;

namespace JamCloud_Firesharp
{
    public partial class LoginForm : Form
    {
        #region Moveable

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void Drag_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
        }

        #endregion
        private IFirebaseClient client;

        // Implementing Auth and path for user validation
        private readonly IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "i4uPx0ZBQRvllWkkDafNkIbILnVaxdRGGVkRMm27",
            BasePath = "https://jamcloud-db-2aea9.firebaseio.com/"
        };

        public LoginForm()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Connection Error");
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            #region Condition

            if (string.IsNullOrWhiteSpace(usernameTB.Text) &&
                string.IsNullOrWhiteSpace(passwordTB.Text))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }

            #endregion

            var res = client.Get(@"Users/" + usernameTB.Text);
            var ResUser = res.ResultAs<User>();
            var CurrentUser = new User
            {
                Username = usernameTB.Text,
                Password = passwordTB.Text
            };

            // User validation check
            if (User.IsEqual(ResUser, CurrentUser))
            {
                var menuForm = new MenuForm();
                menuForm.Show();

                Hide();
            }
            else
            {
                MessageBox.Show(User.errorMessage);
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            var reg = new Register();
            reg.Show();

            Hide();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
            Application.Exit();
        }
    }
}