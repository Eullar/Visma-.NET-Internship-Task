using VismaMeetingsTask.Models;
using VismaMeetingsTask.Interfaces;
using System.Text.Json;
using System.Linq;
using System;

namespace VismaMeetingsTask.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly string _meetingsJson;
        private readonly string _personMeetingJson;

        public MeetingRepository(string meetingsJson = "Meetings.json",string personMeetingJson = "PersonMeeting.json")
        {
            var rootDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;
            _meetingsJson = rootDirectory + "/Files/" + meetingsJson;
            _personMeetingJson = rootDirectory + "/Files/" + personMeetingJson;
        }
        public string AddMeetingToJson(MeetingModel meeting)
        {
            var meetings = (List<MeetingModel>)GetMeetings();
            meetings.Add(meeting);
            var jsonString = JsonSerializer.Serialize(meetings);
            File.WriteAllText(_meetingsJson, jsonString);
            return meeting.Name;
        }
        public void DeleteMeetingFromJson(string model, string person)
        {
            var meetings = GetMeetings();
            if(meetings == null)
            {
                throw new Exception("There are no meetings");
            }
            var meeting = meetings.Where(m => m.Name == model).FirstOrDefault();
            if(meeting.ResponsiblePerson != person)
            {
                throw new Exception("You are not the meetings responsible person.");
            }
            meetings = meetings.Where(m => m.Name != model);
            var jsonString = JsonSerializer.Serialize(meetings);
            File.WriteAllText(_meetingsJson,jsonString);
        }
        public void AddPersonMeetingToJson(string person, string meetingName, DateTime dateAdded)
        {
            var peopleInMeetings = (List<PersonMeetingModel>)GetPeopleInMeeting();
            var peopleInThisMeeting = GetPeopleInMeeting().Where(m => m.Meeting == meetingName);
            var meeting = GetMeetings().Where(m => m.Name == meetingName).FirstOrDefault();
            if(meeting == null)
            {
                throw new Exception("There is no meeting with that name");
            }
            else if (peopleInMeetings.Any(m => m.Name == person && m.Meeting == meetingName))
            {
                throw new Exception("Person is already in this meeting");
            }
            else if(meeting.StartTime > dateAdded)
            {
                throw new Exception("Person would be added before the meeting has started");
            }
            else if (meeting.EndTime < dateAdded)
            {
                throw new Exception("Person would be added after the meeting has ended");
            }
            
            if(peopleInMeetings.Any(m => m.Name == person && m.EndTime > dateAdded && m.StartTime < dateAdded || m.StartTime < meeting.EndTime && m.EndTime > dateAdded))
            {
                Console.WriteLine("Person is already in another meeting during the time that you are trying to add him.");
                if (!ConfirmAction())
                {
                    return;
                }
            }
            peopleInMeetings.Add(new PersonMeetingModel(person, meetingName, dateAdded, meeting.EndTime));
            var jsonString = JsonSerializer.Serialize(peopleInMeetings);
            File.WriteAllText(_personMeetingJson, jsonString);

        }
        public IEnumerable<MeetingModel> GetMeetings()
        {
            if (File.Exists(_meetingsJson))
            {
                var jsonString = File.ReadAllText(_meetingsJson);
                var meetings = JsonSerializer.Deserialize<IEnumerable<MeetingModel>>(jsonString);
                return meetings;
            }
            return new List<MeetingModel>();
        }
        public IEnumerable<PersonMeetingModel> GetPeopleInMeeting()
        {
            if (File.Exists(_personMeetingJson))
            {
                var jsonString = File.ReadAllText(_personMeetingJson);
                var peopleInMeetings = JsonSerializer.Deserialize<IEnumerable<PersonMeetingModel>>(jsonString);
                return peopleInMeetings;
            }
            return new List<PersonMeetingModel>();
        }
        private static bool ConfirmAction()
        {
            Console.WriteLine("Do you want to confirm your action?");
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
