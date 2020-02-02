using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class PermissaoController : Controller
    {
        // GET: Permissao
        public ActionResult Index()
        {
            ViewBag.GeralAtivo = "active";
            ViewBag.GeralPermissoesAtivo = "active";
            ViewBag.GeralShow = "show";

            return View();
        }
    }
}