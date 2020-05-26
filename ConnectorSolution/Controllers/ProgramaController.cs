using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Connector.Models;

namespace Connector.Controllers
{
    public class ProgramaController : BaseController
    {
        // GET: Programa
        public ActionResult Index()
        {
            ViewBag.FabricaAtivo = "active";
            ViewBag.FabricaProgramaAtivo = "active";
            ViewBag.FabricaShow = "show";
            ViewBag.ListaEmpresas = PegaEmpresas();
            ViewBag.ListaProgramas = CarregaDadosProgramas();

            return View();
        }

        public JsonResult ProgramaPost(string descricao, int idprograma, int empresa_anterior, int empresa_nova)
        {
            string sret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                //Programa oPrograma = new Programa();
                ////
                //if (idprograma > 0)
                //{
                //    oPrograma = db.Programa.Where(a => a.Id_Empresa == empresa_anterior && a.Id == idprograma).FirstOrDefault();
                //    //
                //    if (oPrograma != null)
                //    {
                //        oPrograma.Descricao = descricao;
                //        //
                //        if (oPrograma.Id_Empresa != empresa_nova)
                //        {
                //            List<ColetorAlerta> lista_coletor_alerta = db.ColetorAlerta.Where(x => x.Id_Coletor == idprograma && x.Id_Empresa == oPrograma.Id_Empresa).ToList();
                //            //
                //            foreach (ColetorAlerta item in lista_coletor_alerta)
                //            {
                //                item.Id_Empresa = empresa_nova;
                //                db.Entry(item).State = EntityState.Modified;
                //                db.SaveChanges();
                //            }
                //        }
                //        //
                //        oPrograma.Id_Empresa = empresa_nova;
                //        db.Entry(oPrograma).State = EntityState.Modified;
                //        db.SaveChanges();
                //    }
                //    else
                //    {
                //        sret = "Coletor não encontrado!";
                //    }
                //}
                //else
                //{
                //    oPrograma.Descricao = descricao;
                //    oPrograma.Id_Empresa = empresa_nova;
                //    //
                //    db.Programa.Add(oPrograma);
                //    db.SaveChanges();
                //    db.Entry(oPrograma).Reload();
                //}
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

        public JsonResult CarregaDados()
        {
            string sret = string.Empty;
            List<ProgramaModel> lista_programas = new List<ProgramaModel>();
            //
            try
            {
                lista_programas = CarregaDadosProgramas();
                //
                sret = "ok";
            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
            //
            return Json(new { data = sret, lista_programas, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }

        public List<ProgramaModel> CarregaDadosProgramas()
        {
            //List<Programa> lista_programas = new List<Programa>();
            List<EmpresaModel> lista_empresas = new List<EmpresaModel>();
            List<ProgramaModel> lista_programa_models = new List<ProgramaModel>();
            //
            //if (Codigo_Empresa > 0)
            //{
            //    lista_programas = db.Programa.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            //    if (Tipo_Empresa == 1)
            //    {
            //        ViewBag.ShowCrudIncluir = "inline";
            //        ViewBag.ShowCrudEditar = "inline";
            //        ViewBag.ShowCrudExcluir = "inline";

            //        Empresa empresa = db.Empresa.Where(x => x.Id == Codigo_Empresa).FirstOrDefault();
            //        //
            //        EmpresaModel empresa_model = new EmpresaModel();
            //        empresa_model.Id = empresa.Id;
            //        empresa_model.Nome = empresa.Nome;
            //        lista_empresas.Add(empresa_model);
            //        //
            //        string empresas = empresa.Empresas;
            //        if (!string.IsNullOrEmpty(empresas))
            //        {
            //            string[] empresas_array = empresas.Split(',');
            //            int[] ids = Array.ConvertAll(empresas_array, s => int.Parse(s));
            //            var lista_sub_empresas = db.Empresa.Where(r => ids.Contains(r.Id));
            //            if (lista_sub_empresas.Any())
            //            {
            //                List<Programa> lista_sub_coletores = new List<Programa>();
            //                foreach (Empresa sub_empresa in lista_sub_empresas)
            //                {
            //                    empresa_model = new EmpresaModel();
            //                    empresa_model.Id = sub_empresa.Id;
            //                    empresa_model.Nome = sub_empresa.Nome;
            //                    lista_empresas.Add(empresa_model);

            //                    lista_sub_coletores = db.Programa.Where(x => x.Id_Empresa == sub_empresa.Id).ToList();
            //                    lista_programas.AddRange(lista_sub_coletores);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        ViewBag.ShowCrudIncluir = "none";
            //        ViewBag.ShowCrudEditar = "none";
            //        ViewBag.ShowCrudExcluir = "none";
            //    }
            //    //
            //    string spossuiAlerta = string.Empty;
            //    foreach (Programa item in lista_programas)
            //    {
            //        ProgramaModel oPrograma = new ProgramaModel();
            //        //
            //        oPrograma.Id = item.Id;
            //        oPrograma.Id_Empresa = item.Id_Empresa.HasValue ? item.Id_Empresa.Value : Codigo_Empresa;
            //        //oPrograma.Id_Processo     = item.Id_Maquina;
            //        //
            //        oPrograma.EmpresaDesc = item.Empresa.Nome;
            //        oPrograma.Descricao = item.Descricao;
            //        //int tot_alertas = db.ColetorAlerta.Where(x => x.Id_Coletor == item.Id && x.Id_Empresa == oColetor.Id_Empresa).ToList().Count;
            //        //
            //        lista_programa_models.Add(oPrograma);
            //    }
            //}
            //
            return lista_programa_models;
        }

    }
}