using System;
using System.Collections.Generic;
using Corporate.Chat.Domain.Interfaces.Model;

namespace Corporate.Chat.Domain.Model
{
    public class UserChat : IEntity
    {
        public int UserChatId { get; set; }

        public string ConnectionId { get; set; }
        public string Name { get; set; }
        
		public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}