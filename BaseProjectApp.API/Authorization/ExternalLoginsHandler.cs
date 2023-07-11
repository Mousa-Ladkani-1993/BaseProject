using AutoMapper;
using BaseProjectApp.API.Authentication;
using BaseProjectApp.API.Middlewares;
using BaseProjectApp.Library.DbModels;
using BaseProjectApp.Library.DbSpsResult;
using BaseProjectApp.Library.Templates.DTOs;
using BaseProjectApp.Library.Templates.Responses;
using BaseProjectApp.Library.Repositories.UnitOfwork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseProjectApp.Library.Utility;
using BaseProjectApp.API.Authorization;
using BaseProjectApp.Library.Templates;
using Microsoft.AspNetCore.Identity;
using BaseProjectApp.API.Controllers;
using BaseProjectApp.Library.Templates.SecurityClasses;
using System.Text.RegularExpressions;
using BaseProjectApp.Library.Templates.Enums;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using Google.Apis.Auth;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;

namespace BaseProjectApp.API.Authorization
{
    public class ExternalLoginsHandler
    {

        private readonly HttpClient _httpClient;

        public ExternalLoginsHandler()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com/v2.8/")
            };
            _httpClient.DefaultRequestHeaders
                .Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<T> GetFacebookData<T>(string accessToken, string endpoint, string args = null)
        {
            var response = await _httpClient.GetAsync($"{endpoint}?access_token={accessToken}&{args}");
            if (!response.IsSuccessStatusCode)
                return default(T);

            var result = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(result);
        }

        public async static Task<GoogleJsonWebSignature.Payload> GetPayload_Google(ExternalAuthDto externalAuth, string ClientId)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { ClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(externalAuth.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                //log an exception
                return null;
            }
        }

        public async static Task<string> GetPayload_Apple(ExternalAuthDto externalAuth, string ClientId, string ClientSecret , string authorization_code)
        {
            var client = new HttpClient();
            var response = client.PostAsync("https://appleid.apple.com/auth/token", new StringContent(
                $"client_id={ClientId}&client_secret={ClientSecret}&grant_type={authorization_code}&code={externalAuth.IdToken}",
                Encoding.UTF8, "application/x-www-form-urlencoded")).Result;
            if (!response.IsSuccessStatusCode)
            {
                // Invalid token
                return "";
            }

            // Token is valid
            var content = response.Content.ReadAsStringAsync().Result;
            var json = JObject.Parse(content);
            string userId = json["sub"].Value<string>();

            return "";
        }
        public static void VerifyAppleIDToken(string token, string clientId)
        {
            //Read the token and get it's claims using System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims;
            SecurityKey publicKey; SecurityToken validatedToken;

            //Get the expiration of the token and convert its value from unix time seconds to DateTime object
            var expirationClaim = claims.FirstOrDefault(x => x.Type == "exp").Value;
            var expirationTime = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationClaim)).DateTime;

            if (expirationTime < DateTime.UtcNow)
            {
                throw new SecurityTokenExpiredException("Expired token");
            }

            using (var httpClient = new HttpClient())
            {
                //Request Apple's JWKS used for verifying the tokens.
                var applePublicKeys = httpClient.GetAsync("https://appleid.apple.com/auth/keys");
                var keyset = new JsonWebKeySet(applePublicKeys.Result.Content.ReadAsStringAsync().Result);

                //Since there is more than one JSON Web Key we select the one that has been used for our token.
                //This is achieved by filtering on the "Kid" value from the header of the token
                publicKey = keyset.Keys.FirstOrDefault(x => x.Kid == jwtSecurityToken.Header.Kid);
            }

            //Create new TokenValidationParameters object which we pass to ValidateToken method of JwtSecurityTokenHandler.
            //The handler uses this object to validate the token and will throw an exception if any of the specified parameters is invalid.

            var validationParameters = new TokenValidationParameters
            {
                ValidIssuer = "https://appleid.apple.com",
                IssuerSigningKey = publicKey,
                ValidAudience = clientId
            };

            tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
        }

    }
}
