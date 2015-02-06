using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMELuckyDraw.Util
{
    public class ConfigHelper
    {
        //获取Configuration对象
        Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        static ConfigHelper _instance = new ConfigHelper();

        private ConfigHelper() { }

        public static ConfigHelper Instance()
        {
            return _instance;
        }

        private void refreshAppSettings()
        {
            try
            {
                //一定要记得保存，写不带参数的config.Save()也可以
                config.Save(ConfigurationSaveMode.Modified);
                //刷新，否则程序读取的还是之前的值（可能已装入内存）
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception e)
            {
                LogHelper.DEBUG("refreshAppSettings failed.", e);
                throw e;
            }
        }

        public string GetAppSettings(string key)
        {
            return config.AppSettings.Settings[key].Value;
        }

        public void AppendAppSettings(string key, string value)
        {
            //追加<add>元素的Value
            config.AppSettings.Settings[key].Value += value;
            refreshAppSettings();
        }

        public void AddAppSettings(string key, string value)
        {
            //增加<add>元素
            config.AppSettings.Settings.Add(key, value);
            refreshAppSettings();
        }

        public void RemoveAppSettings(string key)
        {
            //删除<add>元素
            config.AppSettings.Settings.Remove(key);
            refreshAppSettings();
        }

        public void UpdateAppSettings(string key, string value)
        {
            //写入<add>元素的Value
            config.AppSettings.Settings[key].Value = value;
            refreshAppSettings();
        }
    }
}
