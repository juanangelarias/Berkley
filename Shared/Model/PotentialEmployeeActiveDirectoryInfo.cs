using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    public class PotentialEmployeeActiveDirectoryInfo
    {
        private string? _selectedEmail;
        public string Username { get; set; }
        public string FullName { get; set; }
        public List<string> Emails { get; set; }
        public string Initials { get; set; }
        public string Title { get; set; }

        public string? SelectedEmail
        {
            get => _selectedEmail ??= Emails.FirstOrDefault();
            set => _selectedEmail = value;
        }
    }
}
