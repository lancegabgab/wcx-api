namespace wcx_api.DTOs.Outputs
{
	public class ScheduleOutput
	{
		public int Id { get; set; }
		public string AgentId { get; set; } = string.Empty;
		public DateTime Date { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public string Status { get; set; } = string.Empty;
	}
}