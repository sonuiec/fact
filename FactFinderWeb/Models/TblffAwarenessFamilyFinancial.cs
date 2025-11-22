using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAwarenessFamilyFinancial
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public string? Stock { get; set; }

    public string? Income { get; set; }

    public string? Payment { get; set; }

    public string? Holiday { get; set; }

    public string? Shopping { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Addby { get; set; }
}
