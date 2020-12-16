using Nextwin.Net;
using Nextwin.Protocol;
using Nextwin.Util;
using System.Collections.Concurrent;
using System.Threading;
using UnityEngine;

namespace Nextwin.Game
{
    /// <summary>
    /// 네트워크 스레드 작업자
    /// </summary>
    public class NetworkThreadManager : Singleton<NetworkThreadManager>
    {
        public ConcurrentQueue<DataBundle> ServiceQueue { get; private set; }
        private NetworkManager _networkManager;

        /// <summary>
        /// 네트워크 스레드 생성
        /// </summary>
        /// <param name="networkManager">게임매니저에서 사용하는 NetworkManager</param>
        /// <returns>서버와 통신하는 스레드</returns>
        public Thread CreateNetworkThread(NetworkManager networkManager = null)
        {
            _networkManager = networkManager ?? NetworkManager.Instance;
            ServiceQueue = new ConcurrentQueue<DataBundle>();
            return new Thread(new ThreadStart(CheckReceivingAndEnqueueServices));
        }

        private void CheckReceivingAndEnqueueServices()
        {
            Debug.Log("Network thread created.");

            while(_networkManager.IsConnected)
            {
                Header header = NetworkManager.Instance.Receive();
                byte[] data = NetworkManager.Instance.Receive(header);

                DataBundle dataBundle = new DataBundle(header, data);
                ServiceQueue.Enqueue(dataBundle);

                Thread.Sleep(100);
            }

            Debug.Log("Network thread terminated.");
        }
    }
}
