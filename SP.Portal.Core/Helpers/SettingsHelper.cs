using Microsoft.SharePoint.Utilities;
using System.Configuration;
using System.IO;

namespace SP.Portal.Core.Helpers
{
    public class SettingsHelper
    {
        public static string GetConnectionString(string name = "SP.Portal")
        {
            var dataBaseConfigPath = SPUtility.GetVersionedGenericSetupPath(Defaults.Paths.DatabaseConfig, 15);
            var dataBaseConfigProdPath = SPUtility.GetVersionedGenericSetupPath(Defaults.Paths.DatabaseConfigProd, 15);
            string currentConfig = File.Exists(dataBaseConfigProdPath) ? dataBaseConfigProdPath : dataBaseConfigPath;

            if (File.Exists(currentConfig))
            {
                var cfgMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = currentConfig
                };
                var cfg = ConfigurationManager.OpenMappedExeConfiguration(cfgMap, ConfigurationUserLevel.None);
                var conStringsSection = cfg.GetSection("connectionStrings") as ConnectionStringsSection;
                if (conStringsSection != null)
                    return conStringsSection.ConnectionStrings[name].ToString();
            }

            return null;
        }

        public static string GetAppSetting(string name)
        {
            var dataBaseConfigPath = SPUtility.GetVersionedGenericSetupPath(Defaults.Paths.DatabaseConfig, 15);
            var dataBaseConfigProdPath = SPUtility.GetVersionedGenericSetupPath(Defaults.Paths.DatabaseConfigProd, 15);
            string currentConfig = File.Exists(dataBaseConfigProdPath) ? dataBaseConfigProdPath : dataBaseConfigPath;

            return SettingsHelper.GetAppSetting(currentConfig);
        }

        public static string GetAppSetting(string configPath, string name)
        {
            if (File.Exists(configPath))
            {
                var cfgMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configPath
                };
                var cfg = ConfigurationManager.OpenMappedExeConfiguration(cfgMap, ConfigurationUserLevel.None);
                return cfg.AppSettings.Settings[name]?.Value;
            }

            return null;
        }

        public static KeyValueConfigurationCollection GetAppSettings(string configPath)
        {
            if (File.Exists(configPath))
            {
                var cfgMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = configPath
                };
                var cfg = ConfigurationManager.OpenMappedExeConfiguration(cfgMap, ConfigurationUserLevel.None);
                return cfg.AppSettings.Settings;
            }

            return null;
        }

        public static T GetConfigSection<T>(string sectionName)
            where T : ConfigurationSection
        {
            var dataBaseConfigPath = SPUtility.GetVersionedGenericSetupPath(Defaults.Paths.DatabaseConfig, 15);
            var dataBaseConfigProdPath = SPUtility.GetVersionedGenericSetupPath(Defaults.Paths.DatabaseConfigProd, 15);
            string currentConfig = File.Exists(dataBaseConfigProdPath) ? dataBaseConfigProdPath : dataBaseConfigPath;

            if (File.Exists(currentConfig))
            {
                var cfgMap = new ExeConfigurationFileMap
                {
                    ExeConfigFilename = currentConfig
                };
                var cfg = ConfigurationManager.OpenMappedExeConfiguration(cfgMap, ConfigurationUserLevel.None);
                return cfg.GetSection(sectionName) as T;

            }
            return null;
        }
    }
}
