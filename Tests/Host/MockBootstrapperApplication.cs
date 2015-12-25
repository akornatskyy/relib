using System;

namespace ReusableLibrary.Host.Tests
{
    internal sealed class MockBootstrapperApplication : BootstrapperApplication
    {
        public MockBootstrapperApplication()
            : base("Mock")
        {
        }

        public bool RaiseErrorOnStart { get; set; }

        public bool RaiseErrorOnStop { get; set; }

        public override void OnStart()
        {
            if (RaiseErrorOnStart)
            {
                throw new InvalidOperationException();
            }

            base.OnStart();
        }

        public override void OnStop()
        {
            if (RaiseErrorOnStop)
            {
                throw new InvalidOperationException();
            }

            base.OnStop();
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
