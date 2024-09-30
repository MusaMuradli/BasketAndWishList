using FastKart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastKart.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    [Authorize(Roles = $"{RoleConstans.Admin}, {RoleConstans.Moderator}")]
    public class AdminController : Controller
    {
        
    }
}
