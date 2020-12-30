using System.Text;

namespace Nextwin.Util
{
    public static class ArrayStringConverter
    {
        public static string BytesToString(byte[] bytes)
        {
            return ConvertToString(bytes);
        }

        public static string IntsToString(int[] ints)
        {
            return ConvertToString(ints);
        }

        public static string FloatsToString(float[] floats)
        {
            return ConvertToString(floats);
        }

        private static string ConvertToString<T>(T[] objects)
        {
            StringBuilder sb = new StringBuilder("{ ");
            foreach(var b in objects)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");

            return sb.ToString();
        }
    }
}
