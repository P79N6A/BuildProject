using UnityEngine;

namespace Sakura
{
    public class AbstractApp:MonoDispatcher
    {
        public static GameObject PoolContainer { get; protected set; }
        public static GameObject SoundContainer { get; protected set; }

        public static CoreLoaderQueue coreLoaderQueue { get; protected set; }
    }
}