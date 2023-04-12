using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSA_
{
    internal class RSA
    {
        public int[] GenerateKey()
        {
            var rand = new Random();
            int p = 0;
            int q = 0;
            bool check = false;

            Thread thread1 = new Thread(() =>
            {
                while (check != true)
                {
                    p = rand.Next(1001, 10000);
                    check = FermatPrimalityTest((int)p);
                }
            });
            Thread thread2 = new Thread(() =>
            {
                while (check != true)
                {
                    q = rand.Next(1001, 10000);
                    check = FermatPrimalityTest((int)q);
                }
            });
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            int fi = (p - 1) * (q - 1);

            int e = 0;
            check = false;
            while (check != true)
            {
                e = rand.Next(0,fi);
                check = IsCoprime(e, fi);
            }
            int n = p * q;
            int d = Euclid(e, fi);
            int[] ned = new int[3] { n, e, d };
            return ned;
        }
        public int[] Encrypt(int n, int e, string text)
        {
            char[] letters = text.ToCharArray();
            int[] Cipher = new int[letters.Length];

            for(int i = 0; i < letters.Length; i++)
            {
                Cipher[i] = (int)FastExponentiation(letters[i],e,n);
            }
            return Cipher; 
        }
        public string Decrypt(int n, int d, string text)
        {
            string[] ciphertxt = text.Split(' ');
            char[] opentxt = new char[ciphertxt.Length];
            for(int i = 0;i< ciphertxt.Length; i++)
            {
                opentxt[i] = Convert.ToChar(FastExponentiation(Convert.ToInt32(ciphertxt[i]),d,n));
            }
            string opentext = String.Join(null,opentxt);
            return opentext;
        }
        private static bool IsCoprime(int num1, int num2)
        {
            if (num1 == num2)
            {
                return num1 == 1;
            }
            else
            {
                if (num1 > num2)
                {
                    return IsCoprime(num1 - num2, num2);
                }
                else
                {
                    return IsCoprime(num2 - num1, num1);
                }
            }
        }
        private int Euclid(int a, int n)
        {
            int mod = n;
            int q = 0;
            int y = 0;
            int y2 = 0;
            int y1 = 1;
            int r = 1;
            while (r != 0)
            {
                q = n / a;
                r = n % a;
                y = y2 - q * y1;
                n = a;
                a = r;
                y2 = y1;
                y1 = y;
            }
            return Modulo(y2, mod);
        }
        private int Modulo(int a, int n)
        {
            if (a >= n || a < 0)
            {
                if (a < 0)
                {
                    a = a % n;
                    a = a + n;
                }
                else a = a % n;
            }
            return a;
        }
        private bool FermatPrimalityTest(int p)
        {
            var rand = new Random();
            int a = rand.Next(2, p);
            long f;
            for (int i = 0; i < 10; i++)
            {
                f = FastExponentiation(a, p - 1, p);
                if (f != 1) return false;
                a = rand.Next(2, p);
            }
            return true;
        }

        // (основание, степень, модуль)
        private long FastExponentiation(int a, int _b, int c)
        {
            byte[] b;
            b = BitConverter.GetBytes(_b);
            BitArray array = new BitArray(b);
            int len = array.Length;
            long[,] res = new long[2, len];
            if (array.Get(0) == true)
            {
                res[0, 0] = a;
                res[1, 0] = a;
            }
            else
            {
                res[0, 0] = a;
                res[1, 0] = 1;
            }
            int g = 0;
            for (int i = 1; i < len; i++)
            {
                res[0, i] = (res[0, i - 1] * res[0, i - 1]) % c;
                if (array.Get(i) == true)
                {
                    res[1, i] = (res[0, i] * res[1, i - 1]) % c;
                    g = i;
                }
                else res[1, i] = res[1, i - 1] % c;

            }
            return res[1, g];
        }
    }
}
