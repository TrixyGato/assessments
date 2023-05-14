using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssessmentsWebApp.Models;

public partial class Teacher
{
    public string Id { get; set; } = null!;

    public string? Username { get; set; }

    [JsonIgnore]
    public virtual ICollection<Subject> Subjects { get; set; } = new List<Subject>();
}
