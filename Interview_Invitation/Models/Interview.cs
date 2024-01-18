namespace Interview_Invitation.Models
{
    public class Interview
    {
        public int InterviewId { get; set; }
        public string InterviewerName { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public int Duration { get; set; }
        public string InterviewType { get; set; } // Remotely/Inside

      
        public List<Subject> Subjects { get; set; }
        public List<Interviewee> Interviewees { get; set; }

    }
}
