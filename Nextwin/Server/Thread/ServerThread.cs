using Nextwin.Net;
using Nextwin.Protocol;
using System;
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
                    byte[] receivedData = _networkManager.Receive();
                    OnReceivedData(SerializableData.ReadMsgTypeFromBytes(receivedData), receivedData);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                _networkManager.Disconnect();
                OnExitServer();
            }
        }

        /// <summary>
        /// 클라이언트로부터 데이터를 수신했을 때 호출됨
        /// </summary>
        /// <param name="msgType">받은 데이터의 메시지 타입</param>
        /// <param name="receivedData">직렬화된 수신 데이터</param>
        protected abstract void OnReceivedData(int msgType, byte[] receivedData);

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
