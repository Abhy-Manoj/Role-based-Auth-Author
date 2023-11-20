using System;
using System.Web.Mvc;

namespace Auth.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Homepage()
        {
            try 
            { 
                string role = (string)Session["Role"];
                string username = (string)Session["Username"];
                if (role != null && role == "User")
                {
                    ViewBag.Role = role;
                    ViewBag.Username = username;

                    return View();
                }
                else
                {
                    TempData["ErrorMessage"] = "Unauthorized access! sign in required.";
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