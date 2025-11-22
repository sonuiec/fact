using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAwarenessSpouse
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public string SpouseName { get; set; } = null!;

    public string SpouseGender { get; set; } = null!;

    public string? SpouseDob { get; set; }

    public string? SpousePhone { get; set; }

    public string? SpouseAltPhone { get; set; }

    public string? SpouseAadhaar { get; set; }

    public string? SpouseEmail { get; set; }

    public string? SpouseSecEmail { get; set; }

    public string? SpousePan { get; set; }

    public string? SpouseOccupation { get; set; }

    public string? SpouseCompanyName { get; set; }

    public string? SpouseCompanyAddress { get; set; }

    public string? SpouseCompanyCity { get; set; }

    public string? SpouseCompanyState { get; set; }

    public string? SpouseCompanyCountry { get; set; }

    public string? SpouseCompanyPincode { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }
}
