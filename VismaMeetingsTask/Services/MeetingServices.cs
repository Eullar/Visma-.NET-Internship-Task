using VismaMeetingsTask.Models;
using VismaMeetingsTask.Interfaces;
using VismaMeetingsTask.Repositories;
using VismaMeetingsTask.Handlers;

namespace VismaMeetingsTask.Services
{
    public class MeetingServices : IMeetingServices
    {
        private IMeetingRepository _repository;
        public MeetingServices()
        {
            _repository = new MeetingRepository();
        }
        public string CreateAMeeting(MeetingModel meeting)
        {
            if(_repository.GetMeetings().Any(m => m.Name.Equals(meeting.Name)))
            {
                throw new Exception("Meeting with this name already exists");
            }
            return _repository.AddMeetingToJson(meeting);
        }
        public void DeleteAMeeting(string meeting, string name)
        {
            var temp = _repository.GetMeetings().FirstOrDefault(m => m.Name == meeting);
            if(temp == null)
            {
                throw new Exception($"There is no meeting with the name {meeting}");
            }
            _repository.DeleteMeetingFromJson(meeting, name);
        }
        public void AddPersonToMeeting(string person, string meetingName, DateTime dateAdded)
        {
            if (_repository.GetMeetings().Count().Equals(0))
            {
                throw new Exception("There are no meetings.");
            }
            _repository.AddPersonMeetingToJson(person, meetingName, dateAdded);
        }
        public void DeletePersonFromMeeting(string name, string meetingName)
        {
            if(name == null)
            {
                throw new Exception("Name can not be empty.");
            }
            if(meetingName == null)
            {
                throw new Exception("Meeting can not be empty.");
            }
            _repository.DeletePersonMeetingFromJson(name, meetingName);
        }
        public List<MeetingModel> GetMeetings(string filter)
        {
            var meetings = _repository.GetMeetings();
            switch (filter.ToLower().TrimEnd())
            {
                case "description":
                    var description = InputHandler.StringInput("Please type in part of the desciption:");
                    return meetings.Where(m => m.Description.Contains(description)).ToList();
                case "person":
                    var person = InputHandler.StringInput("Please type in the responsible person for the meeting:");
                    return meetings.Where(m => m.ResponsiblePerson.Equals(person)).ToList();
                case "category":
                    var category = InputHandler.CategorySelect();
                    return meetings.Where(m => m.Category.Equals(category)).ToList();
                case "type":
                    var type = InputHandler.TypeSelect();
                    return meetings.Where(m => m.Type.Equals(type)).ToList();
                case "date":
                    DateTime time;
                    Console.WriteLine("Do you want to input a beggining date?");
                    if (InputHandler.ConfirmAction())
                    {
                        time = InputHandler.DateParse();
                        meetings = meetings.Where(m => m.StartTime >= time);
                    }
                    Console.WriteLine("Do you want to input an end date?");
                    if (InputHandler.ConfirmAction())
                    {
                        time = InputHandler.DateParse();
                        meetings = meetings.Where(m => m.EndTime <= time);
                    }
                    return meetings.ToList();
                case "attendees":
                    var amount = int.Parse(InputHandler.StringInput("Type in the least amount of attendees that the meeting must have:"));
                    return meetings.Where(m => _repository.PeopleInSpecificMeeting(m.Name) >= amount).ToList();
                default:
                    return meetings.ToList();
            }
        }
    }
}
