using System.Collections.Generic;

namespace Sakura
{
    public class CoreLoaderQueue:MonoDispatcher
    {
        public static bool DEBUG = false;
        public static int MAX = 300;

        public static int CONCURRENCE = 4;

        protected Queue<RFLoader> queue = new Queue<RFLoader>();
        protected List<RFLoader> runingList=new List<RFLoader>();

        private int runing = 0;
    }
}