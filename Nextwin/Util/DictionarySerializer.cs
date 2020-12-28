using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Nextwin.Util
{
    public static class DictionarySerializer
    {
        public static byte[] Serialize(Dictionary<string, object> dictionary)
        {
            //BitConverter.GetBytes
            return null;
        }

        public static Dictionary<string, object> Deserialize(byte[] bytes)
        {
            return null;
        }

        private static byte[] ObjectToBytes(object obj)
        {
            try
            {
                using(MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, obj);
                    byte[] bytes = stream.ToArray();

                    Logger.Log()
                    return bytes;
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            
            return null;
        }

        private static object BytesToObject(byte[] bytes)
        {
            try
            {
                using(MemoryStream stream = new MemoryStream(bytes))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    stream.Position = 0;
                    return binaryFormatter.Deserialize(stream);
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
            }
            return null;
        }
    }
}
