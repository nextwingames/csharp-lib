using Nextwin.Client.Util;
using Nextwin.Net;
using Nextwin.Protocol;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Nextwin.Client.Game
{
    [RequireComponent(typeof(NetworkThreadManager))]
    /// <summary>
    /// NetworkManager를 사용하여 서버와 통신하기 위한 GameManager의 상위 클래스
    /// </summary>
    public abstract class GameManagerBase : Singleton<GameManagerBase>
    {
        protected NetworkManager _networkManager;
        protected Thread _networkThread;

        [SerializeField]
        protected string _ip = "127.0.0.1";
        [SerializeField]
        protected int _port;

        [SerializeField]

        protected virtual void Start()
        {
            _networkManager = CreateNetworkManager();
            _networkManager.Connect(_ip, _port);
        }

        /// <summary>
        /// Network Manager 생성
        /// </summary>
        /// <returns></returns>
        protected virtual NetworkManager CreateNetworkManager()
        {
            return new NetworkManager();
        }

        protected virtual void Update()
        {
            if(!_networkManager.IsConnected)
            {
                return;
            }

            CreateNetworkThread();
            CheckServiceQueue();
        }

        protected virtual void CreateNetworkThread()
        {
            if(_networkThread != null)
            {
                return;
            }

            _networkThread = NetworkThreadManager.Instance.CreateNetworkThread(_networkManager);
            _networkThread.Start();
            _networkThread.IsBackground = true;
        }

        private void CheckServiceQueue()
        {
            if(NetworkThreadManager.Instance.ServiceQueue.Count == 0)
            {
                return;
            }

            if(!NetworkThreadManager.Instance.ServiceQueue.TryDequeue(out Dictionary<string, object> receivedData))
            {
                return;
            }
            
            OnReceivedData(receivedData, out Service service);

            if(service == null)
            {
                return;
            }
            service.Execute();
        }

        /// <summary>
        /// 수신한 Dictionary가 가지고 있는 Key 값에 따라 적절한 service 객체와 dto 객체 생성
        /// </summary>
        /// <param name="receivedData">수신한 데이터</param>
        /// <param name="service">실질적인 작업을 수행할 서비스 객체</param>
        protected abstract void OnReceivedData(Dictionary<string, object> receivedData, out Service service);
    }
}
