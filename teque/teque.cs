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
        private static readonly Teque<long> Q = new Teque<long>();

        private static void Main(string[] args)
        {
            int noOps = int.Parse(Console.ReadLine());

            ReadCommands(noOps);
        }

        private static void ReadCommands(int noOps)
        {
            char[] inputCommand = new char[11];
            int inputCommandLength = 0;
            char[] inputValue = new char[10];
            int inputValueLength = 0;
            int state = 0;
            char[] readBuffer = new char[1024];

            var output = new StringBuilder(1024 * 5);

            while (noOps > 0)
            {
                int count = Console.In.Read(readBuffer, 0, readBuffer.Length);

                for (int i = 0; i < count; i++)
                {
                    char ch = readBuffer[i];
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
                                    inputCommand[inputCommandLength++] = ch;
                                    break;
                            }

                            break;
                        case 1:
                            switch (ch)
                            {
                                case '\r':
                                case '\n':

                                    if (inputCommand[0] == 'g')
                                    {
                                        int position = int.Parse(new string(inputValue, 0, inputValueLength));
                                        output.AppendFormat("{0}", Q.Get(position)).AppendLine();
                                    }
                                    else if (inputCommand[5] == 'b')
                                    {
                                        long value = long.Parse(new string(inputValue, 0, inputValueLength));
                                        Q.PushBack(value);
                                    }
                                    else if (inputCommand[5] == 'f')
                                    {
                                        long value = long.Parse(new string(inputValue, 0, inputValueLength));
                                        Q.PushFront(value);
                                    }
                                    else if (inputCommand[5] == 'm')
                                    {
                                        long value = long.Parse(new string(inputValue, 0, inputValueLength));
                                        Q.PushMiddle(value);
                                    }
                                    else
                                        throw new ArgumentException("Invalid command: " + new string(inputCommand, 0, inputCommandLength));

                                    noOps--;
                                    inputCommandLength = 0;
                                    inputValueLength = 0;
                                    state = 0;

                                    if (output.Length >= output.Capacity - 50)
                                    {
                                        Console.Write(output);
                                        output.Clear();
                                    }

                                    break;
                                default:
                                    inputValue[inputValueLength++] = ch;
                                    break;
                            }

                            break;
                        default:
                            throw new InvalidOperationException("Invalid state: " + state);
                    }
                }
            }

            if (output.Length > 0)
            {
                Console.Write(output);
                output.Clear();
            }
        }
    }
}