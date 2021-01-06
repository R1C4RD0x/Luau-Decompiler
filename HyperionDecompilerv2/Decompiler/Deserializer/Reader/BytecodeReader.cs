﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// This reader is a modified version of: https://github.com/katalash/DSLuaDecompiler/blob/e5c2a33616b58ec7b753c9626b6e6dcc8b0fe3ef/Utilities/BinaryReaderEx.cs

namespace HyperionDecompilerv2.Decompiler.Deserializer.Reader
{
    public class BytecodeReader
    {
        private static readonly Encoding ASCII = Encoding.ASCII;

        private BinaryReader Reader;

        /// <summary>
        /// The given stream.
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// The length of the stream.
        /// </summary>
        public long Length => Stream.Length;


        /// <summary>
        /// The current position of the stream.
        /// </summary>
        public long Position {
            get => Stream.Position;
            set => Stream.Position = value;
        }

        /// <summary>
        /// Initializes a new BytecodeReader reading from the specified byte array.
        /// </summary>
        public BytecodeReader(byte[] bytes) : this(new MemoryStream(bytes)) { }

        /// <summary>
        /// Initializes a new BytecodeReader reading from the specified stream.
        /// </summary>
        public BytecodeReader(Stream stream)
        {
            Stream = stream;
            Reader = new BinaryReader(stream);
        }

        /// <summary>
        /// Reads a one-byte boolean value.
        /// </summary>
        public bool ReadBoolean()
        {
            byte boolean = Reader.ReadByte();

            if (boolean == 0 || boolean == 1)
                return Convert.ToBoolean(boolean);
            else
                throw new InvalidDataException($"ReadBoolean encountered non-boolean value: 0x{boolean:X2}");
        }

        /// <summary>
        /// Reads a 3 byte integer.
        /// </summary>
        public int ReadInt32()
        {
            return Reader.ReadInt32();
        }

        /// <summary>
        /// Reads a 3 byte unsigned integer.
        /// </summary>
        public uint ReadUInt32()
        {
            return Reader.ReadUInt32();
        }

        /// <summary>
        /// Reads a compressed 32 bit integer.
        /// </summary>
        public int ReadInt32Compressed()
        {
            int integer = ReadByte();

            int v = integer | 0x80;
            integer <<= 7;

            return (integer != 0) ? v &= 0x7f : v;
        }

        /// <summary>
        /// Reads a one-byte unsigned integer.
        /// </summary>
        public byte ReadByte()
        {
            return Reader.ReadByte();
        }

        /// <summary>
        /// Reads an array of one-byte unsigned integers from the specified offset without advancing the stream.
        /// </summary>
        public byte[] ReadBytes(int count)
        {
            byte[] result = Reader.ReadBytes(count);

            if (result.Length != count)
                throw new EndOfStreamException("Remaining size of stream was smaller than requested number of bytes.");
            return result;
        }

        /// <summary>
        /// Reads a two-byte signed integer.
        /// </summary>
        public short ReadInt16()
        {
            return Reader.ReadInt16();
        }

        /// <summary>
        /// Reads a two-byte unsigned integer.
        /// </summary>
        public ushort ReadUInt16()
        {
            return Reader.ReadUInt16();
        }

        /// <summary>
        /// Reads a 64 bit integer.
        /// </summary>
        public double ReadDouble()
        {
            return Reader.ReadDouble();
        }

        /// <summary>
        /// Reads a 64 bit unsigned integer.
        /// </summary>
        public double ReadU64()
        {
            return Reader.ReadUInt64();
        }

        /// <summary>
        /// Reads an ASCII string with the specified length in bytes.
        /// </summary>
        public string ReadASCII(int length)
        {
            byte[] bytes = ReadBytes(length);
            return ASCII.GetString(bytes);
        }
    }
}
