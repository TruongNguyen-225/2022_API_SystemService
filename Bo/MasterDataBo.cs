using System.Linq;
using System.Threading.Tasks;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Context;

namespace SystemServiceAPI.Bo
{
    public class MasterDataBo : IMasterData
    {
        private readonly AppDbContext _dbContext;
        public MasterDataBo(AppDbContext appDbContext)
        {
            _dbContext = appDbContext;
        }

        public async Task<object> GetServices()
        {
            var data = _dbContext.Services;
            if (data != null)
            {
                var result = data.ToList();

                return await Task.FromResult(result);
            }

            return await Task.FromResult(default(object));
        }

        public async Task<object> GetRetails()
        {
            var data = _dbContext.Retails;
            if (data != null)
            {
                var result = data.ToList();

                return await Task.FromResult(result);
            }

            return await Task.FromResult(default(object));
        }

        public async Task<object> GetBanks()
        {
            var data = _dbContext.Banks;
            if (data != null)
            {
                var result = data.ToList();

                return await Task.FromResult(result);
            }

            return await Task.FromResult(default(object));
        }

        public async Task<object> GetVillages()
        {
            var data = _dbContext.Villages;
            if (data != null)
            {
                var result = data.ToList();

                return await Task.FromResult(result);
            }

            return await Task.FromResult(default(object));
        }
    }
}
