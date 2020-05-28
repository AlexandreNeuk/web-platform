using Connector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class ReceitaController : BaseController
    {
        // GET: ReceitaPasso
        public ActionResult Index()
        {
            //ViewBag.ListaReceitas
            ViewBag.FabricaAtivo = "active";
            ViewBag.FabricaReceitaAtivo = "active";
            ViewBag.FabricaShow = "show";
            ViewBag.ListaReceitas = CarregaDadosReceitas();
            ViewBag.ListaEmpresas = PegaEmpresas();
            return View();
        }
        //
        public List<ReceitaModel> CarregaDadosReceitas()
        {
            List<Receita> lista_receita = new List<Receita>();
            List<EmpresaModel> lista_empresas = new List<EmpresaModel>();
            List<ReceitaModel> lista_receita_models = new List<ReceitaModel>();
            //
            if (Codigo_Empresa > 0)
            {
                lista_receita = db.Receita.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
                //
                string spossuiAlerta = string.Empty;
                foreach (Receita item in lista_receita)
                {
                    ReceitaModel oReceitaModel = new ReceitaModel();
                    //
                    oReceitaModel.Id = item.Id;
                    oReceitaModel.Id_Empresa = item.Id_Empresa.HasValue ? item.Id_Empresa.Value : Codigo_Empresa;
                    //
                    oReceitaModel.Ativo = item.Ativo;
                    oReceitaModel.Descricao = item.Descricao;
                    int tot_recei_centr = 0; // db.ReceitaPassoCentrifugacao.Where(x => x.Id_Receita == item.Id).ToList().Count;
                    int tot_recei_lava = 0; // db.ReceitaPassoLavagem.Where(x => x.Id_Receita == item.Id).ToList().Count;
                    //
                    oReceitaModel.TotalPassos = tot_recei_centr + tot_recei_lava;
                    //
                    lista_receita_models.Add(oReceitaModel);
                }
            }
            //
            return lista_receita_models;
        }
        //
        public JsonResult ReceitaPost(string descricao, int idreceita)
        {
            string sret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                Receita oReceita = new Receita();
                //
                if (idreceita > 0)
                {
                    oReceita = db.Receita.Where(a => a.Id_Empresa == Codigo_Empresa && a.Id == idreceita).FirstOrDefault();
                    //
                    if (oReceita != null)
                    {
                        oReceita.Descricao = descricao;
                        db.Entry(oReceita).State = System.Data.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        sret = "Coletor não encontrado!";
                    }
                }
                else
                {
                    oReceita.Descricao = descricao;
                    oReceita.Ativo = true;
                    oReceita.Id_Empresa = Codigo_Empresa;
                    //
                    db.Receita.Add(oReceita);
                    db.SaveChanges();
                    db.Entry(oReceita).Reload();
                }
                //
                sret = "ok";
            }
            catch (Exception exc)
            {
                erro = exc.Message;
                sret = "erro";
            }
            //
            return Json(new { data = sret, results = 0, success = true, erro }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult CarregaDados()
        {
            string sret = string.Empty;
            List<ReceitaModel> lista_receita = new List<ReceitaModel>();
            //
            try
            {
                lista_receita = CarregaDadosReceitas();
                sret = "ok";
            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
            //
            return Json(new { data = sret, lista_receita, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult ExcluiReceitaPost(int idreceita)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            try
            {
                Receita oReceita = db.Receita.Where(a => a.Id == idreceita && a.Id_Empresa == Codigo_Empresa).FirstOrDefault();
                //
                if (oReceita != null)
                {
                    db.Entry(oReceita).State = System.Data.EntityState.Deleted;
                    db.SaveChanges();
                    ret = "ok";
                }
                else
                {
                    ret = "nao_encontrada";
                }
            }
            catch (Exception exc)
            {
                ret = "erro";
                erro = exc.Message;
            }

            return Json(new { ret, results = 0, erro, success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}