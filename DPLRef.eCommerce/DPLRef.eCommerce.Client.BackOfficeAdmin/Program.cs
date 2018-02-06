using System;
using DPLRef.eCommerce.Common.Contracts;

namespace DPLRef.eCommerce.Client.BackOfficeAdmin
{
    class Program
    {

        private static BaseUICommand[] commands = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Starting BackOffice");

            var context = new AmbientContext() { SellerId = 2, AuthToken = "MyToken" };

            commands = new BaseUICommand[]
            {
                new TotalsCommand(context),
            };

            // get the first menu selection
            int menuSelection = InitConsoleMenu();

            while (menuSelection != 99)
            {
                if (menuSelection < commands.Length)
                {
                    var cmd = commands[menuSelection];
                    cmd?.Run();
                }
                else
                {
                    Console.WriteLine("Invalid Command");
                }
                // re-initialize the menu selection
                menuSelection = InitConsoleMenu();
            }
        }

        private static int InitConsoleMenu()
        {
            int result;

            Console.WriteLine("Select desired option:");

            for (var i = 0; i < commands.Length; i++)
            {
                var cmd = commands[i];
                Console.WriteLine($" {i}: {cmd.Name}");
            }

            Console.WriteLine(" 99: exit");
            string selection = Console.ReadLine();
            if (int.TryParse(selection, out result) == false)
            {
                result = 0;
            }

            return result;
        }
    }
}
