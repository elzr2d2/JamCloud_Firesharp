using System.Diagnostics;

namespace JamCloud_Firesharp
{
    class User
    {

        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }

        protected static User currentUser;

        public static string errorMessage { get; set; }

        public static bool IsEqual(User signedUser, User loggingUser)
        {
            if (signedUser == null || loggingUser == null)
            {
                errorMessage = "User does not exist, please considering to sign up...";
                return false;
            }
            if (signedUser.Username != loggingUser.Username)
            {

                errorMessage = "Username does not exist!";
                return false;
            }
            else if (signedUser.Password != loggingUser.Password)
            {

                errorMessage = "Username and password does not match!";
                return false;
            }

            currentUser = loggingUser;
            Debug.WriteLine("\n\ncurrent user is: "+currentUser.Username+"\n\n");
            return true;
        }

        public User GetCurrentUser() => currentUser;      
    }
}
