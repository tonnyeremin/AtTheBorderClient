using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;

namespace Onboard
{
    /// <summary>
    /// 
    /// </summary>
    public class StorageHelper
    {
        private static readonly string JournalFileName = "Data.xml";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public static void SaveData(XmlDataWrapper data)
        {
            TextWriter writer = null;
            try
            {
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream file = storage.OpenFile(JournalFileName, FileMode.Create);
                writer = new StreamWriter(file);
                XmlSerializer xs = new XmlSerializer(typeof(XmlDataWrapper));
                xs.Serialize(writer, data);
                writer.Close();
                file.Close();
            }
            catch
            {
            }
            finally
            {
                if (writer != null)
                    writer.Dispose();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static XmlDataWrapper LoadData()
        {
            XmlDataWrapper data = new XmlDataWrapper();
            TextReader reader = null;
            try
            {
                IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream file = storage.OpenFile(JournalFileName, FileMode.OpenOrCreate);
                reader = new StreamReader(file);
                XmlSerializer xs = new XmlSerializer(typeof(XmlDataWrapper));
                data = (XmlDataWrapper)xs.Deserialize(reader);
                reader.Close();
                file.Close();
            }
            catch
            {
                return null;
            }
            finally
            {
                if (reader != null)
                    reader.Dispose();
            }
            return data;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class XmlDataWrapper
    {
        /// <summary>
        /// 
        /// </summary>
        public XmlDataWrapper()
        {
            ToRussia = new List<CrossingPoint>();
            FromRussia = new List<CrossingPoint>();
            Journal = new List<JournalItemInfo>();
        }
        public XmlDataWrapper(MainViewModel model)
        {
            ToRussia = model.ToRussia;
            FromRussia = model.FromRussia;
            Journal = model.Journal;
        }
        /// <summary>
        /// 
        /// </summary>
        public  List<CrossingPoint> ToRussia { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public  List<CrossingPoint> FromRussia { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<JournalItemInfo> Journal { get; set; }
    }
}
