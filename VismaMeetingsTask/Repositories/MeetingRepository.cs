using VismaMeetingsTask.Models;
using VismaMeetingsTask.Interfaces;
using VismaMeetingsTask.Handlers;
using System.Text.Json;

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
            AddPersonMeetingToJson(meeting.ResponsiblePerson, meeting.Name, meeting.StartTime);
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
            DeleteAllPeopleFromMeeting(model);
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
            
            if(peopleInMeetings.Any(m => m.Name == person && m.EndTime > dateAdded && m.StartTime < dateAdded || m.Name == person && m.StartTime < meeting.EndTime && m.EndTime > dateAdded))
            {
                Console.WriteLine("Person is already in another meeting during the time that you are trying to add him.");
                Console.WriteLine("Do you want to confirm your action?");
                if (!InputHandler.ConfirmAction())
                {
                    return;
                }
            }
            peopleInMeetings.Add(new PersonMeetingModel(person, meetingName, dateAdded, meeting.EndTime));
            var jsonString = JsonSerializer.Serialize(peopleInMeetings);
            File.WriteAllText(_personMeetingJson, jsonString);
        }
        public void DeletePersonMeetingFromJson(string name, string meeting)
        {
            var peopleInMeetings = GetPeopleInMeeting();
            if(peopleInMeetings == null)
            {
                throw new Exception("There are no people in any meetings.");
            }
            if(!peopleInMeetings.Any(m => m.Name == name && m.Meeting == meeting))
            {
                throw new Exception($"Person {name} is not participating in {meeting}.");
            }
            else
            {
                var specificMeeting = peopleInMeetings.Where(m => m.Name == name && m.Meeting == meeting).FirstOrDefault();
                peopleInMeetings = peopleInMeetings.Where(m => m != specificMeeting);
                var jsonString = JsonSerializer.Serialize(peopleInMeetings);
                File.WriteAllText(_personMeetingJson, jsonString);
            }
        }
        public void DeleteAllPeopleFromMeeting(string meeting)
        {
            var peopleInMeetings = GetPeopleInMeeting();
            if(peopleInMeetings == null)
            {
                throw new Exception("There are no people in any meetings");
            }
            if(!peopleInMeetings.Any(m => m.Meeting == meeting))
            {
                throw new Exception($"There are no people in {meeting} meeting");
            }
            peopleInMeetings = peopleInMeetings.Where(m => m.Meeting != meeting);
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
        public int PeopleInSpecificMeeting(string meeting)
        {
            return GetPeopleInMeeting().Count(m => m.Meeting == meeting);
        }
    }
}
