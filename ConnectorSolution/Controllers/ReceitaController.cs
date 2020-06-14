using Connector.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            ViewBag.ListaMaquinas = PegaMaquinas();
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
                    int tot_recei_centr = 0; // db.ReceitaPassoCentrifugacao.Where(x => x.Id_Receita == item.Id).ToList().Count;
                    int tot_recei_lava = 0; // db.ReceitaPassoLavagem.Where(x => x.Id_Receita == item.Id).ToList().Count;
                    //
                    foreach (var item_receita_passo in item.ReceitaPasso)
                    {
                        if (item_receita_passo.ReceitaPassoCentrifugacao.Count > 0)
                        {
                            tot_recei_centr++;
                        }
                        if (item_receita_passo.ReceitaPassoLavagem.Count > 0)
                        {
                            tot_recei_lava++;
                        }
                    }
                    //
                    ReceitaModel oReceitaModel = new ReceitaModel();
                    //
                    oReceitaModel.Id = item.Id;
                    oReceitaModel.Id_Empresa = item.Id_Empresa.HasValue ? item.Id_Empresa.Value : Codigo_Empresa;
                    //
                    oReceitaModel.Ativo = item.Ativo;
                    oReceitaModel.Descricao = item.Descricao;

                    //
                    oReceitaModel.TotalLavagem = tot_recei_lava;
                    oReceitaModel.TotalCentrifuga = tot_recei_centr;
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
        //
        public JsonResult MontaPacoteMqtt(int idreceita, string id_maquinas)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            List<PacoteBrokerModels> list_ret = new List<PacoteBrokerModels>();
            try
            {
                Receita oReceita = db.Receita.Where(a => a.Id == idreceita && a.Id_Empresa == Codigo_Empresa).FirstOrDefault();
                //
                if (oReceita != null)
                {
                    string[] lista_maquinas = id_maquinas.Split(',');
                    //
                    foreach (var item in lista_maquinas)
                    {
                        int id = Int32.Parse(item);
                        Maquina maquina = db.Maquina.Where(a => a.ID == id).FirstOrDefault();
                        //
                        if (maquina != null)
                        {
                            string topico = string.Empty;
                            string mensagem = string.Empty;
                            //
                            topico = "topico" + maquina.ID + "Maquina" + maquina.ID;
                            //
                            int j = 1;
                            //
                            foreach (ReceitaPasso item_ReceitaPasso in oReceita.ReceitaPasso)
                            {
                                if (item_ReceitaPasso.ReceitaPassoCentrifugacao.Count > 0)
                                {

                                }
                                if (item_ReceitaPasso.ReceitaPassoLavagem.Count > 0)
                                {                                    
                                    foreach (var item_ReceitaPassoLavagem in item_ReceitaPasso.ReceitaPassoLavagem)
                                    {
                                        ReceitaPassoLavagem rpl = item_ReceitaPassoLavagem;
                                        //
                                        #region Tipos
                                        /*
                                        <option id="1">Desabilitado</option>
                                        <option id="2">Lavagem</option>
                                        <option id="3">Centrifugação</option>
                                        <option id="4">Umectação</option>
                                        <option id="5">Pré-Lavagem</option>
                                        <option id="6">Alvejamento</option>
                                        <option id="7">Enxague</option>
                                        <option id="8">Neutralização</option>
                                        <option id="9">Amaciante</option>
                                        <option id="10">Molho</option>
                                         */
                                        #endregion
                                        //
                                        mensagem = MontaPalavra(item_ReceitaPasso.Tipo);
                                        mensagem += MontaPalavra(rpl.TempoOperacao);
                                        mensagem += MontaPalavra(rpl.TempoReversao);
                                        mensagem += MontaPalavra(rpl.RPM);
                                        mensagem += MontaPalavra(rpl.Temperatura);
                                        mensagem += MontaPalavra(rpl.SemVapor);
                                        mensagem += MontaPalavra(rpl.Entrada);
                                        mensagem += MontaPalavra(rpl.Nivel);
                                        mensagem += MontaPalavra(rpl.Saida);
                                        mensagem += MontaPalavra(rpl.ProdutoA);
                                        mensagem += MontaPalavra(rpl.ProdutoB);
                                        mensagem += MontaPalavra(rpl.ProdutoC);
                                        mensagem += MontaPalavra(rpl.ProdutoD);
                                        mensagem += MontaPalavra(rpl.ProdutoE);
                                        mensagem += MontaPalavra(rpl.ProdutoF);
                                        mensagem += MontaPalavra(rpl.ProdutoG);
                                        //
                                        // maq1_mensagem0
                                        // maq1_mensagem1
                                        // maq1_mensagem2
                                        topico = "maq1_passo" + j;
                                        PacoteBrokerModels pbm = new PacoteBrokerModels();
                                        pbm.Id = maquina.ID;
                                        pbm.Topico = topico;
                                        pbm.Mensagem = mensagem;
                                        //
                                        list_ret.Add(pbm);
                                        j++;
                                    }                                    
                                    //mensagem = itemReceitaPasso.ReceitaPassoLavagem[0]
                                }
                            }
                            //
                            int tot = 20 - list_ret.Count;
                            int idiceTopico = list_ret.Count;
                            idiceTopico++;
                            for (int i = 0; i < tot; i++)
                            {
                                PacoteBrokerModels pbm = new PacoteBrokerModels();
                                pbm.Id = maquina.ID;
                                pbm.Topico = "maq1_passo" + idiceTopico;
                                pbm.Mensagem = "0000000000000000000000000000000000000000000000000000000000000000";
                                list_ret.Add(pbm);
                                idiceTopico++;
                            }
                        }
                    }
                    //
                    
                    //db.Entry(oReceita).State = System.Data.EntityState.Deleted;
                    //tet();
                    //db.SaveChanges();
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

            return Json(new { ret, results = 0, erro, list_ret, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public string MontaPalavra(string text)
        {
            if (text != null)
            {
                if (text.Length > 4)
                {
                    text = text.Substring(0, 4);
                }
                else
                {
                    switch (text.Length)
                    {
                        case 0:
                            text = "0000";
                            break;
                        case 1:
                            text = "000" + text;
                            break;
                        case 2:
                            text = "00" + text;
                            break;
                        case 3:
                            text = "0" + text;
                            break;
                        case 4:
                            break;
                        default:
                            text = "0000";
                            break;
                    }
                }
            }
            else text = "0000";
            //
            return text;
        }
    }
}