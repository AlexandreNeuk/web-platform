using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class RequisicaoController : BaseController
    {
        // GET: Requisicao
        public ActionResult Index()
        {
            return View();
        }
        //
        public JsonResult Pressao(string mac, string pressao)
        {
            string ret = string.Empty;
            try
            {
                var coletor = db.Coletor.Where(x => x.MAC.ToLower().Equals(mac)).FirstOrDefault();
                if (coletor != null)
                {
                    ColetorPressaoHistorico oColetorPressaoHistorico = new ColetorPressaoHistorico();
                    //
                    int n;
                    bool isNumeric = int.TryParse(pressao, out n);
                    if (isNumeric)
                    {
                        oColetorPressaoHistorico.Pressao = (Convert.ToDouble(pressao) / 10).ToString();
                    }
                    else
                    {
                        oColetorPressaoHistorico.Pressao = "0";
                    }
                    oColetorPressaoHistorico.Id_Coletor = coletor.Id;
                    oColetorPressaoHistorico.DataHora = getData();
                    db.ColetorPressaoHistorico.Add(oColetorPressaoHistorico);
                    db.SaveChanges();
                    //
                    if (coletor.ColetorAlerta.Any())
                    {
                        ValidaAlertas(coletor, pressao);
                    }
                    //
                    ret = "ok";
                }
                else
                {
                    ret = "não encontrou medidor";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            return Json(new { data = ret, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult Temperatura(string mac, string temperatura)
        {
            string ret = string.Empty;
            try
            {
                var coletor = db.Coletor.Where(x => x.MAC.ToLower().Equals(mac)).FirstOrDefault();
                if (coletor != null)
                {
                    ColetorTemperaturaHistorico oColetorTemperaturaHistorico = new ColetorTemperaturaHistorico();
                    oColetorTemperaturaHistorico.Temperatura = temperatura;
                    oColetorTemperaturaHistorico.Id_Coletor = coletor.Id;
                    oColetorTemperaturaHistorico.DataHora = getData();
                    db.ColetorTemperaturaHistorico.Add(oColetorTemperaturaHistorico);
                    db.SaveChanges();
                    //
                    if (coletor.ColetorAlerta.Any())
                    {
                        ValidaAlertas(coletor, temperatura);
                    }
                    //
                    ret = "ok";
                }
                else
                {
                    ret = "não encontrou o coletor com o MAC: " + mac;
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            return Json(new { data = ret, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult Producao(string mac, string valor)
        {
            string ret = string.Empty;
            try
            {
                var coletor = db.Coletor.Where(x => x.MAC.ToLower().Equals(mac)).FirstOrDefault();
                if (coletor != null)
                {
                    ColetorProducaoHistorico oColetorProducaoHistorico = new ColetorProducaoHistorico();
                    oColetorProducaoHistorico.Valor = valor;
                    oColetorProducaoHistorico.Id_Coletor = coletor.Id;
                    oColetorProducaoHistorico.DataHora = getData();
                    db.ColetorProducaoHistorico.Add(oColetorProducaoHistorico);
                    db.SaveChanges();
                    //
                    if (coletor.ColetorAlerta.Any())
                    {
                        ValidaAlertas(coletor, valor);
                    }
                    //
                    ret = "ok";
                }
                else
                {
                    ret = "não encontrou o coletor com o MAC: " + mac;
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            return Json(new { data = ret, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult LogAtividade(int empresa, string id, int tipo)
        {
            string ret = string.Empty;
            try
            {
                LogAtividade log = new LogAtividade();
                log.Id_Dispositivo = id;
                log.Id_Empresa = empresa;
                log.Tipo = tipo;
                log.DataHora = getData();
                db.LogAtividade.Add(log);
                db.SaveChanges();
                ret = "ok";
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            return Json(new { data = ret, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult Sensor(string id_sensor, string valor_sensor)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            string exce = string.Empty;
            //try
            //{
            //    ColetorSensorMovHistorico csm = new ColetorSensorMovHistorico();

            //    csm.Coletor = id_sensor;
            //    csm.Valor = valor_sensor;
            //    csm.DataHora = getData();
            //    db.ColetorSensorMovHistorico.Add(csm);
            //    db.SaveChanges();
            //    ret = "ok";
            //}
            //catch (DbEntityValidationException e)
            //{
            //    exce = e.Message;
            //    foreach (var eve in e.EntityValidationErrors)
            //    {
            //        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //            erro += ve.ErrorMessage + " - ";
            //        }
            //    }
            //    //
            //    ret = "nok";
            //}
            //catch (Exception exc)
            //{
            //    ret = exc.Message;
            //}
            return Json(new { data = ret, erro, exce, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public void ValidaAlertas(Coletor coletor, string valorenviado)
        {
            List<ColetorAlerta> lista_alertas = coletor.ColetorAlerta.Where(x => x.Ativo.Value == 1).ToList();
            //
            foreach (ColetorAlerta alerta in lista_alertas)
            {
                int iregra = alerta.Regra.HasValue ? alerta.Regra.Value : 0;
                double dvalorenviado = Convert.ToDouble(valorenviado) / 10;
                //
                if (alerta.Valor.IndexOf(".") > -1)
                {
                    alerta.Valor = alerta.Valor.Replace(".", ",");
                }
                double dvalor_regra = Convert.ToDouble(alerta.Valor);
                //
                switch (iregra)
                {
                    case 1: // Maior
                        if (dvalorenviado > dvalor_regra)
                        {
                            CriaLogAlerta(coletor.Id, alerta.Id, (Convert.ToDouble(dvalor_regra)).ToString(), (Convert.ToDouble(dvalorenviado)).ToString());
                        }
                        break;
                    case 2: // Menor
                        if (dvalorenviado < dvalor_regra)
                        {
                            CriaLogAlerta(coletor.Id, alerta.Id, (Convert.ToDouble(dvalor_regra)).ToString(), (Convert.ToDouble(dvalorenviado)).ToString());
                        }
                        break;
                    case 3: // Igual
                        if (dvalorenviado == dvalor_regra)
                        {
                            CriaLogAlerta(coletor.Id, alerta.Id, (Convert.ToDouble(dvalor_regra)).ToString(), (Convert.ToDouble(dvalorenviado)).ToString());
                        }
                        break;
                    case 4: // Maior ou Igual
                        if (dvalorenviado >= dvalor_regra)
                        {
                            CriaLogAlerta(coletor.Id, alerta.Id, (Convert.ToDouble(dvalor_regra)).ToString(), (Convert.ToDouble(dvalorenviado)).ToString());
                        }
                        break;
                    case 5: // Menor ou Igual
                        if (dvalorenviado <= dvalor_regra)
                        {
                            CriaLogAlerta(coletor.Id, alerta.Id, (Convert.ToDouble(dvalor_regra)).ToString(), (Convert.ToDouble(dvalorenviado)).ToString());
                        }
                        break;
                }
            }
        }
        //
        public void CriaLogAlerta(int idcoletor, int idcoletoralerta, string valorregra, string valorenviado)
        {
            try
            {
                ColetorAlertaLog cal = new ColetorAlertaLog();
                //
                cal.Id_Coletor = idcoletor;
                cal.Id_ColetorAlerta = idcoletoralerta;
                cal.DataHora = getData();
                cal.ValorRegra = valorregra;
                cal.ValorEnviado = valorenviado;
                //
                db.ColetorAlertaLog.Add(cal);
                db.SaveChanges();
            }
            catch (Exception exc)
            {

            }
        }
    }
}