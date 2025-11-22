using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using Microsoft.EntityFrameworkCore;

namespace FactFinderWeb.Services
{
    public class AwarenessServicesRepo  
    {

        private ResellerBoyinawebFactFinderWebContext _context;

        public AwarenessServicesRepo(ResellerBoyinawebFactFinderWebContext context)
        {
            _context = context;
        } 
        /*

        public async Task SaveAwarenessAsync(AwarenessViewModel model)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Save each part individually
                _context.TblffAwarenessProfileDetails.Add(model.ProfileDetail);
                _context.TblffAwarenessFamilyFinancials.Add(model.FamilyFinancial);
                _context.TblffAwarenessSpouses.Add(model.MaritalDetails);
                _context.TblffAwarenessAssumptions.Add(model.Assumptions);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }*/
    }

}
