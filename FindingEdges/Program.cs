using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindingEdges
{
    class Program
    {
        static void Main(string[] args)
        {
            new SequentialEdgeFinding();
            new ParallelEdgeFinding();
            Console.ReadLine();
        }
    }
}
