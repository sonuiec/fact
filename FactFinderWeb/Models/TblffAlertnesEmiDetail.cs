using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAlertnesEmiDetail
{
    public int Id { get; set; }

    public long? ProfileId { get; set; }

    public string? Name { get; set; }

    public decimal? Outstanding { get; set; }

    public decimal? Interest { get; set; }

    public decimal? Principal { get; set; }

    public decimal? Monthly { get; set; }

    public DateOnly? Till { get; set; }

    public bool? FollowUp { get; set; }

    public bool? IsNew { get; set; }
}
