/*
https://open.kattis.com/problems/teque
*/

using System;
using System.Collections.Generic;

namespace Teque
{
    class Teque<T>
    {
        List<T> _q = new List<T>();

        public void PushBack(T value)
        {
            _q.Add(value);
        }

        public void PushMiddle(T value)
        {
            int position = (_q.Count + 1) / 2;
            _q.Insert(position, value);
        }

        public void PushFront(T value)
        {
            _q.Insert(0, value);
        }

        public T Get(int position)
        {
            return _q[position];
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var q = new Teque<long>();

            int noOps = int.Parse(Console.ReadLine());
            while(noOps>0) {
                noOps--;            
                var line = Console.ReadLine();
                if(line[0] == 'g') 
                {
                    int position = int.Parse(line.Substring(4));
                    Console.WriteLine(q.Get(position));
                } 
                else if(line[5] == 'b') 
                {
                    long value = long.Parse(line.Substring(10));
                    q.PushBack(value);
                } 
                else if(line[5] == 'f') 
                {
                    long value = long.Parse(line.Substring(11));
                    q.PushFront(value);                    
                } 
                else if(line[5] == 'm') 
                {
                    long value = long.Parse(line.Substring(12));
                    q.PushMiddle(value);                    
                } 
                else 
                {
                    throw new ArgumentException("Invalid line: " + line);
                }
            }
        }
    }
}