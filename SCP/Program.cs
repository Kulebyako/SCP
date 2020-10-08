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
            args2 = (string[])args.Clone();
            CheckForUniqueness();
            if (args.Length % 2 == 0 || args.Length < 3  || args.Length == 0)
            {
                Console.WriteLine("Set an odd number of unique arguments more than three");
                return;
            }

            Game();            
        }

        static void CheckForUniqueness()
        {
            int dublicate = 0;
            for (int i = 0; i < args2.Length; i++)
            {
                dublicate = 0;
                for (int j = 0; j < args2.Length; j++)
                {
                    if (args2[i] == args2[j])
                    {
                        dublicate++;
                        if (dublicate >= 2)
                        {
                            Console.WriteLine("Duplicate found: " + args2[j]);
                            Console.WriteLine("Set an odd number of unique arguments more than three");
                            Process.GetCurrentProcess().Kill();
                            return;
                        }
                    }
                }
            }
        }

            static void Game()
        {
            PCMove();            
            RNG();
            Menu();
        }

        static void Menu()
        {
            Console.WriteLine("Available moves: ");
            for (int i = 0; i < args2.Length; i++)
            {
                Console.WriteLine(i + 1 + " - " + args2[i]);
            }
            Console.WriteLine("0 - exit");
            string UserMoveStr = Console.ReadLine();
            bool isParsable = Int32.TryParse(UserMoveStr, out usermove);
            if (isParsable && usermove == 0)
            {
                Console.WriteLine("Good Bye!");
                return;
            }

            if (isParsable && usermove <= args2.Length && usermove >0)
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
            Console.WriteLine("HMAC: " + hmac);

            return;
            
        }

        public static String PCMove()
        {
            Random randommove = new Random();
            move = randommove.Next(1, args2.Length + 1);
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
            int half;
            Console.WriteLine("You move: " + args2[usermove - 1]);
            half = Convert.ToInt32((args2.Length - 1) * 0.5);

            if (move == usermove)
            {
                Console.WriteLine("Dead Heat");
                Console.WriteLine("PC move: " + pcmove);
                Console.WriteLine("HMAC key: " + secureKey);
                return;
            }
            if ((move + half) > args2.Length)
            {                
                int i =1;
                while (i<= half)
                {
                    if (usermove == move - i)
                    {
                        Console.WriteLine("PC move: " + pcmove);
                        Console.WriteLine("You lose!");
                        Console.WriteLine("HMAC key: " + secureKey);
                        return;
                    }
                    i++;
                }
                Console.WriteLine("PC move: " + pcmove);
                Console.WriteLine("You win");
                Console.WriteLine("HMAC key: " + secureKey);
            } else
            {
                int i = 1;
                while (i <= half)
                {
                    if (usermove == move + i)
                    {
                        Console.WriteLine("PC move: " + pcmove);
                        Console.WriteLine("You win");
                        Console.WriteLine("HMAC key: " + secureKey);
                        return;
                    }
                    i++;
                }
                Console.WriteLine("PC move: " + pcmove);
                Console.WriteLine("You lose!");
                Console.WriteLine("HMAC key: " + secureKey);
            }
        }
    }
}
