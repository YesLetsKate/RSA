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
            int[] pqe = new int[3];
            bool check = false;

            Thread thread1 = new Thread(() =>
            {
                while (check != true)
                {
                    pqe[0] = rand.Next(1001, 10000);
                    check = FermatPrimalityTest((int)pqe[0]);
                }
            });
            Thread thread2 = new Thread(() =>
            {
                while (check != true)
                {
                    pqe[1] = rand.Next(1001, 10000);
                    check = FermatPrimalityTest((int)pqe[1]);
                }
            });
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            int fi = (pqe[0] - 1) * (pqe[1] - 1);
            check = false;
            while (check != true)
            {
                pqe[2] = rand.Next(0,fi);
                check = IsCoprime(pqe[2], fi);
            }
            return pqe;
        }
        public int[] Encrypt(int p,int q, int e, string text)
        {
            int n = p * q;
            char[] letters = text.ToCharArray();
            int[] Cipher = new int[letters.Length];

            for(int i = 0; i < letters.Length; i++)
            {
                Cipher[i] = (int)FastExponentiation(letters[i],e,n);
            }
            return Cipher; 
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
            return Modulo(y2, n);
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
