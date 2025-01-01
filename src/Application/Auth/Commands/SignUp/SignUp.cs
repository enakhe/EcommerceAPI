﻿using System.ComponentModel.DataAnnotations;
using EcommerceAPI.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Application.User.Commands.SignUp;

public record SignUpCommand : IRequest<IActionResult>
{
    [Required, EmailAddress]
    public required string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public required string Password { get; set; }

    [Required, Compare(nameof(Password))]
    public required string ConfirmPassword { get; set; }
}

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    private readonly IApplicationDbContext _context;

    public SignUpCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("The email field must be a valid email address")
            .NotNull()
            .NotEmpty()
            .WithMessage("The email field is required");

        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull()
            .WithMessage("The password field is required")
            .MinimumLength(6)
            .WithMessage("The minimum length of the password must be more than six characters");

        RuleFor(x => x.ConfirmPassword)
            .NotNull()
            .NotEmpty()
            .WithMessage("The confirmation password field is required.")
            .Matches(x => x.Password)
            .WithMessage("The password and confirmation password do not match.");

    }
}

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, IActionResult>
{
    private readonly IIdentityService _identityService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SignUpCommandHandler(IIdentityService identityService, IHttpContextAccessor httpContextAccessor)
    {
        _identityService = identityService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        if (request == null)
            return new BadRequestObjectResult("Invalid sign up request");

        var signUpResponse = await _identityService.SignUpAsync(request.Email, request.Password);
        if (!signUpResponse.Succeeded)
        {
            _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return new BadRequestObjectResult(new
            {
                error = signUpResponse.Errors
            });
        }

        return new OkObjectResult(new
        {
            data = signUpResponse
        });
    }
}
