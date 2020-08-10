using SISGED.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SISGED.Shared.Models
{
    public class CalendarDay
    {
        public int DayNumber { get; set; }
        public DateTime Date { get; set; }
        public bool IsEmpty { get; set; }
        public List<Derivacion> Events { get; set; } = new List<Derivacion>();
    }
}
