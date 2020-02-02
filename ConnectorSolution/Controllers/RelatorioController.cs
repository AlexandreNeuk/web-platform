using Connector.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class RelatorioController : BaseController
    {
        // GET: Relatorio
        public ActionResult Index()
        {
            ViewBag.RelatorioAtivo = "active";

            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;

            return View();
        }

        public JsonResult RelatorioMaquina(int idempresa, string lista_ids, int periodo)
        {
            string sreult = string.Empty;
            string sFileName = string.Empty;
            Empresa empresaLista = null;
            //
            List<EmpresaRelatorioModel> lista_empresa_model = new List<EmpresaRelatorioModel>();
            List<ColetorAlerta> lista_coletoralerta_model = new List<ColetorAlerta>();
            List<ColetorAlertaLog> lista_coletor_alerta_log = new List<ColetorAlertaLog>();
            //
            List<ColetorAlertaModel> lista_coletor_alerta_item_model = new List<ColetorAlertaModel>();
            List<ColetorAlertaLogModel> lista_coletor_alerta_log_model = new List<ColetorAlertaLogModel>();
            List<MaquinaItensReportModel> lista_maquina_itens_report_model = new List<MaquinaItensReportModel>();            
            //
            empresaLista = db.Empresa.Where(x => x.Id == idempresa).FirstOrDefault();
            //
            if (empresaLista != null)
            {
                EmpresaRelatorioModel tmp = new EmpresaRelatorioModel();
                //
                tmp.Id = empresaLista.Id;
                tmp.Nome = empresaLista.Nome;
                tmp.Numero = empresaLista.Numero;
                tmp.Telefone = empresaLista.Telefone;
                tmp.Site = empresaLista.Site;
                tmp.EstadoSigla = PegaEstadoSigla(empresaLista.Estado);
                tmp.Endereco = empresaLista.Endereco;
                tmp.CEP = empresaLista.CEP;
                tmp.Cidade = empresaLista.Cidade;
                tmp.NomeFantasia = empresaLista.NomeFantasia;
                tmp.Email = empresaLista.Email;
                tmp.Bairro = empresaLista.Bairro;
                lista_empresa_model.Add(tmp);
            }
            //
            string[] lista_ids_maquinas = lista_ids.Split(',');
            for (int i = 0; i < lista_ids_maquinas.Length; i++)
            {
                int idmaquina = Convert.ToInt32(lista_ids_maquinas[i]);
                Coletor coletor = db.Coletor.Where(x => x.Maquina != null && x.Maquina.ID == idmaquina).FirstOrDefault();
                //
                if (coletor != null)
                {
                    MaquinaItensReportModel mirm = new MaquinaItensReportModel();
                    //
                    mirm.DescricaoColetor = coletor.Descricao;
                    mirm.DescricaoMaquina = coletor.Maquina.Descricao;
                    //
                    lista_maquina_itens_report_model.Add(mirm);
                    //
                    lista_coletoralerta_model = db.ColetorAlerta.Where(x => x.Id_Coletor == coletor.Id).ToList();
                    foreach (ColetorAlerta item in lista_coletoralerta_model)
                    {
                        ColetorAlertaModel cam = new ColetorAlertaModel();
                        //
                        cam.Id = item.Id;
                        cam.Id_Empresa = item.Id_Empresa;
                        cam.Id_Coletor = item.Id_Coletor;
                        cam.Id_TipoAlerta = item.Id_TipoAlerta;
                        cam.Prioridade = item.Prioridade.HasValue ? item.Prioridade.Value : 0;
                        cam.DescricaoColetor = "";
                        cam.Descricao = item.Descricao;
                        cam.Email = item.Email;
                        switch (item.ColetorTipoAlerta.Tipo.Value)
                        {
                            case 1:
                                cam.DescricaoTipoAlerta = "Temperatura";
                                break;
                            case 2:
                                cam.DescricaoTipoAlerta = "Pressão";
                                break;
                            case 3:
                                cam.DescricaoTipoAlerta = "Produção";
                                break;
                        }
                        cam.AtivoDescricao = item.Ativo.Value == 1 ? "Sim" : "Não";
                        //
                        cam.Ativo = item.Ativo.Value;
                        cam.Valor = item.Valor;
                        cam.Regra = item.Regra.Value;
                        cam.AtivoDescricao = "";
                        cam.Prioridade = 0;
                        //
                        lista_coletor_alerta_item_model.Add(cam);
                    }
                    //

                }
            }
            //
            ReportDocument oRelatorioMaquina = null;
            ReportDocument oRelatorioMaquinaItem = null;
            ReportDocument oRelatorioMaquinaItemAlerta = null;
            ReportDocument oRelatorioMaquinaItemLogAlerta = null;
            //
            try
            {
                oRelatorioMaquina = new ReportDocument();
                oRelatorioMaquinaItem = new ReportDocument();
                oRelatorioMaquinaItemAlerta = new ReportDocument();
                oRelatorioMaquinaItemLogAlerta = new ReportDocument();
                oRelatorioMaquina = new ReportDocument();
                //                
                string local = AppDomain.CurrentDomain.BaseDirectory + "Temp";
                sFileName = "Relatório_Máquinas" + DateTime.Today.ToShortDateString() + "_"+ DateTime.Now.ToLongTimeString() + "_.pdf";
                sFileName = sFileName.Replace("/", "-");
                sFileName = sFileName.Replace("\\", "-");
                sFileName = sFileName.Replace(":", "-");
                //
                oRelatorioMaquina.Load(System.Web.HttpContext.Current.Server.MapPath("~/Relatorio/") + "RelatorioMaquinaTemp.rpt");
                oRelatorioMaquina.SetDataSource(lista_empresa_model);
                oRelatorioMaquina.Database.Tables[0].SetDataSource(lista_empresa_model);
                //oRelatorioMaquina.Subreports["RelatorioMaquinaItem.rpt"].Database.Tables[0].SetDataSource(lista_maquina_itens_report_model);
                ////
                //oRelatorioMaquinaItemAlerta.Load(System.Web.HttpContext.Current.Server.MapPath("~/Relatorio/") + "RelatorioMaquinaItemAlerta.rpt");
                //oRelatorioMaquinaItemAlerta.SetDataSource(lista_coletor_alerta_item_model);
                //oRelatorioMaquinaItemAlerta.Database.Tables[0].SetDataSource(lista_coletor_alerta_item_model);
                //
                //oRelatorioMaquinaItem.Load(System.Web.HttpContext.Current.Server.MapPath("~/Relatorio/") + "RelatorioMaquinaItem.rpt");
                //oRelatorioMaquinaItem.SetDataSource(lista_maquina_itens_report_model);
                //oRelatorioMaquinaItem.Database.Tables[0].SetDataSource(lista_maquina_itens_report_model);
                //oRelatorioMaquinaItem.Subreports["RelatorioMaquinaItemAlerta.rpt"].Database.Tables[0].SetDataSource(lista_coletor_alerta_item_model);
                //

                //

                //

                //
                oRelatorioMaquina.ExportToDisk(ExportFormatType.PortableDocFormat, local + "\\" + sFileName);
                //
                oRelatorioMaquinaItemAlerta.Close();
                oRelatorioMaquina.Close();
                oRelatorioMaquinaItem.Close();
                //
                oRelatorioMaquinaItemAlerta.Dispose();
                oRelatorioMaquina.Dispose();
                oRelatorioMaquinaItem.Dispose();
                //
                System.GC.Collect();
                sreult = "ok";
            }
            catch (Exception exc)
            {
                //Utils.Utils.Log.GravaLogExc(exc);
                sFileName = "Erro: " + exc.Message;
                sreult = "nok";
            }
            //
            return Json(new
            {
                data = sFileName,
                sreult,
                results = lista_empresa_model.Count(),
                success = true,
                errors = String.Empty
            }, JsonRequestBehavior.AllowGet);
        }
    }
}