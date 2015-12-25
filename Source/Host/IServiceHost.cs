using System;
using System.Collections.ObjectModel;

namespace ReusableLibrary.Host
{
    public interface IServiceHost
    {
        ReadOnlyCollection<Uri> BaseAddresses { get; }

        void Open();

        void Close();
    }
}
