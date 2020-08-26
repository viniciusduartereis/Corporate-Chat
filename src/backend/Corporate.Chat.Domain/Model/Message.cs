using System;
using Corporate.Chat.Domain.Interfaces.Model;

namespace Corporate.Chat.Domain.Model
{
	/// <summary>
	///  Message Contract
	/// </summary>
	public class Message : IEntity
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
