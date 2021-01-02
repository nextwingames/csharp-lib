using Nextwin.Protocol;
using System.Text;
using UnityEngine;

namespace Nextwin.Util
{
    public class JsonManager
    {
        public static bool PrintLog { get; set; }

        /// <summary>
        /// 구조체를 바이트 배열로 변환
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(object obj)
        {
            byte[] bytes = JsonToBytes(ObjectToJson(obj));
            if(PrintLog)
            {
                Print.Log(string.Format("converted byte array from object: {0}", BytesToStringFormat(bytes)));
            }
            return bytes;
        }

        /// <summary>
        /// 헤더를 바이트 배열로 변환
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public static byte[] ObjectToBytes(Header header)
        {
            string json = ObjectToJson(header);
            SetHeaderLength(header, ref json);
            return JsonToBytes(json);
        }

        /// <summary>
        /// 바이트 배열을 구조체로 변환
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T BytesToObject<T>(byte[] bytes)
        {
            if(PrintLog)
            {
                Print.Log(string.Format("byte array for converting to object: {0}", BytesToStringFormat(bytes)));
            }
            return JsonToObject<T>(BytesToJson(bytes));
        }

        private static string ObjectToJson(object obj)
        {
            string json = JsonUtility.ToJson(obj);
            if(PrintLog)
            {
                Print.Log(string.Format("converted JSON from object: {0}", json));
            }
            return json;
        }

        private static T JsonToObject<T>(string json)
        {
            if(PrintLog)
            {
                Print.Log(string.Format("JSON for converting to object: {0}", json));
            }
            return JsonUtility.FromJson<T>(json);
        }

        private static byte[] JsonToBytes(string json)
        {
            string jsonCamel = RenameToCamelCase(json);
            byte[] bytes = Encoding.UTF8.GetBytes(jsonCamel);
            return bytes;
        }

        private static string BytesToJson(byte[] bytes)
        {
            string json = Encoding.Default.GetString(bytes);
            if(PrintLog)
            {
                Print.Log($"JSON from received data: {json}");
            }
            string jsonPascal = RenameToPascalCase(json);
            return jsonPascal;
        }

        /// <summary>
        /// 헤더의 json 길이를 30으로 맞춤, MsgType은 100보다 작은 수여야 함
        /// </summary>
        /// <param name="header"></param>
        /// <param name="jsonHeader"></param>
        public static void SetHeaderLength(Header header, ref string jsonHeader)
        {
            if(header.MsgType < 10)
                jsonHeader += ' ';

            int length = 100000;
            while(length >= 10)
            {
                if(header.Length < length)
                    jsonHeader += ' ';
                length /= 10;
            }
        }

        /// <summary>
        /// C# 명명 규칙 -> JAVA 명명 규칙
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static string RenameToCamelCase(string json)
        {
            string camelJson = "{";

            int length = json.Length;

            for(int i = 1; i < length; i++)
            {
                char c = json[i];

                camelJson += c;

                if(json[i] == '\"')
                {
                    if(json[i - 1] == '{' || json[i - 1] == ',')
                    {
                        // 필드 변수 첫 글자를 소문자로
                        camelJson += char.ToLower(json[i + 1]);
                        i++;
                    }
                }
            }
            return camelJson;
        }

        /// <summary>
        /// JAVA 명명 규칙 -> C# 명명 규칙
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static string RenameToPascalCase(string json)
        {
            string pascalJson = "";

            int length = json.Length;

            for(int i = 0; i < length; i++)
            {
                char c = json[i];

                if(c == ' ')
                    continue;

                pascalJson += c;

                if(json[i] == '\"')
                {
                    if(json[i + 1] != ':' && json[i - 1] != ':' && json[i - 1] != '[')
                    {
                        // 필드 변수 첫 글자를 대문자로
                        pascalJson += char.ToUpper(json[i + 1]);
                        i++;
                    }
                }
            }

            return pascalJson;
        }

        private static object BytesToStringFormat(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();

            foreach(byte e in bytes)
            {
                sb.Append(string.Format(" {0}", e));
            }

            return sb.ToString();
        }
    }
}
