using VismaMeetingsTask.Models;
using VismaMeetingsTask.Interfaces;
using VismaMeetingsTask.Repositories;

namespace VismaMeetingsTask.Services
{
    public class MeetingServices : IMeetingServices
    {
        private IMeetingRepository _repository;
        public MeetingServices(IMeetingRepository repo = null)
        {
            _repository = repo ??= new MeetingRepository();
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
        public List<MeetingModel> GetMeetings(FilterModel filter)
        {
            var meetings = _repository.GetMeetings();
            switch (filter.Type)
            {
                case FilterType.Description:
                    return meetings.Where(m => m.Description.Contains(filter.Value)).ToList();
                case FilterType.Date:
                    if (filter.DateFrom != default)
                    {
                        meetings = meetings.Where(m => m.StartTime >= filter.DateFrom);
                    }
                    if (filter.DateTo != default)
                    {
                        meetings = meetings.Where(m => m.EndTime <= filter.DateTo);
                    }
                    return meetings.ToList();
                case FilterType.Attendees:
                    return meetings.Where(m => _repository.PeopleInSpecificMeeting(m.Name) >= int.Parse(filter.Value)).ToList();
                case FilterType.ResponsiblePerson:
                    return meetings.Where(m => m.ResponsiblePerson.Equals(filter.Value)).ToList();
                case FilterType.Category:
                    return meetings.Where(m => m.Category.Equals(filter.Value)).ToList();
                case FilterType.Type:
                    return meetings.Where(m => m.Type.Equals(filter.Value)).ToList();
                default:
                    return meetings.ToList();
            }
        }
    }
}
