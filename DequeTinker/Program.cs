using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DequeTinker
{
    class Program
    {
        static void Main(string[] args)
        {
            var deque = new Deque<int>();
            deque.Add(1);
            deque.Add(2);
            deque.Add(3);
            deque.RemoveAt(1);
            foreach (var item in deque)
            {
                Console.WriteLine(item);
            }

            Console.ReadLine();
        }
    }
}
