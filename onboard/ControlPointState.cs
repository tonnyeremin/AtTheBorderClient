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
using System.Device.Location;

namespace Onboard
{
    public class CrossingPoint
    {
        public CrossingPointName Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public CrossingDirection Direction
        {
            get { return _Direction; }
            set { _Direction = value; }
        }

        public CrossingPointInfo Info
        {
            get { return _Info; }
            set { _Info = value; }
        }

        public GeoCoordinate Coordinates
        {
            get { return _Coordinates; }
            set { _Coordinates = value; }
        }

        private CrossingDirection _Direction;
        private CrossingPointName _Name;
        private CrossingPointInfo _Info;
        private GeoCoordinate _Coordinates;

    }
    
    [DataContract]
    public class CrossingPointInfo
    {
        #region Public Fields
        [DataMember(Name = "number")]
        public string Number
        {
            get { return _number; }
            set { _number = value; }
        }

        [DataMember(Name = "date")]
        public double Date
        {
            get { return _date; }
            set { _date = value; }
        }

        [DataMember(Name = "comment")]
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        [DataMember(Name = "changes")]
        public int Changes
        {
            get { return _changes; }
            set { _changes = value; }
        }

        [DataMember(Name = "author")]
        public string Author
        {
            get { return _author; }
            set { _author = value; }
        }

        [DataMember(Name = "url")]
        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        [DataMember(Name = "waiting")]
        public int Waiting
        {
            get { return _waiting; }
            set { _waiting = value; }
        }
        #endregion

        private string _number;
        private double _date;
        private string _comment;
        private int _changes;
        private string _author;
        private string _url;
        private int _waiting;
    }

}
