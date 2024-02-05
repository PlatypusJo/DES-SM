using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3DP
{
    public static class BitsListExtensions
    {
        public static List<bool> ToBitsList(this BitArray bitArr)
        {
            bool[] bits = new bool[bitArr.Length];
            bitArr.CopyTo(bits, 0);
            return bits.ToList();
        }

        public static byte FromBitsToByte(this List<bool> bits)
        {
            if (bits.Count > 8)
                throw new ArgumentException("Кол-во битов превышает размер байта");

            byte mask = (byte)(1 << bits.Count - 1);
            byte number = 0;
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i])
                    number |= mask;
                mask >>= 1;
            }

            return number;
        }

        public static List<bool> LeftShift(this List<bool> bits, int count)
        {
            BitArray bitArr = new BitArray(bits.ToArray());
            bitArr.LeftShift(count);
            return bitArr.ToBitsList();
        }
        
        public static List<bool> RightShift(this List<bool> bits, int count)
        {
            BitArray bitArr = new BitArray(bits.ToArray());
            bitArr.RightShift(count);
            return bitArr.ToBitsList();
        }

        public static List<bool> Or(this List<bool> bits, List<bool> value)
        {
            BitArray bitArr1 = new BitArray(bits.ToArray());
            BitArray bitArr2 = new BitArray(value.ToArray());
            bitArr1.Or(bitArr2);
            return bitArr1.ToBitsList();
        }
        
        public static List<bool> And(this List<bool> bits, List<bool> value)
        {
            BitArray bitArr1 = new BitArray(bits.ToArray());
            BitArray bitArr2 = new BitArray(value.ToArray());
            bitArr1.And(bitArr2);
            return bitArr1.ToBitsList();
        }

        public static List<bool> Xor(this List<bool> bits, List<bool> value)
        {
            BitArray bitArr1 = new BitArray(bits.ToArray());
            BitArray bitArr2 = new BitArray(value.ToArray());
            bitArr1.Xor(bitArr2);
            return bitArr1.ToBitsList();
        }
    }
}
