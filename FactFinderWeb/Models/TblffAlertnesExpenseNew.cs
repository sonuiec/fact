using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAlertnesExpenseNew
{
    public long Id { get; set; }

    public long? ExpenseId { get; set; }

    public string? Section { get; set; }

    public string? FieldName { get; set; }

    public string? FieldValue { get; set; }

    public string? Type { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual TblffAlertnesExpense? Expense { get; set; }
}
