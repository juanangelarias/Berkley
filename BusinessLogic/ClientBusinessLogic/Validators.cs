using System.Net.Mail;

namespace ClientBusinessLogic
{
    public static class Validators
    {
        /// <summary>
        /// Validate whether an email is a valid format.
        /// </summary>
        /// <param name="email">The email to validate.</param>
        /// <returns>True if email is valid, false if it is invalid.</returns>
        public static bool ValidateEmail(string email)
        {
            if ("" == email)
            {
                return false;
            }
            else
            {
                try
                {
                    MailAddress m = new MailAddress(email);
                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
        }
    }
}