using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class GatewayController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.FabricaAtivo = "active";
            ViewBag.FabricaGatewayAtivo = "active";
            ViewBag.FabricaShow = "show";

            List<Gateway> lista_gateways = new List<Gateway>();
            List<GatewayModel> lista_gateway_model = new List<GatewayModel>();
            //
            if (Codigo_Empresa > 0)
            {
                lista_gateways = db.Gateway.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
                //
                foreach (Gateway item in lista_gateways)
                {
                    GatewayModel gm = new GatewayModel();
                    //
                    gm.Id = item.Id;
                    gm.Id_Empresa = Codigo_Empresa;
                    //
                    gm.Ativa = item.Ativo;
                    gm.Descricao = item.Descricao;
                    gm.MAC = item.MAC;
                    //
                    lista_gateway_model.Add(gm);
                }
            }
            //
            ViewBag.ListaGateways = lista_gateway_model;
            //
            return View();
        }
        //
        public JsonResult GatewayPost(string descricao, string mac, int idgateway)
        {
            string sret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                Gateway gateway = new Gateway();
                //
                if (idgateway > 0)
                {
                    gateway = db.Gateway.Where(a => a.Id_Empresa == Codigo_Empresa && a.Id == idgateway).FirstOrDefault();
                    //
                    if (gateway != null)
                    {
                        gateway.Descricao = descricao;
                        gateway.MAC = mac;
                        db.Entry(gateway).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    gateway.Descricao = descricao;
                    gateway.MAC = mac;
                    gateway.Ativo = true;
                    gateway.Id_Empresa = Codigo_Empresa;
                    //
                    db.Gateway.Add(gateway);
                    db.SaveChanges();
                    db.Entry(gateway).Reload();
                }
                //
                sret = "ok";
            }
            catch (Exception exc)
            {
                erro = exc.Message;
                sret = "nok";
            }
            //
            return Json(new { data = sret, results = 0, success = true, erro }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult ExcluiGatewayPost(int idgateway)
        {
            string ret = string.Empty;
            try
            {
                Gateway gateway = db.Gateway.Where(a => a.Id == idgateway).FirstOrDefault();
                //
                if (gateway != null)
                {
                    db.Entry(gateway).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = "ok";
                }
                else
                {
                    ret = "nok";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }

            return Json(new { ret, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}