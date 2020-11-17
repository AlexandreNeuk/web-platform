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
            List<PacoteBrokerModels> list_ret_tmp = new List<PacoteBrokerModels>();
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
                                #region Descrição Passos - Tipos de lavagens
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

                                if (item_ReceitaPasso.ReceitaPassoCentrifugacao.Count > 0)
                                {
                                    foreach (var item_ReceitaPassoCentrifugacao in item_ReceitaPasso.ReceitaPassoCentrifugacao)
                                    {
                                        ReceitaPassoCentrifugacao rpc = item_ReceitaPassoCentrifugacao;
                                        //
                                        mensagem = MontaPalavra(item_ReceitaPasso.Tipo);
                                        mensagem += MontaPalavra(rpc.Velocidade1);
                                        mensagem += MontaPalavra(rpc.Tempo1);
                                        mensagem += MontaPalavra(rpc.Velocidade2);
                                        mensagem += MontaPalavra(rpc.Tempo2);
                                        mensagem += MontaPalavra(rpc.Velocidade3);
                                        mensagem += MontaPalavra(rpc.Tempo3);
                                        mensagem += MontaPalavra(rpc.Velocidade4);
                                        mensagem += MontaPalavra(rpc.Tempo4);
                                        mensagem += MontaPalavra(rpc.Velocidade5);
                                        mensagem += MontaPalavra(rpc.Tempo5);
                                        mensagem += MontaPalavra(rpc.Saida);
                                        mensagem += "00000000000000000000000000000000000000000000";
                                        //
                                        PacoteBrokerModels pbm = new PacoteBrokerModels();
                                        pbm.Id = maquina.ID;
                                        pbm.Topico = maquina.Topico + "_passo" + j; ;
                                        pbm.Mensagem = mensagem;
                                        pbm.MaquinaTopico = maquina.Topico;
                                        pbm.NomeReceita = oReceita.Descricao;
                                        //
                                        list_ret_tmp.Add(pbm);
                                        j++;
                                    }
                                }
                                if (item_ReceitaPasso.ReceitaPassoLavagem.Count > 0)
                                {
                                    foreach (var item_ReceitaPassoLavagem in item_ReceitaPasso.ReceitaPassoLavagem)
                                    {
                                        ReceitaPassoLavagem rpl = item_ReceitaPassoLavagem;
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
                                        if (rpl.ProdutoA.Equals(""))
                                        {
                                            mensagem += "0000";
                                        }
                                        else
                                        {
                                            mensagem += "0001";
                                        }
                                        mensagem += MontaPalavra(rpl.ProdutoA);
                                        if (rpl.ProdutoB.Equals(""))
                                        {
                                            mensagem += "0000";
                                        }
                                        else
                                        {
                                            mensagem += "0001";
                                        }
                                        mensagem += MontaPalavra(rpl.ProdutoB);
                                        if (rpl.ProdutoC.Equals(""))
                                        {
                                            mensagem += "0000";
                                        }
                                        else
                                        {
                                            mensagem += "0001";
                                        }
                                        mensagem += MontaPalavra(rpl.ProdutoC);
                                        if (rpl.ProdutoD.Equals(""))
                                        {
                                            mensagem += "0000";
                                        }
                                        else
                                        {
                                            mensagem += "0001";
                                        }
                                        mensagem += MontaPalavra(rpl.ProdutoD);
                                        if (rpl.ProdutoE.Equals(""))
                                        {
                                            mensagem += "0000";
                                        }
                                        else
                                        {
                                            mensagem += "0001";
                                        }
                                        mensagem += MontaPalavra(rpl.ProdutoE);
                                        if (rpl.ProdutoF.Equals(""))
                                        {
                                            mensagem += "0000";
                                        }
                                        else
                                        {
                                            mensagem += "0001";
                                        }
                                        mensagem += MontaPalavra(rpl.ProdutoF);
                                        if (rpl.ProdutoG.Equals(""))
                                        {
                                            mensagem += "0000";
                                        }
                                        else
                                        {
                                            mensagem += "0001";
                                        }
                                        mensagem += MontaPalavra(rpl.ProdutoG);
                                        //
                                        PacoteBrokerModels pbm = new PacoteBrokerModels();
                                        pbm.Id = maquina.ID;
                                        pbm.Topico = maquina.Topico + "_passo" + j;
                                        pbm.Mensagem = mensagem;
                                        pbm.MaquinaTopico = maquina.Topico;
                                        pbm.NomeReceita = oReceita.Descricao;
                                        //
                                        list_ret_tmp.Add(pbm);
                                        j++;
                                    }
                                }
                            }
                            //
                            int tot = 20 - list_ret_tmp.Count;
                            int idiceTopico = list_ret_tmp.Count;
                            idiceTopico++;
                            for (int i = 0; i < tot; i++)
                            {
                                PacoteBrokerModels pbm = new PacoteBrokerModels();
                                pbm.Id = maquina.ID;
                                pbm.Topico = maquina.Topico + "_passo" + idiceTopico;
                                pbm.MaquinaTopico = maquina.Topico;
                                pbm.NomeReceita = oReceita.Descricao;
                                pbm.Mensagem = "00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
                                list_ret_tmp.Add(pbm);
                                idiceTopico++;
                            }
                            //
                            foreach (var item_tmp in list_ret_tmp)
                            {
                                list_ret.Add(item_tmp);
                            }
                            //
                            list_ret_tmp = new List<PacoteBrokerModels>();
                        }
                    }
                    //
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