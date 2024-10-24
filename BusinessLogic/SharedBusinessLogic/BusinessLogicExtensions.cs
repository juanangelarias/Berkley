using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using James.Shared.Model;

namespace SharedBusinessLogic
{
    public static class BusinessLogicExtensions
    {
        //TODO:  If this is added to the DB with schema modifications, move or remove as needed.
        public static string? ToWritingCompany(this Insurer insurer)
        {
            //TODO: Discuss with Matt
            switch (insurer.DefaultRateGroup)
            {
                case "CCIC":
                case "BRIC":
                    return insurer.DefaultRateGroup;
                default:
                    return insurer.DefaultRateGroup?.StartsWith("BIC")==true ? "BIC" : null;
            }
        }
    }
}
