using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SCP
{
    public class Program
    {
        private static string[] args2;
        private static int usermove;

        public static void Main(string[] args)
        {
            args2 = (string[])args.Clone();
            CheckForArgumentes();
            Game();
        }

        private static void CheckForArgumentes()
        {
            CheckForMinLenght();
            CheckForOdd();
            CheckForUniqueness();
        }

        private static void CheckForUniqueness()
        {
            IEnumerable<string> dist = args2.Distinct();
            if (dist.ToArray().Length != args2.Length)
            {
                ExitDuringCheck();
            }
        }

        private static void CheckForOdd()
        {
            if (args2.Length % 2 == 0)
            {
                ExitDuringCheck();
            }
        }

        private static void CheckForMinLenght()
        {
            if (args2.Length < 3)
            {
                ExitDuringCheck();
            }
        }

        private static void ExitDuringCheck()
        {
            Console.WriteLine("Set an odd number of unique arguments more than three");
            Environment.Exit(0);
        }

        private static void Game()
        {
            //PCMove();
            Console.WriteLine("HMAC: " + GetHash(PCMove(Move()), GetSecureKey()));
            Menu();
        }

        private static void Menu()
        {
            AvailableMoves();
            UserMove();
        }

        private static void AvailableMoves()
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

        private static void UserMove()
        {
            string UserMoveStr = Console.ReadLine();
            bool isParsable = int.TryParse(UserMoveStr, out usermove);
            if (isParsable && usermove == 0)
            {
                Console.WriteLine("Good Bye!");
                return;
            }else if
                (isParsable && usermove <= args2.Length && usermove > 0)
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

        private static string GetSecureKey()
        {
            string secureKey = "";
            byte[] random = new byte[32];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetBytes(random);
            foreach (byte bytevalue in random)
            {
                secureKey += bytevalue.ToString();
            }
            return secureKey;
        }

        private static string PCMove(int move)
        {
            return args2[move - 1];
        }

        private static int Move()
        {
            Random randommove = new Random();
            int move = randommove.Next(1, args2.Length + 1);
            return move;
        }

        private static string GetHash(string text, string key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] textBytes = encoding.GetBytes(text);
            byte[] keyBytes = encoding.GetBytes(key);
            HMACSHA256 hash = new HMACSHA256(keyBytes);
            byte[] hashBytes = hash.ComputeHash(textBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        private static void Result(string result)
        {
            Console.WriteLine($"PC move: {PCMove(Move())}\r\n{result}!\r\nHMAC key: {GetSecureKey()}");
            Environment.Exit(0);
        }

        private static void Judging()
        {
            Console.WriteLine("You move: " + args2[usermove - 1]);
            int half = Convert.ToInt32((args2.Length - 1) * 0.5);

            CheckDeadJeat();
            CheckWay(half);
        }
        private static void CheckDeadJeat()
        {
            if (Move() == usermove)
            {
                Result("Dead Heat");
            }
        }
        private static void LeftWay(int half)
        {
            for (int i = 1; i <= half; i++)
            {
                if (usermove == Move() - i)
                {
                    Result("You lose");
                }
            }
            Result("You win");
        }
        private static void RightWay(int half)
        {
            for (int i = 1; i <= half; i++)
            {
                if (usermove == Move() + i)
                {
                    Result("You win");
                }
            }
            Result("You lose");
        }
        private static void CheckWay(int half)
        {
            if ((Move() + half) > args2.Length)
            {
                LeftWay(half);
            }
            else
            {
                RightWay(half);
            }
        }
    }
}