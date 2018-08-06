using System;
using System.Linq;

namespace TrainPassangers
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] cn = Console.ReadLine().Split().Select(x => int.Parse(x)).ToArray();
            int capacity = cn[0];
            int stations = cn[1];
            int passangers = 0;

            try
            {
                for (int i = 0; i < stations; i++)
                {
                    int[] les = Console.ReadLine().Split().Select(x => int.Parse(x)).ToArray();
                    int left = les[0];
                    int enter = les[1];
                    int stay = les[2];

                    if (left > passangers)
                        throw new Exception();

                    passangers -= left;
                    passangers += enter;

                    if (passangers > capacity)
                        throw new Exception();

                    if (stay > 0 && passangers < capacity)
                        throw new Exception();
                }

                if (passangers > 0)
                    throw new Exception();

                Console.WriteLine("possible");
            }
            catch (Exception)
            {
                Console.WriteLine("impossible");
            }
        }
    }
}