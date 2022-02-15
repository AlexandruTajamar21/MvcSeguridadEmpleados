using Microsoft.AspNetCore.Mvc;
using MvcSeguridadEmpleados.Filters;
using MvcSeguridadEmpleados.Models;
using MvcSeguridadEmpleados.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcSeguridadEmpleados.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public IActionResult Empleados()
        {
            List<Empleado> empleados = this.repo.GetEmpleados();
            return View(empleados);
        }

        [AuthorizeEmpleados]
        public IActionResult PerfilEmpleado()
        {
            return View();
        }

        [AuthorizeEmpleados]
        public IActionResult Compis()
        {
            string dato = HttpContext.User.FindFirst("Departamento").Value;
            int id = int.Parse(dato);
            List<Empleado> empleados = this.repo.GetEmpleadosDepartamento(id);
            return View(empleados);
        }
    }
}
