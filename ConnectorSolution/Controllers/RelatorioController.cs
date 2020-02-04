using Connector.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class RelatorioController : BaseController
    {
        // GET: Relatorio
        public ActionResult Index()
        {
            ViewBag.RelatorioAtivo = "active";
            //
            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;
            //
            DateTime dt = DateTime.Now;
            DateTime dt_ini = DateTime.Now.AddDays(-15);
            //
            var dia_ini = dt_ini.Day > 9 ? dt_ini.Day.ToString() : "0" + dt_ini.Day.ToString();
            var mes_ini = dt_ini.Month > 9 ? dt_ini.Month.ToString() : "0" + dt_ini.Month.ToString();
            var ano_ini = dt_ini.Year > 9 ? dt_ini.Year.ToString() : "0" + dt_ini.Year.ToString();
            var dia_fim = dt.Day > 9 ? dt.Day.ToString() : "0" + dt.Day.ToString();
            var mes_fim = dt.Month > 9 ? dt.Month.ToString() : "0" + dt.Month.ToString();
            var ano_fim = dt.Year > 9 ? dt.Year.ToString() : "0" + dt.Year.ToString();
            //
            ViewBag.DataFiltro = mes_ini + "/" + dia_ini + "/" + ano_ini + " - " + mes_fim + "/" + dia_fim + "/" + ano_fim;
            //
            return View();
        }

        public ActionResult Temperatura()
        {
            ViewBag.RelatorioAtivo = "active";
            //
            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;
            //
            DateTime dt = DateTime.Now;
            DateTime dt_ini = DateTime.Now.AddDays(-15);
            //
            var dia_ini = dt_ini.Day > 9 ? dt_ini.Day.ToString() : "0" + dt_ini.Day.ToString();
            var mes_ini = dt_ini.Month > 9 ? dt_ini.Month.ToString() : "0" + dt_ini.Month.ToString();
            var ano_ini = dt_ini.Year > 9 ? dt_ini.Year.ToString() : "0" + dt_ini.Year.ToString();
            var dia_fim = dt.Day > 9 ? dt.Day.ToString() : "0" + dt.Day.ToString();
            var mes_fim = dt.Month > 9 ? dt.Month.ToString() : "0" + dt.Month.ToString();
            var ano_fim = dt.Year > 9 ? dt.Year.ToString() : "0" + dt.Year.ToString();
            //
            ViewBag.DataFiltro = mes_ini + "/" + dia_ini + "/" + ano_ini + " - " + mes_fim + "/" + dia_fim + "/" + ano_fim;
            //
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

        public JsonResult RelatorioTemperaturaAtmosfera(int idempresa, string lista_ids, string periodo, string horaini, string horafim)
        {
            DateTime data_filtro_ini = new DateTime();
            DateTime data_filtro_fim = new DateTime();
            //
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
            if (!string.IsNullOrEmpty(periodo))
            {
                string[] datas_filro = periodo.Split('-');
                string[] data_um = datas_filro[0].ToString().Split('/');
                string[] data_dois = datas_filro[1].ToString().Split('/');
                //
                if (!string.IsNullOrEmpty(horaini) && horaini.Contains(":"))
                {
                    data_filtro_ini = new DateTime(Convert.ToInt32(data_um[2]), 
                        Convert.ToInt32(data_um[0]), 
                        Convert.ToInt32(data_um[1]), 
                        Convert.ToInt32(horaini.Split(':')[0]),
                        Convert.ToInt32(horaini.Split(':')[1]), 1);
                }
                else
                {
                    data_filtro_ini = new DateTime(Convert.ToInt32(data_um[2]), Convert.ToInt32(data_um[0]), Convert.ToInt32(data_um[1]), 0, 0, 1);
                }
                //
                if (!string.IsNullOrEmpty(horaini) && horafim.Contains(":"))
                {
                    data_filtro_fim = new DateTime(Convert.ToInt32(data_dois[2]), 
                        Convert.ToInt32(data_dois[0]), 
                        Convert.ToInt32(data_dois[1]),
                        Convert.ToInt32(horafim.Split(':')[0]),
                        Convert.ToInt32(horafim.Split(':')[1]), 59);
                }
                else
                {
                    data_filtro_fim = new DateTime(Convert.ToInt32(data_dois[2]), Convert.ToInt32(data_dois[0]), Convert.ToInt32(data_dois[1]), 23, 59, 59);
                }
            }
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
                    //
                    string sql_temp = @"SELECT Cast(datahora AS DATE)                   AS 'Data',
                                        DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
                                        Datepart(month, datahora)                AS Mes, 
                                        Datepart(day, datahora)                  AS Dia, 
                                        ROUND((Avg(Cast(temperatura AS FLOAT)) / 10 ), 2) AS 'TemperaturaMedia' 
                                 FROM   ColetorTemperaturaHistorico 
                                WHERE Id_Coletor = " + coletor.Id + @"
                                    AND DataHora >= '" + data_filtro_ini.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                    AND DataHora <= '" + data_filtro_fim.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                    AND Temperatura IS NOT NULL 
                                 GROUP  BY Cast(datahora AS DATE), 
                                           Datepart(day, datahora), 
                                           Datepart(month, datahora) 
                                 ORDER  BY Datepart(month, datahora), 
                                           Datepart(day, datahora)";
                    //
                    SQLController sqlcontroller = new SQLController();
                    DataTable data_temp = sqlcontroller.ExecutaSQL(sql_temp);
                    //
                    if (data_temp != null && data_temp.Rows.Count > 0)
                    {
                        //var myChart = new Chart(width: 600, height: 400)
                        //    .AddTitle("Chart Title")
                        //    .AddSeries(
                        //        name: "Employee",
                        //        xValue: new[] { "Peter", "Andrew", "Julie", "Mary", "Dave" },
                        //        yValues: new[] { "2", "6", "4", "5", "3" })
                        //    .Write();
                        //myChart.Save(@"C:\Users\Alexandre\Documents\Git\connector\PlataformaWeb\ConnectorSolution\Temp\teste", "png");

                        double[] valoresY = new double[data_temp.Rows.Count];
                        string[] valoresX = new string[data_temp.Rows.Count];
                        //
                        for (int k = 0; k < data_temp.Rows.Count; k++)
                        {
                            valoresX[k] = Convert.ToString(data_temp.Rows[k]["Dia"]);
                            valoresY[k] = Convert.ToDouble(data_temp.Rows[k]["TemperaturaMedia"]);
                        }
                        //
                        string sFileNameImg = "img" + DateTime.Today.ToShortDateString().Replace("/", "_") + "_" + DateTime.Now.ToLongTimeString().Replace(":", "_") + ".png";
                        //
                        var myChartdois = new Chart(width: 1200, height: 600)
                                                .AddTitle("Temperatura")
                                                .AddSeries(chartType: "line", xValue: valoresX, yValues: valoresY, axisLabel: "axisLabel", xField: "xField", yFields: "yFields");
                        /*
                        var myChartdois = new Chart(width: 800, height: 400)
                            .AddTitle("Chart Title")
                            .AddSeries(
                                      chartType: "line",
                                      xValue: new[] { DateTime.Now,
                              DateTime.Now.AddSeconds(1),
                              DateTime.Now.AddSeconds(2),
                              DateTime.Now.AddSeconds(3),
                              DateTime.Now.AddSeconds(4) },
                                      yValues: new[] { 40, 100, 60, 80, 20 });
                                      */
                        myChartdois.Save(@"C:\Users\Alexandre\Documents\Git\connector\PlataformaWeb\ConnectorSolution\Temp\" + sFileNameImg, "png");
                    }
                    #region ANTES                    
                    /*
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
                    */
                    #endregion
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
                sFileName = "Relatório_Máquinas" + DateTime.Today.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString() + "_.pdf";
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

        private Byte[] RetornaImagemLink(string caminho)
        {
            Byte[] bytes = null;
            //
            string app_path = ((HttpRequestWrapper)this.Request).PhysicalApplicationPath;
            app_path += "Content\\images\\Complementos\\";
            string arquivo = string.Empty;
            //
            if (caminho.IndexOf("\\") > 0)
            {
                arquivo = caminho.Split('\\')[caminho.Split('\\').Length - 1];
            }
            //
            string scaminho = app_path + "\\" + arquivo;
            //
            if (System.IO.File.Exists(scaminho))
            {
                FileStream fs = new FileStream(scaminho, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                bytes = br.ReadBytes(Convert.ToInt32(br.BaseStream.Length));
            }
            //
            return bytes;
        }

    }
}