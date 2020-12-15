namespace Nextwin
{
    namespace Protocol
    {
        /// <summary>
        /// 헤더 번호, 데이터 길이
        /// </summary>
        public class Header
        {
            public const int HeaderLength = 30;

            public int MsgType;
            public int Length;

            public Header(int msgType, int length)
            {
                MsgType = msgType;
                Length = length;
            }
        }
    }
}