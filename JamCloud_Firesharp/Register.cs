using System;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;
using JamCloud_Firesharp;

namespace JamCloud_Firesharp
{

    public partial class Register : Form
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
        public Register()
        {
            InitializeComponent();
        }

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "i4uPx0ZBQRvllWkkDafNkIbILnVaxdRGGVkRMm27",
            BasePath = "https://jamcloud-db-2aea9.firebaseio.com/"
        };

        IFirebaseClient client;

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Register_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
            }
            catch
            {
                MessageBox.Show("Connection Error");
            }
        }


        private void SignupButton_Click(object sender, EventArgs e)
        {
            #region Condition
            if (string.IsNullOrWhiteSpace(fullnameTB.Text) &&
               string.IsNullOrWhiteSpace(usernameTB.Text) &&
               string.IsNullOrWhiteSpace(emailTB.Text) &&
               string.IsNullOrWhiteSpace(passwordTB.Text) &&
               string.IsNullOrWhiteSpace(confirmpasswordTB.Text))
            {
                MessageBox.Show("Please fill all the fields");
                return;
            }
            else if (passwordTB.Text != confirmpasswordTB.Text)
            {
                MessageBox.Show("Password does not match!");
                return;
            }

            #endregion

            User user = new User()
            {
                Fullname = fullnameTB.Text,
                Username = usernameTB.Text,
                Email = emailTB.Text,
                Password = passwordTB.Text,
                ConfirmPassword = confirmpasswordTB.Text

            };

            
            SetResponse response = client.Set(@"Users/" + usernameTB.Text, user);
            MessageBox.Show("You successfully joined JamCloud!");
            LoginForm login = new LoginForm();

            login.Show();

            this.Close();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            LoginForm login = new LoginForm();

            login.Show();

            this.SendToBack();
            this.Close();
        }
    }
}
