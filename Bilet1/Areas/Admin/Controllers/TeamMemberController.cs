using Bilet1.Contexts;
using Bilet1.Helpers;
using Bilet1.ViewModels.TeamMember;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Bilet1.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]

public class TeamMemberController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _environment;
    private readonly string folderPath;

    public TeamMemberController(AppDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
        folderPath = Path.Combine(_environment.WebRootPath, "img");
    }


    public async Task<IActionResult> IndexAsync()
    {
        var teamMembers = await _context.TeamMembers.Select(teamMember => new TeamMemberGetVM()
        {
            Id = teamMember.Id,
            Name = teamMember.Name,
            Description = teamMember.Description,
            ImagePath = teamMember.ImagePath,
            MemberPositionName = teamMember.MemberPosition.Name
        }).ToListAsync();
        return View(teamMembers);
    }

    public async Task<IActionResult> Create()
    {
        await SendMemberPositionsWithViewBag();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TeamMemberCreateVM vm)
    {
        await SendMemberPositionsWithViewBag();

        if (!ModelState.IsValid) return View(vm);

        var isExistMemberPosition = await _context.MemberPositions.AnyAsync(x => x.Id == vm.MemberPositionId);
        if (!isExistMemberPosition)
        {
            ModelState.AddModelError("MemberPositionId", "This position is not found");
            ModelState.AddModelError("MemberPositionId", "This position is not found");
            return View(vm);
        }

        if (vm.Image.Length > 2 * 1024 * 1024)
        {
            ModelState.AddModelError("Image", "Image max size must be 2 MB");
            return View(vm);
        }

        if (!vm.Image.ContentType.Contains("image"))
        {
            ModelState.AddModelError("Image", "You can upload file with image type only");
            return View(vm);
        }

        string uniqueFileName = await vm.Image.FileUploadAsync(folderPath);

        TeamMember teamMember = new()
        {
            Name = vm.Name,
            Description = vm.Description,
            MemberPositionId = vm.MemberPositionId,
            ImagePath = uniqueFileName
        };

        await _context.TeamMembers.AddAsync(teamMember);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var teamMember = await _context.TeamMembers.FindAsync(id);
        if (teamMember is null) return NotFound();
        
        _context.TeamMembers.Remove(teamMember);
        await _context.SaveChangesAsync();

        string deletedFilePath = Path.Combine(folderPath, teamMember.ImagePath);

        FileHelper.FileDelete(deletedFilePath);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id) 
    {
        var teamMember = await _context.TeamMembers.FindAsync(id);
        if(teamMember is null) return NotFound();

        TeamMemberUpdateVM vm = new()
        {
            Id = teamMember.Id,
            Name = teamMember.Name,
            Description = teamMember.Description,
            MemberPositionId = teamMember.MemberPositionId
        };

        await SendMemberPositionsWithViewBag();

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(TeamMemberUpdateVM vm)
    {
        await SendMemberPositionsWithViewBag();
        if (!ModelState.IsValid) return View(vm);

        var existTeamMember = await _context.TeamMembers.FindAsync(vm.Id);

        if (existTeamMember is null) return BadRequest();

        var isExistMemberPosition = await _context.MemberPositions.AnyAsync(x => x.Id == vm.MemberPositionId);
        if (!isExistMemberPosition)
        {
            ModelState.AddModelError("MemberPositionId", "This position is not found");
            return View(vm);
        }

        if (!vm.Image?.CheckSize(2) ?? false)
        {
            ModelState.AddModelError("Image", "Image max size must be 2 MB");
            return View(vm);
        }

        if (!vm.Image?.CheckType("image") ?? false)
        {
            ModelState.AddModelError("Image", "You can upload file with image type only");
            return View(vm);
        }

        existTeamMember.Name = vm.Name;
        existTeamMember.Description = vm.Description;
        existTeamMember.MemberPositionId = vm.MemberPositionId;

        if(vm.Image is { })
        {
            string newImagePath = await vm.Image.FileUploadAsync(folderPath);
            
            string deletedImagePath = Path.Combine(folderPath, existTeamMember.ImagePath);
            FileHelper.FileDelete(deletedImagePath);

            existTeamMember.ImagePath = newImagePath;
        }

        _context.TeamMembers.Update(existTeamMember);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    private async Task SendMemberPositionsWithViewBag()
    {
        var memberPositions = await _context.MemberPositions.Select(x => new SelectListItem()
        {
            Text = x.Name,
            Value = x.Id.ToString()
        }).ToListAsync();
        ViewBag.MemberPositions = memberPositions;
    }
}