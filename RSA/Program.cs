// See https://aka.ms/new-console-template for more information
using RSA_;
RSA rsa = new RSA();
int[] pqe = rsa.GenerateKey();

for (int i =0; i < pqe.Length; i++)
{
    Console.WriteLine(pqe[i]);
}
int[] cipher = rsa.Encrypt(pqe[0], pqe[1], pqe[2], Console.ReadLine());
foreach(int i in cipher)
{
    Console.Write($"{i} ");
}



