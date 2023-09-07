using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YCTest.Models;
using static YCTest.Models.YC;
using static YCTest.Models.DTO;

namespace YCTest.Controllers;

public class HomeController : Controller
{
    private readonly YCDbContext _db;

    public HomeController(YCDbContext dbComtext)
    {
        _db = dbComtext;
    }

    public IActionResult Index() => View(_db.House.ToList());
    public IActionResult House()
    {
        ViewBag.sales = _db.Sales.ToList();
        return View();
    }
    public IActionResult HouseDetail(int id)
    {
        ViewBag.sales = _db.Sales.ToList();
        return View(_db.House.Find(id));
    }
    public IActionResult Sales() => View();
    public IActionResult SalesDetail(Guid id) => View(_db.Sales.Find(id));
    public IActionResult Detail(int id)
    {
        var surce = _db.House.Include(h => h.AttributionSales).SingleOrDefault(h => h.Id == id);
        var data = new YCPriview
        {
            Title = surce?.Title,
            Local = surce?.Local,
            Master = surce?.Master,
            MasterPhone = surce?.MasterPhone,
            SelseName = surce?.AttributionSales?.Name,
            SelsePhoneNumber = surce?.AttributionSales?.PhoneNumber,
            Sid = surce?.AttributionSales?.Id ?? Guid.Empty,
            Hid = surce?.Id ?? 0,
            Price = surce?.Price ?? 0
        };
        return View(data);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

}