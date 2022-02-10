using VismaMeetingsTask.Handlers;

namespace VismaMeetingsTask
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Welcome to Visma Internal Meetings Manager");
            Console.WriteLine("To see all the commands, type 'help'");
            while (true)
            {
                string command = Console.ReadLine();
                ErrorHandler.Handle(() => InputHandler.Handle(command));
            }
        }
    }
}