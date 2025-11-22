using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAwarenessAddress
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public string AddressType { get; set; } = null!;

    public string? Address { get; set; }

    public string? Country { get; set; }

    public int? CountryCode { get; set; }

    public string? State { get; set; }

    public int? StateCode { get; set; }

    public string? City { get; set; }

    public string? PinCode { get; set; }

    public string? Company { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
