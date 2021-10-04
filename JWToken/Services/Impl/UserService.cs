using JWToken.Data;
using JWToken.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWToken.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _db;
        public IConfiguration Configuration { get; }
        public UserService(UserDbContext db, IConfiguration configuration)
        {
            _db = db;
            Configuration = configuration;
        }
        //private List<User> _users = new List<User>
        //{
        //    new User { Id = 1, Email = "string", Password = "string", Role = "admin"},
        //    new User { Id = 2, Email = "user2", Password = "password2", Role = "guest"}
        //};

        public string Login(string userName, string password)
        {
            var user = _db.Users.SingleOrDefault(x => x.Email == userName && x.Password == password);

            string SECRET = Configuration.GetSection("secret").Value;

            // return null if user not found
            if (user == null)
            {
                return string.Empty;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SECRET);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.Role, "Guest")
                }),

                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.Token;
        }

        //public JwtSecurityToken verify(string token)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(Startup.SECRET);
        //    tokenHandler.ValidateToken(token,  new TokenValidationParameters
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(key),
        //        ValidateIssuer = false,
        //        ValidateAudience = false
        //    }, out SecurityToken validatedToken);
        //    return (JwtSecurityToken)validatedToken;
        //}
        public string EncodePassword(string pass)
        {
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);
        }

        public string UserLogin(Login login)
        {
            string pass = EncodePassword(login.Password);
            User user = _db.Users.Where(s => s.Email == login.Email).FirstOrDefault();
            if (user == null)
            {
                return "No User Found,Please Sign Up";
                
            }
            else
            {
                if (pass != user.Password)
                {
                    return "Invalid Password";
                }
                else
                {
                    var token = Login(login.Email, pass);

                    if (token == null || token == String.Empty)
                        return "User name or password is incorrect";
                    //Request.Headers.Add("Authorization", "Bearer " + token);
                    //Request.Headers.Add("Authorization", "Bearer " + token);
                    return token;
                }
            }
        }

        public string UserSignUp(User user)
        {
            User newuser = _db.Users.Where(s => s.Email == user.Email).FirstOrDefault();
            if (newuser == null)
            {
                string pass = EncodePassword(user.Password);
                user.Password = pass.ToString();
                _db.Users.Add(user);
                _db.SaveChanges();
                return "Successfully Registered";
            }
            else
            {
                return "User Already Exits!";
                
            }
        }

        public User UserDetails(int id)
        {
            User user = _db.Users.Where(s => s.Id == id).FirstOrDefault();
            return user;
        }
    }
}
