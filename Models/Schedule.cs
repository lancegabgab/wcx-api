namespace wcx_api.Models
{
	public class Schedule
	{
		public int Id { get; set; }
		public string AgentId { get; set; } = string.Empty;
		public User Agent { get; set; } = null!;
		public DateTime Date { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public string Status { get; set; } = "Scheduled";
	}
}
