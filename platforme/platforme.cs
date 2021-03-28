using System;
using System.Collections.Generic;
using System.Linq;

namespace platforme
{
    internal struct Platform
    {
        public int Height { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
    }

    internal class Program
    {
        private static int GetTotalPillarLength(Platform[] platforms)
        {
            int pillarLength = 0;
            var heights = new Dictionary<int, int>();
            
            foreach (Platform platform in platforms.OrderBy(x => x.Height))
            {
                pillarLength += heights.TryGetValue(platform.Left, out int supportingPlatformOnLeft)
                    ? platform.Height - supportingPlatformOnLeft
                    : platform.Height;

                pillarLength += heights.TryGetValue(platform.Right - 1, out int supportingPlatformOnRight)
                    ? platform.Height - supportingPlatformOnRight
                    : platform.Height;

                for (int i = platform.Left; i <= platform.Right - 1; i++)
                {
                    heights[i] = platform.Height;
                }
            }

            return pillarLength;
        }

        private static void Main(string[] args)
        {
            int noPlatforms = int.Parse(Console.ReadLine());

            var platforms = new Platform[noPlatforms];
            ReadPlatforms(platforms);

            int pillarLength = GetTotalPillarLength(platforms);

            Console.WriteLine(pillarLength);
        }

        private static void ReadPlatforms(Platform[] platforms)
        {
            for (int i = 0; i < platforms.Length; i++)
            {
                string[] lineArr = Console.ReadLine().Split();
                platforms[i] = new Platform
                {
                    Height = int.Parse(lineArr[0]),
                    Left = int.Parse(lineArr[1]),
                    Right = int.Parse(lineArr[2])
                };
            }
        }
    }
}