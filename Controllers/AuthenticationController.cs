using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using SystemServiceAPI.Entities.Table;
using SystemServiceAPICore3.Bo.Interface;
using SystemServiceAPICore3.Dto.Other;

namespace SystemServiceAPICore3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationBo authenticationBo;
        private IConfiguration configuration;
        private readonly ITokenService tokenService;
        public AuthenticationController(IAuthenticationBo authenticationBo, IConfiguration configuration, ITokenService tokenService)
        {
            this.authenticationBo = authenticationBo;
            this.configuration = configuration;
            this.tokenService = tokenService;
        }
        
        [HttpPost]
        [Route("Login")]
        public async Task<object> Login([FromBody] AuthenticationDto auth)
        {
            string userName = auth.Username;
            string password = auth.Password;
            
            if(!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(password))
            {
                Account account = await authenticationBo.GetUser(userName, password);
                if(account != null)
                {
                    var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("AccountID", account.AccountID.ToString()),
                        new Claim("UserName", account.UserName),
                        new Claim("Role", account.Role.ToString()),
                        new Claim("Email", account.Email?? String.Empty),
                        new Claim("Active", account.Active.ToString()),
                        new Claim("LastLogin", account.LastLogin?.ToString("dd/MM/yyyy") ?? DateTime.Now.ToString("dd/MM/yyyy"))
                    };

                    var accessToken = tokenService.GenerateAccessToken(claims);
                    var refreshToken = tokenService.GenerateRefreshToken();

                    account.RefreshToken = refreshToken;
                    account.RefreshTokenExpiryTime = DateTime.Now.AddHours(1);
                    authenticationBo.UpdateRefreshToken(account);

                    return Ok(new
                    {
                        Token = accessToken,
                        RefreshToken = refreshToken,
                        Expried = DateTime.Now.AddHours(1)
                    }) ;
                }

                return Unauthorized();
            }

            return BadRequest("Invalid client request");
        }
    }
}
