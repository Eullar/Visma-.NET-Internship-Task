using System;
using VismaMeetingsTask.Models;
using VismaMeetingsTask.Services;
using VismaMeetingsTask.Interfaces;

namespace VismaMeetingsTask.Handlers
{
    static class InputHandler
    {
        private static IMeetingServices _services = new MeetingServices();
        public static void Handle(string command)
        {
            switch (command.ToLower().TrimEnd())
            {
                case "help":
                    Help();
                    break;
                case "create":
                    Create();
                    break;
                case "delete":
                    Delete();
                    break;
                case "add":
                    Add();
                    break;
                case "remove":
                    Remove();
                    break;
                case "meetings":
                    Meetings();
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
        private static void Create()
        {
            Console.WriteLine("\r\nCreate a new meeting:");
            Console.WriteLine("Type a name for the meeting:");
            var name = Console.ReadLine();
            Console.WriteLine("Type who is responsible for the meeting:");
            var person = Console.ReadLine();
            Console.WriteLine("Type a description for the meeting:");
            var description = Console.ReadLine();
            Console.WriteLine("Select meetings category:");
            var category = CategorySelect();
            Console.WriteLine("Select a type for the meeting:");
            var type = TypeSelect();
            Console.WriteLine("Input a date for the start of the meeting:");
            var startDate = DateParse();
            Console.WriteLine("Input a date for the end of the meeting:");
            var endDate = DateParse(startDate);
            _services.CreateAMeeting(new MeetingModel(name, person, description, category, type, startDate, endDate));
        }
        private static void Delete()
        {
            Console.WriteLine("\r\nType in your name:");
            var name = Console.ReadLine();
            Console.WriteLine("Type a name of the meeting you want to delete:");
            var meeting = Console.ReadLine();
            _services.DeleteAMeeting(meeting, name);
        }
        private static void Add()
        {
            Console.WriteLine("\r\nType the name of person being added:");
            var name = Console.ReadLine();
            Console.WriteLine("Type the meeting in which he is being added to:");
            var meeting = Console.ReadLine();
            Console.WriteLine("Type the date at which he is being added at:");
            var date = DateParse();
            _services.AddPersonToMeeting(name, meeting, date);
        }
        public static void Remove()
        {
            Console.WriteLine("Please type in the name of the person you want to remove:");
            var name = Console.ReadLine();
            Console.WriteLine("Please type in the name of the meeting that you want the person to be removed from:");
            var meeting = Console.ReadLine();
            _services.DeletePersonFromMeeting(name, meeting);
        }
        public static void Meetings()
        {
            Console.WriteLine("Do you want to filter the meetings?");
            FilterType filterType = FilterType.None;
            FilterModel filter = new(filterType);
            if (ConfirmAction())
            {
                filterType = filterSelect();
            }
            string filterValue;
            switch (filterType)
            {
                case FilterType.Date:
                    DateTime dateFrom = default;
                    Console.WriteLine("Do you want to input a beggining date?");
                    if (InputHandler.ConfirmAction())
                    {
                        dateFrom = InputHandler.DateParse();
                    }
                    DateTime dateTo = default;
                    Console.WriteLine("Do you want to input an end date?");
                    if (InputHandler.ConfirmAction())
                    {
                        dateTo = InputHandler.DateParse();
                    }
                    filter = new(filterType, dateFrom, dateTo);
                    break;
                case FilterType.Category:
                    filterValue = CategorySelect();
                    filter = new(filterType, filterValue);
                    break;
                case FilterType.Type:
                    filterValue = TypeSelect();
                    filter = new(filterType, filterValue);
                    break;
                case FilterType.Attendees:
                    Console.WriteLine("Enter a minimum amount of attendees required in the meeting:");
                    filterValue = Console.ReadLine();
                    filter = new(filterType, filterValue);
                    break;
                case FilterType.ResponsiblePerson:
                    Console.WriteLine("Enter responsible person for the  meeting:");
                    filterValue = Console.ReadLine();
                    filter = new(filterType, filterValue);
                    break;
                case FilterType.Description:
                    Console.WriteLine("Enter the description of the meeting:");
                    filterValue = Console.ReadLine();
                    filter = new(filterType, filterValue);
                    break;
                default:
                    break;
            }
            var meetings = _services.GetMeetings(filter);
            foreach (var meeting in meetings)
            {
                Console.WriteLine(meeting.ToString());
            }
        }
        private static FilterType filterSelect()
        {
            Console.WriteLine("1 - By Description | 2 - By Responsible Person | 3 - By Category | 4 - By Type | 5 - By Date | 6 - By Attendee Count");
            switch (Console.ReadLine().TrimEnd())
            {
                case "1":
                    return FilterType.Description;
                case "2":
                    return FilterType.ResponsiblePerson;
                case "3":
                    return FilterType.Category;
                case "4":
                    return FilterType.Type;
                case "5":
                    return FilterType.Date;
                case "6":
                    return FilterType.Attendees;
                default:
                    Console.WriteLine("Please select from the list.");
                    return filterSelect();
            }
        }
        public static string CategorySelect()
        {
            Console.WriteLine("1 - CodeMonkey | 2 - Hub | 3 - Short | 4 - TeamBuilding");
            switch (Console.ReadLine().TrimEnd())
            {
                case "1":
                    return "CodeMonkey";
                case "2":
                    return "Hub";
                case "3":
                    return "Short";
                case "4":
                    return "TeamBuilding";
                default:
                    Console.WriteLine("Select an option from the list.");
                    return CategorySelect();
            }
        }
        public static string TypeSelect()
        {
            Console.WriteLine("1 - Live | 2 - InPerson");
            switch (Console.ReadLine().TrimEnd())
            {
                case "1":
                    return "Live";
                case "2":
                    return "InPerson";
                default:
                    Console.WriteLine("Select an option from the list.");
                    return TypeSelect();
            }
        }
        public static DateTime DateParse()
        {
            Console.WriteLine("Input date in the format of Year/Month/Day HH:mm");
            DateTime date = TestDate();
            return date;
        }
        public static DateTime DateParse(DateTime start)
        {
            DateTime date = DateParse();
            if (date < start)
            {
                Console.WriteLine("Ending time can not be earlier than starting time");
                date = DateParse(start);
            }
            return date;
        }
        private static DateTime TestDate()
        {
            string date = Console.ReadLine();
            DateTime returnDate;
            try
            {
                returnDate = DateTime.ParseExact(date, "yyyy/M/dd HH:mm", null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Incorrect date format. Error message:");
                Console.WriteLine(ex.Message);
                returnDate = TestDate();
            }
            return returnDate;
        }
        public static string StringInput(string text)
        {
            Console.WriteLine(text);
            return Console.ReadLine();
        }
        public static bool ConfirmAction()
        {
            Console.WriteLine("Please type yes or no");
            switch (Console.ReadLine().ToLower().TrimEnd())
            {
                case "yes":
                    return true;
                case "no":
                    return false;
                default:
                    Console.WriteLine("Incorrect type of answer.");
                    return ConfirmAction();
            }
        }
    }
}
