using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SCP
{
    

    public class Program
    {
        static string[] args2;
        static string secureKey, pcmove, hmac = "";
        static int usermove, move;
        
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
                Judging();
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
            move = randommove.Next(1, args2.Length+1);
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
            decimal half;
            Console.WriteLine("You move: " + args2[usermove - 1]);
            half = Convert.ToDecimal(args2.Length * 0.5);
            half = Math.Truncate(half);
            if (move == usermove)
            {
                Console.WriteLine("Dead Heat");
                Console.WriteLine("PC move: " + pcmove);
                return;
            }
            if ((move + half) > args2.Length)
            {
                Console.WriteLine("PC move: " + pcmove);
                Console.WriteLine("half: " + half);
                Console.WriteLine("Lenght: " + args2.Length);
                Console.WriteLine("((move + half) > args2.Length)");
                //Console.WriteLine("PC move: " + pcmove);
                int y = (move - Convert.ToInt32(half));

                //for (int i = move; i >= y; i--)
                int i = move;
                while (i >= y)
                {
                    Console.WriteLine("i: " + i);
                    //Console.WriteLine("i args: " + args2[i]);
                    if (i == usermove)
                    {
                        Console.WriteLine("You LOSE больше");
                        Console.WriteLine("PC move: " + pcmove);
                        return;
                    }
                    i--;
                }
                if (i == y)
                {
                    Console.WriteLine("You WIN больше");
                    Console.WriteLine("PC move: " + pcmove);
                    return;
                }

            }
            else
            {
                Console.WriteLine("((move + half) < args2.Length)");
                Console.WriteLine("PC move: " + pcmove);
                int y = (move + Convert.ToInt32(half));
                int i = move;
                //for (i = move; i <= y; i++)
                while (i <= y)
                {
                    if (i == usermove)
                    {
                        Console.WriteLine("You LOSE меньше");
                        Console.WriteLine("PC move: " + pcmove);
                        return;
                    }
                    i++;
                }
                if (i == y)
                {
                    Console.WriteLine("You WIN меньше");
                    Console.WriteLine("PC move: " + pcmove);
                    return;
                }
            }

            //Console.WriteLine("You windrful: " + pcmove);
        }
    }
}
