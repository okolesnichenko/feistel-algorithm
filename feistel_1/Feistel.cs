using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace feistel_1
{
    class Feistel
    {
        static public byte[] Encoder(byte[] myMsg, int n, ulong key)
        {
            ushort[] keys = new ushort[n];
            byte[] encodMsg = new byte[myMsg.Length];
            
            //генерация ключей
            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = KeyGenerator(key, i);
            }
            //объяление блоков до и после выхода из алг
            ushort x0 = 0;
            ushort x1 = 0;
            ushort x2 = 0;
            ushort x3 = 0;
            ushort c0, c1, c2, c3;

            for (int i = 0; i < myMsg.Length; i += 8)
            {
                x0 = 0;
                x1 = 0;
                x2 = 0;
                x3 = 0;
                //заполнение блоков x 
                x0 = (ushort)(x0 | myMsg[i]);
                x0 = (ushort)((x0 << 8) | myMsg[i + 1]);

                x1 = (ushort)(x1 | myMsg[i + 2]);
                x1 = (ushort)((x1 << 8) | myMsg[i + 3]);

                x2 = (ushort)(x2 | myMsg[i + 4]);
                x2 = (ushort)((x2 << 8) | myMsg[i + 5]); 

                x3 = (ushort)(x3 | myMsg[i + 6]);
                x3 = (ushort)((x3 << 8) | myMsg[i + 7]);
                //алгоритм шифрования
                for (int j = 0; j < n; j++)
                {
                    //Console.Write("{0}|{1}|{2}|{3} || ", Convert.ToString(x0, 8), Convert.ToString(x1, 8), Convert.ToString(x2, 8), Convert.ToString(x3, 8));
                    c0 = (ushort)(F((ushort)(x0 ^ x1)) ^ (x3 ^ keys[j]));
                    c1 = x0;
                    c2 = (ushort)(x3 ^ keys[j]);
                    c3 = x2;
                    //Console.Write("{0}|", keys[j]);
                    x0 = c0;
                    x1 = c1;
                    x2 = c2;
                    x3 = c3;
                }
                //Console.WriteLine("{0}|{1}|{2}|{3} || ", Convert.ToString(x0, 8), Convert.ToString(x1, 8), Convert.ToString(x2, 8), Convert.ToString(x3, 8));
                encodMsg[i] = (byte)(x0 >> 8);
                encodMsg[i + 1] = (byte)x0;
                encodMsg[i + 2] = (byte)(x1 >> 8);
                encodMsg[i + 3] = (byte)x1;
                encodMsg[i + 4] = (byte)(x2 >> 8);
                encodMsg[i + 5] = (byte)x2;
                encodMsg[i + 6] = (byte)(x3 >> 8);
                encodMsg[i + 7] = (byte)x3;
            }
            //Console.WriteLine("===========");
            return encodMsg;
        }

        static public byte[] Decoder(byte[] encodMsg, int n, ulong key)
        { 
            ushort[] keys = new ushort[n];
            byte[] decodMsg = new byte[encodMsg.Length];
            

            for (int i = 0; i < keys.Length; i++)
            { 
                keys[i] = KeyGenerator(key, i);
            }

            ushort c0;
            ushort c1;
            ushort c2;
            ushort c3;
            ushort x0, x1, x2, x3;

            for (int i = 0; i < encodMsg.Length; i+=8)
            {
                c0 = 0;
                c1 = 0;
                c2 = 0;
                c3 = 0;

                c0 = (ushort)(c0 | encodMsg[i]);
                c0 = (ushort)((c0 << 8) | encodMsg[i + 1]);

                c1 = (ushort)(c1 | encodMsg[i + 2]);
                c1 = (ushort)((c1 << 8) | encodMsg[i + 3]);

                c2 = (ushort)(c2 | encodMsg[i + 4]);
                c2 = (ushort)((c2 << 8) | encodMsg[i + 5]);

                c3 = (ushort)(c3 | encodMsg[i + 6]);
                c3 = (ushort)((c3 << 8) | encodMsg[i + 7]);

                for(int j = 0; j < n; j++)
                {
                    //Console.Write("{0}|{1}|{2}|{3} || ", Convert.ToString(x0, 8), Convert.ToString(x1, 8), Convert.ToString(x2, 8), Convert.ToString(x3, 8));
                    x0 = c1;
                    x1 = (ushort)(F((ushort)(c0 ^ c2)) ^ c1);
                    x2 = c3;
                    x3 = (ushort)(c2^keys[n-1-j]);
                    //Console.Write("{0}|", keys[n-j-1]);
                    c0 = x0;
                    c1 = x1;
                    c2 = x2;
                    c3 = x3;
                }
                //Console.WriteLine("{0}|{1}|{2}|{3} || ", Convert.ToString(x0, 8), Convert.ToString(x1, 8), Convert.ToString(x2, 8), Convert.ToString(x3, 8));
                decodMsg[i] = (byte)(c0 >> 8);
                decodMsg[i + 1] = (byte)c0;
                decodMsg[i + 2] = (byte)(c1 >> 8);
                decodMsg[i + 3] = (byte)c1;
                decodMsg[i + 4] = (byte)(c2 >> 8);
                decodMsg[i + 5] = (byte)c2;
                decodMsg[i + 6] = (byte)(c3 >> 8);
                decodMsg[i + 7] = (byte)c3;

                
            } 
            //Console.WriteLine("===========");
            return decodMsg;
        }

        static public ushort F(ushort num)
        {            
            return (ushort)(~num);
        }

        static private ushort KeyGenerator(ulong key, int i)
        {
            return (ushort)((LeftShiftForKey(key, i * 5)) ^ (RightShiftForKey(key, i * 3)));
        }

        static private ulong RightShiftForKey(ulong number, int n)
        {
            n = n % (sizeof(ulong) * 8);
            return (number >> n) | (number << (sizeof(ulong) * 8 - n));
        }

        static private ushort RightShift(ushort num, int n)
        {
            n = n % (sizeof(ushort) * 8);
            return (ushort)((num >> n) | (num << (sizeof(ushort) * 8 - n)));
        }

        static private ulong LeftShiftForKey(ulong num, int n)
        {
            n = n % (sizeof(ulong) * 8);
            return (num << n) | (num >> (sizeof(ulong) * 8 - n));
        }

        static ushort LeftShift(ushort num, int n)
        {
            n = n % (sizeof(ushort) * 8);
            return (ushort)((num << n) | (num >> (sizeof(ushort) * 8 - n)));
        }
    }
}
