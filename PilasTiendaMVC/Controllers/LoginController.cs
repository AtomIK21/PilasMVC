﻿using PilasTiendaMVC.Pilas.DAL;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using static System.Collections.Specialized.BitVector32;
using TiendaPilasMVC.Controllers;
using PilasTiendaMVC.Pilas.BLL;

namespace PilasTiendaMVC.Controllers
{
    public class LoginController : CacheController // Hereda de CacheController
    {
        Negocios neg = new Negocios();

        public ActionResult Index()
        {
            if (Usuario_AUTH())
            {
                string username = Session["Username"].ToString();
                ViewBag.Username = username;
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Comprador()
        {
            if (Usuario_AUTH())
            {
                No_cache(); // Deshabilitar la caché
                return View(); // Usuario autenticado, mostrar la página de mensaje
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(string username, string password)
        {
            var userAuth = await neg.AutenticarUsuario(username, password);

            if (userAuth != null)
            {
                Session["Username"] = userAuth.nomUser;

                if (userAuth.rol == "Administrador")
                    return RedirectToAction("Admin");
                else
                    return RedirectToAction("Comprador");
            }
            else
            {
                ViewBag.ErrorMessage = "Nombre de usuario o contraseña incorrectos.";
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear(); // Limpiar la sesión
            return RedirectToAction("Login");
        }

        public ActionResult Admin()
        {
            if (Usuario_AUTH())
            {
                No_cache(); // Deshabilitar la caché
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
    }
}
