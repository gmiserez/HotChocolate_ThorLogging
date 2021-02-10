#nullable disable

namespace Demo.Models
{
	public class EventHubTrace
	{
		public string ActivityId { get; set; }
		public int ApplicationId {get;set;}
		public string Id {get;set;}
		public string Level {get;set;}
		public string Message {get;set;}
	    public string Name { get; set; }
		public EventHubTracePayload Payloads { get; set; }
	    public string ProviderName { get; set; }
		public string SessionName { get; set; }
		public int TaskId { get; set; }
    }

	public class EventHubTracePayload
    {
		public int StatusCode { get; set; }
		public string StatusText { get; set; }
	}
}
