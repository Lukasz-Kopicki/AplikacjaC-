using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Application.ViewModels;
using DataAccessLogic.Entities;
using DataAccessLogic.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V3.Pages.Internal.Account.Manage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AplikacjaFryzjer_v2.Controllers
{
    [Authorize]
    public class AdministratorController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICategoriesRepository _categoriesRepository;

        public AdministratorController(RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager, ICategoriesRepository categoriesRepository
            )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _categoriesRepository = categoriesRepository;
        }

        [HttpGet]
        public ViewResult ManageCategories()
        {
            //pobranie wszytkich kategorii - ustawic  przypisanie do zmiennej wywolania funkcji- 
            //repository nie moze byc tu zaimplementowany
            var model = _categoriesRepository.GetAllCategories();
            return View(model);
        }

        [HttpGet]
        public ViewResult CreateCategory()
        {
            //implementacja viewBag przeniesiona do serwisu, zeby oddzielic inicjalizacje od
            //reszty
            ViewBag.Subcategories = new List<Subcategory>() { };
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(Category category)
        {
            //public HttpResponseMessage Post(Product product)
            //{
            //    if (ModelState.IsValid)
            //    {
            //        // Do something with the product (not shown).

            //        return new HttpResponseMessage(HttpStatusCode.OK);
            //    }
            //    else
            //    {
            //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            //    }
            //wyzej przyklad ze stacka o co chodzi z isvalid- cvzyli czy poprawne dane z jasona przyszly
            //oddzielic w srodku tworzenie listy subcategorii oraz dodawaniue nowych
                if (!ModelState.IsValid)
            {
                // store Subcategories data which has been added
                ViewBag.Subcategories = category.Subcategories == null ? new List<Subcategory>() 
                { } : category.Subcategories;
                return View("CreateCategory");

            }
            _categoriesRepository.AddCategory(category);
            return RedirectToAction("ManageCategories");
        }

        [HttpGet]
        public ViewResult EditCategory(int Id)
        {

            //getcategory do serwisu  
            var category = _categoriesRepository.GetCategory(Id);

            if (category == null)
            {
                ViewBag.ErrorMessage = $"Kategoria o id = {Id} nie została odnaleziona";
                return View("NotFound");
            }

            ViewBag.Subcategories = category.Subcategories;
            return View(category);
        }

        [HttpPost]
        public IActionResult EditCategory(Category category)
        {
            //isvalid - cvzyli czy poprawne dane z jasona przyszly
            //oddzielic w srodku tworzenie listy subcategorii oraz dodawaniue nowych

            if (!ModelState.IsValid)
            {
                // store Subcategories data which has been added
                ViewBag.Subcategories = category.Subcategories == null ? new List<Subcategory>() 
                { } : category.Subcategories;
                return View("EditCategory");

            }
            _categoriesRepository.UpdateCategory(category);
            return RedirectToAction("ManageCategories");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int Id)
        {
            //pobranie kategorii do serwisu
            //usuniecie rowniez
            var category = _categoriesRepository.GetCategory(Id);

            if (category == null)
            {
                ViewBag.ErrorMessage = $"Kategoria o id = {Id} nie została odnaleziona";
                return View("NotFound");
            }

            _categoriesRepository.DeleteCategory(Id);
            return RedirectToAction("ManageCategories");
        }


        [HttpGet]
        public IActionResult MyAccount()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            // logika tworzenia roli do servicce
            //wszystko z identity nara
            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administrator");
                }

                foreach(IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            // logika edycji ról do servicce
            //wszystko z identity nara
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Profil o id = {id} nie został odnaleziony";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };

            foreach(var user in await _userManager.Users.ToListAsync())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                };
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            // logika edycji roli do servicce
            //wszystko z identity nara
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Profil o id = {model.Id} nie został odnaleziony";
                return View("NotFound");
            }
            else
            {
                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string id)
        {
            // logika usuwania roli do servicce
            //wszystko z identity nara
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Profil o id = {id} nie został odnaleziony";
                return View("NotFound");
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name,
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(EditRoleViewModel model)
        {
            // logika usuwania roli do servicce
            //wszystko z identity nara
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Profil o id = {model.Id} nie został odnaleziony";
                return View("NotFound");
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {

            // logika edycji userow roli do servicce


            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if(role ==null)
            {
                ViewBag.ErrorMessage = $"Profil o id = {roleId} nie został odnaleziony";
                return View("NotFound");
            }

            var model = new List<UserRoleViewModel>();

            foreach(var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if(await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            // logika edycji userow roli do servicce oraz całe zliczanie
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Profil o id = {roleId} nie został odnaleziony";
                return View("NotFound");
            }

            for(int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;

                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if(!model[i].IsSelected && (await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}