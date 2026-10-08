namespace wcx_api.DTOs.Outputs
{
	public class Response<T>
	{
		public bool Success { get; set; }
		public string Message { get; set; } = String.Empty;
		public T? Data { get; set; }
	}
}
