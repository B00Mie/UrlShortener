﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Abstract;
using UrlShortener.Models;

namespace UrlShortener.Middleware
{
    public class CheckToken
    {
        private readonly RequestDelegate _next;
        private readonly TokenModel _appSettings;

        public CheckToken(RequestDelegate next, IOptions<TokenModel> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }
        public async Task Invoke(HttpContext context, IJWTUserService userService)
        {
            var token = context.Request.Cookies["Authorization"]?.Split(" ").Last();

            if (!String.IsNullOrWhiteSpace(token))
            {
                AttachUserToContext(context, userService, _appSettings, token);
                context.Request.Headers.Add("Authorization", $"Bearer {token}");
            }

            await _next.Invoke(context);
        }

        private void AttachUserToContext(HttpContext context, IJWTUserService userService, TokenModel settings, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(settings.Key);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
                context.Items["User"] = userService.GetById(userId);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
