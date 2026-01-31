using System;
using System.Collections.Generic;

namespace EventRegistrationAPI.DTOs
{
	public class EventResponseDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public DateTime Date { get; set; }
		public int Capacity { get; set; }
		public int RegisteredCount { get; set; }
		public string Location { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}