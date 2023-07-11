using BaseProjectApp.Library.Templates;
using BaseProjectApp.Library.Templates.Responses;
using BaseProjectApp.Library.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Principal;

namespace BaseProjectApp.API.Authentication
{
    public static class TokenManager
    {
        public static string CreateToken(string UserId, string UserEmail, bool Admin, string secret, string issuer, string audience)
        {
            //set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("UserId", UserId.ToString(), ClaimValueTypes.String));
            claimsIdentity.AddClaim(new Claim("UserEmail", UserEmail.ToString(), ClaimValueTypes.String));
            claimsIdentity.AddClaim(new Claim("Admin", Admin.ToString(), ClaimValueTypes.String));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: issuer,
                        audience: audience,
                        subject: claimsIdentity,
                        notBefore: issuedAt,
                        expires: expires,
                        signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);




            return tokenString;
        }

        public static string GenerateToken(string UserId, string UserEmail, bool Admin, string secret, string issuer, string audience, string Permissions, int? ClientId = null)
        {
            //set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(10);
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim(CustomJWTClaimTypes.UserID, UserId.ToString(), ClaimValueTypes.String));
            claimsIdentity.AddClaim(new Claim(CustomJWTClaimTypes.Email, UserEmail.ToString(), ClaimValueTypes.String));
            claimsIdentity.AddClaim(new Claim(CustomJWTClaimTypes.Admin, Admin.ToString(), ClaimValueTypes.String));
            claimsIdentity.AddClaim(new Claim(CustomJWTClaimTypes.Permessions, Permissions, ClaimValueTypes.String));
            claimsIdentity.AddClaim(new Claim(CustomJWTClaimTypes.ClientId, ClientId.ToString(), ClaimValueTypes.Integer));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: issuer,
                        audience: audience,
                        subject: claimsIdentity,
                        notBefore: issuedAt,
                        expires: expires,
                        signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);


            return tokenString;
        }

        //public static string GenerateToken(string UserId, string UserEmail, bool Admin, string secret, string issuer, string audience,string Permissions)
        //{
        //    //set issued at date
        //    DateTime issuedAt = DateTime.UtcNow;
        //    //set the time when it expires
        //    DateTime expires = DateTime.UtcNow.AddDays(10);
        //    var tokenHandler = new JwtSecurityTokenHandler();

        //    //create a identity and add claims to the user which we want to log in
        //    ClaimsIdentity claimsIdentity = new ClaimsIdentity();
        //    claimsIdentity.AddClaim(new Claim("UserId", UserId.ToString(), ClaimValueTypes.String));
        //    claimsIdentity.AddClaim(new Claim("UserEmail", UserEmail.ToString(), ClaimValueTypes.String));
        //    claimsIdentity.AddClaim(new Claim("Admin", Admin.ToString(), ClaimValueTypes.String));
        //    claimsIdentity.AddClaim(new Claim(CustomJWTClaimTypes.Permessions,Permissions, ClaimValueTypes.String));

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        //    var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    //create the jwt
        //    var token =
        //        (JwtSecurityToken)
        //            tokenHandler.CreateJwtSecurityToken(issuer: issuer,

        //                subject: claimsIdentity,
        //                notBefore: issuedAt,
        //                expires: expires,                        audience: audience,
        //                signingCredentials: signingCredentials);

        //    var tokenString = tokenHandler.WriteToken(token);


        //    return tokenString;
        //}

        //public static async Task<string> CreateRefreshToken(UserManager<IdentityUser> _userManager, IdentityUser _user)
        //{
        //    await _userManager.RemoveAuthenticationTokenAsync(_user, "BaseProjectApp.API", "RefreshToken");
        //    var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, "BaseProjectApp.API", "RefreshToken");
        //    var result = await _userManager.SetAuthenticationTokenAsync(_user, "BaseProjectApp.API", "RefreshToken", newRefreshToken);
        //    return newRefreshToken;
        //}

        public static async Task<TokenRequest> VerifyRefreshToken(UserManager<IdentityUser> _userManager, TokenRequest request, string secret, string issuer, string audience, string Permissions = "")
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var userId = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == "UserId")?.Value;
            var UserEmail = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == "UserEmail")?.Value;
            var Admin = DbTypeConvertor.ToNullableBool(tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == "Admin")?.Value) ?? false;
            var ClientId = DbTypeConvertor.ToNullableInt(tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == "ClientId")?.Value);


            IdentityUser _user = await _userManager.FindByIdAsync(userId);
            try
            {
                var isValid = await _userManager.
                    VerifyUserTokenAsync(_user, "BaseProjectApp.API", "RefreshToken", request.RefreshToken);

                if (isValid)
                {
                    return new TokenRequest
                    {
                        Token = GenerateToken(userId, UserEmail, Admin, secret, issuer, audience, Permissions, ClientId)
                        ,
                        RefreshToken = await CreateRefreshToken(_userManager, _user)
                    };
                }
                await _userManager.UpdateSecurityStampAsync(_user);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return null;
        }

        public static string CreateToken(string UserId, string UserEmail, bool Admin, string secret, string issuer, string audience, int? ClientId = null)
        {
            //set issued at date
            DateTime issuedAt = DateTime.Now;
            //set the time when it expires
            DateTime expires = DateTime.Now.AddDays(7);
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("UserId", UserId.ToString(), ClaimValueTypes.String));
            claimsIdentity.AddClaim(new Claim("UserEmail", UserEmail.ToString(), ClaimValueTypes.String));
            claimsIdentity.AddClaim(new Claim("Admin", Admin.ToString(), ClaimValueTypes.String));
            claimsIdentity.AddClaim(new Claim("ClientId", ClientId.ToString(), ClaimValueTypes.Integer));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: issuer,
                        audience: audience,
                        subject: claimsIdentity,
                        issuedAt: issuedAt,
                        notBefore: issuedAt,
                        expires: expires,
                        signingCredentials: signingCredentials);

            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        public static async Task<string> CreateRefreshToken(UserManager<IdentityUser> _userManager, IdentityUser _user)
        {
            try
            {
                await _userManager.RemoveAuthenticationTokenAsync(_user, "BaseProjectApp.API", "RefreshToken");

                var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, "BaseProjectApp.API", "RefreshToken");

                var result = await _userManager.SetAuthenticationTokenAsync(_user, "BaseProjectApp.API", "RefreshToken", newRefreshToken);

                return newRefreshToken;
            }
            catch (Exception ex) { }

            return "";
        }

        public static async Task<TokenRequest> VerifyRefreshToken(UserManager<IdentityUser> _userManager, TokenRequest request, string secret, string issuer, string audience)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var userId = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == "UserId")?.Value;
            var UserEmail = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == "UserEmail")?.Value;
            var Admin = DbTypeConvertor.ToNullableBool(tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == "Admin")?.Value) ?? false;
            var ClientId = DbTypeConvertor.ToNullableInt(tokenContent.Claims.ToList().FirstOrDefault(q => q.Type == "ClientId")?.Value);

            try
            {

                IdentityUser _user = await _userManager.FindByIdAsync(userId);
                 
                var isValid = await _userManager.VerifyUserTokenAsync(_user, "BaseProjectApp.API","RefreshToken", request.RefreshToken);

                if (isValid)
                {
                    return new TokenRequest
                    {
                        Token = CreateToken(userId, UserEmail, Admin, secret, issuer, audience, ClientId),
                        RefreshToken = await CreateRefreshToken(_userManager, _user),
                        UserId = userId,
                    };
                }

                await _userManager.UpdateSecurityStampAsync(_user);
            }

            catch (Exception ex)
            {
                return null;
            }

            return null;
        }




        public static bool ValidateToken(string authToken, string secret, string issuer, string audience)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters(secret, issuer, audience);

                SecurityToken validatedToken;
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);

                return validatedToken != null && validatedToken.ValidTo > DateTime.Now;
            }
            catch (Exception ex)
            { return false; }
        }

        private static TokenValidationParameters GetValidationParameters(string secret, string issuer, string audience)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)) // The same key as the one that generate the token
            };
        }
    }
}
