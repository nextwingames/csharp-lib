using System;
using System.Collections.Generic;
using System.Text;

namespace Nextwin.Util
{
    public static class ArrayStringConverter
    {
        public static string BytesToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder("{ ");
            foreach(var b in bytes)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");

            return sb.ToString();
        }

        public static string IntsToString(int[] ints)
        {
            StringBuilder sb = new StringBuilder("{ ");
            foreach(var b in ints)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");

            return sb.ToString();
        }

        public static string FloatsToString(float[] floats)
        {
            StringBuilder sb = new StringBuilder("{ ");
            foreach(var b in floats)
            {
                sb.Append(b + ", ");
            }
            sb.Append("}");

            return sb.ToString();
        }
    }
}
