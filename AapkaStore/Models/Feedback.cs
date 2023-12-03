using System;
using System.Collections.Generic;

namespace AapkaStore.Models;

public partial class Feedback
{
    public int FeedBackId { get; set; }

    public int? UserFid { get; set; }

    public string FeedbackType { get; set; } = null!;

    public string Message { get; set; } = null!;

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }

    public string? Status { get; set; }

    public virtual User? UserF { get; set; }
}
