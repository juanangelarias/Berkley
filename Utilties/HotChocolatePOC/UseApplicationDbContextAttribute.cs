using System.Reflection;
using HotChocolate.Types.Descriptors;
using James.Data.Server.Model;

namespace HotChocolatePOC
{
    public class UseApplicationDbContextAttribute : ObjectFieldDescriptorAttribute
    {
        protected override void OnConfigure(
            IDescriptorContext context,
            IObjectFieldDescriptor descriptor,
            MemberInfo member)
        {
            descriptor.UseDbContext<JamesDatabaseContext>();
        }
    }
}