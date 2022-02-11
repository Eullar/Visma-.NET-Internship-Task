namespace VismaMeetingsTask.Models
{
    public class MeetingModel
    {
        public string Name { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public MeetingModel(string name, string responsiblePerson, string description, string category, string type, DateTime startTime, DateTime endTime)
        {
            Name = name;
            ResponsiblePerson = responsiblePerson;
            Description = description;
            Category = category;
            Type = type;
            StartTime = startTime;
            EndTime = endTime;
        }
        public override string ToString()
        {
            return string.Format("|{0,-30}|{1,-30}|{2,-50}|{3,-14}|{4,-9}|{5,-20}|{6,-20}|", Name, ResponsiblePerson, Description, Category, Type, StartTime, EndTime);
        }
    }
}
