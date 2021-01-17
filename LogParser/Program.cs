using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LogParser.Entity;

namespace LogParser
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args == null || args.Length == 0)
                throw new ArgumentException("Kindly provide the filename");
            LogParser obj = new LogParser();
            obj.Start(args[0]);
        }
    }
}
