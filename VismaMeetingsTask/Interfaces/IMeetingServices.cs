using VismaMeetingsTask.Models;

namespace VismaMeetingsTask.Interfaces
{
    public interface IMeetingServices
    {
        string CreateAMeeting(MeetingModel meeting);
        void DeleteAMeeting(string meeting, string person);
    }
}
