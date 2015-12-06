using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace 攒机助手
{
    class SettingFile
    {
        public SettingFile()
        {
            filePath = System.Windows.Forms.Application.StartupPath + "\\settings.bin";
            Read();
        }
        private Dictionary<string, string> settingItems = new Dictionary<string, string>();

        private string filePath;

        public string FilePath
        {
            get { return filePath; }
            set { filePath = value; }
        }

        public string GetValue(string name)
        {
            if (settingItems.ContainsKey("name"))
            {
                return settingItems[name];
            }
            else
            {
                return string.Empty;
            }

        }
        public void SetValue(string name, string value)
        {

            if (settingItems.ContainsKey(name))
            {
                settingItems[name] = value;
                //return settingItems[name];
            }
            else
            {
                settingItems.Add(name, value);
                //return "";
            }
        }

        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(new FileStream(filePath, FileMode.Create), settingItems);
            //StringBuilder sb = new StringBuilder();
            //foreach (KeyValuePair<string,string> item in settingItems)
            //{
            //    sb.AppendFormat("", item.Key + item.Value);
            //    //sb.Append(item.Key +item.Value );
            //}
        }
        public void Read()
        {
            BinaryFormatter bf = new BinaryFormatter();
            if (File.Exists(filePath))
            {
                object obj = bf.Deserialize(new FileStream(filePath, FileMode.Open));
                settingItems = obj as Dictionary<string, string>;
            }
        }

        //Dictionary<string, string> dict = new Dictionary<string, string>();
    }
}
