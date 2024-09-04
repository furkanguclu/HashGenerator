using System;
using System.Security.Cryptography;
using System.Text;

namespace Hash_Generator
{

    internal class Program
    {
        static void Main(string[] args)
        {
            // Program başında hoş geldiniz mesajı ve yazar bilgisi
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Author: [Furkan Güçlü]");
            Console.ResetColor();

            while (true)
            {
                try
                {
                    // Kullanıcıdan şifreyi al
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("Şifrenizi girin (Çıkmak için 'exit' yazın):");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    string userPassword = Console.ReadLine();

                    // Kullanıcı çıkış yapmak istiyorsa döngüden çık
                    if (userPassword?.ToLower() == "exit")
                    {
                        break;
                    }

                    // Boş veya geçersiz girdi kontrolü
                    if (string.IsNullOrWhiteSpace(userPassword))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Geçersiz şifre girdiniz. Lütfen geçerli bir şifre girin.");
                        continue;
                    }

                    // Adım 1: Kullanıcının girdiği şifrenin SHA-1 hash'ini hesapla
                    string firstHash = ComputeSha1Hash(userPassword);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nSHA-1 Hash: {firstHash}");

                    // Adım 2: İlk hash değerini binary formatta döndür
                    byte[] binaryData = ConvertHexStringToByteArray(firstHash);
                    if (binaryData == null)
                    {
                        continue;
                    }
#if DEBUG
                    Console.WriteLine($"Binary Veri: {BitConverter.ToString(binaryData).Replace("-", "")}");
#endif
                    // Adım 3: Binary verinin SHA-1 hash'ini tekrar hesapla
                    string secondHash = ComputeSha1Hash(binaryData);
#if DEBUG
                    Console.WriteLine($"SHA-1 Hash: {secondHash}");
#endif
                    // Adım 4: Hash değerini büyük harfe çevir ve başına '*' ekle
                    string finalHash = '*' + secondHash.ToUpper();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"\nHASH: {finalHash}");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                }
                finally
                {
                    // Program bittiğinde renkleri varsayılan renge döndür
                    Console.ResetColor();
                }
            }
        }

        // SHA-1 hash hesaplama fonksiyonu (string girdisi için)
        static string ComputeSha1Hash(string text)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        // SHA-1 hash hesaplama fonksiyonu (binary veri için)
        static string ComputeSha1Hash(byte[] data)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(data);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        // Hex string'i byte dizisine dönüştürme fonksiyonu
        static byte[] ConvertHexStringToByteArray(string hexString)
        {
            try
            {
                int length = hexString.Length;
                byte[] byteArray = new byte[length / 2];

                for (int i = 0; i < length; i += 2)
                {
                    byteArray[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
                }

                return byteArray;
            }
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Geçersiz hexadecimal string. Lütfen geçerli bir girdi sağlayın.");
                return null;
            }
        }
    }
}
