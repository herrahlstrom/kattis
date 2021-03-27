/*
https://open.kattis.com/problems/teque
*/

using System;
using System.Collections.Generic;

namespace Teque
{
    class Teque<T>
    {
        private Dictionary<int, T> _front = new Dictionary<int, T>();
        private Dictionary<int, T> _back = new Dictionary<int, T>();
        private int _frontHead = -1;
        private int _frontTail = 0;
        private int _backHead = -1;
        private int _backTail = 0;

        public T Get(int position)
        {
            return position < _front.Count
                ? _front[_frontHead + position + 1]
                : _back[position - _front.Count + _backHead + 1];
        }
        
        public void PushBack(T value)
        {
            _back[_backTail++] = value;
            ReArrange();
        }

        public void PushFront(T value)
        {
            _front[_frontHead--] = value;
            ReArrange();
        }

        public void PushMiddle(T value)
        {
            _front[_frontTail++] = value;
            ReArrange();
        }

        private void ReArrange()
        {
            if (_front.Count < _back.Count)
            {
                _front[_frontTail++] = _back[_backHead + 1];
                _back.Remove(++_backHead);
            }
            else if (_front.Count - 1 > _back.Count)
            {
                _back[_backHead--] = _front[_frontTail - 1];
                _front.Remove(--_frontTail);
            }
        }
    }

    class Program
    {
        private static Teque<long> _q = new Teque<long>();
        
        private static void Command(string line)
        {
            var lineArr = line.Split(' ');
            
            switch (lineArr[0])
            {
                case "push_back":
                    _q.PushBack(long.Parse(lineArr[1]));
                    break;
                
                case "push_front":
                    _q.PushFront(long.Parse(lineArr[1]));
                    break;
                
                case "push_middle":
                    _q.PushMiddle(long.Parse(lineArr[1]));
                    break;
                
                case "get":
                    Console.WriteLine(_q.Get(int.Parse(lineArr[1])));
                    break;
                
                default:
                    throw new ArgumentException("Invalid line: " + line);
            }
        }
        static void Main(string[] args)
        {
            //var sw = System.Diagnostics.Stopwatch.StartNew();



            int noOps = int.Parse(Console.ReadLine());

            var input = new Queue<string>();

            var buffer = new char[1024];
            bool readDone = false;
            do
            {
                var readed = Console.In.Read(buffer, 0, buffer.Length);
                if (readed < buffer.Length)
                {
                    readDone = true;
                }

                var r = new System.IO.StringReader()

            } while (!readDone);

            while (noOps > 0)
            {
                noOps--;
                string line = Console.ReadLine();
                Command(line);
            }

            // sw.Stop();
            // Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms.");
        }
    }
}