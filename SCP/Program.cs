using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

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
            CheckForArgumentes();
            Game();
        }
        static void CheckForArgumentes()
        {
            CheckForMinLenght();
            CheckForOdd();
            CheckForUniqueness();
        }
        static void CheckForUniqueness()
        {
            IEnumerable<string> dist = args2.Distinct();
            if (dist.ToArray().Length != args2.Length)
            {
                ExitDuringCheck();
            }
        }
        static void CheckForOdd()
        {
            if (args2.Length % 2 == 0)
            {
                ExitDuringCheck();
            }
        }
        static void CheckForMinLenght()
        {
            if (args2.Length < 3)
            {
                ExitDuringCheck();
            }
        }
        static void ExitDuringCheck()
        {
            Console.WriteLine("Set an odd number of unique arguments more than three");
            Environment.Exit(0);
        }
        static void Game()
        {
            PCMove();            
            RNG();
            Menu();
        }
        static void Menu()
        {
            AvailableMoves();
            UserMove();
        }
        static void AvailableMoves()
        {
            Console.WriteLine("Available moves: ");
            int i = 0;
            foreach (string arg in args2)
            {
                Console.WriteLine(i + 1 + " - " + args2[i]);
                i++;
            }
            Console.WriteLine("0 - exit");
        }
        static void UserMove()
        {
            string UserMoveStr = Console.ReadLine();
            bool isParsable = Int32.TryParse(UserMoveStr, out usermove);
            if (isParsable && usermove == 0)
            {
                Console.WriteLine("Good Bye!");
                return;
            }

            if (isParsable && usermove <= args2.Length && usermove > 0)
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
        static String PCMove()
        {
            Random randommove = new Random();
            move = randommove.Next(1, args2.Length + 1);
            pcmove = args2[move - 1];
            return pcmove;
        }
        static String GetHash(String text, String key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);
            HMACSHA256 hash = new HMACSHA256(keyBytes);
            Byte[] hashBytes = hash.ComputeHash(textBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
        static void Result(string result)
        {
            Console.WriteLine($"PC move: {pcmove}\r\n{result}!\r\nHMAC key: {secureKey}");
            Environment.Exit(0);
        }
        static void Judging()
        {
            int half;
            Console.WriteLine("You move: " + args2[usermove - 1]);
            half = Convert.ToInt32((args2.Length - 1) * 0.5);

            if (move == usermove)
            {
                Result("Dead Heat");
            }
            if ((move + half) > args2.Length)
            {
                for (int i = 1; i <= half; i++)
                {
                    if (usermove == move - i)
                    {
                        Result("You lose");
                    }
                }
                Result("You win");
            } else
            {
                for (int i = 1; i <= half; i++)
                {
                    if (usermove == move + i)
                    {
                        Result("You win");
                    }
                }
                Result("You lose");
            }
        }
    }
}