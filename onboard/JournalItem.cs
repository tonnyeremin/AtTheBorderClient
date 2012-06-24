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
using System.Runtime.Serialization;

namespace Onboard
{
    [DataContract]
    public class JournalItemInfo
    {
        #region Public Fields
        [DataMember(Name = "direction")]
        public CrossingDirection Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        [DataMember(Name = "station")]
        public CrossingPointName Station
        {
            get { return _station; }
            set { _station = value; }
        }
        [DataMember(Name = "number")]
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }
        [DataMember(Name = "comment")]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }
        [DataMember(Name = "author")]
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }
        [DataMember(Name = "date")]
        public double Date
        {
            get { return _date; }
            set { _date = value; }
        }
        #endregion

        public JournalItemInfo Clone()
        {
            return this.MemberwiseClone() as JournalItemInfo;
        }

        private CrossingDirection _direction;
        private CrossingPointName _station;
        private string _number;
        private string _comment;
        private string _author;
        private double _date;

        public override bool Equals(object obj)
        {
           if(obj != null && obj is JournalItemInfo)
           {
               JournalItemInfo item = obj as JournalItemInfo;
               if(item.Author == Author &&
                   item.Comment == Comment &&
                   item.Date == Date &&
                   item.Direction == Direction &&
                   item.Number == Number &&
                   item.Station == Station)
                   return true;
           }
           return false;
        }
    }
}
