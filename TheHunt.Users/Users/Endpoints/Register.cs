using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using TheHunt.Common.Model;

namespace TheHunt.Users.Users.Endpoints
{
    public class Register(IUserService userService,
        UserManager<User> userManager) :
        Endpoint<Endpoints.RegisterRequest, RegisterResponse>
    {
        private readonly IUserService _userService = userService;
        private readonly UserManager<User> _userManager = userManager;

        public override void Configure()
        {
            Post("/register");
            AllowAnonymous();
        }

        public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
        {
            try
            {
                var existingUser = await _userService.GetUserByEmailAsync(req.Email);
                List<ValidationFailure> validationErrors = [];
                if (existingUser is not null)
                {
                    // todo: validate model properly
                    validationErrors.Add(new ValidationFailure("Email", "A user already exists with that email address.")
                    {
                        AttemptedValue = req.Email
                    });
                }
                if (validationErrors.Count > 0)
                    await HttpContext.Response.SendErrorsAsync(validationErrors, StatusCodes.Status409Conflict, cancellation: ct);

                User user = new()
                {
                    Email = req.Email,
                    UserName = req.UserName,
                    JoinedDate = DateTime.UtcNow,
                    LastEditedDate = DateTime.UtcNow
                };

                var createUserResult = await _userManager.CreateAsync(user, req.Password);

                if (!createUserResult.Succeeded)
                {
                    foreach (var error in createUserResult.Errors)
                    {
                        validationErrors.Add(new ValidationFailure(error.Code, error.Description));
                    }
                    await HttpContext.Response.SendErrorsAsync(validationErrors, StatusCodes.Status400BadRequest, cancellation: ct);
                }

                var addClaimUserResult = await _userManager.AddClaimAsync(user, new Claim(AuthConstants.FreeMemberUserClaimName, "true"));

                if (!addClaimUserResult.Succeeded)
                {
                    foreach (var error in addClaimUserResult.Errors)
                    {
                        validationErrors.Add(new ValidationFailure(error.Code, error.Description));
                    }
                    await HttpContext.Response.SendErrorsAsync(validationErrors, StatusCodes.Status400BadRequest, cancellation: ct);
                }

                var response = new RegisterResponse(user.Id, user.Email, user.UserName, user.JoinedDate);

                await HttpContext.Response.SendCreatedAtAsync<GetById>(new { user.Id }, response);
            }
            catch (Exception e)
            {
                await HttpContext.Response.SendAsync(e.Message, StatusCodes.Status500InternalServerError, cancellation: ct);
            }
        }
    }
}
