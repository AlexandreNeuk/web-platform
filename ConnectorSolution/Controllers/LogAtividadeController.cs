using Connector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class LogAtividadeController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.LogAtividadeAtivo = "active";
            List<Maquina> lista_maquina = new List<Maquina>();
            //
            if (Tipo_Empresa == 1)
            {
                List<EmpresaModel> list_empresas = PegaEmpresas();
                List<Maquina> lista_maquina_temp = new List<Maquina>();
                //
                foreach (EmpresaModel item in list_empresas)
                {
                    lista_maquina_temp = db.Maquina.Where(a => a.Id_Empresa == item.Id).ToList();
                    lista_maquina.AddRange(lista_maquina_temp);
                }                
            }
            else
            {
                lista_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            }
            //
            ViewBag.ListaMaquinas = lista_maquina;
            return View();
        }
        //
        public JsonResult CarregaDados()
        {
            string sret = string.Empty;
            List<Medidor_MaquinaModels> lista_retorno = new List<Medidor_MaquinaModels>();
            List<EmpresaModel> lista_empresas = new List<EmpresaModel>();
            List<Maquina> lista_maquinas = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            //
            try
            {
                if (Tipo_Empresa == 1)
                {
                    Empresa empresa = db.Empresa.Where(x => x.Id == Codigo_Empresa).FirstOrDefault();
                    //
                    EmpresaModel empresa_model = new EmpresaModel();
                    empresa_model.Id = empresa.Id;
                    empresa_model.Nome = empresa.Nome;
                    lista_empresas.Add(empresa_model);
                    //
                    string empresas = empresa.Empresas;
                    if (!string.IsNullOrEmpty(empresas))
                    {
                        string[] empresas_array = empresas.Split(',');
                        int[] ids = Array.ConvertAll(empresas_array, s => int.Parse(s));
                        var lista_sub_empresas = db.Empresa.Where(r => ids.Contains(r.Id));
                        if (lista_sub_empresas.Any())
                        {
                            List<Maquina> lista_sub_maquinas = new List<Maquina>();
                            foreach (Empresa sub_empresa in lista_sub_empresas)
                            {
                                empresa_model = new EmpresaModel();
                                empresa_model.Id = sub_empresa.Id;
                                empresa_model.Nome = sub_empresa.Nome;
                                lista_empresas.Add(empresa_model);

                                lista_sub_maquinas = db.Maquina.Where(x => x.Id_Empresa == sub_empresa.Id).ToList();
                                lista_maquinas.AddRange(lista_sub_maquinas);
                            }
                        }
                    }
                }
                //
                foreach (Maquina item in lista_maquinas)
                {
                    Medidor_MaquinaModels novo = new Medidor_MaquinaModels();
                    novo.ID = item.ID;
                    novo.Id_Empresa = item.Id_Empresa;
                    novo.Descricao = item.Descricao;
                    novo.DescricaoEmpresa = item.Empresa.Nome;
                    //
                    Coletor oColetor = db.Coletor.Where(a => a.Maquina.Id_Empresa == item.Empresa.Id && a.Id_Maquina == item.ID).FirstOrDefault();
                    //
                    if (oColetor != null)
                    {
                        novo.DescricaoMedidor = oColetor.Descricao;
                        novo.Id_Coletor = oColetor.Id;
                    }
                    //
                    lista_retorno.Add(novo);
                }
                //
                sret = "ok";
            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
            //
            lista_retorno = lista_retorno.OrderBy(x => x.ID).ToList();
            //
            return Json(new { data = sret, lista_retorno, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult PegaLogAtividade(string list_emp, string lista_ids, bool chkmaq, bool chkpress, bool chktemp, bool chkprod, int totreg)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            List<LogAtividadeModel> lista_retorno = new List<LogAtividadeModel>();
            //
            try
            {
                string[] lista_maquina_id = lista_ids.Split(',');
                string[] lista_empresa_id = list_emp.Split(',');
                //
                for (int i = 0; i < lista_maquina_id.Length; i++)
                {
                    int idmaquina = Convert.ToInt32(lista_maquina_id[i]);
                    int idempresa = Convert.ToInt32(lista_empresa_id[i]);
                    //
                    Maquina maq = db.Maquina.Where(x => x.ID == idmaquina && x.Id_Empresa == idempresa).FirstOrDefault();
                    //
                    if (maq != null)
                    {                        
                        Coletor col = db.Coletor.Where(x => x.Id_Empresa == idempresa && x.Id_Maquina == idmaquina).FirstOrDefault();
                        //
                        if (col != null)
                        {
                            string iddispositivo = col.MAC;
                            //
                            if (chkmaq)
                            {
                                List<LogAtividade> lista_atividades = db.LogAtividade
                                    .Where(a => a.Id_Empresa == idempresa && a.Id_Dispositivo.ToLower() == iddispositivo.ToLower())                                    
                                    .OrderByDescending(x => x.Id)
                                    .Take(totreg)
                                    .ToList();
                                Empresa emp = new Empresa();
                                //
                                if (lista_atividades.Count > 0)
                                {
                                    emp = db.Empresa.Where(x => x.Id == idempresa).FirstOrDefault();
                                }
                                //
                                foreach (LogAtividade item in lista_atividades)
                                {
                                    LogAtividadeModel lam = new LogAtividadeModel();
                                    //
                                    lam.Id = item.Id;
                                    lam.Descricao = item.Descricao;
                                    lam.Id_Dispositivo = item.Id_Dispositivo.ToUpper();
                                    lam.NomeColetor = col.Descricao;
                                    lam.NomeEmpresa = emp.Nome;
                                    lam.NomeMaquina = maq.Descricao;
                                    lam.DataHora = Convert.ToDateTime(item.DataHora);
                                    lam.DataHoraDesc = FormataDataHoraTela(item.DataHora);
                                    
                                    //
                                    switch (item.Tipo)
                                    {
                                        case 1:
                                            lam.Descricao = "Incício Programa";
                                            lam.Imagem = "<i class='fa fa-code'></i>";
                                            break;
                                        case 2:
                                            lam.Descricao = "Incício loop";
                                            lam.Imagem = "<i class='fa fa-bell'></i>";
                                            break;
                                        case 3:
                                            lam.Descricao = "Fim loop";
                                            lam.Imagem = "<i class='fa fa-bell-slash'></i>";
                                            break;
                                        case 4:
                                            lam.Descricao = "Fim Programa";
                                            lam.Imagem = "<i class='fa fa-code'></i>";
                                            break;
                                        case 5: lam.Descricao = "Pega dados maquina"; break;
                                    }
                                    //
                                    lista_retorno.Add(lam);
                                }
                            }
                            //
                            if (chkpress)
                            {
                                List<ColetorPressaoHistorico> lista_colt_press_histo = db.ColetorPressaoHistorico
                                    .Where(a => a.Id_Coletor == col.Id)                                    
                                    .OrderByDescending(x => x.DataHora)
                                    .Take(totreg)
                                    .ToList();
                                //
                                foreach (ColetorPressaoHistorico item in lista_colt_press_histo)
                                {
                                    LogAtividadeModel lam = new LogAtividadeModel();
                                    //
                                    lam.Id = item.Id;
                                    lam.Descricao = "Pressão: " + item.Pressao;
                                    lam.Id_Dispositivo = item.Coletor.MAC.ToUpper();
                                    lam.NomeColetor = col.Descricao;
                                    lam.NomeEmpresa = item.Coletor.Empresa.Nome;
                                    lam.NomeMaquina = maq.Descricao;
                                    lam.DataHora = Convert.ToDateTime(item.DataHora);
                                    lam.DataHoraDesc = FormataDataHoraTela(item.DataHora);
                                    lam.Imagem = "<i class='fa fa-tachometer-alt'></i>";
                                    //
                                    lista_retorno.Add(lam);
                                }
                            }
                            //
                            if (chktemp)
                            {
                                List<ColetorTemperaturaHistorico> lista_colt_temp_histo = db.ColetorTemperaturaHistorico
                                    .Where(a => a.Id_Coletor == col.Id)
                                    .OrderByDescending(x => x.DataHora)
                                    .Take(totreg)
                                    .ToList();
                                //
                                foreach (ColetorTemperaturaHistorico item in lista_colt_temp_histo)
                                {
                                    LogAtividadeModel lam = new LogAtividadeModel();
                                    //
                                    lam.Id = item.Id;
                                    lam.Descricao = "Temperatura: " + item.Temperatura;
                                    lam.Id_Dispositivo = item.Coletor.MAC.ToUpper();
                                    lam.NomeColetor = col.Descricao;
                                    lam.NomeEmpresa = item.Coletor.Empresa.Nome;
                                    lam.NomeMaquina = maq.Descricao;
                                    lam.DataHora = Convert.ToDateTime(item.DataHora);
                                    lam.DataHoraDesc = FormataDataHoraTela(item.DataHora);
                                    lam.Imagem = "<i class='fa fa-thermometer-half'></i>";
                                    //
                                    lista_retorno.Add(lam);
                                }
                            }
                            //
                            if (chkprod)
                            {
                                List<ColetorProducaoHistorico> lista_colt_prod_histo = db.ColetorProducaoHistorico
                                    .Where(a => a.Id_Coletor == col.Id)
                                    .OrderByDescending(x => x.DataHora)
                                    .Take(totreg)
                                    .ToList();
                                //
                                foreach (ColetorProducaoHistorico item in lista_colt_prod_histo)
                                {
                                    LogAtividadeModel lam = new LogAtividadeModel();
                                    //
                                    lam.Id = item.Id;
                                    lam.Descricao = "Ciclos: " + item.Valor;
                                    lam.Id_Dispositivo = item.Coletor.MAC.ToUpper();
                                    lam.NomeColetor = col.Descricao;
                                    lam.NomeEmpresa = item.Coletor.Empresa.Nome;
                                    lam.NomeMaquina = maq.Descricao;
                                    lam.DataHora = Convert.ToDateTime(item.DataHora);
                                    lam.DataHoraDesc = FormataDataHoraTela(item.DataHora);
                                    lam.Imagem = "<i class='fa fa-tasks'></i>";
                                    //
                                    lista_retorno.Add(lam);
                                }
                            }
                            //
                            ret = "ok";
                        }
                        else
                        {
                            ret = "col";
                        }
                    }
                    else
                    {
                        ret = "maq";
                    }
                }
            }
            catch (Exception exc)
            {
                erro = exc.Message;
                ret = "nok";
            }
            //
            lista_retorno = lista_retorno.OrderByDescending(x => x.DataHora).ToList();
            //
            return Json(new { ret, lista_retorno, results = 1, erro, success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}