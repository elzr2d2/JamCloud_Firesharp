using JamCloud_Firesharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JamCloud_Firesharp
{
    public partial class CurrentUserForm : Form
    {
        public string currentUserName { get=>currentUserName; set=>currentUserName = user.GetCurrentUser().Username; }
        User user = new User();

        public CurrentUserForm()
        {
            InitializeComponent();
        }

        private void CurrentUser_Load(object sender, EventArgs e)
        {
            
            CurrentUserTxt.Text = currentUserName;
            MessageBox.Show("The current user is: "+ currentUserName);
            this.Hide();
        }
        
    }
}
