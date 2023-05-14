using System.Text.Json.Serialization;

namespace AssessmentsWebApp.Models
{
    public class Student_Grading
    {
        public string StudentId { get; set; } = null!;

        public string? Username { get; set; }

        public int? Grade { get; set; }

        public string? Comment { get; set; }
    }
}