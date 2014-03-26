using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Server;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            QsTaskProcess task = new QsSendInfoToServer("Hello");
            for (int i = 0; i < 10; i++)
                task.Process();
            System.Console.ReadKey();
        }
    }
}
