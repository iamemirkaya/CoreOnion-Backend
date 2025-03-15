using CoreOnion_Backend.Application.Features.Auth.Command.Login;
using CoreOnion_Backend.Application.Features.Auth.Command.PasswordReset;
using CoreOnion_Backend.Application.Features.Auth.Command.RefreshToken;
using CoreOnion_Backend.Application.Features.Auth.Command.Register;
using CoreOnion_Backend.Application.Features.Auth.Command.Revoke;
using CoreOnion_Backend.Application.Features.Auth.Command.UpdatePassword;
using CoreOnion_Backend.Application.Features.Auth.Command.VerifyResetToken;
using CoreOnion_Backend.Application.Interfaces.MailService;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreOnion_Backend.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IMailService mailService;

        public AuthController(IMediator mediator, IMailService mailService)
        {
            this.mediator = mediator;
            this.mailService = mailService;
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommandRequest request)
        {
            await mediator.Send(request);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCommandRequest request)
        {
            var response = await mediator.Send(request);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken(RefreshTokenCommandRequest request)
        {
            var response = await mediator.Send(request);
            return StatusCode(StatusCodes.Status200OK, response);
        }

        [HttpPost]
        public async Task<IActionResult> Revoke(RevokeCommandRequest request)
        {
            await mediator.Send(request);
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpGet]
        public async Task<IActionResult> ExampleMailTest()
        {
            await mailService.SendMailAsync("iamemirkaya@gmail.com", "bbbbbbbbb", "ccccccccccc");
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetCommandRequest passwordResetCommandRequest)
        {
            PasswordResetCommandResponse response = await mediator.Send(passwordResetCommandRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyResetTokenCommandRequest verifyResetTokenCommandRequest)
        {
            VerifyResetTokenCommandResponse response = await mediator.Send(verifyResetTokenCommandRequest);
            return Ok(response);
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            UpdatePasswordCommandResponse response = await mediator.Send(updatePasswordCommandRequest);
            return Ok(response);
        }
    }
}
