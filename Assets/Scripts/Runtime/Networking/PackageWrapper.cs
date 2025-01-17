using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace TeamShrimp.GGJ23.Networking
{

    public class PackageWrapper
    {
        public PackageWrapper(int size, byte packetType)
        {
            this.Setup(size, packetType);
        }
        
        public void Setup(int size, byte packetType)
        {
            size++;
            this.m_buffer = new byte[size];
            this.m_stream = new MemoryStream(this.m_buffer);
            this.m_writer = new BinaryWriter(this.m_stream);
            this.Writer.Write(packetType);
        }

        public void Dispose()
        {
            this.m_stream.Dispose();
            this.m_writer.Dispose();
        }

        public BinaryWriter Writer
        {
            get
            {
                return this.m_writer;
            }
        }

        public byte[] Buffer
        {
            get
            {
                return this.m_buffer;
            }
        }

        public void Write(byte b)
        {
            this.Writer.Write(b);
        }

        public void Write(string s)
        {
            if (s.Length > 255)
            {
                Debug.LogError("STRINGS LONGER THAN 255 CHARACTERS ARE NOT SUPPORTED, OFFENDING STRING: " + s);
            }
            this.Writer.Write((byte)s.Length);
            var nameBytes = Encoding.ASCII.GetBytes(s);
            this.Writer.Write(nameBytes);
        }

        public void Write(long l)
        {
            this.Writer.Write(l.Serialize());
        }
        
        public void Write(Vector2Int vector2Int)
        {
            this.Writer.Write(vector2Int.Serialize());
        }
     
        public void Write(byte[] bytes)
        {
            this.Writer.Write(bytes);
        }

        private byte[] m_buffer;

        private BinaryWriter m_writer;

        private BinaryReader m_reader;

        private MemoryStream m_stream;
    }
}
