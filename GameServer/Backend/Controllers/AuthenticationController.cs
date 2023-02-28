using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System.Numerics;

namespace Backend.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase {
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IAuthenticationService authenticationService) {
        _authenticationService = authenticationService;
    }

    [HttpPost("register")]
    public IActionResult Register(AuthenticationRequest request) {
        var (result, content) = _authenticationService.Register(request.UserName, request.Password);
        if (!result) {
            return BadRequest(content);
        }

        return Login(request);
    }

    // Not currently being used by game client. But the API has it for future development
    [HttpPost("login")]
    public IActionResult Login(AuthenticationRequest request) {
        var (result, content) = _authenticationService.Login(request.UserName, request.Password);
        if (!result) {
            return BadRequest(content);
        }
        return Ok(new AuthenticationResponse() { Token = content });
    }
}
