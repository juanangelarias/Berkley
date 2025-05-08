using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    public class JamesSearchResult
    {
        public required string Name { get; init; }
        public required LegalEntity Entity { get; set; }
        public SearchResultType Type { get; init; }
        public bool FromDescription { get; set; }
        public int Confidence { get; set; }
        //The below are optional fields that are only used for certain SearchResultTypes
        public List<Bond>? BondList { get; set; } //Used by Type BondList and Bond 
        public ImagingDocument? Document { get; set; } //Used by Type File 
        public LegalEntity? Parent { get; set; } //Used by type Person
        public string? AccountNum { get; set; }//Used by Accounts
        public string? AgencyNumber { get; set; }//Used by Agencies

        /// <summary>
        /// The full string that matched the search term
        /// </summary>
        /// <remarks>Use this to filter previous results when the search term becomes more specific.</remarks>
        public required string SearchString { get; set; }

        public override string ToString()
        {
            return $"{Name}|{Entity?.FullName??"No Name"}|{Type}|{SearchString}";
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            if (obj is JamesSearchResult other)
                //Loosely matching even if confidence, FromDescription or BondList do not match
                return ToString() == other.ToString();
            return false;
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

    public enum SearchResultType
    {
        Unset = 0,
        Account = 1,
        Bond = 2,
        BondList = 3,
        Agency = 4,
        Obligee = 5,
        File = 6,
        Person = 7
    }
}
