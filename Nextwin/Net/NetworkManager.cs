using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Nextwin.Net
{
    public class NetworkManager
    {
        public bool IsConnected
        {
            get
            {
                try
                {
                    return _socket.Connected;
                }
                catch(Exception)
                {
                    return false;
                }
            }
        }

        protected Socket _socket;

        protected MemoryStream _stream = new MemoryStream();
        protected BinaryFormatter _formatter = new BinaryFormatter();

        protected HashSet<string> _connectedAddressSet = new HashSet<string>();

        public NetworkManager() { }

        public NetworkManager(Socket socket)
        {
            _socket = socket;
        }

        /// <summary>
        /// 클라이언트에서 서버에 연결
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public virtual void Connect(string ip, int port)
        {
            string address = ToAddress(ip, port);

            if(_connectedAddressSet.Contains(address))
            {
                Debug.LogError(string.Format("Already connected to {0}", address));
                return;
            }

            IPAddress ipAddress = IPAddress.Parse(ip);
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

            _socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(remoteEP);
            _connectedAddressSet.Add(address);

            Debug.Log("Socket connected to " + _socket.RemoteEndPoint.ToString());
        }

        /// <summary>
        /// 데이터 전송
        /// </summary>
        /// <param name="dictionary">전송할 데이터를 담은 Dictionary</param>
        public virtual void Send(Dictionary<string, object> dictionary)
        {
            _formatter.Serialize(_stream, dictionary);
            byte[] bytes = _stream.ToArray();

            _socket.Send(bytes);
        }

        /// <summary>
        /// 수신한 데이터를 Dictionary로 반환
        /// </summary>
        /// <returns></returns>
        public virtual Dictionary<string, object> Receive()
        {
            byte[] bytes = new byte[1024];
            _socket.Receive(bytes);

            Dictionary<string, object> data = (Dictionary<string, object>)_formatter.Deserialize(_stream);
            return data;
        }

        /// <summary>
        /// 소켓 연결 해제
        /// </summary>
        public virtual void Disconnect()
        {
            if(_socket.Connected)
            {
                _socket.Close();
            }
        }

        private string ToAddress(string ip, int port)
        {
            return string.Format("{0}:{1}", ip, port);
        }
    }
}
