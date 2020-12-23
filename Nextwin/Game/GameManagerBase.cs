using Nextwin.Net;
using Nextwin.Protocol;
using Nextwin.Util;
using System.Threading;

namespace Nextwin.Game
{
    /// <summary>
    /// NetworkManager를 사용하여 서버와 통신하기 위한 GameManager의 상위 클래스
    /// </summary>
    public abstract class GameManagerBase : Singleton<GameManagerBase>
    {
        protected NetworkManager _networkManager;
        private Thread _networkThread;

        protected virtual void Start()
        {
            SetNetworkManager();
        }

        /// <summary>
        /// 적절한 NetworkManager 세팅
        /// </summary>
        protected virtual void SetNetworkManager()
        {
            _networkManager = NetworkManager.Instance;
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

        private void CreateNetworkThread()
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

            if(!NetworkThreadManager.Instance.ServiceQueue.TryDequeue(out DataBundle dataBundle))
            {
                return;
            }

            Header header = dataBundle.Header;
            byte[] data = dataBundle.Data;

            Service.Service service = null;
            IDto dto = null;
            CreateService(header.MsgType, data, service, dto);

            if(service == null)
            {
                return;
            }
            service.Execute();
        }

        /// <summary>
        /// msgType에 따라 service 객체와 dto 객체를 생성
        /// </summary>
        /// <param name="msgType">Protocol에 정의되어 있는 메시지 타입</param>
        /// <param name="data">JsonManager를 사용하여 DTO로 변환하기 위한 데이터</param>
        /// <param name="service">실질적인 작업을 수행할 서비스 객체</param>
        /// <param name="dto">data이 JsonManager를 통해 변환된 결과를 받는 DTO</param>
        protected abstract void CreateService(int msgType, byte[] data, Service.Service service, IDto dto);
    }
}
