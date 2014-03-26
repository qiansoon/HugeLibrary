using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            QsNetIOListener listener = new QsNetIOListener(5252);
            listener.Listen();
            System.Console.ReadKey();
        }
    }
}
