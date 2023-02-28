using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemServiceAPICore3.Dto.Other
{
    public class AuthenticationDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticationResponse
    {
        public int AccountID { get; set; }
        public string UserName { get; set; }
        public int Role { get; set; }
        public bool Active { get; set; }
        public string Email { get; set; }
        public DateTime? LastLogin { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }

    public class RefreshTokenDto
    {
        public string RefreshToken { get; set; }
        public string Token { get; set; }
    }
}
