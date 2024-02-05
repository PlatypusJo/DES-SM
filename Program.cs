using System;
using System.Collections;

namespace Lab3DP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string plain = "plain.txt";
            string key = "key.txt";
            string syncMsg = "syncMsg.txt";
            Console.WriteLine("Шифруем с помощью DES");
            DES dES = new DES(key);
            dES.EncryptDecrypt(plain, "encryption.txt", syncMsg);
            Console.WriteLine("Расшифровываем с помощью DES");
            dES.EncryptDecrypt("encryption.txt", "decryption.txt", syncMsg);
            Console.WriteLine("Конец");
        }
    }
}