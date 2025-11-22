using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblFfAlertnesNewOtherAsset
{
    public int Id { get; set; }

    public int? NetWorthId { get; set; }

    public long ProfileId { get; set; }

    public string? AssetName { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? CreatedAt { get; set; }
}
