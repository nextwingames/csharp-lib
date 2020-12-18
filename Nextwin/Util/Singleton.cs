using UnityEngine;

namespace Nextwin
{
    namespace Util
    {
        public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
        {
            private static T _instance;
            public static T Instance
            {
                get
                {
                    if(_instance == null)
                    {
                        var obj = FindObjectOfType<T>();
                        if(obj != null)
                        {
                            _instance = obj;
                        }
                        //else
                        //{
                        //    var newsingletone = new GameObject().AddComponent<T>();
                        //    _instance = newsingletone;
                        //}
                    }
                    return _instance;
                }
                private set
                {
                    _instance = value;
                }
            }

            [SerializeField]
            protected bool _dontDestroyOnLoad;

            protected virtual void Awake()
            {
                var objs = FindObjectsOfType<T>();
                if(objs.Length != 1)
                {
                    Destroy(gameObject);
                    return;
                }

                if(_dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
        }
    }
}
