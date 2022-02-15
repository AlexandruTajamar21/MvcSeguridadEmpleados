using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcSeguridadEmpleados.Models;
using MvcSeguridadEmpleados.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcSeguridadEmpleados.Controllers
{
    
    public class ManageController : Controller
    {
        private RepositoryEmpleados repo;

        public ManageController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public IActionResult LogIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogIn(string username, string password)
        {
            Empleado empleado = this.repo.ExisteEmpleado(username, int.Parse(password));
            if(empleado != null)
            {
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                Claim claimName = new Claim(ClaimTypes.Name, empleado.Apellido);
                Claim claimId = new Claim(ClaimTypes.NameIdentifier, empleado.IdEmpleado.ToString());
                Claim claimRole = new Claim(ClaimTypes.Role, empleado.Oficio);
                Claim claimSalario = new Claim("Salario", empleado.Salario.ToString());
                Claim claimDepartamento = new Claim("Departamento", empleado.Departamento.ToString());
                identity.AddClaim(claimName);
                identity.AddClaim(claimId);
                identity.AddClaim(claimRole);
                identity.AddClaim(claimSalario);
                identity.AddClaim(claimDepartamento);
                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                //var data = HttpContext.Request.Cookies["routedata"];
                //string[] arr = data.Split(',');
                //string controller = arr[0];
                //string action = arr[1];

                string controller = TempData["controller"].ToString();
                string action = TempData["action"].ToString();

                return RedirectToAction(action,controller);
            }
            else
            {
                ViewData["MENSAJE"] = "Usuario/Password incorrectos";
            }
            return View();
        }

        public IActionResult ValidarEnfermo(int idUsuario)
        {

        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ErrorAcceso()
        {
            return View();
        }
    }
}
