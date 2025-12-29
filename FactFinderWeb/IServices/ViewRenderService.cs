using FactFinderWeb.ModelsView;
using FactFinderWeb.Utils;

namespace FactFinderWeb.IServices
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
    }

}
