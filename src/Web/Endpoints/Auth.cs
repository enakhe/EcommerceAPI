﻿using EcommerceAPI.Application.Auth.Commands.ChangeEmail;
using EcommerceAPI.Application.Auth.Commands.ChangePassword;
using EcommerceAPI.Application.Auth.Commands.GetPasswordResetToken;
using EcommerceAPI.Application.Auth.Commands.LogOut;
using EcommerceAPI.Application.Auth.Commands.ResetPassword;
using EcommerceAPI.Application.Common.Security;
using EcommerceAPI.Application.User.Commands.Login;
using EcommerceAPI.Application.User.Commands.SendOTP;
using EcommerceAPI.Application.User.Commands.SignUp;
using EcommerceAPI.Application.User.Commands.VerifyEmail;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Web.Endpoints;

public class Auth : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(SignIn, "signin")
            .MapPost(SignUp, "signup")
            .MapPost(SendOTP, "send-otp")
            .MapPost(VerifyEmail, "verify-email")
            .MapPost(GetPasswordRestToken, "password-reset-token")
            .MapPost(ResetPassword, "reset-password")
            .MapPost(ChangeEmail, "change-email")
            .MapPost(ChangePassword, "change-password")
            .MapPost(LogOut, "logout");
    }

    public Task<IActionResult> SignIn(ISender send, SignInCommand command)
    {
        return send.Send(command);
    }

    public Task<IActionResult> SignUp(ISender send, SignUpCommand command)
    {
        return send.Send(command);
    }

    [AuthorizeUser]
    public Task<IActionResult> SendOTP(ISender send)
    {
        return send.Send(new SendOTPCommand());
    }

    [AuthorizeUser]
    public Task<IActionResult> VerifyEmail(ISender sender, VerifyEmailCommand command)
    {
        return sender.Send(command);
    }

    public Task<IActionResult> GetPasswordRestToken(ISender sender, GetPasswordResetTokenCommand command)
    {
        return sender.Send(command);
    }

    public Task<IActionResult> ResetPassword(ISender sender, ResetPasswordCommand command)
    {
        return sender.Send(command);
    }

    public Task<IActionResult> ChangePassword(ISender sender, ChangePasswordCommand command)
    {
        return sender.Send(command);
    }

    public Task<IActionResult> ChangeEmail(ISender sender, ChangeEmailCommand command)
    {
        return sender.Send(command);
    }

    public Task<IActionResult> LogOut(ISender sender)
    {
        return sender.Send(new LogOutCommand());
    }
}
