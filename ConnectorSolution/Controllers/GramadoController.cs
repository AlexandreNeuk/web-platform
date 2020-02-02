using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class GramadoController : Controller
    {
        public ActionResult Barras()
        {
            ViewBag.BarrastAtivo = "active";
            return View();
        }

        public ActionResult Mapacalor()
        {
            ViewBag.MapaCalorAtivo = "active";
            return View();
        }

        public ActionResult Bubleplot()
        {
            ViewBag.BubleplotAtivo = "active";
            return View();
        }

        public ActionResult Series()
        {
            ViewBag.SeriestAtivo = "active";
            return View();
        }
    }
}