using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAwarenessChild
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public string ChildName { get; set; } = null!;

    public string ChildGender { get; set; } = null!;

    public string? ChildDob { get; set; }

    public string? ChildPhone { get; set; }

    public string? ChildAadhaar { get; set; }

    public string? ChildEmail { get; set; }

    public string? ChildPan { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
