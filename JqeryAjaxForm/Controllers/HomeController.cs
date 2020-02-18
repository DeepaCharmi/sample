using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JqeryAjaxForm.Models;
using System.Data.SqlClient;
namespace JqeryAjaxForm.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

       public ActionResult Index()
    {
        return View(this.GetCustomers(2));
    }
 
    [HttpPost]
    public ActionResult Index(int currentPageIndex)
    {
        return View(this.GetCustomers(currentPageIndex));
    }

    private Register GetCustomers(int currentPage)
    {
        int maxRows = 9;
        var b = 0;
        using (Training_TestEntities entities = new Training_TestEntities())
        {
            Register customerModel = new Register();

            customerModel.details = (from Register in entities.Registers
                                     select Register)
                         .OrderBy(register => register.Id)
                         .Skip((currentPage - 1) * maxRows)
                         .Take(maxRows).ToList();

            double pageCount = (double)((decimal)entities.Registers.Count() / Convert.ToDecimal(maxRows));
            customerModel.pagecount = (int)Math.Ceiling(pageCount);

            customerModel.currentIndex = currentPage;
            List<Register> l1 = new List<Register>();

            foreach (var i in customerModel.details)
            {
                l1.Add(new Register()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Location = i.Location,
                    Gender = i.Gender,
                    PhoneNumber = i.PhoneNumber,
                    F_Name = i.F_Name,
                });
            }


            return customerModel;

        }
    }

        public JsonResult Display()
          {
            Training_TestEntities tab = new Training_TestEntities();
            var a = tab.Registers.Select (x => new
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                Gender = x.Gender,
                PhoneNumber = x.PhoneNumber,
                F_Name = x.F_Name,
                

            }).ToList();
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public  JsonResult Update(List<Register> datas)
        {
            string name = datas[0].Name;
            string password = datas[0].Password;
            string confirmpassword = datas[0].ConfirmPassword;
            string gender = datas[0].Gender;
            string location = datas[0].Location;
            string phoneno = datas[0].PhoneNumber;
            SqlConnection cnn = new SqlConnection(@"Server=10.0.0.5;Database=Training_Test;Uid=DucenTraining;PWD=Duc3n123!");
            cnn.Open();
            SqlCommand command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            String sql = "";
            sql = String.Format("Insert into Register (Name, Password ,ConfirmPassword,Gender,PhoneNumber,Location) Values('{0}', '{1}', '{2}','{3}','{4}','{5}');"
             + "Select @@identity", name, password, confirmpassword, gender, phoneno, location);
            command = new SqlCommand(sql, cnn);
            string result = command.ExecuteScalar().ToString();
            Training_TestEntities tab = new Training_TestEntities();
            var a = tab.Registers.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Location = x.Location,
                Gender = x.Gender,
                PhoneNumber = x.PhoneNumber,
                F_Name = x.F_Name,


            }).ToList();
            return Json(a, JsonRequestBehavior.AllowGet);
        }
    }
}
