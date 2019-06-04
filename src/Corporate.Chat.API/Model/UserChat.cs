using System;
using System.Collections.Generic;

namespace Corporate.Chat.API.Model
{
    public class UserChat
    {
        public int UserChatId { get; set; }

        public string ConnectionId { get; set; }
        public string Name { get; set; }
        
		public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}