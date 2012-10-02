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
using System.IO;
using System.Runtime.Serialization.Json;
using System.Windows.Threading;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;

namespace Onboard
{
    /// <summary>
    /// 
    /// </summary>
    public class RequestUpdateState
    {
        /// <summary>
        /// 
        /// </summary>
        public HttpWebRequest AsyncRequest { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public HttpWebResponse AsyncResponse { get; set; }

        public int UpdateMode { get; set; }

        public string Data { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void JournalEventHandler(object sender, JournalEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    public class JournalEventArgs : EventArgs
    {
        private List<JournalItemInfo> _JournalItems;
        /// <summary>
        /// 
        /// </summary>
        public List<JournalItemInfo> JournalItems
        {
            get { return _JournalItems; }
            set { _JournalItems = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public JournalEventArgs(List<JournalItemInfo> items)
        {
            _JournalItems = items;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void StateEventHandler(object sender, StateEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    public class StateEventArgs : EventArgs
    {
        private List<CrossingPoint> _ToRussia;
        /// <summary>
        /// 
        /// </summary>
        public List<CrossingPoint> ToRussia
        {
            get { return _ToRussia; }
            set { _ToRussia = value; }
        }
        private List<CrossingPoint> _FromRussia;
        /// <summary>
        /// 
        /// </summary>
        public List<CrossingPoint> FromRussia
        {
            get { return _FromRussia; }
            set { _FromRussia = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="toRussia"></param>
        /// <param name="fromRussia"></param>
        public StateEventArgs(List<CrossingPoint> toRussia, List<CrossingPoint> fromRussia)
        {
            this._FromRussia = fromRussia;
            this._ToRussia = toRussia;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ClientExeptionHandler(object sender, ClientExeptionEventArgs e);
    /// <summary>
    /// 
    /// </summary>
    public class ClientExeptionEventArgs : EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public ClientExeptionEventArgs(string message, Exception ex)
        {
            _Exeption = ex;
            _Message = message;
        }

        private Exception _Exeption;
        /// <summary>
        /// 
        /// </summary>
        public Exception Exeption
        {
            get { return _Exeption; }
            set { _Exeption = value; }
        }

        private string _Message;
        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get { return _Message; }
            set { _Message = value; }
        }
    }

    public delegate void SendEventHandler(object sender, SendEventArgs e);

    public class SendEventArgs : EventArgs
    {
        private bool _Successed;

        public bool Successed
        {
            get { return _Successed; }
            set { _Successed = value; }
        }

        public SendEventArgs(bool succesed)
        {
            _Successed = succesed;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Client
    {
        /// <summary>
        /// 
        /// </summary>
        public event JournalEventHandler OnJournalUpdated;
        /// <summary>
        /// 
        /// </summary>
        public event JournalEventHandler OnJournalGetPrevious;
        /// <summary>
        /// 
        /// </summary>
        public event StateEventHandler OnBoardInfoUpdated;
        /// <summary>
        /// 
        /// </summary>
        public event ClientExeptionHandler OnClientException;

        public event SendEventHandler OnSend;

        public void SendInfo(MainViewModel model)
        {
            UriBuilder fullUri = new UriBuilder(SEND_INFO);
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(fullUri.Uri);
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Method = "POST";
            Request.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.4 (KHTML, like Gecko) Chrome/22.0.1229.79 Safari/537.4";
            RequestUpdateState forecastState = new RequestUpdateState();
            forecastState.UpdateMode = 0;
            forecastState.AsyncRequest = Request;
            forecastState.Data = "direction=0&point=0&number=1&lenght=&comment=Пусто&author=trackmonster";
            Request.BeginGetRequestStream(RequestReady, forecastState);

            Request.BeginGetResponse(new AsyncCallback(HandleSendResponce), forecastState);

        }

        private void RequestReady(IAsyncResult result)
        {
            RequestUpdateState state = result.AsyncState as RequestUpdateState;
            HttpWebRequest request = state.AsyncRequest;
            Stream stream = request.EndGetRequestStream(result);
            Deployment.Current.Dispatcher.BeginInvoke(delegate()
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(state.Data);
                writer.Flush();
                writer.Close();
                request.BeginGetResponse(HandleStateResponse, state);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetJournal()
        {
            UriBuilder fullUri = new UriBuilder(GET_TEN_LAST_JOURNAL_ITEMS);
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(fullUri.Uri);
            RequestUpdateState forecastState = new RequestUpdateState();
            forecastState.UpdateMode = 0;
            forecastState.AsyncRequest = Request;
            Request.BeginGetResponse(new AsyncCallback(HandleResponse), forecastState);
        }

        public void GetJournal(int offset)
        {
            UriBuilder fullUri = new UriBuilder(GET_NEXT_JOURNAL_ITEMS + offset);
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(fullUri.Uri);
            RequestUpdateState forecastState = new RequestUpdateState();
            forecastState.UpdateMode = 1;
            forecastState.AsyncRequest = Request;
            Request.BeginGetResponse(new AsyncCallback(HandleResponse), forecastState);
        }
        /// <summary>
        /// 
        /// </summary>
        public void GetState()
        {
            UriBuilder fullUri = new UriBuilder(GET_CURRENT_STATE);
            HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(fullUri.Uri);
            RequestUpdateState forecastState = new RequestUpdateState();
            forecastState.AsyncRequest = Request;
            Request.BeginGetResponse(new AsyncCallback(HandleStateResponse), forecastState);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Update()
        {
            _ExeptionThrown = false;
            GetJournal();
            GetState();
        }

        public void Update(UpdateMode mode)
        {
            _ExeptionThrown = false;

            if (mode == UpdateMode.Journal)
                GetJournal();
            if (mode == UpdateMode.Traffic)
                GetState();
        }

        public XmlDataWrapper LoadInfo()
        {
            return StorageHelper.LoadData();
        }

        private void HandleResponse(IAsyncResult asyncResult)
        {
            List<JournalItemInfo> result = new List<JournalItemInfo>();
            try
            {
                RequestUpdateState state = (RequestUpdateState)asyncResult.AsyncState;
                HttpWebRequest request = (HttpWebRequest)state.AsyncRequest;
                state.AsyncResponse = (HttpWebResponse)request.EndGetResponse(asyncResult);

                Stream streamResult;
                string jsonString;

                streamResult = state.AsyncResponse.GetResponseStream();

                using (StreamReader reader = new StreamReader(streamResult))
                    jsonString = reader.ReadToEnd();

                JournalItemInfo[] arr = Newtonsoft.Json.JsonConvert.DeserializeObject<JournalItemInfo[]>(jsonString);

                foreach (JournalItemInfo info in arr)
                    result.Add(info);
                if (state.UpdateMode == 0)
                    RaiseOnJournalUpdated(new JournalEventArgs(result));
                else
                    RaiseOnJournalGetPrevious(new JournalEventArgs(result));
            }
            catch (Exception ex)
            {
                RaiseClientExeption(new ClientExeptionEventArgs("Updating journal failed.", ex));
            }


        }

        private void HandleStateResponse(IAsyncResult asyncResult)
        {
            List<CrossingPoint> toRussia = new List<CrossingPoint>();
            List<CrossingPoint> fromRussia = new List<CrossingPoint>();
            JObject token = null;
            CrossingPoint point = null;
            Stream streamResult;
            string str;
            try
            {
                RequestUpdateState state = (RequestUpdateState)asyncResult.AsyncState;
                HttpWebRequest request = (HttpWebRequest)state.AsyncRequest;

                state.AsyncResponse = (HttpWebResponse)request.EndGetResponse(asyncResult);

                streamResult = state.AsyncResponse.GetResponseStream();
                using (StreamReader reader = new StreamReader(streamResult))
                {
                    str = reader.ReadToEnd();

                }
                JArray root = JArray.Parse(str);

                //Get from Russia traffic.
                token = (JObject)root[0];
                GetCrossingPoint(token, ref point, CrossingPointName.Torfynovka);
                // point.Coordinates = GeoConstants.Torfyanovka;
                point.Direction = CrossingDirection.FromRussia;
                fromRussia.Add(point);
                GetCrossingPoint(token, ref point, CrossingPointName.Brusnichnoe);
                point.Direction = CrossingDirection.FromRussia;
                //point.Coordinates = GeoConstants.Brusnichnoe;
                fromRussia.Add(point);
                GetCrossingPoint(token, ref point, CrossingPointName.Svetogorsk);
                point.Direction = CrossingDirection.FromRussia;
                //point.Coordinates = GeoConstants.Svetogorsk;
                fromRussia.Add(point);

                //Get to Russia traffic.
                token = (JObject)root[1];
                GetCrossingPoint(token, ref point, CrossingPointName.Torfynovka);
                point.Direction = CrossingDirection.ToRussia;
                // point.Coordinates = GeoConstants.Torfyanovka;
                toRussia.Add(point);
                GetCrossingPoint(token, ref point, CrossingPointName.Brusnichnoe);
                point.Direction = CrossingDirection.ToRussia;
                //point.Coordinates = GeoConstants.Brusnichnoe;
                toRussia.Add(point);
                GetCrossingPoint(token, ref point, CrossingPointName.Svetogorsk);
                point.Direction = CrossingDirection.ToRussia;
                toRussia.Add(point);
                //point.Coordinates = GeoConstants.Svetogorsk;
                RaiseOnStateUpdated(new StateEventArgs(toRussia, fromRussia));
            }
            catch (Exception ex)
            {
                RaiseClientExeption(new ClientExeptionEventArgs("Updating traffic failed.", ex));
            }
        }

        private void HandleSendResponce(IAsyncResult asyncResult)
        {
            Stream streamResult;
            string str;
            try
            {
                RequestUpdateState state = (RequestUpdateState)asyncResult.AsyncState;
                HttpWebRequest request = (HttpWebRequest)state.AsyncRequest;

                state.AsyncResponse = (HttpWebResponse)request.EndGetResponse(asyncResult);

                streamResult = state.AsyncResponse.GetResponseStream();
                using (StreamReader reader = new StreamReader(streamResult))
                {
                    str = reader.ReadToEnd();

                }
            }
            catch (Exception ex)
            {

            }
        }

        private void RaiseOnSend(SendEventArgs args)
        {
            if (OnSend != null)
            {
                OnSend(this, args);
            }
        }

        private void RaiseOnJournalUpdated(JournalEventArgs args)
        {
            if (OnJournalUpdated != null)
            {
                OnJournalUpdated(this, args);
            }
        }

        private void RaiseOnStateUpdated(StateEventArgs args)
        {
            if (OnBoardInfoUpdated != null)
            {
                OnBoardInfoUpdated(this, args);
            }
        }

        private void RaiseClientExeption(ClientExeptionEventArgs args)
        {
            if (OnClientException != null && !_ExeptionThrown)
            {
                OnClientException(this, args);
                _ExeptionThrown = true;
            }
        }

        private void RaiseOnJournalGetPrevious(JournalEventArgs args)
        {
            if (OnJournalGetPrevious != null)
            {
                OnJournalGetPrevious(this, args);
            }
        }

        private void GetCrossingPoint(JObject token, ref CrossingPoint point, CrossingPointName name)
        {
            string position = ((int)name).ToString();
            JObject obj = (JObject)token[position];
            CrossingPointInfo info = Newtonsoft.Json.JsonConvert.DeserializeObject<CrossingPointInfo>(obj.ToString());
            info.Author = string.Empty; //Deleting unnesserary info.
            info.Comment = string.Empty;
            point = new CrossingPoint();
            point.Name = name;
            point.Info = info;
        }


        private readonly string GET_TEN_LAST_JOURNAL_ITEMS = "http://api.nagranitse.ru/journal.json";
        private readonly string GET_CURRENT_STATE = "http://api.nagranitse.ru/data.json";
        private readonly string GET_NEXT_JOURNAL_ITEMS = "http://api.nagranitse.ru/journal.json?offset=";
        private readonly string SEND_INFO = "http://m.nagranitse.ru/info.php?lang=ru";
        private bool _ExeptionThrown = false;
    }

    public enum CrossingPointName
    {
        Torfynovka = 1,
        Brusnichnoe = 2,
        Svetogorsk = 3
    }

    public enum CrossingDirection
    {
        ToRussia = 1,
        FromRussia = 0
    }

    public enum UpdateMode
    {
        Traffic,
        Journal
    }




}
