using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssessmentsWebApp.Models;

public partial class Student
{
    public string Id { get; set; } = null!;

    public string? Username { get; set; }

    [JsonIgnore]
    public virtual ICollection<Grading> Gradings { get; set; } = new List<Grading>();
}
