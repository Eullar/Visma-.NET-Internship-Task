using VismaMeetingsTask.Models;
using VismaMeetingsTask.Interfaces;
using VismaMeetingsTask.Repositories;

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
                throw new Exception("There are no meetings");
            }
            _repository.AddPersonMeetingToJson(person, meetingName, dateAdded);
        }
    }
}
