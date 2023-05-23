using System.Text.Json.Serialization;

namespace AssessmentsWebApp.Models.RESPONSEModel
{
    public class Student_Grading
    {
        public string StudentId { get; set; } = null!;

        public string? Username { get; set; }

        public string? GradingId { get; set; }

        public int? Grade { get; set; }

        public string? Comment { get; set; }
    }
}