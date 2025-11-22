using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;

namespace FactFinderWeb.Services
{
    public class KnowledgeThatMattersServices
    {
        private ResellerBoyinawebFactFinderWebContext _context;
         private readonly long _userID;
        private readonly HttpContext _httpContext;
        private readonly long _profileId;

        public KnowledgeThatMattersServices(ResellerBoyinawebFactFinderWebContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContext = httpContextAccessor.HttpContext;
            var userIdStr = _httpContext.Session.GetString("UserId");
            _userID = Convert.ToInt64(userIdStr);
            _profileId = Convert.ToInt64(_httpContext.Session.GetString("profileId"));
        }

        public async Task<MVKnowledgeRisk> KnowledgeThatMattersView()
        {
            var mvKnowledgeRisk = await _context.TblffKnowledgeRisks
                .Where(x => x.ProfileId == _profileId)
                .Select(x => new MVKnowledgeRisk
                {
                    Id = x.Id,
                    ProfileId = x.ProfileId,
                    RiskCapacity = x.RiskCapacity,
                    RiskRequirement = x.RiskRequirement,
                    RiskTolerance = x.RiskTolerance,
                    TotalRiskProfileScore=x.TotalRiskProfileScore,
                    PlannerAssessmentOnRiskProfile=x.PlannerAssessmentOnRiskProfile,
                    CreateDate = x.CreateDate,
                    UpdateDate = x.UpdateDate
                })
                .FirstOrDefaultAsync(); 

            return mvKnowledgeRisk;
        }

        public async Task<Int64> KnowledgeThatMattersAddUpdate(MVKnowledgeRisk mvKnowledge)
        {
            int AddorUpdate = 0;
            TblffKnowledgeRisk KnowledgeRisk = await _context.TblffKnowledgeRisks.FirstOrDefaultAsync(x => x.ProfileId == _profileId);
            if (KnowledgeRisk == null)
            {
                AddorUpdate = 1;
                TblffKnowledgeRisk TblffKnowledgeRiskvar = new TblffKnowledgeRisk();
                KnowledgeRisk = TblffKnowledgeRiskvar;
            }

            KnowledgeRisk.RiskCapacity = mvKnowledge.RiskCapacity;
            KnowledgeRisk.RiskRequirement = mvKnowledge.RiskRequirement;
            KnowledgeRisk.RiskTolerance = mvKnowledge.RiskTolerance;
            KnowledgeRisk.TotalRiskProfileScore = mvKnowledge.TotalRiskProfileScore;
            KnowledgeRisk.PlannerAssessmentOnRiskProfile = mvKnowledge.PlannerAssessmentOnRiskProfile;

            KnowledgeRisk.ProfileId = _profileId;
 
            if (AddorUpdate == 1)
            {
                KnowledgeRisk.CreateDate = DateTime.Now;
                KnowledgeRisk.UpdateDate = DateTime.Now;
                _context.TblffKnowledgeRisks.Add(KnowledgeRisk);
            }
            else
            {
                KnowledgeRisk.UpdateDate = DateTime.Now;
                _context.TblffKnowledgeRisks.Update(KnowledgeRisk);
            }

            return  await _context.SaveChangesAsync();
        }

        public async Task<Int64> KnowledgeThatMattersUpdate(MVKnowledgeRisk mvKnowledge)
        {

            var tblknowledge = new TblffKnowledgeRisk
            {
                ProfileId = _profileId,
                RiskCapacity = mvKnowledge.RiskCapacity,
                RiskRequirement = mvKnowledge.RiskRequirement,
                RiskTolerance = mvKnowledge.RiskTolerance,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };

            _context.TblffKnowledgeRisks.Update(tblknowledge);
            await _context.SaveChangesAsync();

            return tblknowledge.Id;
        }
    }
}
