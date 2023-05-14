using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssessmentsWebApp.Models;

public partial class Subject
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? TeacherId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Grading> Gradings { get; set; } = new List<Grading>();

    [JsonIgnore]
    public virtual Teacher? Teacher { get; set; }
}
