using System.Diagnostics;
using HotChocolate.Subscriptions;
using James.Data.Server.Model;
using James.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace HotChocolatePOC
{
    [MutationType]
    public class MyMutationType:ObjectType<FirstMutation>{}
    public class FirstMutation
    {
        //public string Test { get; set; }="Test";
        //public string AnotherTest;
        //[UseMutationConvention]
        //public async Task<string> FakeMutation(string input)
        //{
        //    await Task.Delay(50);
        //    return $"Processed {input}";
        //}
        ////[UseMutationConvention]
        //public async Task<string> NoArgMutation()
        //{
        //    await Task.Delay(50);
        //    return $"Processed imaginary stuff";
        //}
        ////[UseMutationConvention]
        //public async Task<Account> AddFakeAccount()
        //{
        //    await Task.Delay(50);
        //    var fakeAccount = new Account() { AccountNum = "1234567", IdNavigation = new LegalEntity() { FullName = "Fake Account" } };
        //    return fakeAccount;
        //}
        ////[UseMutationConvention]
        //public async Task<AddProgramPayload> AddFakeAccountProgram(AccountProgram program)
        //{
        //    await Task.Delay(50);
        //    var fakeAccount = new Account() { AccountNum = "1234567", IdNavigation = new LegalEntity() { FullName = "Fake Account" } };
        //    return new AddProgramPayload() { Account = fakeAccount };
        //}

        [UseMutationConvention]
        public async Task<AddProgramPayload> AddProgram(AddProgramDataInput input,
            [Service] ITopicEventSender eventSender, [Service] IDbContextFactory<JamesDatabaseContext> contextFactory)
        {
            var ctx = await contextFactory.CreateDbContextAsync();
            var program = new AccountProgram
            {
                AccountNum = input.AccountNum, Single = input.Single, Aggregate = input.Aggregate,
                Effective = input.Effective, Expiration = input.Expiration
            };
            var response = new AddProgramPayload();
            var errors = new List<Error>();

            //Validate
            if (program == null) errors.Add(new Error("Program is required"));
            else
            {
                if (program.Single > program.Aggregate)
                    errors.Add(new Error("Single program cannot be larger than the aggregate program", "3"));
                if (program.Effective > program.Expiration)
                    errors.Add(new Error("Effective date must be before expiration date.", "4"));
                if (program.Effective < new DateTime(2014, 0, 0))
                    errors.Add(new Error("Effective date out of range", "5"));
            }

            switch (errors.Count)
            {
                case 0:
                    //Get existing account and program
                    response.Account = ctx.Accounts.Include(a => a.IdNavigation)
                        .Include(a => a.IdNavigation.LegalEntityAddresses)
                        .ThenInclude(a => a.Address)
                        .Include(a => a.IdNavigation.LegalEntityPhones)
                        .ThenInclude(a => a.PhoneNumber)
                        .Include(a => a.IdNavigation.LegalEntityEmails)
                        .Include(a => a.AgencyNumberNavigation)
                        .ThenInclude(ag => ag.IdNavigation)
                        .ThenInclude(agi => agi.LegalEntityAddresses)
                        .ThenInclude(agia => agia.Address)
                        .Include(a => a.Underwriter)
                        .ThenInclude(uw => uw.IdNavigation)
                        .Include(a => a.Agent)
                        .ThenInclude(ag => ag.IdNavigation)
                        .Include(a => a.HomeOfficeReviewByNavigation)
                        .Include(a => a.BranchReviewByNavigation)
                        .Include(a => a.AccountPrograms)
                        .FirstOrDefault(a => a.AccountNum.Trim() == program.AccountNum.Trim());
                    if (null == response.Account)
                    {
                        response.Error = new ErrorMessage {Code = "1", Message = "Invalid Account"};
                        break;
                    }

                    var existing = response.Account.AccountPrograms.OrderByDescending(ap => ap.Effective)
                        .FirstOrDefault(ap => ap.Expiration > program.Effective);
                    await using (var transaction = await ctx.Database.BeginTransactionAsync())
                    {
                        Debug.Assert(program != null, nameof(program) + " != null");
                        if (null != existing)
                            existing.Expiration = program.Expiration;

                        //Add program
                        program.Created = program.Modified = DateTime.Now;
                        //HACK: Not sure if both of the below are needed
                        var ent = await ctx.AccountPrograms.AddAsync(program);
                        response.Account.AccountPrograms.Add(ent.Entity);

                        await ctx.SaveChangesAsync();
                        await transaction.CommitAsync();
                    }

                    break;
                case 1:
                    response.Error = errors.Select(e => new ErrorMessage {Code = e.Code, Message = e.Message}).First();
                    break;
                default:
                    response.Error = new AggregateErrorMessage(errors.Select(e => new ErrorMessage
                        {Code = e.Code, Message = e.Message}));
                    break;
            }

            if (null != response.Account)
            {
                //Fire subscription events 
                await eventSender.SendAsync(nameof(AddProgram), response.Account, CancellationToken.None);
            }

            return response;
        }
    }

    public class AddProgramPayload
    {
        public Account? Account { get; set; }
        //HACK: This is not the best practices way to return errors, just my first attempt
        public ErrorMessage? Error { get; set; }
    }

    public class ErrorMessage
    {
        public virtual string Code { get; set; }
        public virtual string Message { get; set; }
    }

    public class AggregateErrorMessage : ErrorMessage
    {
        public AggregateErrorMessage(IEnumerable<ErrorMessage> errors)
        {
            ErrorMessages = errors.ToArray();
            //Code = "Aggregate";
        }
        public ErrorMessage[] ErrorMessages { get; init; }
        public override string Code => "Aggregate";
        public override string Message => "The following errors were thrown: \r\n" +
                                          string.Join("\r\n|", ErrorMessages.Select(em => $"({em.Code}){em.Message}"));

    }

    public class AddProgramDataInput
    {
        public string AccountNum { get; set; }
        public int Single { get; set; }
        public int Aggregate { get; set; }
        public DateTime Effective { get; set; }
        public DateTime Expiration { get; set; }   
    }

    /// <summary>
    /// Class that only exists because of a strange error that "AccountProgramInput" already exists.
    /// </summary>
    public class NewAccountProgramInput : AccountProgram
    {
        //public NewAccountProgramInput(AccountProgram program, bool isDifferent = true)
        //{
        //    var properties = program.GetType().GetProperties(BindingFlags.Public);
        //    foreach (var property in properties)
        //    {
        //        property.SetValue(this, property.GetValue(program));
        //    }
        //    IsDifferent = isDifferent;
        //}

        public bool? IsDifferent { get; set; } = true;
    }
}
