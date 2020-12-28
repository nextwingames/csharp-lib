using Nextwin.Server.Thread;
using System;
using System.Net;
using System.Net.Sockets;

namespace Nextwin.Server
{
    public abstract class Server
    {
        public void Listen()
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                serverSocket.Bind(new IPEndPoint(IPAddress.Any, GetPort()));
                serverSocket.Listen(GetBacklog());

                while(true)
                {
                    Socket socket = serverSocket.Accept();
                    ServerThread serverThread = CreateServerThread(socket);
                    serverThread.Start();
                }
            }
            catch(Exception e)
            {
                Console.Write(e.ToString());
            }
            finally
            {
                if(serverSocket != null)
                {
                    serverSocket.Close();
                }
            }
        }

        /// <summary>
        /// 포트 번호
        /// </summary>
        /// <returns></returns>
        protected abstract int GetPort();

        /// <summary>
        /// 연결을 기다리는 클라이언트의 최대 수
        /// </summary>
        /// <returns></returns>
        protected abstract int GetBacklog();

        /// <summary>
        /// 클라이언트별로 서버 스레드 생성
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        protected abstract ServerThread CreateServerThread(Socket socket);
    }
}
