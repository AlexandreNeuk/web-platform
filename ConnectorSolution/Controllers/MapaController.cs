using System.Web.Mvc;

namespace Connector.Controllers
{
    public class MapaController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MapaAtivo = "active";
            return View();
        }
    }
}