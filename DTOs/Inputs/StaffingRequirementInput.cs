namespace wcx_api.DTOs.Inputs
{
	public class StaffingRequirementInput
	{
		public DateTime Date { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public int RequiredAgents { get; set; }
	}
}
