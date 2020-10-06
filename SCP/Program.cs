using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace SCP
{
    

    public class Program
    {
        static string[] args2;
        static string secureKey, pcmove, hmac = "";
        static int usermove;
        
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

            Game();

            
        }

        static void Game()
        {
            PCMove();
            Console.WriteLine("The computer has made its move.");
            RNG();
            Menu();
        }

        static void Menu()
        {
            for (int i = 0; i < args2.Length; i++)
            {
                Console.WriteLine(i + 1 + " - " + args2[i]);
            }
            Console.WriteLine("0 - exit");
            string UserMoveStr = Console.ReadLine();

            //int usermove;

            bool isParsable = Int32.TryParse(UserMoveStr, out usermove);
            if (usermove == 0)
            {
                Console.WriteLine("Good Bye!");
                return;
            }

            if (isParsable && usermove <= args2.Length)
            {

                Console.WriteLine("You move: " + args2[usermove - 1]);                
            }
            else
            {
                Console.WriteLine("Wrong move. Try again");
                Menu();
                return;
            }
        }



        //Генерим ключ в 32 байта
        static void RNG()
        {
            secureKey = "";
            byte[] random = new Byte[32];            
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random); 
            foreach (byte bytevalue in random)
            {
                secureKey += bytevalue.ToString();
            }            

            hmac = GetHash(pcmove, secureKey);
            Console.WriteLine(hmac);

            return;
            
        }

        public static String PCMove()
        {
            Random randommove = new Random();
            int move = randommove.Next(1, args2.Length+1);
            //Console.WriteLine(pcmove1.ToString());
            //Console.WriteLine("PC move: " + move.ToString() + " - " + args2[move - 1]);
            pcmove = args2[move - 1];
            return pcmove;
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

        static void Judging()
        {
            //Who did win ?
        }
    }
}
