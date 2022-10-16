using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using Nextwin.Protocol;

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
        protected HashSet<string> _connectedAddressSet = new HashSet<string>();
        protected ISerializer _serializer;

        public NetworkManager(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public NetworkManager(Socket socket, ISerializer serializer)
        {
            _socket = socket;
            _serializer = serializer;
        }

        /// <summary>
        /// 클라이언트에서 서버에 연결
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public virtual void Connect(string ip, int port)
        {
            string address = ToAddress(ip, port);

            try
            {
                if (_connectedAddressSet.Contains(address))
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
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 데이터 전송
        /// </summary>
        /// <param name="data">전송할 데이터</param>
        public virtual void Send<T>(T data)
        {
            try
            {
                byte[] buffer = _serializer.Serialize(data);
                _socket.Send(buffer);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// 수신한 데이터 반환
        /// </summary>
        /// <returns></returns>
        public virtual byte[] Receive()
        {
            byte[] buffer = new byte[1024];
            int size = _socket.Receive(buffer);

            byte[] data = new byte[size];
            Array.Copy(buffer, data, size);

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
