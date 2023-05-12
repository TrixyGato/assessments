using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssessmentsWebApp.Models;

public partial class Grading
{
    public string Id { get; set; } = null!;

    public int? Grade { get; set; }

    public string? Comment { get; set; }

    public DateTime? Date { get; set; }

    public string? StudentId { get; set; }

    public string? StreamId { get; set; }

    public string? SubjectId { get; set; }

    [JsonIgnore]
    public virtual Stream? Stream { get; set; }
    [JsonIgnore]
    public virtual Student? Student { get; set; }
    [JsonIgnore]
    public virtual Subject? Subject { get; set; }
}
