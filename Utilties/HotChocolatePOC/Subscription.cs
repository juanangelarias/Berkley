using James.Shared.Model;

namespace HotChocolatePOC
{
    public class Subscription
    {
        [Subscribe]
        [Topic(nameof(FirstMutation.AddProgram))]
        public Account OnProgramAdded([EventMessage] Account account)
            => account;

        [Subscribe]
        [Topic(nameof(Mutation.UpdateBank))]
        public Account OnAccountModified([EventMessage] Account account) => account;
    }
}
