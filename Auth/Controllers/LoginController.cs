using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Mvc;
using Auth.Models;

namespace Auth.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Index(UserDetails model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool isAuthenticated = false;
                    string _connectionString = ConfigurationManager.ConnectionStrings["GetConnection"].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(_connectionString))
                    {
                        connection.Open();

                        using (SqlCommand command = new SqlCommand("SP_Login", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@Username", model.Username);
                            command.Parameters.AddWithValue("@Password", model.Password);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    isAuthenticated = true;
                                    model.Id = (int)reader["Id"];
                                    model.Username = reader["Username"].ToString();
                                    model.Password = reader["Password"].ToString();
                                    model.Role = reader["Role"].ToString();
                                }
                            }
                        }
                    }

                    // Check if authenticated
                    if (isAuthenticated)
                    {
                        Session["Username"] = model.Username;

                        if (model.Role == "Admin")
                        {
                            Session["Role"] = model.Role;
                            return RedirectToAction("Homepage", "Admin");
                        }
                        else if (model.Role == "User")
                        {
                            Session["Role"] = model.Role;
                            return RedirectToAction("Homepage", "User");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid login credentials!");
                    }
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while processing your request.");
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            try
            {
                Session.Clear();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while logging out.");
                return View("Index");
            }
        }
    }
}
