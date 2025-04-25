using BankAdminSystem3.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAdminSystem2.Services
{
    public static class Runner
    {
        public static void RunNextOld(
            string title = "",
            string bodyToShow = "",
            Dictionary<string, Action>? keyAndDelegatePair = null
            )
        {
            keyAndDelegatePair ??= new Dictionary<string, Action>();


            while (true)
            {
                MyConsole.RefreshConsole();

                if (title != "")
                {
                    Console.WriteLine(title);
                    Console.WriteLine("-------------------------------------------");
                }

                if (bodyToShow != "")
                {
                    Console.WriteLine(bodyToShow);
                }

                Console.WriteLine("\nActions:\n-----------");

                int actionsCounter = 0;
                foreach (var pair in keyAndDelegatePair)
                {
                    actionsCounter++;
                    Console.WriteLine($"{actionsCounter}. {pair.Key}");
                }
                Console.WriteLine("0. exit");

                int selectedOption = MyReader.ReadOption(0, keyAndDelegatePair.Count);

                if (selectedOption == 0)
                {
                    Console.WriteLine("Exiting...");
                    Thread.Sleep(1000);
                    return;
                }

                //RunNext(
                //    title: title,
                //    bodyToShow: bodyToShow,
                //    keyAndDelegatePair: keyAndDelegatePair
                //);
                keyAndDelegatePair.ElementAt(selectedOption - 1).Value.Invoke();
                Thread.Sleep(1000);
            }


        }



        public static void RunNext(
            string title = "",
            Action? action = null,
            Dictionary<string, Action>? keyAndDelegatePair = null
            )
        {
            keyAndDelegatePair ??= new Dictionary<string, Action>();


            while (true)
            {
                MyConsole.RefreshConsole();

                if (title != "")
                {
                    Console.WriteLine(title);
                    Console.WriteLine("-------------------------------------------");
                }

                if (action != null)
                {
                    action.Invoke();
                }

                Console.WriteLine("\nActions:\n-----------");

                int actionsCounter = 0;
                foreach (var pair in keyAndDelegatePair)
                {
                    actionsCounter++;
                    Console.WriteLine($"{actionsCounter}. {pair.Key}");
                }
                Console.WriteLine("0. exit");

                int selectedOption = MyReader.ReadOption(0, keyAndDelegatePair.Count);

                if (selectedOption == 0)
                {
                    Console.WriteLine("Exiting...");
                    Thread.Sleep(100);
                    return;
                }

                //RunNext(
                //    title: title,
                //    bodyToShow: bodyToShow,
                //    keyAndDelegatePair: keyAndDelegatePair
                //);
                keyAndDelegatePair.ElementAt(selectedOption - 1).Value.Invoke();
                Thread.Sleep(1000);
            }


        }
    }
}
