using Nextwin.Net;
using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Nextwin.Server.Thread
{
    public abstract class ServerThread
    {
        protected Socket _socket;
        protected NetworkManager _networkManager;

        public ServerThread(Socket socket)
        {
            _socket = socket;
            _networkManager = new NetworkManager(socket);
        }

        public void Start()
        {
            Work();

            if(_socket != null)
            {
                _socket.Close();
            }
        }

        private void Work()
        {
            OnEnterServer();

            try
            {
                while(_networkManager.IsConnected)
                {
                    Dictionary<string, object> receivedData = _networkManager.Receive();
                    OnReceivedData(receivedData);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                OnExitServer();
                _networkManager.Disconnect();
            }
        }

        /// <summary>
        /// 클라이언트로부터 데이터를 수신할 때 호출됨
        /// </summary>
        /// <param name="receivedData"></param>
        protected abstract void OnReceivedData(Dictionary<string, object> receivedData);

        /// <summary>
        /// 클라이언트가 서버에 접속할 때 호출됨
        /// </summary>
        protected abstract void OnEnterServer();

        /// <summary>
        /// 클라이언트의 접속이 끊길 때 호출됨
        /// </summary>
        protected abstract void OnExitServer();
    }
}
