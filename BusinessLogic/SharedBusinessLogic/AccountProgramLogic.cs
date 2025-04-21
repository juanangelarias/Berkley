using James.Shared.Data;
using James.Shared.Model;
using System.Reflection.Metadata.Ecma335;

namespace SharedBusinessLogic
{
    public static class AccountProgramBusinessLogic
    {
        /// <summary>
        /// Approval Conditions: 
        ///     1 User must have Program Authority
        ///     2 If previous program was Home Office Approved, User must be Home Office Underwriter
        ///     3 If Field Office, they cannot approve program on a child account
        ///     4 Program Single/Agg must be within User Single/Agg
        /// </summary>
        /// <param name="programToApprove">The program being approved</param>
        /// <returns>True if approval request is valid.</returns>
        public static bool ApproveAccountProgram(AccountProgram programToApprove)
        {
            //Get Account

            //Get User Program Authority


            return true;
        }
    }
}