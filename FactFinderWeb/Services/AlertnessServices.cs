using FactFinderWeb.IServices;
using FactFinderWeb.Models;
using FactFinderWeb.ModelsView;
using FactFinderWeb.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;


namespace FactFinderWeb.Services
{
    public class AlertnessServices
    {
        private ResellerBoyinawebFactFinderWebContext _context;

        public AlertnessServices(ResellerBoyinawebFactFinderWebContext context)
        {
            _context = context;
        }

        public async Task<Int64> AlertnessAdd(TblffAwarenessProfileDetail alertness)
        {
            alertness.UpdateDate = DateTime.Now;
            //user.Password = CommonUtillity.EncryptData(user.Password);
            //alertness.Mobile = "0000000000";
            //alertness.Createddate = DateTime.Now;
            _context.TblffAwarenessProfileDetails.Add(alertness);

            await _context.SaveChangesAsync();

            return alertness.ProfileId;
        }

    }
}
