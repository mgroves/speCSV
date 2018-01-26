using System;
using System.Diagnostics;
using Oakton;

namespace specsv
{
    class Program
    {
        static int Main(string[] args)
        {
            return CommandExecutor.ExecuteCommand<specsvCommand>(args);
        }
    }
}
