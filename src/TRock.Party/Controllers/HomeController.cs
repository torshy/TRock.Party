using System.Dynamic;
using System.Web.Mvc;

namespace TRock.Party.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Playlist()
        {
            return View();
        }

        public ActionResult Favorites()
        {
            return View();
        }

        public ActionResult Search(string query)
        {
            dynamic model = new ExpandoObject();
            model.Query = query;
            return View(model);
        }
    }
}
