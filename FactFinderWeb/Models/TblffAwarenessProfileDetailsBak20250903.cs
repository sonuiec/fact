using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAwarenessProfileDetailsBak20250903
{
    public long Id { get; set; }

    public long ProfileId { get; set; }

    public string PlanType { get; set; } = null!;

    public int PlanYear { get; set; }

    public string? PlanDuration { get; set; }

    public string Name { get; set; } = null!;

    public string? Gender { get; set; }

    public string? Dob { get; set; }

    public string Phone { get; set; } = null!;

    public string? Altphone { get; set; }

    public string? Aadhaar { get; set; }

    public string? Email { get; set; }

    public string? SecEmail { get; set; }

    public string? Pan { get; set; }

    public string? MaritalStatus { get; set; }

    public string? HaveChildren { get; set; }

    public string? Education { get; set; }

    public string? Hobbies { get; set; }

    public string? Occupation { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyAddress { get; set; }

    public string? CompanyCity { get; set; }

    public string? CompanyState { get; set; }

    public string? CompanyCountry { get; set; }

    public string? CompanyPincode { get; set; }

    public bool? IsSameAddress { get; set; }

    public string? ResAddress { get; set; }

    public string? ResCity { get; set; }

    public string? ResState { get; set; }

    public string? ResCountry { get; set; }

    public string? ResPincode { get; set; }

    public string? PermAddress { get; set; }

    public string? PermCity { get; set; }

    public string? PermState { get; set; }

    public string? PermCountry { get; set; }

    public string? Stock { get; set; }

    public string? Income { get; set; }

    public string? Payment { get; set; }

    public string? Holiday { get; set; }

    public string? Shopping { get; set; }

    public string? PermPincode { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? UpdateDate { get; set; }

    public string? Addby { get; set; }

    public long? Addbyid { get; set; }

    public string? ProfileStatus { get; set; }

    public int? Advisorid { get; set; }

    public string? AdvisorName { get; set; }

    public string? Awakenid { get; set; }
}
