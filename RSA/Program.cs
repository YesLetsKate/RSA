// See https://aka.ms/new-console-template for more information

using RSA_;

RSA rsa = new RSA();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Генерация ключей...");
int[] ned = rsa.GenerateKey();

Console.WriteLine($"n = {ned[0]}\ne = {ned[1]}\nd = {ned[2]}");

Console.ForegroundColor = ConsoleColor.Yellow;
Console.Write("Введите текст: ");
string text = Console.ReadLine();

int[] cipher = rsa.Encrypt(ned[0], ned[1], text);
Console.ForegroundColor = ConsoleColor.Magenta;
Console.WriteLine("Шифротекст: ");
foreach (int i in cipher)
{
    Console.Write($"{i} ");
}
Console.WriteLine();

Console.ForegroundColor = ConsoleColor.Magenta;
Console.Write("Введите шифротекст: ");
string opentext = rsa.Decrypt(ned[0], ned[2], Console.ReadLine());

Console.ForegroundColor = ConsoleColor.Yellow;
Console.Write("Открытый текст: ");
Console.WriteLine(opentext);



Console.ResetColor();


