using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace BaseProjectApp.Library.Utility
{
    public class ResetPasswordUtility
    {

        public static string GenerateResetJwtToken(string UserEmail, string ResetCode, string Secret)
        {
            //set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddMinutes(30);
            // DateTime expires = DateTime.Now;
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("UserEmail", UserEmail.ToString(), ClaimValueTypes.String));
            claimsIdentity.AddClaim(new Claim("ResetCode", ResetCode.ToString(), ClaimValueTypes.String));
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(
                        // issuer: issuer,
                        // audience: audience,
                        subject: claimsIdentity,
                        notBefore: issuedAt,
                        expires: expires,
                        signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public static string GenerateResetPasswordCode(int length = 20)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }

        public static (bool, string, string) ValidateToken(string token, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters(secret);

            SecurityToken validatedToken;
            try
            {
                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var jwt = tokenHandler.ReadJwtToken(token);
                var resetCode = jwt.Claims.FirstOrDefault(claim => claim.Type == "ResetCode").Value;
                var userEmail = jwt.Claims.FirstOrDefault(claim => claim.Type == "UserEmail").Value;
                return (true, resetCode, userEmail);
            }
            catch (System.Exception)
            {
                return (false, null, null);
            }

        }

        private static TokenValidationParameters GetValidationParameters(string secret)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)) // The same key as the one that generate the token
            };
        }
    }
}
