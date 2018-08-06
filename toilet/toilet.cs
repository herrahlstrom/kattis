using System;
using System.Linq;

namespace Toilet
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = Console.ReadLine();

            Console.WriteLine(CalcAlwaysUp(data));
            Console.WriteLine(CalcAlwaysDown(data));
            Console.WriteLine(CalcAsPrefered(data));
        }

        private static int CalcAlwaysUp(string data)
        {
            int moves = 0;
            char current = data[0];
            foreach (var c in data.Skip(1))
            {
                if (current != c)
                    moves++;
                if (c != 'U')
                    moves++;
                current = 'U';
            }
            return moves;
        }

        private static int CalcAlwaysDown(string data)
        {
            int moves = 0;
            char current = data[0];
            foreach (var c in data.Skip(1))
            {
                if (current != c)
                    moves++;
                if (c != 'D')
                    moves++;
                current = 'D';
            }
            return moves;
        }

        private static int CalcAsPrefered(string data)
        {
            int moves = 0;
            char current = data[0];
            foreach (var c in data.Skip(1))
            {
                if (current != c)
                    moves++;
                current = c;
            }
            return moves;
        }
    }
}