using James.Shared.Model;
using System.Net.Mail;
using SharedBusinessLogic;

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
        
        public static bool ValidateAddress(Address address)
        {
             
            if (address == null)
            {
                return false;
            }
            else if (string.IsNullOrWhiteSpace(address.Address1) ||
                string.IsNullOrWhiteSpace(address.City) ||
                string.IsNullOrWhiteSpace(address.StateCode) ||
                string.IsNullOrWhiteSpace(address.PostalCode))
            {
                return false;
            }
            else
            {
                //TODO: Check for valid StateCode.
                return true;
            }
        }

        /// <summary>
        /// Validates if a filename is a valid Windows filename with an extension
        /// </summary>
        /// <param name="filename">The filename</param>
        /// <returns>True if valid</returns>
        /// <remarks>Ignores any path before the file name.</remarks>
        public static bool ValidateFileName(string filename)
        {
            return ImagingBusinessLogic.ValidWindowsFilenamePattern.IsMatch(filename);
        }

        /// <summary>
        /// Validates the fields of a Power of Attorney object and returns a list of errors if any required fields are invalid or missing.
        /// </summary>
        /// <param name="poa">The Power of Attorney object to validate.</param>
        /// <returns>A list of error messages indicating the fields that are invalid or missing. Returns an empty list if all fields are valid.</returns>
        public static List<string> ValidatePoaFields(PowerOfAttorney poa)
        {
            var errors = new List<string>();
            
            if (poa.AgencyId == Guid.Empty)
            {
                errors.Add("Agency has not been defined. Please correct and try again.");
            }

            if (string.IsNullOrWhiteSpace(poa.Status))
            {
                errors.Add("Status has not been defined. Please correct and try again.");
            }

            if (poa.Insurer.Id == Guid.Empty)
            {
                errors.Add("Insurer has not been defined. Please correct and try again.");
            }

            return errors;
        }
    }
}