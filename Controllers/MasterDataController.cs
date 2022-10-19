using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SystemServiceAPI.Bo.Interface;
using SystemServiceAPI.Dto.BillDto;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SystemServiceAPI.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]
    public class MasterDataController : ControllerBase
    {
        private readonly IMasterData _masterBo;
        private readonly ILogger<MasterDataController> _logger;
        public MasterDataController(IMasterData masterBo, ILogger<MasterDataController> logger)
        {
            _masterBo = masterBo;
            _logger = logger;
        }

        [HttpGet]
        [Route("GetServices")]
        public async Task<object> GetServices()
        {
            try
            {
                var result = await _masterBo.GetServices();
                if (result != null)
                {
                    return Ok(result);
                }

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("GetRetails")]
        public async Task<object> GetRetails()
        {
            try
            {
                var result = await _masterBo.GetRetails();
                if (result != null)
                {
                    return Ok(result);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("GetBanks")]
        public async Task<object> GetBanks()
        {
            try
            {
                var result = await _masterBo.GetBanks();
                if (result != null)
                {
                    return Ok(result);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }

        [HttpGet]
        [Route("GetVillages")]
        public async Task<object> GetVillages()
        {
            try
            {
                var result = await _masterBo.GetVillages();
                if (result != null)
                {
                    return Ok(result);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw;
            }
        }
    }
}

