using System;
using System.Configuration;

namespace ReusableLibrary.Abstractions.Repository
{
    public sealed class DbConnectionStringProvider
    {
        public DbConnectionStringProvider(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            ConnectionString = ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }

        public string ConnectionString { get; private set; }
    }
}
