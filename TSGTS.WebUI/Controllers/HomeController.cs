using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TSGTS.WebUI.Models;

namespace TSGTS.WebUI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View("Landing");
    }

    [HttpGet]
    [Microsoft.AspNetCore.Authorization.Authorize]
    public IActionResult Dashboard()
    {
        return View("Index"); // dashboard
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
