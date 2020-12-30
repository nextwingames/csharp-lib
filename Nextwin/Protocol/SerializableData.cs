using MessagePack;

namespace Nextwin.Protocol
{
    /// <summary>
    /// 데이터 전송을 위한 최상위 클래스로 SerializableData를 상속받는 모든 클래스의 필드변수의 Key는 1부터 시작
    /// </summary>
    [MessagePackObject]
    public abstract class SerializableData
    {
        [Key(0)]
        public int MsgType { get; set; }

        public SerializableData(int msgType)
        {
            MsgType = msgType;
        }

        public static int ReadMsgTypeFromBytes(byte[] bytes)
        {
            return bytes[1];
        }
    }
}