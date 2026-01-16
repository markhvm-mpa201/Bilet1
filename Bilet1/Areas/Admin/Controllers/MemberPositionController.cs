using Bilet1.Contexts;
using Bilet1.Helpers;
using Bilet1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Bilet1.Areas.Admin.Controllers;

[Area("Admin")]
public class MemberPositionController : Controller
{
    private readonly AppDbContext _context;

    public MemberPositionController(AppDbContext context)
    {
        _context = context;
    }

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

    
    public async Task<IActionResult> Delete(int id)
    {
        var memberPosition = await _context.MemberPositions.FindAsync(id);
        if (memberPosition is null) return NotFound();

        _context.MemberPositions.Remove(memberPosition);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id) 
    {
        var memberPosition = await _context.MemberPositions.FindAsync(id);
        if (memberPosition is null) return NotFound();

        MemberPositionUpdateVM vm = new()
        {
            Id = memberPosition.Id,
            Name = memberPosition.Name
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update (MemberPositionUpdateVM vm)
    {
        if (!ModelState.IsValid) return View(vm);
        var existMemberPosition = await _context.MemberPositions.FindAsync(vm.Id);
        if (existMemberPosition is null) return BadRequest();

        existMemberPosition.Name = vm.Name;
        _context.MemberPositions.Update(existMemberPosition);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
