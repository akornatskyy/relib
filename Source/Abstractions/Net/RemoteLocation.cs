using System;
using ReusableLibrary.Abstractions.Helpers;
using ReusableLibrary.Abstractions.Models;

namespace ReusableLibrary.Abstractions.Net
{
    [Serializable]
    public sealed class RemoteLocation : IKeyProvider
    {
        public static readonly RemoteLocation Localhost = new RemoteLocation() 
        { 
            Hosts = new[] { IpNumberHelper.Localhost }
        };

        public RemoteLocation() 
        { 
        }

        public RemoteLocation(RemoteLocation that)
        {
            if (that == null)
            {
                throw new ArgumentNullException("that");
            }

            if (that.HasHosts)
            {
                Hosts = new string[that.Hosts.Length];
                Array.Copy(that.Hosts, 0, Hosts, 0, Hosts.Length);
            }
        }

        public string[] Hosts { get; set; }

        public bool HasHosts 
        {
            get { return Hosts != null && Hosts.Length > 0; }
        }

        #region IKeyProvider Members

        public string ToKeyString()
        {
            if (HasHosts)
            {
                return string.Join(";", Hosts);
            }

            return string.Empty;
        }

        #endregion
    }
}
