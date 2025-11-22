using FactFinderWeb.ModelsView;
using FactFinderWeb.Utils;

namespace FactFinderWeb.IServices
{
	public interface IUser
	{
		void CreateOrUpdate(MVLogin UserView);

		Task<bool> Authenticate(MVLogin userLoginView);   //login
		Task<bool> Register(MVLoginRegister userRegisterView);
		Task<bool> Update(MVLogin userRegisterView);

		/*
		Task<PaginatedResult<MVLogin>> GetAll(string searchTerm = null, string sortBy = "Name",
		bool sortDescending = false, int pageNumber = 1, int pageSize = 10);

		Task<MVLogin> GetById(int id);
		Task<MVLogin> GetByEmail(string email);
		Task<MVLogin> DeleteById(int id);
		//AuthenticateView Authenticate(MVLogin userLoginView);
		*/
	}

}
