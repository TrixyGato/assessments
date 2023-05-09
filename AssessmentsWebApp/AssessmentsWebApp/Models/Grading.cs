using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssessmentsWebApp.Models;

public partial class Grading
{
    [JsonIgnore]
    public int Id { get; set; }

    public int? Grade { get; set; }

    [JsonIgnore]
    public DateTime? Date { get; set; }

    public string? Comment { get; set; }

    public string? StudentId { get; set; }

    [JsonIgnore]
    public virtual Student? Student { get; set; }
}
