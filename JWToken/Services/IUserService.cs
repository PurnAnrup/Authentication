using JWToken.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace JWToken.Services
{
    public interface IUserService
    {
        string Login(string userName, string password);
        string EncodePassword(string pass);
        string UserLogin(Login login);
        string UserSignUp(User user);
        User UserDetails(int id);
        //JwtSecurityToken verify(string token);
    }
}
