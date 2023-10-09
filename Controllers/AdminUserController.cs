using System;
using Microsoft.AspNetCore.Mvc;
using TicketReservation.Models;
using TicketReservation.Services;

namespace TicketReservation.Controllers;

[Controller]
[Route("api/[controller]")]

public class AdminController : Controller {

    private readonly AdminUserService _adminUserService;

    public AdminController(AdminUserService adminService) {
        _adminUserService = adminService;
    }

    [HttpGet]
   
    public async Task<List<Admin>> Get() {

        return await _adminUserService.GetUsersAsync();
    }


    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Admin admin) {
        await _adminUserService.CreateAsync(admin);
        return CreatedAtAction(nameof(Get), new { id = admin.Id }, admin);


    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] Admin admin) {
        var user1 = await _adminUserService.loginAsync(admin.Email, admin.Password);
        if (user1 == null) {
            return NotFound();
        }
        return Ok(user1);
    }
  
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(string id, [FromBody] Admin admin) {
        await _adminUserService.UpdateUserAsync(id, admin);
        return NoContent();
    }



    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id) {
        await _adminUserService.DeleteAsync(id);
        return NoContent();
    }



}


