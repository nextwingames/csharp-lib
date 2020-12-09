using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Nextwin.Protocol;
using Nextwin.Util;
using UnityEngine;

namespace Nextwin
{
    namespace Net
    {
        public class NetworkManager
        {
            private static NetworkManager _instance;
            public static NetworkManager Instance
            {
                get
                {
                    if(_instance == null)
                        _instance = new NetworkManager();
                    return _instance;
                }
            }

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

            private Socket _socket;
            private HashSet<string> _connectedAddressSet = new HashSet<string>();

            private NetworkManager() { }

            /// <summary>
            /// 서버에 연결
            /// </summary>
            public void Connect(string ip, int port)
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
            /// 구조체 전송
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="msgType"></param>
            /// <param name="obj"></param>            
            public void Send<T>(int msgType, T obj)
            {
                byte[] data = JsonManager.ObjectToBytes(obj);

                Header header = new Header(msgType, data.Length);
                byte[] head = JsonManager.ObjectToBytes(header);

                // 최종 전송할 패킷(헤더 + 데이터) 바이트화
                byte[] packet = new byte[head.Length + data.Length];
                Buffer.BlockCopy(head, 0, packet, 0, head.Length);
                Buffer.BlockCopy(data, 0, packet, head.Length, data.Length);

                // _socket.BeginSend(packet, 0, packet.Length, 0, new AsyncCallback(SendCallback), _socket);
                _socket.Send(packet, packet.Length, SocketFlags.None);
            }

            /// <summary>
            /// 헤더 수신
            /// </summary>
            /// <returns></returns>
            public Header Receive()
            {
                byte[] head = new byte[Header.HeaderLength];
                _socket.Receive(head, Header.HeaderLength, SocketFlags.None);

                Header header = JsonManager.BytesToObject<Header>(head);
                return header;
            }

            /// <summary>
            /// 데이터 수신
            /// </summary>
            /// <param name="header"></param>
            /// <returns></returns>
            public byte[] Receive(Header header)
            {
                int length = header.Length;
                byte[] data = new byte[length];
                _socket.Receive(data, length, SocketFlags.None);
                return data;
            }

            /// <summary>
            /// 연결 해제
            /// </summary>
            public void Disconnect()
            {
                _socket.Close();
            }

            private string ToAddress(string ip, int port)
            {
                return string.Format("{0}:{1}", ip, port);
            }
        }
    }
}
