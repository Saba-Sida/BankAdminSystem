using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAdminSystem3.Services
{
    public class MyConsole
    {
        private static string systemTitle = "" +
            "  ____              _     ____            _                 \r\n" +
            " | __ )  __ _ _ __ | | __/ ___| _   _ ___| |_ ___ _ __ ___  \r\n" +
            " |  _ \\ / _` | '_ \\| |/ /\\___ \\| | | / __| __/ _ \\ '_ ` _ \\ \r\n" +
            " | |_) | (_| | | | |   <  ___) | |_| \\__ \\ ||  __/ | | | | |\r\n" +
            " |____/ \\__,_|_| |_|_|\\_\\|____/ \\__, |___/\\__\\___|_| |_| |_|\r\n" +
            "                                |___/                       \n" +
            "__________________________________________________________________\n";
        public static void RefreshConsole()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(systemTitle);
            Console.ResetColor();
        }
    }
}
