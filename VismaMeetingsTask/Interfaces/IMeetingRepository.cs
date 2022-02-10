using VismaMeetingsTask.Models;

namespace VismaMeetingsTask.Interfaces
{
    public interface IMeetingRepository
    {
        string AddMeetingToJson(MeetingModel meeting);
        void DeleteMeetingFromJson(string model, string person);
        void AddPersonMeetingToJson(string person, string meetingName, DateTime dateAdded);
        IEnumerable<MeetingModel> GetMeetings();
        IEnumerable<PersonMeetingModel> GetPeopleInMeeting();
    }
}
