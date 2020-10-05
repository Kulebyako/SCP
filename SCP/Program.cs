using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace SCP
{
    

    public class Program
    {
        static string[] args2;
        public static void Main(string[] args)
        {
            //копирую массив
            args2 = (string[])args.Clone();
            //Проверка условий
            if (args.Length % 2 == 0 || args.Length < 3  || args.Length == 0)
            {
                Console.WriteLine("Задайте нечетное количество агрументов больше трех");
                return;
            }


            //Выводим аргументы
            Menu();
            RNG();
        }

        static void Menu()
        {
            for (int i = 0; i < args2.Length; i++)
            {
                Console.WriteLine(i + 1 + " - " + args2[i]);
            }
            Console.WriteLine("0 - exit");
            string UserMoveStr = Console.ReadLine();

            int UserMoveInt;

            bool isParsable = Int32.TryParse(UserMoveStr, out UserMoveInt);

            if (isParsable)
                Console.WriteLine("User move integer: " + UserMoveInt);
            else
            {
                Console.WriteLine("Could not be parsed.");
                Menu();
                return;
            }


            Console.WriteLine("User move: " + UserMoveStr);

        }



        //Генерим ключ в 32 байта
        static void RNG()
        {
            string SecureKey = "";
            byte[] random = new Byte[32];

            //RNGCryptoServiceProvider is an implementation of a random number generator.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random); // The array is now filled with cryptographically strong random bytes.

            foreach (byte bytevalue in random)
            {
                SecureKey += bytevalue.ToString();
            }
            Console.WriteLine(SecureKey);
            Console.WriteLine(GetHash("Сюда передать выбор ПК строкой", SecureKey));

            return;
            
        }

        public static String GetHash(String text, String key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            HMACSHA256 hash = new HMACSHA256(keyBytes);
            Byte[] hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        }


    }
}
