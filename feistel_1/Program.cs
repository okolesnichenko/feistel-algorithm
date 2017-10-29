using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feistel_1
{
    class Program
    {
        static void Main(string[] args)
        {
            for (;;)
            {
                int n = 4;
                Console.Write("Type text: ");
                string s = Console.ReadLine();
                Random rnd = new Random();
                ulong key = ulong.MaxValue - (ulong)rnd.Next();

                Encoding encoding = Encoding.Default;

                byte[] myMsg = encoding.GetBytes(s);

                if (myMsg.Length % 8 != 0)
                {
                    int oldLength = myMsg.Length;
                    Array.Resize<byte>(ref myMsg, myMsg.Length + (8 - myMsg.Length % 8));
                    for (int i = oldLength; i < myMsg.Length; i++)
                    {
                        myMsg[i] = 0;
                    }
                }

                //проверка Данила
                List<ulong> list = new List<ulong>(); // Массив для поиска колизий (одинаковых хешей у разных входных данных)
                for (ulong i = 0; i < 12000; i++)
                {
                    list.Add(Feistel.Hash(BitConverter.GetBytes(i), n, key));
                }
                var result = list
            .Select(number => new { Hash = number, Count = list.Count(l => l == number) })
            .Where(obj => obj.Count > 1)
            .Distinct()
            .ToDictionary(obj => obj, obj => obj.Count);

                byte[] encodeMsg = Feistel.Encoder(myMsg, n, key);
                string strEncodeMsg = encoding.GetString(encodeMsg);
                Console.WriteLine("Encripted: {0}", strEncodeMsg);

                byte[] decodeMsg = Feistel.Decoder(encodeMsg, n, key);
                string strDecodeMsg = encoding.GetString(decodeMsg);
                Console.WriteLine("Decripted: {0}", strDecodeMsg);

                ulong hash = Feistel.Hash(myMsg, n, key);
                Console.WriteLine("Hash: {0}", hash);

               //Console.WriteLine("###############################################");
               //Console.WriteLine(Feistel.F(32770));
            }
        }
    }
}