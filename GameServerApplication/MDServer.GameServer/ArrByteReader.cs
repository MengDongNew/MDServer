using System;
using System.Text;

namespace MDServer.GameServer
{
    /// <summary>
    ///                  |--------------------------客户端数据--------|
    ///                  |--------------------------Length-----------|
    ///|----4B:ConnId----|---2B:Length---|---1B:OperationCode---|--Data---|
    public class ArrByteReader
    {
        private Encoding m_UTF8;
        public Encoding UTF8 {
            get { if(m_UTF8==null)m_UTF8 = new UTF8Encoding(false,false);
                return m_UTF8;
            }
        }

        private ArrByte64K _arrByte;
        private int _readLen;
        public int ReadLen {
            get { return _readLen; }
        }

        public void SetArrByte(ArrByte64K arrByte)
        {
            _arrByte = arrByte;
            _readLen = 0;
        }
        public sbyte ReadSByte() {
            return (sbyte)ReadByte();
        }
        public byte ReadByte()
        {
            _readLen += 1;
            if (_readLen > _arrByte.len)
                return 0;
            return _arrByte.arrByte64K[_readLen - 1];
        }
        public bool ReadBool() {
            return ReadByte() == 1;
        }
        public byte[] ReadBytes()
        {
            //byte[] bytes = null;
            int len = ReaduShort();
            _readLen += len;

            if (_readLen > _arrByte.len) return null;

            byte[] bytes = new byte[len];

            for (int i = 0; i < len; i++)
            {
                bytes[i] = _arrByte.arrByte64K[_readLen - len + i];
            }

            return bytes;
        }
        public void ReadBytes(out byte[] bytes, out int len)
        {
            bytes = null;
            len = ReaduShort();
            _readLen += len;

            if (_readLen > _arrByte.len) return;

            bytes = new byte[len];

            for (int i = 0; i < len; i++)
            {
                bytes[i] = _arrByte.arrByte64K[_readLen - len + i];
            }
        }
        public short ReadShort()
        {
            return (short)ReaduShort();
        }
        public ushort ReaduShort()
        {
            _readLen += 2;
            if (_readLen > _arrByte.len)
                return 0;
            return GetuShort(_arrByte.arrByte64K, _readLen - 2);
        }
        public int ReadInt()
        {
            _readLen += 4;
            if (_readLen > _arrByte.len)
                return 0;
            return GetInt(_arrByte.arrByte64K, _readLen - 4);
        }
        public uint ReadUint() {
            return (uint)ReadInt();
        }
        public ulong ReaduLong()
        {
            _readLen += 8;
            if (_readLen > _arrByte.len)
                return 0;

            return (((ulong)_arrByte.arrByte64K[_readLen - 8]) << 56)
                   | (((ulong)_arrByte.arrByte64K[_readLen - 7]) << 48)
                   | (((ulong)_arrByte.arrByte64K[_readLen - 6]) << 40)
                   | (((ulong)_arrByte.arrByte64K[_readLen - 5]) << 32)
                   | (((ulong)_arrByte.arrByte64K[_readLen - 4]) << 24)
                   | (((ulong)_arrByte.arrByte64K[_readLen - 3]) << 16)
                   | (((ulong)_arrByte.arrByte64K[_readLen - 2]) << 8)
                   | (((ulong)_arrByte.arrByte64K[_readLen - 1]))
                ;
        }
        public long ReadLong()
        {
            _readLen += 8;
            if (_readLen > _arrByte.len)
                return 0;

            return (((long)_arrByte.arrByte64K[_readLen - 8]) << 56)
                   | (((long)_arrByte.arrByte64K[_readLen - 7]) << 48)
                   | (((long)_arrByte.arrByte64K[_readLen - 6]) << 40)
                   | (((long)_arrByte.arrByte64K[_readLen - 5]) << 32)
                   | (((long)_arrByte.arrByte64K[_readLen - 4]) << 24)
                   | (((long)_arrByte.arrByte64K[_readLen - 3]) << 16)
                   | (((long)_arrByte.arrByte64K[_readLen - 2]) << 8)
                   | (((long)_arrByte.arrByte64K[_readLen - 1]))
                ;
        }

        public float ReadFloat()
        {
            string str = ReadUTF8String();
            return float.Parse(str);
        }

        public string ReadUTF8String(bool safeCheck = true)
        {
            ushort l = ReaduShort();
            return ReadUTF8StringSafe(l, safeCheck);
        }
        private bool IsSafeChar(int c)
        {
            return (c >= 0x20 && c < 0xFFFE);
        }
        private string ReadUTF8StringSafe(int fixedLength, bool safeCheck = true)
        {
            if (_readLen + fixedLength > _arrByte.len)
            {
                _readLen += fixedLength;
                return String.Empty;
            }

            int bound = _readLen + fixedLength;

            int count = 0;
            int index = _readLen;
            int start = _readLen;

            while (index < bound && _arrByte.arrByte64K[index++] != 0)
                ++count;

            index = 0;

            byte[] buffer = new byte[count];
            int value = 0;

            while (_readLen < bound && (value = _arrByte.arrByte64K[_readLen++]) != 0)
                buffer[index++] = (byte)value;

            string s = UTF8.GetString(buffer);

            bool isSafe = true;

            for (int i = 0; isSafe && i < s.Length; ++i)
                isSafe = IsSafeChar((int)s[i]);

            _readLen = start + fixedLength;

            if (isSafe || !safeCheck)
                return s;

            StringBuilder sb = new StringBuilder(s.Length);

            for (int i = 0; i < s.Length; ++i)
                if (IsSafeChar((int)s[i]))
                    sb.Append(s[i]);

            return sb.ToString();
        }

        public static ushort GetuShort(byte[] arrByte, int index)
        {
            return (ushort)(((ushort)(arrByte[index]) << 8)
                            | ((ushort)(arrByte[index + 1])));
        }
        public static int GetInt(byte[] arrByte, int index)
        {
            return (((int)arrByte[index]) << 24)
                   | (((int)arrByte[index + 1]) << 16)
                   | (((int)arrByte[index + 2]) << 8)
                   | (((int)arrByte[index + 3]));
        }
    }
}
