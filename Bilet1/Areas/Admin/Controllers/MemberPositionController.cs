using Bilet1.Contexts;
using Bilet1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bilet1.Areas.Admin.Controllers;

[Area("Admin")]
public class MemberPositionController(AppDbContext _context, IWebHostEnvironment _environment) : Controller
{



    public async Task<IActionResult> IndexAsync()
    {
        var memberPositions = await _context.MemberPositions.Select(memberPosition => new MemberPositionGetVM()
        {
            Id = memberPosition.Id,
            Name = memberPosition.Name
        }).ToListAsync();
        return View(memberPositions);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(MemberPositionCreateVM vm)
    {
        if (!ModelState.IsValid) return View(vm);

        MemberPosition memberPosition = new()
        {
            Name = vm.Name
        };

        await _context.MemberPositions.AddAsync(memberPosition);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
