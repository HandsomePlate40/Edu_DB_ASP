using System.ComponentModel.DataAnnotations.Schema;

namespace Edu_DB_ASP.Models
{
    public class InstructorJoins
    {
        public int InstructorId { get; set; }

        [ForeignKey("InstructorId")]
        public Instructor Instructor { get; set; }

        public int ForumId { get; set; }

        [ForeignKey("ForumId")]
        public DiscussionForum DiscussionForum { get; set; }

        public string Post { get; set; }
    }
}