﻿using System;
using System.Collections.Generic;

using System.Text;

namespace XtraLibrary.SecsGem
{
    public class SecsItemU4 :SecsItem<uint[]>
    {

        public SecsItemU4(string name)
            : base(FormatCode.U4, name, null)
        { 
        }

        public SecsItemU4(string name, params uint[] val)
            : base(FormatCode.U4, name, val)
        {
        }

        protected override byte[] GetDataBytes()
        {

            uint[] vals = this.Value;

            if (vals == null || vals.Length == 0)
            {
                //zero length item
                return null;
            }

            uint byteCount = checked((uint)(sizeof(uint) * vals.Length));
            byte[] data = new byte[byteCount];

            byte[] valByte;
            int index = 0;
            foreach (uint v in vals)
            {
                valByte = BitConverter.GetBytes(v);
                Array.Reverse(valByte);
                Array.Copy(valByte, 0, data, index, valByte.Length);
                index += valByte.Length;
            }

            return data;
        }

        protected override void ReadValue(System.IO.MemoryStream reader, uint byteToRead)
        {
            byte[] data = new byte[byteToRead];
            reader.Read(data, 0, data.Length);

            //warning if devine result has fracment
            int valSizeInByte = sizeof(uint);
            int valCount = (data.Length / valSizeInByte);

            uint[] vals = new uint[valCount];

            int index = 0;
            byte[] temp = new byte[valSizeInByte];
            for (int i = 0; i < valCount; i++)
            {
                Array.Copy(data, index, temp, 0, temp.Length);
                Array.Reverse(temp);

                vals[i] = BitConverter.ToUInt32(temp, 0);
                index += valSizeInByte;
            }

            this.Value = vals;
        }
    }
}
