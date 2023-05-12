using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssessmentsWebApp.Models;

public partial class Stream
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }


    [JsonIgnore]
    public virtual ICollection<Grading> Gradings { get; set; } = new List<Grading>();
}
