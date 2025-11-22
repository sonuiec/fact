using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FactFinderWeb.Services
{

    public class ExecutionServices
    {
        private ResellerBoyinawebFactFinderWebContext _context;
        private readonly long _userID;
        private readonly HttpContext _httpContext;
        int updateRows = 0;
        private readonly long _profileId;

        public ExecutionServices(ResellerBoyinawebFactFinderWebContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext;
            var userIdStr = _httpContext.Session.GetString("UserId");
            _userID = Convert.ToInt64(userIdStr);
            _profileId = Convert.ToInt64(_httpContext.Session.GetString("profileId"));
        }

        public async Task<int> InvestCheckDataThenUpdate(long profileId, string _planType)
        {
            // Check if any records exist for the given profileId
            bool dataExists = await _context.TblffWingsGoalStep5ExecutionData.AnyAsync(x => x.ProfileId == profileId);

            if (!dataExists)
            {
                await ExecutionAddToTable(_planType);
            }
            else
            {
                await ExecutionUpdateToTable(_planType);
            }
            return 0;
        }
        public async Task<int> ExecutionAddToTable(string _planType)
        {
            var userGoals = await _context.TblffWings.Where(w => w.ProfileId == _profileId).ToListAsync();
            foreach (var goal in userGoals)
            {

                var query = _context.TblffWingsGoalStep5ExecutionMasters.Where(m => m.Goalid == goal.Goalid );
                if (goal.Goalid != 15)
                {
                    if (_planType.ToLower() == "zero2one")
                    {
                        query = query.Where(u => u.DealType == _planType.ToLower());
                    }
                    else
                    {
                        query = query.Where(u => u.DealType == null);
                    }
                }

                var masterExecutions = await query.Distinct() .ToListAsync();

                foreach (var masterItem in masterExecutions)
                {
                    var newExecutionData = new TblffWingsGoalStep5ExecutionDatum
                    {
                        ProfileId = goal.ProfileId,
                        Step5ExecutionMasterid = masterItem.Id,
                        Goalid = (int)goal.Goalid,
                        GoalName = goal.GoalName,
                        ExecutionDescription = masterItem.ExecutionDescription,
                        ExecutionValueType = masterItem.ExecutionValueType,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Wingid = goal.Id,
                        ExecutionValue = masterItem.ExecutionDescription == "Year of World Travel" ? goal.GoalStartYear?.ToString() : null,
                    };
                    _context.TblffWingsGoalStep5ExecutionData.Add(newExecutionData);
                }
            }
            return await _context.SaveChangesAsync();             
        }
        public async Task<int> ExecutionUpdateToTable(string _planType)
        {
            var existingExecutionData = await _context.TblffWingsGoalStep5ExecutionData
                .Where(w => w.ProfileId == _profileId)
                .ToListAsync();

            var userGoals = await _context.TblffWings
                .Where(w => w.ProfileId == _profileId)
                .ToListAsync();

            // Goals missing in ExecutionData → add
            var goalsToAdd = userGoals
                .Where(incoming => !existingExecutionData.Any(existing => existing.Goalid == incoming.Goalid && existing.GoalName == incoming.GoalName))
                .ToList();

            // Goals no longer in Wings but present in ExecutionData → delete
            var goalsToDelete = existingExecutionData
                .Where(existing => !userGoals.Any(incoming => incoming.Goalid == existing.Goalid && existing.GoalName == incoming.GoalName))
                .ToList();

            // Add new goals
            foreach (var goal in goalsToAdd)
            {
                var query = _context.TblffWingsGoalStep5ExecutionMasters
                    .Where(m => m.Goalid == goal.Goalid);

                // ✅ Apply filter only if planType is "zero2one"
                if (goal.Goalid != 15)
                {
                    if (_planType.ToLower() == "zero2one")
                    {
                        query = query.Where(u => u.DealType == _planType.ToLower());
                    }
                    else
                    {
                        query = query.Where(u => u.DealType == null);
                    }
                }

                var masterExecutions = await query.ToListAsync();

                foreach (var masterItem in masterExecutions)
                {
                    var newExecutionData = new TblffWingsGoalStep5ExecutionDatum
                    {
                        ProfileId = goal.ProfileId,
                        Step5ExecutionMasterid = masterItem.Id,
                        Goalid = (int)goal.Goalid,
                        GoalName = goal.GoalName,
                        ExecutionDescription = masterItem.ExecutionDescription,
                        ExecutionValueType = masterItem.ExecutionValueType,
                        CreateDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        Wingid = goal.Id,
                        ExecutionValue = masterItem.ExecutionDescription == "Year of World Travel" ? goal.GoalStartYear?.ToString() : null,
                    };

                    await _context.TblffWingsGoalStep5ExecutionData.AddAsync(newExecutionData);
                }
            }

            // Remove deleted goals
            if (goalsToDelete.Any())
            {
                _context.TblffWingsGoalStep5ExecutionData.RemoveRange(goalsToDelete);
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<int> ExecutionUpdateToTable1()
        {
            var existingExecutionData = await _context.TblffWingsGoalStep5ExecutionData
                                        .Where(w => w.ProfileId == _profileId).ToListAsync();

            var userGoals = await _context.TblffWings.Where(w => w.ProfileId == _profileId).ToListAsync();

            // Identify goals to add
            var goalsToAdd = userGoals.Where(incoming => !existingExecutionData
                                      .Any(existing => existing.Goalid == incoming.Id))
                                      .Select(incoming => new TblffWingsGoalStep5ExecutionDatum
                                      {Goalid = (int)incoming.Id}).ToList();

            // Identify goals to delete
            var goalsToDelete = existingExecutionData.Where(existing => !userGoals
                                                     .Any(incoming => incoming.Id == existing.Goalid)).ToList();

            if (goalsToAdd.Any())
            {
                foreach (var goal in userGoals)
                {
                    if(!goalsToAdd.Any(g => g.Goalid == (int)goal.Id)){continue;} 
                    // Skip if the goal is already in the add list
                    var masterExecutions = await _context.TblffWingsGoalStep5ExecutionMasters
                                                 .Where(m => m.GoalType == goal.GoalType).ToListAsync();

                    foreach (var masterItem in masterExecutions)
                    {
                        var newExecutionData = new TblffWingsGoalStep5ExecutionDatum
                        {
                            ProfileId = goal.ProfileId,
                            Step5ExecutionMasterid = masterItem.Id,
                            Goalid = (int)goal.Id,
                            GoalName = goal.GoalName,
                            ExecutionDescription = masterItem.ExecutionDescription,
                            ExecutionValueType = masterItem.ExecutionValueType,
                            ExecutionValue = masterItem.ExecutionDescription == "Year of World Travel" ? goal.GoalStartYear?.ToString() :null,
                            CreateDate = DateTime.Now,
                            UpdateDate = DateTime.Now,
                        };
                        _context.TblffWingsGoalStep5ExecutionData.Add(newExecutionData);
                    }
                }
            }

            if (goalsToDelete.Any()) { _context.TblffWingsGoalStep5ExecutionData.RemoveRange(goalsToDelete); }

                return await _context.SaveChangesAsync();
              
        }

        public async Task<int> ExecutionUpdateToTableSubmit(ExecutionWithPrecisionModelView executionWithPrecisionModelView)
        { 
            foreach (var executionData in executionWithPrecisionModelView.wingsGoalStep5ExecutionDataList)
            {
                var dataToUpdate = await _context.TblffWingsGoalStep5ExecutionData
                                .FirstOrDefaultAsync(x =>  x.Id == executionData.Id && x.ProfileId == _profileId);
                if (dataToUpdate != null)
                {
                    dataToUpdate.ExecutionValue = executionData.ExecutionValue;
                    dataToUpdate.UpdateDate = DateTime.Now;
                    _context.TblffWingsGoalStep5ExecutionData.Update(dataToUpdate);
                }
            }
            int updateRows = 0;
            updateRows = await _context.SaveChangesAsync();
            return updateRows;
        }


        public async Task<ExecutionWithPrecisionModelView> GetExecutionData(long profileId)
        {
            ExecutionWithPrecisionModelView executionMV = new ExecutionWithPrecisionModelView();
            executionMV.wingsGoalStep5ExecutionDataList = await (
                        from exec in _context.TblffWingsGoalStep5ExecutionData
                        join wings in _context.TblffWings  on exec.Wingid equals (int)wings.Id
                        where exec.ProfileId == profileId && wings.ProfileId == profileId
                        orderby wings.GoalPriority
                        select new WingsGoalStep5ExecutionDataMV
                        {
                            Id = exec.Id,
                            ProfileId = exec.ProfileId,
                            Step5ExecutionMasterid = exec.Step5ExecutionMasterid,
                            Goalid = exec.Goalid,
                            GoalName = wings.GoalName, // from joined table
                            ExecutionDescription = exec.ExecutionDescription,
                            ExecutionValue = exec.ExecutionValue,
                            CreateDate = exec.CreateDate,
                            UpdateDate = exec.UpdateDate, // Optional: add GoalPriority for sorting or display
                            GoalPriority = wings.GoalPriority
                            //        Id = x.Id,
                            //ProfileId = x.ProfileId,
                            //Step5ExecutionMasterid = x.Step5ExecutionMasterid,
                            //Goalid = x.Goalid,
                            //GoalName = x.GoalName,
                            //ExecutionDescription = x.ExecutionDescription,
                            //ExecutionValue = x.ExecutionValue,
                            //CreateDate = x.CreateDate,
                            //UpdateDate = x.UpdateDate
                        }).ToListAsync();
            /*   await _context.TblffWingsGoalStep5ExecutionData.Where(x => x.ProfileId == profileId)
                .Select(x => new WingsGoalStep5ExecutionData*/
            return executionMV;
        }
    }
}

/*
public async Task<Int64> WingsAddExecutionData(TblffWingsGoalStep5ExecutionDatum wings)
{
    var tblwings = new TblffWingsGoalStep5ExecutionDatum();

    //tblwings.Id = wings.Id;
    tblwings.ProfileId = _userID;
    tblwings.Goalid = wings.Goalid;
    tblwings.GoalName = wings.GoalName;
    tblwings.Step5ExecutionMasterid = wings.Step5ExecutionMasterid;
    tblwings.ExecutionDescription = wings.ExecutionDescription;
    tblwings.ExecutionValue = wings.ExecutionValue;
    tblwings.CreateDate = DateTime.Now;
    tblwings.UpdateDate = DateTime.Now;
    _context.TblffWingsGoalStep5ExecutionData.Add(wings);
    await _context.SaveChangesAsync();
    return wings.Id;
}


public async Task<Int64> WingsUpdateExecutionData(TblffWingsGoalStep5ExecutionDatum wings)
{
    var tblwings = new TblffWingsGoalStep5ExecutionDatum();

    tblwings.Id = wings.Id;
    tblwings.Step5ExecutionMasterid = wings.Step5ExecutionMasterid;
    tblwings.ExecutionDescription = wings.ExecutionDescription;
    tblwings.ExecutionValue = wings.ExecutionValue;
    tblwings.CreateDate = DateTime.Now;
    tblwings.UpdateDate = DateTime.Now;
    _context.TblffWingsGoalStep5ExecutionData.Update(wings);
    await _context.SaveChangesAsync();
    return wings.Id;
}

public async Task<Int64> WingsUpdateExecutionDataForWings(List<WingsGoalStep5ExecutionData> wingsGoalStep5ExecutionDatas1)
{
    //var wingsGoalStep5ExecutionDataList = new List<WingsGoalStep5ExecutionData>();
    foreach (var wingsGoalExecuation in wingsGoalStep5ExecutionDatas1)
    {
        var wingsGoalStep5ExecutionData = new TblffWingsGoalStep5ExecutionDatum();
        wingsGoalStep5ExecutionData.ProfileId = _userID;
        //wingsGoalStep5ExecutionData.Goalid = goal.Goalid;
        wingsGoalStep5ExecutionData.GoalName = wingsGoalExecuation.GoalName;
        wingsGoalStep5ExecutionData.ExecutionDescription = null;
        wingsGoalStep5ExecutionData.ExecutionValue = null;
        wingsGoalStep5ExecutionData.CreateDate = DateTime.Now;
        wingsGoalStep5ExecutionData.UpdateDate = DateTime.Now;
        _context.TblffWingsGoalStep5ExecutionData.Add(wingsGoalStep5ExecutionData);
    //_context.TblffWingsGoalStep5ExecutionData.Add(wingsGoalStep5ExecutionDataList);
    }

    Int64 updateRows = 0;
    updateRows = await _context.SaveChangesAsync();
    return updateRows;
}
*/