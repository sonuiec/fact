using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Utils;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;


namespace FactFinderWeb.Services
{

    public class InvestServices
    {

        private ResellerBoyinawebFactFinderWebContext _context;

        int updateRows = 0;
        private readonly long _userID;
        private readonly HttpContext _httpContext;
        private readonly long _profileId;


        public InvestServices(ResellerBoyinawebFactFinderWebContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext;
            var userIdStr = _httpContext.Session.GetString("UserId");
            _userID = Convert.ToInt64(userIdStr);
            _profileId = Convert.ToInt64(_httpContext.Session.GetString("profileId"));
        }

        public async Task<int> InvestCheckDataThenUpdate(long profileId)
        {
            // Check if any records exist for the given profileId
            bool dataExists = await _context.TblffInvestWingsGoals.AnyAsync(x => x.ProfileId == profileId);

            if (!dataExists)
            {
                await InvestAddToTable();
            }
            else
            {
                await InvestUpdateToTable();

            }
            return 0;
        }

        public async Task<InvestViewModel> GetInvestData(long profileId)
        {
            var tblwings = await _context.TblffWings
                .Where(x => x.ProfileId == _profileId)
                .Select(x => new UserWings
                {
                    Id = x.Id,
                    ProfileId = x.ProfileId,
                    GoalPriority = x.GoalPriority,
                })
                .ToListAsync();
            var investData = await _context.TblffInvestWingsGoals.Where(x => x.ProfileId == profileId).ToListAsync();



            InvestViewModel investViewModel = new InvestViewModel();
            investViewModel.InvestMVList = (from invest in investData
                                            join wings in tblwings
                                            on invest.Goalid equals (int?)wings.Id // Cast if needed
                                            orderby wings.GoalPriority
                                            select new InvestMV
                                            {
                                                Id = invest.Id,
                                                ProfileId = invest.ProfileId,
                                                Goalid = invest.Goalid,
                                                GoalPriority = wings.GoalPriority,
                                                GoalName = invest.GoalName,
                                                LumpsumAmount = invest.LumpsumAmount,
                                                Sipamount = invest.Sipamount,
                                                CreateDate = invest.CreateDate,
                                                UpdateDate = invest.UpdateDate
                                            }).OrderBy(x => x.GoalPriority) .ToList();
        
            var tblInvestmasterData =await  _context.TblffInvestWingsGoalMasters
                .Where(x => x.ProfileId == profileId)
                .Select(x => new TblffInvestWingsGoalMaster
                {
                    Id = x.Id,
                    ProfileId = x.ProfileId,
                    AvailableLumpsum = x.AvailableLumpsum,
                    IntendedSipmonthly = x.IntendedSipmonthly,
                    MonthlySavings = x.MonthlySavings,
                    CreateDate = x.CreateDate,
                    UpdateDate = x.UpdateDate
                })
                .FirstOrDefaultAsync();


            if (tblInvestmasterData != null)
            {
                investViewModel.Id = tblInvestmasterData.Id;
                investViewModel.AvailableLumpsum = tblInvestmasterData.AvailableLumpsum;
                investViewModel.IntendedSIPmonthly = tblInvestmasterData.IntendedSipmonthly;
                investViewModel.MonthlySavings = tblInvestmasterData.MonthlySavings;
            }
            else
            { 
                investViewModel.Id = 0;
                investViewModel.AvailableLumpsum = null;
                investViewModel.IntendedSIPmonthly = null;
                investViewModel.MonthlySavings = null; 
            }

            return investViewModel;
        }


        public async Task<int> InvestAddToTable()
        {
            var tblwings = await GetWingsData(_profileId);

            if (tblwings.Count > 0)
            {
                foreach (var goal in tblwings)
                {
                    var investMV = new TblffInvestWingsGoal();
                    investMV.ProfileId = _profileId;
                    investMV.Goalid = (int)goal.Id;
                    investMV.GoalName = goal.GoalName;
                    investMV.LumpsumAmount = 0;
                    investMV.Sipamount = 0;
                    investMV.CreateDate = DateTime.Now;
                    investMV.UpdateDate = DateTime.Now;
                    _context.TblffInvestWingsGoals.Add(investMV);
                }
                return updateRows = await _context.SaveChangesAsync();
            }
            else
            {
                return 0;
            }
        }

