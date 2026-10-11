using System.ComponentModel.DataAnnotations;

namespace wcx_api.DTOs.Inputs
{
	public class ScheduleInput
	{
		[Required]
		public string AgentId { get; set; } = string.Empty;

		[Required]
		public DateOnly Date { get; set; }

		[Required]
		public TimeSpan StartTime { get; set; }

		[Required]
		public TimeSpan EndTime { get; set; }

		public string Status { get; set; } = "Scheduled";
	}
}
