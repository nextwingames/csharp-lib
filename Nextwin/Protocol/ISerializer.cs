namespace Nextwin.Protocol
{
    public interface ISerializer
    {
        byte[] Serialize(SerializableData data);

        T Deserialize<T>(byte[] bytes);
    }
}
