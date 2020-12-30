using MessagePack;
using MessagePack.Resolvers;
using UnityEngine;

namespace Nextwin.Client.Game
{
    public class MessagePackRegisterer
    {
        private static bool _isSerializerRegistered = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Func();

            if(!_isSerializerRegistered)
            {
                StaticCompositeResolver.Instance.Register(
                     GeneratedResolver.Instance,
                     StandardResolver.Instance
                );

                var option = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);

                MessagePackSerializer.DefaultOptions = option;
                _isSerializerRegistered = true;
            }
        }

        private static void Func()
        {
            Debug.Log("FUNC");
        }
    }
}
