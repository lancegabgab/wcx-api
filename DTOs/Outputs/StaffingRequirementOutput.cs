namespace wcx_api.DTOs.Outputs
{
	public class StaffingRequirementOutput
	{
		public int Id { get; set; }
		public DateOnly Date { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public int RequiredAgents { get; set; }
		public int ScheduledAgents { get; set;  }

	}
}
