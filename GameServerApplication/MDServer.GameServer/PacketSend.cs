﻿using System;
using System.Text;


namespace MDServer.GameServer
{
    /// <summary>
    ///                  |--------------------------客户端数据--------|
    ///                  |--------------------------Length-----------|
    ///                  |---2B:Length---|---1B:CodeType---|----1B:Code---|---2B:ReturnCode---|--Data---|
    /// </summary>
    public class PacketSend
    {
        public enum CodeType
        {
            OperationCode,
            EventCode,
        }
        private ArrByte64K _arrByte64K;
        private ushort _i = 6;//第 _i 个字节 是数据
        public ushort I { get { return _i; } }
        private PacketSend()
        { }
        public static PacketSend Create(byte code, CodeType codeType)
        {
            var p = new PacketSend();
            p._arrByte64K = ArrByte64KPool.Instance.Get();
            p._arrByte64K.arrByte64K[2] = (byte)(codeType);//第2位存储 CodeType
            p._arrByte64K.arrByte64K[3] = (byte)(code);//第3位存储 Code
            return p;
        }
        

        public static PacketSend Create(PacketSend pk)
        {
            var p = new PacketSend();
            p._arrByte64K = ArrByte64KPool.Instance.Get();
            Array.Copy(pk._arrByte64K.arrByte64K, p._arrByte64K.arrByte64K, pk._i);
            p._arrByte64K.len = pk._arrByte64K.len;
            p._i = pk._i;
            return p;
        }
        public ArrByte64K CreateArrByte64K()
        {
            _arrByte64K.arrByte64K[0] = (byte)(_i >> 8);
            _arrByte64K.arrByte64K[1] = (byte)(_i);//0，1字节存储_i的大小
            _arrByte64K.len = _i;
            var ar = _arrByte64K;
            _arrByte64K = null;
            return ar;
        }
        public PacketSend SetReturnCode(short returnCode)
        {
            _arrByte64K.arrByte64K[4] = (byte)(returnCode >> 8);//
            _arrByte64K.arrByte64K[5] = (byte)(returnCode);//第4、5位存储returnCode
            return this;
        }
        public PacketSend Write(bool v)
        {
            if (v)
                return Write((byte)1);
            else
                return Write((byte)0);
        }
        public PacketSend Write(byte v)
        {
            if (_i + 1 >= _arrByte64K.arrByte64K.Length)
            {
                Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (v); _i++;
            return this;
        }
        public PacketSend Write(sbyte v) {
            return Write((byte)v);
        }
        public PacketSend Write(byte[] v)
        {
            int length = v.Length;
            if (_i + length + sizeof(ushort) >= _arrByte64K.arrByte64K.Length)
            {
                Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            Write((ushort)length);
            for (int i = _i; i < _i + length; i++)
            {
                _arrByte64K.arrByte64K[i] = v[i - _i];
            }
            _i += (ushort)length;
            return this;
        }
        public PacketSend Write(byte[] v, int length)
        {

            if (_i + length + sizeof(ushort) >= _arrByte64K.arrByte64K.Length)
            {
                Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            Write((ushort)length);
            for (int i = _i; i < _i + length; i++)
            {
                _arrByte64K.arrByte64K[i] = v[i - _i];
            }
            _i += (ushort)length;
            return this;
        }

        public PacketSend Write(ushort v)
        {
            if (_i + 2 >= _arrByte64K.arrByte64K.Length)
            {
                Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 8); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v); _i++;
            return this;
        }
        public PacketSend Write(short v)
        {
            if (_i + 2 >= _arrByte64K.arrByte64K.Length)
            {
                Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 8); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v); _i++;
            return this;
        }
        public PacketSend Write(int v)
        {
            if (_i + 4 >= _arrByte64K.arrByte64K.Length)
            {
                Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 24); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 16); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 8); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v); _i++;
            return this;
        }
        public PacketSend Write(ulong v)
        {
            if (_i + 8 >= _arrByte64K.arrByte64K.Length)
            {
                Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 56); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 48); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 40); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 32); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 24); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 16); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 8); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v); _i++;
            return this;
        }
        public PacketSend Write(long v)
        {
            if (_i + 8 >= _arrByte64K.arrByte64K.Length)
            {
                Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 56); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 48); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 40); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 32); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 24); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 16); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v >> 8); _i++;
            _arrByte64K.arrByte64K[_i] = (byte)(v); _i++;
            return this;
        }

        public PacketSend Write(float v)
        {
            string s = v.ToString("F2");
            return WriteUTF8(s);
        }

        public PacketSend Write(string value)
        {
            return WriteUTF8(value);
        }
        private PacketSend WriteUTF8(string value)
        {
            int length = Encoding.UTF8.GetByteCount(value);
            if (_i + length + 2 >= _arrByte64K.arrByte64K.Length)
            {
                Log(ArrByteReader.GetuShort(_arrByte64K.arrByte64K, 6).ToString() + " over buff");
                return this;
            }
            Write((ushort)length);
            Encoding.UTF8.GetBytes(value, 0, value.Length, _arrByte64K.arrByte64K, (int)_i);
            _i += (ushort)length;
            return this;
        }


        private static void Log(string s)
        {
            Console.WriteLine(s);
        }

        private static void LogFile(string s)
        {
            Console.WriteLine(s);
        }
    }
}
