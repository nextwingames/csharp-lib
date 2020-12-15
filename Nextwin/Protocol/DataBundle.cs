namespace Nextwin
{
    namespace Protocol
    {
        public class DataBundle
        {
            public Header Header { get; }
            public byte[] Data { get; }

            public DataBundle(Header header, byte[] data)
            {
                Header = header;
                Data = data;
            }
        }
    }
}