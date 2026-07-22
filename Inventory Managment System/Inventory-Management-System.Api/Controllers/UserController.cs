using System.Collections.Generic;
using System.Linq;
using Inventory_Management_System.Api.Application.Ports.Inbound;
using Inventory_Management_System.Api.Domain.Entities;
using Inventory_Management_System.Api.Middlewares;
using Inventory_Management_System.Api.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory_Management_System.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<AuthenticationResponse> Register(RegisterUserRequest request)
        {
            int? performedBy = null;

            if (HttpContext.User.Identity?.IsAuthenticated == true)
            {
                performedBy = HttpContext.GetCurrentUser().Id;
            }

            AuthenticationResult result = _userService.Register(
                request.Name,
                request.Password,
                request.Role,
                performedBy
            );

            return Ok(AuthenticationResponse.FromResult(result));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<AuthenticationResponse> Login(LoginRequest request)
        {
            AuthenticationResult result = _userService.Login(request.Name, request.Password);

            return Ok(AuthenticationResponse.FromResult(result));
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult<UserResponse> GetCurrentUser()
        {
            CurrentUser currentUser = HttpContext.GetCurrentUser();

            User user = _userService.GetUserById(currentUser.Id);

            return Ok(UserResponse.FromDomain(user));
        }

        [Authorize]
        [HttpPut("me")]
        public ActionResult<UserResponse> UpdateCurrentUser(UpdateCurrentUserRequest request)
        {
            CurrentUser currentUser = HttpContext.GetCurrentUser();

            User user = _userService.UpdateCurrentUser(
                currentUser.Id,
                request.Name,
                request.Password
            );

            return Ok(UserResponse.FromDomain(user));
        }

        [Authorize]
        [HttpDelete("me")]
        public IActionResult DeleteCurrentUser()
        {
            CurrentUser currentUser = HttpContext.GetCurrentUser();

            _userService.DeleteCurrentUser(currentUser.Id);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<IReadOnlyList<UserResponse>> GetAllUsers()
        {
            CurrentUser currentUser = HttpContext.GetCurrentUser();

            IReadOnlyList<UserResponse> response = _userService
                .GetAllUsers(currentUser.Id)
                .Select(UserResponse.FromDomain)
                .ToList();

            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id:int}/role")]
        public ActionResult<UserResponse> UpdateRole(int id, UpdateUserRoleRequest request)
        {
            CurrentUser currentUser = HttpContext.GetCurrentUser();

            User user = _userService.UpdateUserRole(currentUser.Id, id, request.Role);

            return Ok(UserResponse.FromDomain(user));
        }
    }
}
