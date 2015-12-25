using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using Microsoft.Practices.Unity.Configuration;
using ReusableLibrary.Abstractions.IoC;

namespace ReusableLibrary.Unity
{
    public static class UnityBootstrapLoader
    {
        public static string[] LoadConfigFilesFromAppSettings()
        {
            return LoadConfigFilesFromAppSettings(null);
        }

        public static string[] LoadConfigFilesFromAppSettings(string name)
        {
            var configFiles = ConfigurationManager.AppSettings[name ?? "UnityBootstrapLoader"]
                .Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            return configFiles;
        }

        public static void Initialize(params string[] configFiles)
        {            
            var workingDir = new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile).DirectoryName;
            var unityConfigurationSections = new List<UnityConfigurationSection>();
            UnityConfigurationSection config = null;                
            foreach (var configFile in configFiles)
            {
                var map = new ExeConfigurationFileMap()
                {
                    ExeConfigFilename = Path.Combine(workingDir, configFile.EndsWith(".config", StringComparison.OrdinalIgnoreCase) ? configFile : configFile + ".config")
                };
                config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None).GetSection("unity") as UnityConfigurationSection;
                if (config == null)
                {
                    throw new ConfigurationErrorsException(String.Format(CultureInfo.InvariantCulture, "Unable to load unity configuration from file '{0}'.", configFile));
                }

                unityConfigurationSections.Add(config);
            }

            config = ConfigurationManager.GetSection("unity") as UnityConfigurationSection;
            if (config != null)
            {
                unityConfigurationSections.Add(config);
            }

            DependencyResolver.InitializeWith(new UnityDependencyResolver(unityConfigurationSections.ToArray()));
        }
    }
}
