using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VismaMeetingsTask.Models
{
    public class FilterModel
    {
        public FilterType Type { get; set; }
        public string Value { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public FilterModel(FilterType type, DateTime dateFrom = default, DateTime dateTo = default)
        {
            Type = type;
            Value = "";
            DateFrom = dateFrom;
            DateTo = dateTo;
        }
        public FilterModel(FilterType type, string value)
        {
            Type = type;
            Value = value;
            DateFrom = default;
            DateTo = default;
        }
        public FilterModel(FilterType type)
        {
            Type = type;
            Value = "";
            DateFrom = default;
            DateTo = default;
        }
    }
    public enum FilterType
    {
        Description,
        ResponsiblePerson,
        Category,
        Type,
        Date,
        Attendees,
        None
    }
}
