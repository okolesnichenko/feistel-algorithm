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

                byte[] encodeMsg = Feistel.Encoder(myMsg, n, key);
                string strEncodeMsg = encoding.GetString(encodeMsg);
                Console.WriteLine("Encripted: {0}", strEncodeMsg);

                byte[] decodeMsg = Feistel.Decoder(encodeMsg, n, key);
                string strDecodeMsg = encoding.GetString(decodeMsg);
                Console.WriteLine("Decripted: {0}", strDecodeMsg);

               //Console.WriteLine("###############################################");
               //Console.WriteLine(Feistel.F(32770));
            }
        }
    }
}