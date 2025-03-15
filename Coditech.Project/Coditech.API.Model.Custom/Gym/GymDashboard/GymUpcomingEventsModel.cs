namespace Coditech.Common.API.Model
{
    public class GymUpcomingEventsModel : BaseModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Type { get; set; }
        public int DayNumber { get; set; }
        public string MonthName { get; set; }
    }
}
