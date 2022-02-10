

namespace VismaMeetingsTask.Models
{
    public class PersonMeetingModel
    {
        public string Name { get; set; }
        public string Meeting { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime {get;set;}
        public PersonMeetingModel(string name, string meeting, DateTime startTime, DateTime endTime)
        {
            Name = name;
            Meeting = meeting;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
