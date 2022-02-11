using VismaMeetingsTask.Models;

namespace VismaMeetingsTask.Interfaces
{
    public interface IMeetingServices
    {
        string CreateAMeeting(MeetingModel meeting);
        void DeleteAMeeting(string meeting, string person);
        void AddPersonToMeeting(string person, string meetingName, DateTime dateAdded);
        void DeletePersonFromMeeting(string name, string meetingName);
        List<MeetingModel> GetMeetings(string filter);
    }
}
