/*
https://open.kattis.com/problems/reduction
*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace reduction
{
    class Case
    {
        public int CaseNumber { get; set; }
        public int StartWorkload { get; set; }
        public int TargetWorkload { get; set; }
        public IList<Agency> Agencies { get; set; }
    }
    class Agency
    {
        public string Name { get; set; }
        public int RateUnit { get; set; }
        public int RateHalf { get; set; }
        public int Cost { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<Case> cases = ReadCases();

            foreach (var c in cases)
            {
                CalculateCosts(c);
            }

            foreach (var c in cases)
            {
                Console.WriteLine("Case {0}", c.CaseNumber);

                foreach (var a in c.Agencies.OrderBy(x => x.Cost).ThenBy(x => x.Name))
                {
                    Console.WriteLine("{0} {1}", a.Name, a.Cost);
                }
            }
        }

        private static void CalculateCosts(Case c)
        {
            foreach (var a in c.Agencies)
            {
                CalculateCosts(c, a);
            }
        }
        private static void CalculateCosts(Case c, Agency a)
        {
            a.Cost = 0;
            var current = c.StartWorkload;

            while (current > c.TargetWorkload)
            {
                var workloadAfterHalf = current / 2;
                var costPerWorkloadWhenHalfing = (float)a.RateHalf / (current - workloadAfterHalf);

                if (workloadAfterHalf >= c.TargetWorkload && costPerWorkloadWhenHalfing < a.RateUnit)
                {
                    current = workloadAfterHalf;
                    a.Cost += a.RateHalf;
                }
                else
                {
                    current--;
                    a.Cost += a.RateUnit;
                }
            }
        }

        private static List<Case> ReadCases()
        {
            int numberOfCases = int.Parse(Console.ReadLine());

            var cases = new List<Case>(numberOfCases);
            for (int i = 0; i < numberOfCases; i++)
            {
                cases.Add(ReadCase(i + 1));
            }

            return cases;
        }

        static Case ReadCase(int caseNumber)
        {
            int[] caseHeader = Console.ReadLine().Split(' ').Select(int.Parse).ToArray();
            var item = new Case
            {
                CaseNumber = caseNumber,
                StartWorkload = caseHeader[0],
                TargetWorkload = caseHeader[1],
                Agencies = new List<Agency>(caseHeader[2])
            };

            for (int i = 0; i < caseHeader[2]; i++)
            {
                item.Agencies.Add(ReadAgency());
            }

            return item;
        }

        static Agency ReadAgency()
        {
            var agencyLine = Console.ReadLine();
            var a1 = agencyLine.Split(':');
            var rates = a1[1].Split(',').Select(int.Parse).ToArray();
            return new Agency
            {
                Name = a1[0],
                RateUnit = rates[0],
                RateHalf = rates[1]
            };
        }
    }
}
