using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Edu_DB_ASP.Models
{
    public class PostMessageViewModel
    {
        public int ForumId { get; set; }
        public string Post { get; set; }
        public List<MessageViewModel> Messages { get; set; } = new List<MessageViewModel>();
    }

    public class MessageViewModel
    {
        public string LearnerName { get; set; }
        public string Content { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}