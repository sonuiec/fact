using FactFinderWeb.ModelsView;

namespace FactFinderWeb.IServices
{
    public interface IAwareness
    { 
        Task SaveAwarenessAsync(AwarenessViewModel model);
    }

}
