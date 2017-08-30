using System;
using System.Collections.Generic;

namespace MDServer.GameServer
{
    public class ArrByte64K
    {
        public int len { get; set; }
        public byte[] arrByte64K { get; set; } // = new byte[256 * 256];//2**16,ushort上限 65535

        public ArrByte64K()
        {
            arrByte64K = new byte[256 * 256];//2**16,ushort上限 65535
        }
    }

    public  class ArrByte64KPool
    {
        private static ArrByte64KPool _instance;

        public static ArrByte64KPool Instance
        {
            get { if (_instance == null) _instance = new ArrByte64KPool(2000); return _instance; }
        }

        private Queue<ArrByte64K> _queue { get; set; }


        private ArrByte64KPool(int size = 2000)
        {
            _queue = new Queue<ArrByte64K>(size);
            for (int i = 0; i < size; i++)
            {
                _queue.Enqueue(new ArrByte64K());
            }
        }

        public ArrByte64K Get()
        {
            lock (_queue)
            {
                if (_queue.Count == 0)
                {
                    LogFile(string.Format("ArrByte64KPool--------------{0}", _queue.Count.ToString()));
                    return new ArrByte64K();
                }
                else
                {
                    if (_queue.Count == 1)
                    {
                    }
                    var ab = _queue.Dequeue();
                    ab.len = 0;
                    return ab;
                }
            }
        }
        public  void Put(ArrByte64K arrByte64K)
        {
            if (_queue.Count % 1000 == 0 && _queue.Count > 0)
                LogFile(string.Format("App_ArrByte64KPool--------------{0}", _queue.Count.ToString()));
            if (arrByte64K == null)
                throw new Exception();
            lock (_queue)
            {
                _queue.Enqueue(arrByte64K);
            }
        }

        private  void Log(string s)
        {
            Console.WriteLine(s);
        }

        private  void LogFile(string s)
        {
            Console.WriteLine(s);
        }
    }
}
