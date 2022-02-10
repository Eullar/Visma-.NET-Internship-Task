using System;

namespace VismaMeetingsTask.Handlers
{
    static class InputHandler
    {
        public static void Handle(string command)
        {
            switch (command.ToLower().TrimEnd())
            {
                case "help":
                    Help();
                    break;
                case "create":
                    break;
                case "delete":
                    break;
                case "add":
                    break;
                case "remove":
                    break;
                case "meetings":
                    break;
                case "exit" or "quit":
                    Console.WriteLine("Exiting program.");
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Command not found. Type 'help' to see all the commands.");
                    break;
            }
            Console.WriteLine("\r\nWaiting for a command");
        }
        private static void Help()
        {
            var helpCommands = new Dictionary<string, string>()
            {
                { "help", "Shows all the commands and their descriptions." },
                { "create", "Creates a new meeting." },
                { "delete", "Deletes an exiting meeting." },
                { "add", "Adds a person to a selected meeting." },
                { "remove", "Removes a person from a selected meeting." },
                { "meetings", "Shows a list of existing meetings." },
                { "exit, quit", "Exits the program." }
            };
            Console.WriteLine("\r\nAvailable commands:");
            foreach(var(command,description) in helpCommands)
            {
                Console.WriteLine($"{command}: {description}");
            }
        }
    }
}
