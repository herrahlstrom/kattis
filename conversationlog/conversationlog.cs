using System;
using System.Collections.Generic;
using System.Linq;

namespace ConversationLog
{
    internal class WordItem
    {
        public string User { get; set; }
        public string Word { get; set; }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            int n = int.Parse(Console.ReadLine());

            var items = new List<WordItem>();
            for (int i = 0; i < n; i++)
            {
                var lineArr = Console.ReadLine().Split();
                items.AddRange(lineArr.Skip(1).Select(x => new WordItem { User = lineArr[0], Word = x }));
            }
            var users = (from item in items
                         group item by item.User into grp
                         select new
                         {
                             User = grp.Key,
                             Words = new HashSet<string>(grp.Select(x => x.Word))
                         }).ToDictionary(x => x.User, x => x.Words);
            var words = (from item in items
                         group item by item.Word into grp
                         select new
                         {
                             Word = grp.Key,
                             Count = grp.Count()
                         }).ToDictionary(x => x.Word, x => x.Count);

            var candidates = new List<KeyValuePair<string, int>>();
            foreach (var wordItem in words)
            {
                if (users.All(u => u.Value.Contains(wordItem.Key)))
                    candidates.Add(wordItem);
            }

            if (candidates.Any())
            {
                foreach (var word in candidates.OrderByDescending(x => x.Value).ThenBy(x=> x.Key))
                {
                    Console.WriteLine(word.Key);
                }
            }
            else
            {
                Console.WriteLine("ALL CLEAR");
            }
        }
    }
}