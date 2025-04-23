using HomeOwner.Models;
using HomeOwner.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class AdminUsersController : Controller
{
    private readonly UserManager<Users> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminUsersController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IActionResult> Index(string searchString, int page = 1, int pageSize = 5)
    {
        var usersQuery = _userManager.Users.AsQueryable();

        // Apply search filter
        if (!string.IsNullOrEmpty(searchString))
        {
            usersQuery = usersQuery.Where(u =>
                u.UserName.Contains(searchString) ||
                u.Email.Contains(searchString));
        }

        // Count total users for pagination
        int totalUsers = await usersQuery.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalUsers / pageSize);

        // Paginate users
        var paginatedUsers = await usersQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Get roles
        var userRolesList = new List<(Users User, List<string> Roles)>();
        foreach (var user in paginatedUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userRolesList.Add((user, roles.ToList()));
        }

        // Pass view data for search + pagination
        ViewBag.CurrentFilter = searchString;
        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

        return View(userRolesList);
    }



    public async Task<IActionResult> EditRoles(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
        var userRoles = await _userManager.GetRolesAsync(user);

        ViewBag.AllRoles = allRoles;
        ViewBag.UserRoles = userRoles;

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> EditRoles(string id, List<string> selectedRoles)
    {
        var user = await _userManager.FindByIdAsync(id);
        var currentRoles = await _userManager.GetRolesAsync(user);

        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRolesAsync(user, selectedRoles);

        return RedirectToAction("Index");
    }

    // Add Create, Edit, and Delete actions similarly
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new AdminCreateUserViewModel
        {
            AllRoles = _roleManager.Roles.Select(r => r.Name).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(AdminCreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(model);
        }

        var newUser = new Users
        {
            FullName = model.Name,
            UserName = model.Email,
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(newUser, model.Password);

        if (result.Succeeded)
        {
            if (model.SelectedRoles != null && model.SelectedRoles.Any())
            {
                await _userManager.AddToRolesAsync(newUser, model.SelectedRoles);
            }

            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
        return View(model);
    }



    // GET: AdminUsers/Edit/id
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return NotFound();

        var userRoles = await _userManager.GetRolesAsync(user);
        var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

        var model = new EditUserViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Roles = userRoles.ToList(),
            AllRoles = allRoles
        };

        return View(model);
    }

    // POST: AdminUsers/Edit/id
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, EditUserViewModel model)
    {
        if (id != model.Id)
            return NotFound();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        if (ModelState.IsValid)
        {
            user.UserName = model.FullName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = model.Roles.Except(userRoles);
            var rolesToRemove = userRoles.Except(model.Roles);

            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            await _userManager.AddToRolesAsync(user, rolesToAdd);

            return RedirectToAction(nameof(Index));
        }

        // In case of error
        model.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
        return View(model);
    }

    // GET: AdminUsers/Delete/id
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var roles = await _userManager.GetRolesAsync(user);

        var model = new EditUserViewModel
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Roles = roles.ToList()
        };

        return View(model); // Create a simple confirmation view for Delete
    }

    // POST: AdminUsers/Delete/id
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
            return View("Delete", new EditUserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Roles = (await _userManager.GetRolesAsync(user)).ToList()
            });
        }

        return RedirectToAction(nameof(Index));
    }

}

