using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    public class PotentialEmployeeActiveDirectoryInfo
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public List<string> Emails { get; set; }
        public string Initials { get; set; }
    }
}
