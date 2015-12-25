using ReusableLibrary.Abstractions.Bootstrapper;
using ReusableLibrary.Web.Helpers;

namespace ReusableLibrary.Web.Integration
{
    public sealed class RegisterShrinkLeaveAtLeast : IStartupTask
    {
        public RegisterShrinkLeaveAtLeast()
        {
            LeaveAtLeast = 1;
        }

        public int LeaveAtLeast { get; set; }

        public void Execute()
        {
            ShrinkHelper.LeaveAtLeast = LeaveAtLeast;
        }
    }
}