        public async Task<List<UserWings>> GetWingsData(long profileId)
        {
            var tblwings = await _context.TblffWings
                .Where(x => x.ProfileId == _profileId)
                .Select(x => new UserWings
                {
                    Id = x.Id,
                    ProfileId = x.ProfileId,
                    GoalType = x.GoalType,
                    GoalPriority = x.GoalPriority,
                    GoalName = x.GoalName,
                    //CreateDate = x.CreateDate,
                    //UpdateDate = x.UpdateDate,
                    NewGoals = x.NewGoals
                })
                .ToListAsync();
            /**/
            return tblwings;
        }
        public async Task<int> InvestUpdateToTable()
        {
            // Fetch existing goals from the database for the current user
            var existingGoals = await _context.TblffInvestWingsGoals
                .Where(x => x.ProfileId == _profileId)
                .ToListAsync();

            // Fetch the incoming goals (e.g., from TblffWings)
            var incomingGoals = await GetWingsData(_profileId);

            // Identify goals to add
            var goalsToAdd = incomingGoals
                .Where(incoming => !existingGoals.Any(existing => existing.Goalid == incoming.Id))
                .Select(incoming => new TblffInvestWingsGoal
                {
                    ProfileId = _profileId,
                    Goalid = (int)incoming.Id,
                    GoalName = incoming.GoalName,
                    LumpsumAmount = 0,
                    Sipamount = 0,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                })
                .ToList();

            // Identify goals to update
            var goalsToUpdate = existingGoals
                .Where(existing => incomingGoals.Any(incoming => incoming.Id == existing.Goalid))
                .ToList();

            foreach (var goal in goalsToUpdate)
            {
                var incomingGoal = incomingGoals.FirstOrDefault(x => x.GoalName == goal.GoalName);
                goal.Goalid = (int)incomingGoal.Id;
                //goal.GoalName = incomingGoal.GoalName;
                goal.UpdateDate = DateTime.Now; // Update other fields as needed
            }

            // Identify goals to delete
            var goalsToDelete = existingGoals
                .Where(existing => !incomingGoals.Any(incoming => incoming.Id == existing.Goalid))
                .ToList();

            // Perform database operations
            if (goalsToAdd.Any())
                await _context.TblffInvestWingsGoals.AddRangeAsync(goalsToAdd);

            if (goalsToDelete.Any())
                _context.TblffInvestWingsGoals.RemoveRange(goalsToDelete);

            _context.TblffInvestWingsGoals.UpdateRange(goalsToUpdate);
            // Save changes to the database
            return updateRows = await _context.SaveChangesAsync();
        }


        public async Task<int> WingsUpdateInvestDataForWings(InvestViewModel investViewModel)
        {
            var tblInvestMaster = await _context.TblffInvestWingsGoalMasters.FirstOrDefaultAsync(x => x.ProfileId == _profileId);

            if (tblInvestMaster == null)
            {
                tblInvestMaster = new TblffInvestWingsGoalMaster
                {
                    ProfileId = _profileId,
                    AvailableLumpsum = investViewModel.AvailableLumpsum,
                    IntendedSipmonthly = investViewModel.IntendedSIPmonthly,
                    MonthlySavings = investViewModel.MonthlySavings,
                    Addby = _userID,
                    Id = 0,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now
                };
                _context.TblffInvestWingsGoalMasters.Add(tblInvestMaster);
            }
            else
            {
                tblInvestMaster.ProfileId = _profileId;
                tblInvestMaster.AvailableLumpsum = investViewModel.AvailableLumpsum;
                tblInvestMaster.IntendedSipmonthly = investViewModel.IntendedSIPmonthly;
                tblInvestMaster.MonthlySavings = investViewModel.MonthlySavings;
                tblInvestMaster.Addby = _userID;
                tblInvestMaster.UpdateDate = DateTime.Now;
                _context.TblffInvestWingsGoalMasters.Update(tblInvestMaster);
            }

            foreach (var investMVgoal in investViewModel.InvestMVList)
            {
                var dataToUpdate = await _context.TblffInvestWingsGoals.FirstOrDefaultAsync(x => x.Goalid == investMVgoal.Goalid && x.ProfileId == _profileId);
                //var dataToUpdate = new TblffInvestWingsGoal();
                //var dataToUpdate =  await _context.TblffInvestWingsGoals.FindAsync(Convert.ToInt64(investMVgoal.Goalid));
                if (dataToUpdate != null)
                {
                    dataToUpdate.Goalid = investMVgoal.Goalid;
                    dataToUpdate.LumpsumAmount = investMVgoal.LumpsumAmount;
                    dataToUpdate.Sipamount = investMVgoal.Sipamount;
                    dataToUpdate.UpdateDate = DateTime.Now;
                    _context.TblffInvestWingsGoals.Update(dataToUpdate);
                }
            }
            updateRows = await _context.SaveChangesAsync();

            return updateRows;
        }
    }
}
