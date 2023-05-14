using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AssessmentsWebApp.Models;

public partial class StreamStudent
{
    public string Id { get; set; } = null!;

    public string? StreamId { get; set; }

    public string? StudentId { get; set; }

    [JsonIgnore]
    public virtual Stream? Stream { get; set; }

    [JsonIgnore]
    public virtual Student? Student { get; set; }
}