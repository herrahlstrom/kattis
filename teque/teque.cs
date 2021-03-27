/*
https://open.kattis.com/problems/teque
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Teque
{
    internal class Teque<T>
    {
        private readonly Dictionary<int, T> _back = new Dictionary<int, T>(1024);
        private readonly Dictionary<int, T> _front = new Dictionary<int, T>(1024);
        private int _backHead = -1;
        private int _backTail;
        private int _frontHead = -1;
        private int _frontTail;

        public T Get(int position)
        {
            return position < _front.Count
                ? _front[_frontHead + position + 1]
                : _back[position - _front.Count + _backHead + 1];
        }

        public void PushBack(T value)
        {
            _back[_backTail++] = value;
        }

        public void PushFront(T value)
        {
            _front[_frontHead--] = value;
        }

        public void PushMiddle(T value)
        {
            ReArrange();
            _front[_frontTail++] = value;
        }

        private void ReArrange()
        {
            while (true)
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
                else
                {
                    break;
                }
            }
        }
    }

    internal class Program
    {
        private static readonly Teque<string> Q = new Teque<string>();

        private static void Command(string command, string value)
        {
            switch (command)
            {
                case "push_back":
                    Q.PushBack(value);
                    break;

                case "push_front":
                    Q.PushFront(value);
                    break;

                case "push_middle":
                    Q.PushMiddle(value);
                    break;

                case "get":
                    int position = int.Parse(value);
                    Console.WriteLine(Q.Get(position));
                    break;

                default:
                    throw new ArgumentException("Invalid command: " + command);
            }
        }

        private static void Main(string[] args)
        {
            //var sw = Stopwatch.StartNew();

            int noOps = int.Parse(Console.ReadLine());

            var inputCommand = new StringBuilder(16);
            var inputValue = new StringBuilder(16);
            int state = 0;

            while (noOps > 0)
            {
                char ch = (char) Console.Read();

                switch (state)
                {
                    case 0:
                        switch (ch)
                        {
                            case '\r':
                            case '\n':
                                break;
                            case ' ':
                                state = 1;
                                break;
                            default:
                                inputCommand.Append(ch);
                                break;
                        }

                        break;
                    case 1:
                        switch (ch)
                        {
                            case '\r':
                            case '\n':
                                Command(inputCommand.ToString(), inputValue.ToString());
                                noOps--;
                                inputCommand.Clear();
                                inputValue.Clear();
                                state = 0;
                                break;
                            default:
                                inputValue.Append(ch);
                                break;
                        }

                        break;
                }
            }

            //sw.Stop();
            //Console.Error.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms.");
        }
    }
}