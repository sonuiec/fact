using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblFfAdminUser
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string Email { get; set; } = null!;

    public string? Password { get; set; }

    public string? Mobile { get; set; }

    public string? Adminuserid { get; set; }

    public string? AdminRole { get; set; }

    public string? Department { get; set; }

    public string? AccountStatus { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Accesskey { get; set; }
}
