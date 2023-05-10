using System;
using System.Collections.Generic;

namespace AssessmentsWebApp.Models;

public partial class Grading
{
    public string Id { get; set; } = null!;

    public int? Grade { get; set; }

    public DateTime? Date { get; set; }

    public string? Comment { get; set; }

    public string? StudentId { get; set; }
}
