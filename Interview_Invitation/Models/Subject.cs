namespace Interview_Invitation.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Code { get; set; }
        public string Title { get; set; }
        public string Interviewer { get; set; }

        // Foreign key
        public int InterviewId { get; set; }
        public Interview Interview { get; set; }
    }
}
