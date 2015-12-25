using System;
using System.Data.Linq.Mapping;
using System.IO;

namespace ReusableLibrary.Supplemental.Repository
{
    public sealed class XmlMappingSourceFromUrlProvider : IMappingSourceProvider
    {
        public XmlMappingSourceFromUrlProvider(string filename)
        {
            var workingDir = new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile).DirectoryName;
            MappingSource = XmlMappingSource.FromUrl(Path.Combine(workingDir, filename));
        }

        #region IMappingSourceProvider Members

        public MappingSource MappingSource { get; private set; }

        #endregion
    }
}
