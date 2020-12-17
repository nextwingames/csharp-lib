using System;
using UnityEngine;

namespace Nextwin.Util
{
    public static class EnumConverter
    {
        public static T ToEnum<T>(string value)
        {
            if(!Enum.IsDefined(typeof(T), value))
            {
                Debug.LogError($"{value} is not defined in enum {typeof(T)}.");
                return default;
            }

            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
