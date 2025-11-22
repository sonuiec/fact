using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblFfRegisterUser
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Mobile { get; set; } = null!;

    public string? Plantype { get; set; }

    public string? Emailverified { get; set; }

    public string? Mobileverified { get; set; }

    public string? Activestatus { get; set; }

    public string? Approvedbyadmin { get; set; }

    public DateTime? Createddate { get; set; }

    public DateTime? Updatedate { get; set; }

    public string? Ptx { get; set; }

    public int? Advisorid { get; set; }

    public string? AdvisorName { get; set; }
}
