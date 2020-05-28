using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class ReceitaPassoController : BaseController
    {
        // GET: ReceitaPasso
        public ActionResult Index()
        {
            //ViewBag.ListaReceitas
            ViewBag.FabricaAtivo = "active";
            ViewBag.FabricaPassoAtivo = "active";
            ViewBag.FabricaShow = "show";
            //ViewBag.ListaReceitasPasso = CarregaDadosReceitas();
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
                    int tot_recei_centr = 0;//db.ReceitaPassoCentrifugacao.Where(x => x.Id_Receita == item.Id).ToList().Count;
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
        public List<ReceitaPassoGridModel> CarregaDadosReceitasId(int id)
        {
            List<ReceitaPassoLavagem> lista_receita_passo_lavagem = new List<ReceitaPassoLavagem>();
            List<ReceitaPassoCentrifugacao> lista_receita_passo__centrifuga = new List<ReceitaPassoCentrifugacao>();
            List<ReceitaPassoGridModel> lista_receita_passo_grid_models = new List<ReceitaPassoGridModel>();
            //
            if (Codigo_Empresa > 0)
            {
                Receita model = db.Receita.Where(a => a.Id_Empresa == Codigo_Empresa && a.Id == id).FirstOrDefault();
                //
                foreach (var item in model.ReceitaPasso)
                {
                    foreach (var item_filho in item.ReceitaPassoLavagem)
                    {
                        ReceitaPassoGridModel tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "Modo Trabalho";
                        tmp.Valor = item_filho.ModoTrabalho;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "Tempo Operação";
                        tmp.Valor = item_filho.TempoOperacao;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "Tempo Reversão";
                        tmp.Valor = item_filho.TempoReversao;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "Temperatura";
                        tmp.Valor = item_filho.Temperatura;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "RPM";
                        tmp.Valor = item_filho.RPM;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "Sem Vapor";
                        tmp.Valor = item_filho.SemVapor;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "Entrada";
                        tmp.Valor = item_filho.Entrada;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "Saída";
                        tmp.Valor = item_filho.Saida;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "Nível";
                        tmp.Valor = item_filho.Nivel;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "ProdutoA";
                        tmp.Valor = item_filho.ValorA;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "ProdutoB";
                        tmp.Valor = item_filho.ValorB;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "ProdutoC";
                        tmp.Valor = item_filho.ValorC;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "ProdutoD";
                        tmp.Valor = item_filho.ValorD;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "ProdutoE";
                        tmp.Valor = item_filho.ValorE;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "ProdutoF";
                        tmp.Valor = item_filho.ValorF;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.Tipo = "Lavagem";
                        tmp.Descricao = item.Decricao;
                        tmp.Variavel = "ProdutoG";
                        tmp.Valor = item_filho.ValorG;
                        lista_receita_passo_grid_models.Add(tmp);
                        /*
                        tmp.ProdutoB = item_filho.ProdutoB;
                        tmp.ProdutoC = item_filho.ProdutoC;
                        tmp.ProdutoD = item_filho.ProdutoD;
                        tmp.ProdutoE = item_filho.ProdutoE;
                        tmp.ProdutoF = item_filho.ProdutoF;
                        tmp.ProdutoG = item_filho.ProdutoG;
                        ////
                        tmp.ValorA = item_filho.ValorA;
                        tmp.ValorB = item_filho.ValorB;
                        tmp.ValorC = item_filho.ValorC;
                        tmp.ValorD = item_filho.ValorD;
                        tmp.ValorE = item_filho.ValorE;
                        tmp.ValorF = item_filho.ValorF;
                        tmp.ValorG = item_filho.ValorG;
                        ////
                        tmp.RPM = item_filho.RPM;
                        tmp.SemVapor = item_filho.SemVapor;
                        tmp.Temperatura = item_filho.Temperatura;
                        tmp.TempoOperacao = item_filho.TempoOperacao;
                        tmp.TempoReversao = item_filho.TempoReversao;
                        tmp.Entrada = item_filho.Entrada;
                        tmp.Saida = item_filho.Saida;
                        //
                        */
                    }
                }
                //
                foreach (var item in model.ReceitaPasso)
                {

                }
            }
            //
            return lista_receita_passo_grid_models;
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
                        db.Entry(oReceita).State = EntityState.Modified;
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
        public JsonResult CarregaDadosById(int id)
        {
            string sret = string.Empty;
            List<ReceitaPassoGridModel> lista_receita_passos_grid = new List<ReceitaPassoGridModel>();
            //
            try
            {
                lista_receita_passos_grid = CarregaDadosReceitasId(id);
                sret = "ok";
            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
            //
            return Json(new { data = sret, lista_receita_passos_grid, results = 0, success = true }, JsonRequestBehavior.AllowGet);
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
        public JsonResult Carrega()
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
        public List<ReceitaModel> CarregaReceitas()
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
            return lista_receita;
        }
    }
}