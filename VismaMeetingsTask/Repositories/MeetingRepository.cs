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

        public MeetingRepository(string meetingsJson = "Meetings.json")
        {
            var rootDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;
            _meetingsJson = rootDirectory + "/Files/" + meetingsJson;
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
            if(meetings is null)
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
    }
}
