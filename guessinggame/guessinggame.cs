/*
https://open.kattis.com/problems/guessinggame
*/

using System;

namespace guessinggame
{
    class ExitGameException : Exception { }
    class Game
    {
        public bool? Result { get; set; }
        bool[] candidates = new bool[10] { true, true, true, true, true, true, true, true, true, true };
        public void ToHighGuess(int value)
        {
            for (int i = value - 1; i < candidates.Length; i++)
            {
                candidates[i] = false;
            }
        }
        public void ToLowGuess(int value)
        {
            for (int i = value - 1; i >= 0; i--)
            {
                candidates[i] = false;
            }
        }
        public void SpotOnGuess(int value)
        {
            Result = candidates[value - 1];
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                while (true)
                {
                    var game = new Game();
                    while (game.Result == null)
                    {
                        ReadGuess(game);
                    }

                    Console.WriteLine(game.Result.Value
                        ? "Stan may be honest"
                        : "Stan is dishonest");
                }
            }
            catch (ExitGameException) { }
        }

        static void ReadGuess(Game game)
        {
            int guess = int.Parse(Console.ReadLine());
            if (guess == 0)
            {
                throw new ExitGameException();
            }

            switch (Console.ReadLine())
            {
                case "too high":
                    game.ToHighGuess(guess);
                    break;

                case "too low":
                    game.ToLowGuess(guess);
                    break;

                case "right on":
                    game.SpotOnGuess(guess);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
