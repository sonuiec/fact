using System;
using System.Collections.Generic;

namespace FactFinderWeb.Models;

public partial class TblffAlertnesExpense
{
    public long Id { get; set; }

    public long? ProfileId { get; set; }

    public string? GroceryProvisionMilk { get; set; }

    public string? DomesticHelp { get; set; }

    public string? IronLaundry { get; set; }

    public string? Driver { get; set; }

    public string? Fuel { get; set; }

    public string? CarCleaning { get; set; }

    public string? Maintenance { get; set; }

    public string? TaxiPublicTransport { get; set; }

    public string? AirTrainTravel { get; set; }

    public string? Mobile { get; set; }

    public string? LandlineBroadband { get; set; }

    public string? DataCard { get; set; }

    public string? Electricity { get; set; }

    public string? HouseTax { get; set; }

    public string? SocietyCharge { get; set; }

    public string? Rents { get; set; }

    public string? Cable { get; set; }

    public string? Lpg { get; set; }

    public string? WaterBill { get; set; }

    public string? NewsPaper { get; set; }

    public string? SchoolFees { get; set; }

    public string? Tuitions { get; set; }

    public string? UniformsAccessories { get; set; }

    public string? BooksStationery { get; set; }

    public string? PicnicsActivities { get; set; }

    public string? MoviesTheatre { get; set; }

    public string? DiningOut { get; set; }

    public string? ClubhouseExpenses { get; set; }

    public string? PartiesAtHome { get; set; }

    public string? ClothingGrooming { get; set; }

    public string? VacationTravel { get; set; }

    public string? Festivals { get; set; }

    public string? KidsBirthdays { get; set; }

    public string? FamilyFunctions { get; set; }

    public string? Medical { get; set; }

    public string? VehicleServicing { get; set; }

    public string? HomeRepair { get; set; }

    public string? NewHomeAppliances { get; set; }

    public string? LifeInsurance { get; set; }

    public string? HomeInsurance { get; set; }

    public string? MedicalInsurance { get; set; }

    public string? CarInsurance { get; set; }

    public string? FeeToCa { get; set; }

    public string? OtherConsultant { get; set; }

    public string? Donations { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public virtual ICollection<TblffAlertnesExpenseNew> TblffAlertnesExpenseNews { get; set; } = new List<TblffAlertnesExpenseNew>();
}
