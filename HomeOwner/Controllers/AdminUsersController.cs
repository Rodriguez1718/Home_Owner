using HomeOwner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class AdminUsersController : Controller
{
    private readonly UserManager<Users> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminUsersController(UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        var users = _userManager.Users.ToList();
        return View(users);
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
        ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string userName, string email, string password, List<string> selectedRoles)
    {
        var newUser = new Users
        {
            UserName = userName,
            Email = email
        };

        var result = await _userManager.CreateAsync(newUser, password);
        if (result.Succeeded)
        {
            if (selectedRoles != null && selectedRoles.Any())
            {
                await _userManager.AddToRolesAsync(newUser, selectedRoles);
            }
            return RedirectToAction("Index");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        ViewBag.Roles = _roleManager.Roles.Select(r => r.Name).ToList();
        return View();
    }

    // GET: AdminUsers/Edit/id
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
            return NotFound();

        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
            return NotFound();

        return View(user);
    }

    // POST: AdminUsers/Edit/id
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, Users updatedUser)
    {
        if (id != updatedUser.Id)
            return NotFound();

        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            // Add other fields if necessary

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        return View(updatedUser);
    }
}

