using System.Linq;
using System.Web.Mvc;
using EBillAppTest.Data;

namespace EBillAppTest.Controllers
{
    using EBillAppTest.Models;
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            EBillEntities db = new EBillEntities();
            var model = new HomeViewModel();
            model.Categories = db.Categories.Select(cat => new Category
            {
                Name = cat.Name,
                Items = cat.Items.Select(item => new Item { Name = item.Name, Id = item.Id })
            });
            return View(model);
        }
    }
}
