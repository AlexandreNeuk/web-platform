using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class ReceitaPassoController : BaseController
    {
        // GET: ReceitaPasso
        public ActionResult Index()
        {
            ViewBag.FabricaAtivo = "active";
            ViewBag.FabricaPassoAtivo = "active";
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
                    //int tot_recei_centr = 0;//db.ReceitaPassoCentrifugacao.Where(x => x.Id_Receita == item.Id).ToList().Count;
                    //int tot_recei_lava = 0; // db.ReceitaPassoLavagem.Where(x => x.Id_Receita == item.Id).ToList().Count;
                    //
                    //oReceitaModel.TotalPassos = tot_recei_centr + tot_recei_lava;
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
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.TipoId = Convert.ToInt32(item.Tipo);
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Tempo Operação";
                        tmp.Valor = item_filho.TempoOperacao;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.TipoId = Convert.ToInt32(item.Tipo);
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Tempo Reversão";
                        tmp.Valor = item_filho.TempoReversao;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Temperatura";
                        tmp.Valor = item_filho.Temperatura;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "RPM";
                        tmp.Valor = item_filho.RPM;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Sem Vapor";
                        tmp.Valor = item_filho.SemVapor;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Entrada";
                        tmp.Valor = item_filho.Entrada;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Saída";
                        tmp.Valor = item_filho.Saida;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Nível";
                        tmp.Valor = item_filho.Nivel;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "ProdutoA";
                        tmp.Valor = item_filho.ProdutoA;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "ProdutoB";
                        tmp.Valor = item_filho.ProdutoB;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "ProdutoC";
                        tmp.Valor = item_filho.ProdutoC;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "ProdutoD";
                        tmp.Valor = item_filho.ProdutoD;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "ProdutoE";
                        tmp.Valor = item_filho.ProdutoE;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "ProdutoF";
                        tmp.Valor = item_filho.ProdutoF;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_filho.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "ProdutoG";
                        tmp.Valor = item_filho.ProdutoG;
                        lista_receita_passo_grid_models.Add(tmp);
                    }
                    //
                    foreach (var item_centri in item.ReceitaPassoCentrifugacao)
                    {
                        ReceitaPassoGridModel tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Saída";
                        tmp.Valor = item_centri.Saida;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Velociade 1";
                        tmp.Valor = item_centri.Velocidade1;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Velociade 2";
                        tmp.Valor = item_centri.Velocidade2;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Velociade 3";
                        tmp.Valor = item_centri.Velocidade3;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Velociade 4";
                        tmp.Valor = item_centri.Velocidade4;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Velociade 4";
                        tmp.Valor = item_centri.Velocidade4;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Velociade 5";
                        tmp.Valor = item_centri.Velocidade5;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Tempo 1";
                        tmp.Valor = item_centri.Tempo1;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Tempo 2";
                        tmp.Valor = item_centri.Tempo2;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Tempo 3";
                        tmp.Valor = item_centri.Tempo3;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Tempo 4";
                        tmp.Valor = item_centri.Tempo4;
                        lista_receita_passo_grid_models.Add(tmp);
                        //
                        tmp = new ReceitaPassoGridModel();
                        tmp.Id = item_centri.Id;
                        tmp.IdReceitaPasso = item.Id;
                        tmp.Tipo = item.Tipo;
                        tmp.Descricao = PegaDescPasso(item.Tipo);
                        tmp.Variavel = "Tempo 5";
                        tmp.Valor = item_centri.Tempo5;
                        lista_receita_passo_grid_models.Add(tmp);
                    }
                }
            }
            //
            return lista_receita_passo_grid_models;
        }
        //
        private string PegaDescPasso(string tipo)
        {
            string sret = string.Empty;
            //
            switch (Convert.ToInt32(tipo))
            {
                case 0:
                    sret = "Desabilitado";
                    break;
                case 1:
                    sret = "Umectação";
                    break;
                case 2:
                    sret = "Pré-Lavagem";
                    break;
                case 3:
                    sret = "Lavagem";
                    break;
                case 4:
                    sret = "Alvejamento";
                    break;
                case 5:
                    sret = "Enxague";
                    break;
                case 6:
                    sret = "Neutralização";
                    break;
                case 7:
                    sret = "Amaciante";
                    break;
                case 8:
                    sret = "Molho";
                    break;
                case 9:
                    sret = "Centrifugação";
                    break;
                default:
                    sret = "Desabilitado";
                    break;
            }
            //
            return sret;
        }
        //
        public JsonResult Passo(int id_receita, int id_receita_passo, int id_receita_passo_lavagem, 
            string descricao, int tipo, string rpm, string temporeversao, string tempooperacao, string entrada, 
            string saida, string nivel, string temperatura, string semvapor, 
            string produtoa, string produtob, string produtoc, string produtod, string produtoe, string produtof, string produtog)
        {
            string sret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                ReceitaPasso oReceitaPasso = db.ReceitaPasso.Where(a => a.Id == id_receita_passo).FirstOrDefault();
                //
                if (oReceitaPasso == null)
                    oReceitaPasso = new ReceitaPasso();
                //
                oReceitaPasso.Id_Receita = id_receita;
                oReceitaPasso.Decricao = descricao;
                oReceitaPasso.Tipo = tipo.ToString();
                oReceitaPasso.Ativo = 1;
                //
                if (id_receita_passo > 0)
                {
                    db.Entry(oReceitaPasso).State = EntityState.Modified;
                }
                else
                {
                    db.ReceitaPasso.Add(oReceitaPasso);
                }
                //
                db.SaveChanges();
                db.Entry(oReceitaPasso).Reload();
                //
                ReceitaPassoLavagem rpl = db.ReceitaPassoLavagem.Where(a => a.Id == id_receita_passo_lavagem).FirstOrDefault();
                //
                if (rpl == null) rpl = new ReceitaPassoLavagem();
                //
                rpl.Id_ReceitaPasso = oReceitaPasso.Id;
                //rpl.ModoTrabalho = modotrabalho == null ? rpl.ModoTrabalho : modotrabalho;
                rpl.Entrada = entrada == null ? rpl.Entrada : entrada;
                rpl.Saida = saida == null ? rpl.Saida : saida;
                rpl.Nivel = nivel == null ? rpl.Nivel : nivel;
                rpl.RPM = rpm == null ? rpl.RPM : rpm;
                rpl.SemVapor = semvapor == null ? rpl.SemVapor : semvapor;
                rpl.Temperatura = temperatura == null ? rpl.Temperatura : temperatura;
                rpl.TempoOperacao = tempooperacao == null ? rpl.TempoOperacao : tempooperacao;
                rpl.TempoReversao = temporeversao == null ? rpl.TempoReversao : temporeversao;
                rpl.ProdutoA = produtoa == null ? rpl.ProdutoA : produtoa;
                //rpl.ValorA = valora == null ? rpl.ValorA : valora;
                rpl.ProdutoB = produtob == null ? rpl.ProdutoB : produtob;
                //rpl.ValorB = valorb == null ? rpl.ValorB : valorb;
                rpl.ProdutoC = produtoc == null ? rpl.ProdutoC : produtoc;
                //rpl.ValorC = valorc == null ? rpl.ValorC : valorc;
                rpl.ProdutoD = produtod == null ? rpl.ProdutoD : produtod;
                //rpl.ValorD = valord == null ? rpl.ValorD : valord;
                rpl.ProdutoE = produtoe == null ? rpl.ProdutoE : produtoe;
                //rpl.ValorE = valore == null ? rpl.ValorE : valore;
                rpl.ProdutoF = produtof == null ? rpl.ProdutoF : produtof;
                //rpl.ValorF = valorf == null ? rpl.ValorF : valorf;
                rpl.ProdutoG = produtog == null ? rpl.ProdutoG : produtog;
                //rpl.ValorG = valorg == null ? rpl.ValorG : valorg;
                //rpl.Ativo = ativo == null ? rpl.Ativo : ativo;
                //
                if (id_receita_passo > 0)
                {
                    db.Entry(rpl).State = EntityState.Modified;
                }
                else
                {
                    db.ReceitaPassoLavagem.Add(rpl);
                }
                //
                db.SaveChanges();
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
        public JsonResult Passocentrifuga(int id_receita, int id_receita_passo, int id_receita_passo_centrifuga,
                                          string descricao, string saida, 
                                          string txtvelocidade1, string txtvelocidade2, string txtvelocidade3, string txtvelocidade4, string txtvelocidade5,
                                          string txttempo1, string txttempo2, string txttempo3, string txttempo4, string txttempo5)
        {
            string sret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                ReceitaPasso oReceitaPasso = db.ReceitaPasso.Where(a => a.Id == id_receita_passo).FirstOrDefault();
                //
                if (oReceitaPasso == null)
                    oReceitaPasso = new ReceitaPasso();
                //
                oReceitaPasso.Id_Receita = id_receita;
                oReceitaPasso.Decricao = descricao;
                oReceitaPasso.Tipo = "9";
                oReceitaPasso.Ativo = 1;
                //
                if (id_receita_passo > 0)
                {
                    db.Entry(oReceitaPasso).State = EntityState.Modified;
                }
                else
                {
                    db.ReceitaPasso.Add(oReceitaPasso);
                }
                //
                db.SaveChanges();
                db.Entry(oReceitaPasso).Reload();
                //
                ReceitaPassoCentrifugacao rpc = db.ReceitaPassoCentrifugacao.Where(a => a.Id == id_receita_passo_centrifuga).FirstOrDefault();
                //
                if (rpc == null) rpc = new ReceitaPassoCentrifugacao();
                //
                rpc.Id_ReceitaPasso = oReceitaPasso.Id;
                rpc.Saida = saida;
                //
                rpc.Velocidade1 = txtvelocidade1 == null ? rpc.Velocidade1 : txtvelocidade1;
                rpc.Velocidade2 = txtvelocidade2 == null ? rpc.Velocidade2 : txtvelocidade2;
                rpc.Velocidade3 = txtvelocidade3 == null ? rpc.Velocidade3 : txtvelocidade3;
                rpc.Velocidade4 = txtvelocidade4 == null ? rpc.Velocidade4 : txtvelocidade4;
                rpc.Velocidade5 = txtvelocidade5 == null ? rpc.Velocidade5 : txtvelocidade5;
                //
                rpc.Tempo1 = txttempo1 == null ? rpc.Tempo1 : txttempo1;
                rpc.Tempo2 = txttempo2 == null ? rpc.Tempo2 : txttempo2;
                rpc.Tempo3 = txttempo3 == null ? rpc.Tempo3 : txttempo3;
                rpc.Tempo4 = txttempo4 == null ? rpc.Tempo4 : txttempo4;
                rpc.Tempo5 = txttempo5 == null ? rpc.Tempo5 : txttempo5;
                //
                if (id_receita_passo > 0)
                {
                    db.Entry(rpc).State = EntityState.Modified;
                }
                else
                {
                    db.ReceitaPassoCentrifugacao.Add(rpc);
                }
                //
                db.SaveChanges();
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
        public JsonResult CarregaDadosEdit(int id_receita, int id_passo, string tipo)
        {
            string sret = string.Empty;
            ReceitaPassoLavagem oReceitaPassoLavagem = new ReceitaPassoLavagem();
            ReceitaPassoCentrifugacao oReceitaPassoCentrifugacao = new ReceitaPassoCentrifugacao();
            ReceitaPassoModel oReceitaPassoModel = new ReceitaPassoModel();
            //
            try
            {
                Receita oReceita = db.Receita.Where(a => a.Id == id_receita).FirstOrDefault();
                //
                bool bacho = false;
                if (oReceita != null)
                {
                    foreach (ReceitaPasso item in oReceita.ReceitaPasso)
                    {
                        if (tipo.Equals("3"))
                        {
                            foreach (var item_centrifuga in item.ReceitaPassoCentrifugacao)
                            {
                                if (item_centrifuga.Id == id_passo)
                                {
                                    oReceitaPassoModel.Id = item.Id;
                                    oReceitaPassoModel.Decricao = item.Decricao;
                                    oReceitaPassoModel.Tipo = item.Tipo;
                                    oReceitaPassoCentrifugacao.Id = item_centrifuga.Id;
                                    oReceitaPassoCentrifugacao.Id_ReceitaPasso = item_centrifuga.Id_ReceitaPasso;
                                    oReceitaPassoCentrifugacao.Saida = item_centrifuga.Saida;
                                    oReceitaPassoCentrifugacao.Tempo1 = item_centrifuga.Tempo1;
                                    oReceitaPassoCentrifugacao.Tempo2 = item_centrifuga.Tempo2;
                                    oReceitaPassoCentrifugacao.Tempo3 = item_centrifuga.Tempo3;
                                    oReceitaPassoCentrifugacao.Tempo4 = item_centrifuga.Tempo4;
                                    oReceitaPassoCentrifugacao.Tempo5 = item_centrifuga.Tempo5;
                                    oReceitaPassoCentrifugacao.Velocidade1 = item_centrifuga.Velocidade1;
                                    oReceitaPassoCentrifugacao.Velocidade2 = item_centrifuga.Velocidade2;
                                    oReceitaPassoCentrifugacao.Velocidade3 = item_centrifuga.Velocidade3;
                                    oReceitaPassoCentrifugacao.Velocidade4 = item_centrifuga.Velocidade4;
                                    oReceitaPassoCentrifugacao.Velocidade5 = item_centrifuga.Velocidade5;
                                    //oReceitaPassoCentrifugacao = item_centrifuga;
                                    oReceitaPassoLavagem.ReceitaPasso = null;
                                    bacho = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (var item_lavagem in item.ReceitaPassoLavagem)
                            {
                                if (item_lavagem.Id == id_passo)
                                {
                                    oReceitaPassoModel.Id = item.Id;
                                    oReceitaPassoModel.Decricao = item.Decricao;
                                    oReceitaPassoModel.Tipo = item.Tipo;
                                    oReceitaPassoLavagem = item_lavagem;
                                    oReceitaPassoLavagem.ReceitaPasso = null;
                                    bacho = true;
                                    break;
                                }
                            }
                        }
                        //
                        if (bacho) break;
                    }
                    //
                    sret = "ok";
                }
                else
                {
                    sret = "receita";
                }
            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
            //
            return Json(
                new {
                    data = sret,
                    receita_passo = oReceitaPassoModel,
                    receita_passo_lavagem = oReceitaPassoLavagem,
                    receita_passo_centrifugacao = oReceitaPassoCentrifugacao,
                    results = 0,
                    success = true }, 
                JsonRequestBehavior.AllowGet);
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
        public JsonResult Deleta(int id_receita, int id_receita_passo, int id_passo, string tipo)
        {
            string sret = string.Empty;
            //
            try
            {
                Receita oReceita = db.Receita.Where(a => a.Id == id_receita).FirstOrDefault();
                //
                if (oReceita != null)
                {
                    ReceitaPasso oReceitaPasso = db.ReceitaPasso.Where(a => a.Id == id_receita_passo).FirstOrDefault();
                    //
                    if (oReceitaPasso != null)
                    {
                        if (!tipo.Equals("3"))
                        {
                            ReceitaPassoLavagem rpl = db.ReceitaPassoLavagem.Where(a => a.Id == id_passo).FirstOrDefault();
                            //
                            if (rpl != null)
                            {
                                db.Entry(rpl).State = EntityState.Deleted;
                                db.Entry(oReceitaPasso).State = EntityState.Deleted;
                                db.SaveChanges();
                            }
                        }
                        else
                        {
                            ReceitaPassoCentrifugacao rpc = db.ReceitaPassoCentrifugacao.Where(a => a.Id == id_passo).FirstOrDefault();
                            //
                            if (rpc != null)
                            {
                                db.Entry(rpc).State = EntityState.Deleted;
                                db.Entry(oReceitaPasso).State = EntityState.Deleted;
                                db.SaveChanges();
                            }
                        }
                    }
                }
                //
                sret = "ok";
            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
            //
            return Json(new { data = sret, results = 0, success = true }, JsonRequestBehavior.AllowGet);
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