using MessagePack;
using System.Collections.Generic;

namespace Nextwin.Protocol
{
    [MessagePackObject]
    public class SerializableData<TKey>
    {
        [Key(0)]
        public Dictionary<TKey, object> DataMap { get; set; }

        public SerializableData()
        {
            DataMap = new Dictionary<TKey, object>();
        }

        public void Add(TKey key, object value)
        {
            DataMap.Add(key, value);
        }
    }
}