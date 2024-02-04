using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace Lab3DP
{
    public class DES
    {
        private int[] _iPTable = new int[64];
        private int[] _iPReverseTable = new int[64];
        private byte[] _key = [2, 3, 4, 5, 6, 7, 8, 11];
        // ключи раундов
        List<bool>[] _roundKeys = new List<bool>[16];

        // перестановка-выбор 1
        private int[] _pC1 = [57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18, 10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36, 63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22, 14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4];
        // перестановка-выбор 1
        private int[] _pC2 = [14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32];
        // число сдвигов ls i-ое
        private int[] _lSTable = [1, 1, 2, 2, 2, 2, 2, 2, 1, 2, 2, 2, 2, 2, 2, 1];
        // схема расширения 32 в 48
        private int[] _extensionScheme = [32, 1, 2, 3, 4, 5, 4, 5, 6, 7, 8, 9, 8, 9, 10, 11, 12, 13, 12, 13, 14, 15, 16, 17, 16, 17, 18, 19, 20, 21, 20, 21, 22, 23, 24, 25, 24, 25, 26, 27, 28, 29, 28, 29, 30, 31, 32, 1];
        // результирующая перестановка p образующей функции
        private int[] _pSwapTable = [16, 7, 20, 21, 29, 12, 28, 17, 1, 15, 23, 26, 5, 18, 31, 10, 2, 8, 24, 14, 32, 27, 3, 9, 19, 13, 30, 6, 22, 11, 4, 25];

        #region S-blocks

        private int[,,] _sBlocksTables =
        {
            {// s1
                { 14, 04, 13, 01, 02, 15, 11, 08, 03, 10, 06, 12, 05, 09, 00, 07 },
                { 00, 15, 07, 04, 14, 02, 13, 01, 10, 06, 12, 11, 09, 05, 03, 08 },
                { 04, 01, 14, 08, 13, 06, 02, 11, 15, 12, 09, 07, 03, 10, 05, 00 },
                { 15, 12, 08, 02, 04, 09, 01, 07, 05, 11, 03, 14, 10, 00, 06, 13 }
            },
            {// s2
                { 15, 01, 08, 14, 06, 11, 03, 04, 09, 07, 02, 13, 12, 00, 05, 10 },
                { 03, 13, 04, 07, 15, 02, 08, 14, 12, 00, 01, 10, 06, 09, 11, 05 },
                { 00, 14, 07, 11, 10, 04, 13, 01, 05, 08, 12, 06, 09, 03, 02, 15 },
                { 13, 08, 10, 01, 03, 15, 04, 02, 11, 06, 07, 12, 00, 05, 14, 09 }
            },
            {// s3
                { 10, 00, 09, 14, 06, 03, 15, 05, 01, 13, 12, 07, 11, 04, 02, 08 },
                { 13, 07, 00, 09, 03, 04, 06, 10, 02, 08, 05, 14, 12, 11, 15, 01 },
                { 13, 06, 04, 09, 08, 15, 03, 00, 11, 01, 02, 12, 05, 10, 14, 07 },
                { 01, 10, 13, 00, 06, 09, 08, 07, 04, 15, 14, 03, 11, 05, 02, 12 }

            },
            {// s4
                { 07, 13, 14, 03, 00, 06, 09, 10, 01, 02, 08, 05, 11, 12, 04, 15 },
                { 13, 08, 11, 05, 06, 15, 00, 03, 04, 07, 02, 12, 01, 10, 14, 09 },
                { 10, 06, 09, 00, 12, 11, 07, 13, 15, 01, 03, 14, 05, 02, 08, 04 },
                { 03, 15, 00, 06, 10, 01, 13, 08, 09, 04, 05, 11, 12, 07, 02, 14 }
            },
            {// s5
                { 02, 12, 04, 01, 07, 10, 11, 06, 08, 05, 03, 15, 13, 00, 14, 09 },
                { 14, 11, 02, 12, 04, 07, 13, 01, 05, 00, 15, 10, 03, 09, 08, 06 },
                { 04, 02, 01, 11, 10, 13, 07, 08, 15, 09, 12, 05, 06, 03, 00, 14 },
                { 11, 08, 12, 07, 01, 14, 02, 13, 06, 15, 00, 09, 10, 04, 05, 03 }
            },
            {// s6
                { 12, 01, 10, 15, 09, 02, 06, 08, 00, 13, 03, 04, 14, 07, 05, 11 },
                { 10, 15, 04, 02, 07, 12, 09, 05, 06, 01, 13, 14, 00, 11, 03, 08 },
                { 09, 14, 15, 05, 02, 08, 12, 03, 07, 00, 04, 10, 01, 13, 11, 06 },
                { 04, 03, 02, 12, 09, 05, 15, 10, 11, 14, 01, 07, 06, 00, 08, 13 }
            },
            {// s7
                { 04, 11, 02, 14, 15, 00, 08, 13, 03, 12, 09, 07, 05, 10, 06, 01 },
                { 13, 00, 11, 07, 04, 09, 01, 10, 14, 03, 05, 12, 02, 15, 08, 06 },
                { 01, 04, 11, 13, 12, 03, 07, 14, 10, 15, 06, 08, 00, 05, 09, 02 },
                { 06, 11, 13, 08, 01, 04, 10, 07, 09, 05, 00, 15, 14, 02, 03, 12 }
            },
            {// s8
                { 13, 02, 08, 04, 06, 15, 11, 01, 10, 09, 03, 14, 05, 00, 12, 07 },
                { 01, 15, 13, 08, 10, 03, 07, 04, 12, 05, 06, 11, 00, 14, 09, 02 },
                { 07, 11, 04, 01, 09, 12, 14, 02, 00, 06, 10, 13, 15, 03, 05, 08 },
                { 02, 01, 14, 07, 04, 10, 08, 13, 15, 12, 09, 00, 03, 05, 06, 11 }
            },
        };

        #endregion

        public DES(string keyFileName)
        {
            // заполнение массивов перестановки
            _iPTable = new int[64];
            _iPReverseTable = new int[64];
            int[] tempForwardBuffer = { 57, 59, 61, 63, 56, 58, 60, 62 };
            for (int i = 0; i < _iPTable.Length; i++)
            {
                if (i % 8 == 0)
                    _iPTable[i] = tempForwardBuffer[i / 8];
                else
                    _iPTable[i] = _iPTable[i - 1] - 8;

                _iPReverseTable[_iPTable[i]] = i;
            }

            // модернизация массивов pc1 и pc2
            for (int i = 0; i < _pC1.Length; i++)
                _pC1[i]--;
            for (int i = 0; i < _pC2.Length; i++)
                _pC2[i]--;
            for (int i = 0; i < _extensionScheme.Length; i++)
                _extensionScheme[i]--;
            for (int i = 0; i < _pSwapTable.Length; i++)
                _pSwapTable[i]--;

            // получение ключа из файла
            //ReadKey(keyFileName);
            // генерация ключей раундов
            GenerateRoundKeys();
        }

        private void ReadKey(string keyFileName)
        {
            FileStream keyFile = File.OpenRead(keyFileName);
            BinaryReader keyReader = new BinaryReader(keyFile);
            byte[] keyByte = new byte[1];

            // считываем из файла ключевую последовательность байт
            List<byte> readedKey = new List<byte>();
            while (keyReader.Read(keyByte, 0, 1) != 0 && readedKey.Count < 8)
            {
                readedKey.Add(keyByte[0]);
            }

            // зацикливаем ключ, если нужно
            int cycleKeyIndex = 0;
            while (readedKey.Count < 8)
            {
                readedKey.Add(readedKey[cycleKeyIndex]);
                cycleKeyIndex++;
            }
            _key = readedKey.ToArray();

            keyReader.Close();
            keyFile.Close();
        }

        private void GenerateRoundKeys()
        {
            BitArray initKey = new(_key);
            List<bool> block56Bit = [];
            // перестановка pc1
            for (int i = 0; i < 56; i++)
                block56Bit.Add(initKey[_pC1[i]]);

            //инициализация блоков C и D
            List<bool> blockC = block56Bit.Take(28).ToList();
            List<bool> blockD = block56Bit.Skip(28).Take(28).ToList();

            // цикл LS
            for (int i = 0; i < _lSTable.Length; i++)
            {
                blockC = blockC.LeftShift(_lSTable[i])
                               .Or(blockC.RightShift(blockC.Count - _lSTable[i]));

                blockD = blockD.LeftShift(_lSTable[i])
                               .Or(blockD.RightShift(blockD.Count - _lSTable[i]));

                List<bool> blockSum = [.. blockC, .. blockD];
                
                List<bool> roundKey = [];
                for (int j = 0; j < 48; j++)
                    roundKey.Add(blockSum[_pC2[j]]);

                _roundKeys[i] = roundKey;
            }
        }

        private List<bool> InitialPermutation(List<bool> block, int mode)
        {
            List<bool> resbits = [];
            int[] indexes = mode == 1 ? _iPTable : _iPReverseTable;
            for (int i = 0; i < block.Count; i++)
            {
                resbits.Add(block[indexes[i]]);
            }
            return resbits;
        }

        private List<bool> GeneratingFunction(List<bool> block, int roundNumber)
        {
            List<bool> extended = [];
            for (int i = 0; i < 48; i++)
                extended.Add(block[_extensionScheme[i]]);
            extended = extended.Xor(_roundKeys[roundNumber]);

            List<bool>[] sBlocks = new List<bool>[8];
            for (int i = 0; i < 8; i++)
            {
                sBlocks[i] = extended.Skip(i * 6).Take(6).ToList();
            }

            List<bool> block32Bit = [];
            for (int i = 0; i < 8; i++)
            {
                int line = new List<bool>([sBlocks[i].First(), sBlocks[i].Last()]).FromBitsToByte();
                int column = sBlocks[i].GetRange(1, 4).ToList().FromBitsToByte();
                byte[] bufByte = [(byte)_sBlocksTables[i, line, column]];
                block32Bit.AddRange(new BitArray(bufByte).ToBitsList().Take(4));
            }

            List<bool> resultBlock = [];
            for (int i = 0; i < 32; i++)
                resultBlock.Add(block32Bit[_pSwapTable[i]]);

            return resultBlock;
        }

        public List<bool> ECB(List<bool> block, bool encryptionMode = true)
        {
            block = InitialPermutation(block, 1);
            List<bool> blockL = block.Take(32).ToList();
            List<bool> blockR = block.Skip(32).Take(32).ToList();

            for (int i = 0; i < 16; i++)
            {
                List<bool> bufferR = new(blockR);

                int roundNumber = encryptionMode ? i : 15 - i;
                List<bool> bufferResult = GeneratingFunction(blockR, roundNumber);
                blockR = blockL.Xor(bufferResult);

                blockL = new List<bool>(bufferR);
            }

            block = [.. blockR, .. blockL];
            block = InitialPermutation(block, -1);

            return block;
        }

        public List<bool> RM(List<bool> block)
        {

            return block;
        }
    }
}
