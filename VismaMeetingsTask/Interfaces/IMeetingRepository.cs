using VismaMeetingsTask.Models;

namespace VismaMeetingsTask.Interfaces
{
    public interface IMeetingRepository
    {
        string AddMeetingToJson(MeetingModel meeting);
        void DeleteMeetingFromJson(string model, string person);
        IEnumerable<MeetingModel> GetMeetings();
    }
}
