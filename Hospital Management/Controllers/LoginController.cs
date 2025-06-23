using Hospital_Management.Models;
using Hospital_Management.Services;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly LoginService _service;
    public LoginController(LoginService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Execute([FromBody] LoginDTO requestModel)
    {
        var response = await _service.LoginAsync(requestModel);

        return response.Success ? Ok(response) : BadRequest(response);
    }

}
