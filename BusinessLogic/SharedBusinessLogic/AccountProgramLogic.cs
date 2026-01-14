using James.Shared.Model;
using static System.DateTime;

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
            // ToDo: add the actual logic
            
            //Get Account

            //Get User Program Authority


            return true;
        }

        /// <summary>
        /// Calculates the prorated bond amount based on the total amount, start date, end date, and an optional actual date.
        /// The calculation considers the portion of the bond's life remaining relative to its total duration.
        /// </summary>
        /// <param name="amount">The total bond amount to be prorated.</param>
        /// <param name="startDate">The start date of the bond coverage period.</param>
        /// <param name="endDate">The end date of the bond coverage period.</param>
        /// <param name="actualDate">The current or specific date for prorating the amount. Defaults to today if not provided.</param>
        /// <returns>The prorated bond amount as an integer.</returns>
        public static int CalculateProratedBondAmount(int amount, DateOnly startDate, DateOnly endDate,
            DateOnly? actualDate = null)
        {
            if(amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount must be greater than zero.");
            
            if(startDate > endDate)
                throw new ArgumentOutOfRangeException(nameof(startDate), "Start date must be before end date.");
            
            actualDate ??= DateOnly.FromDateTime(Today);

            var lifeInDays = endDate.DayNumber - startDate.DayNumber;
            var remnantLife = endDate > actualDate.Value 
                ? endDate.DayNumber - actualDate.Value.DayNumber
                : 0;
        
            return lifeInDays > 0 ? amount * remnantLife / lifeInDays : 0;
        }
    }
}