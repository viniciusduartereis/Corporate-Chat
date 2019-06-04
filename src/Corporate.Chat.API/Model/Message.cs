using System;

namespace Corporate.Chat.API.Model
{
	/// <summary>
	///  Message Contract
	/// </summary>
	public class Message
	{
		/// <summary>
		/// 
		/// </summary>
		public int MessageId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int UserChatId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int EventId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public DateTime CreatedDate { get; set; }

	}
}
