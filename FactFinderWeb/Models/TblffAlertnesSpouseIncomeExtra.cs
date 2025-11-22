using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAlertnesSpouseIncomeExtra
{
    public int Id { get; set; }

    public long? IncomeDetailId { get; set; }

    public string? FieldName { get; set; }

    public decimal? FieldValue { get; set; }

    public string? Type { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }
}
