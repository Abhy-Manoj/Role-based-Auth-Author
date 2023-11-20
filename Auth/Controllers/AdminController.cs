using System;
using System.Web.Mvc;
using Auth.Models;

namespace Auth.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Homepage()
        {
            try
            {
                string role = (string)Session["Role"];
                string username = (string)Session["Username"];

                if (role != null && role == "Admin")
                {
                    ViewBag.Role = role;
                    ViewBag.Username = username;

                    return View();
                }
                else
                {
                    TempData["ErrorMessage"] = "Unauthorized access! Sign in required.";
                    return RedirectToAction("Index", "Login");
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return RedirectToAction("Index", "Login");
            }

        }
    }
}