using System;
using System.Collections;

namespace Lab3DP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DES dES = new DES(string.Empty);
            byte[] bit64 = [112, 102, 11, 10, 44, 5, 231, 199];
            List<bool> bitVector = new BitArray(bit64).ToBitsList();
            PrintBitVector(bitVector);

            Console.WriteLine("\nЗашифровали DES");
            List<bool> newBitVector = dES.ECB(bitVector);
            PrintBitVector(newBitVector);

            Console.WriteLine("\nРасшифруем DES");
            bitVector.Clear();
            bitVector = dES.ECB(newBitVector, false);
            PrintBitVector(bitVector);

        }

        static void PrintBitVector(List<bool> bitVector)
        {
            for (int i = bitVector.Count - 1; i >= 0; i--) 
            {
                if (bitVector[i])
                    Console.Write("1");
                else 
                    Console.Write("0");
            }
        }
    }
}