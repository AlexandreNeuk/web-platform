using Connector.Models;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

        /*
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
        */

        public JsonResult RelatorioTemperaturaGetDados(int idempresa, string lista_ids, string periodo, string horaini, string horafim)
        {
            List<Object> lista = new List<Object>();
            DateTime data_filtro_ini = new DateTime();
            DateTime data_filtro_fim = new DateTime();
            string[] lista_ids_maquinas = lista_ids.Split(',');
            //
            string maq = string.Empty;
            string sreult = string.Empty;
            string sFileName = string.Empty;
            Empresa empresaLista = null;
            //
            List<EmpresaRelatorioModel> lista_empresa_model = new List<EmpresaRelatorioModel>();
            List<ColetorAlerta> lista_coletoralerta_model = new List<ColetorAlerta>();
            List<ColetorAlertaLog> lista_coletor_alerta_log = new List<ColetorAlertaLog>();
            //
            //List<ColetorAlertaModel> lista_coletor_alerta_item_model = new List<ColetorAlertaModel>();
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
            try
            {
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
                        if (string.IsNullOrEmpty(maq))
                        {
                            maq = coletor.Maquina.Descricao;
                        }
                        else
                        {
                            maq = maq + " - " + coletor.Maquina.Descricao;
                        }
                        //
                        lista_maquina_itens_report_model.Add(mirm);
                        //
                        lista_coletoralerta_model = db.ColetorAlerta.Where(x => x.Id_Coletor == coletor.Id).ToList();
                        //
                        string teste = @"
                                    SELECT Cast(datahora AS DATE)                   AS 'Data',
                                            DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
                                            Datepart(month, datahora)                AS Mes, 
                                            Datepart(day, datahora)                  AS Dia, 
                                            Temperatura
                                        FROM   ColetorTemperaturaHistorico 
                                    WHERE Id_Coletor = 47
                                        AND DataHora >= '2019-12-15 14:40:01.000'
                                        AND DataHora <= '2019-12-31 14:40:01.000'
                                        AND Temperatura IS NOT NULL 

                                        ORDER  BY Datepart(month, datahora), 
                                                Datepart(day, datahora)
                                    ";
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
                        DataTable data_temp = sqlcontroller.ExecutaSQL(teste);
                        //
                        if (data_temp != null && data_temp.Rows.Count > 0)
                        {
                            double[] valoresY = new double[data_temp.Rows.Count];
                            string[] valoresX = new string[data_temp.Rows.Count];
                            //
                            for (int k = 0; k < data_temp.Rows.Count; k++)
                            {
                                valoresX[k] = Convert.ToString(data_temp.Rows[k]["Dia"]) + "/" + Convert.ToString(data_temp.Rows[k]["Mes"]);
                                valoresY[k] = Convert.ToDouble(data_temp.Rows[k]["Temperatura"]);
                            }
                            //
                            List<Object> lista_itens = new List<Object>();
                            lista_itens.Add(valoresX);
                            lista_itens.Add(valoresY);
                            lista_itens.Add(coletor.Maquina.Descricao);
                            //
                            lista.Add(lista_itens);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                sFileName = "Erro: " + exc.Message;
                sreult = "nok";
            }
            //
            return Json(new
            {
                data = "OK",
                results = 1,
                success = true,
                errors = 0,
                dados = lista,
                maquinas = maq,
                periodo = data_filtro_ini.ToShortDateString() + " " + data_filtro_ini.ToShortTimeString() + " - " + data_filtro_fim.ToShortDateString() + " " + data_filtro_fim.ToShortTimeString()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RelatorioTemperaturaGetDados2(int idempresa, string lista_ids, string periodo, string horaini, string horafim)
        {
            List<Object> lista = new List<Object>();
            DateTime data_filtro_ini = new DateTime();
            DateTime data_filtro_fim = new DateTime();
            string[] lista_ids_maquinas = lista_ids.Split(',');
            //
            string maq = string.Empty;
            string sreult = string.Empty;
            string sFileName = string.Empty;
            Empresa empresaLista = null;
            //
            List<EmpresaRelatorioModel> lista_empresa_model = new List<EmpresaRelatorioModel>();
            List<ColetorAlerta> lista_coletoralerta_model = new List<ColetorAlerta>();
            List<ColetorAlertaLog> lista_coletor_alerta_log = new List<ColetorAlertaLog>();
            //
            //List<ColetorAlertaModel> lista_coletor_alerta_item_model = new List<ColetorAlertaModel>();
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
            try
            {
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
                        if (string.IsNullOrEmpty(maq)) {
                            maq = coletor.Maquina.Descricao;
                        }
                        else
                        {
                            maq = maq + " - " + coletor.Maquina.Descricao;
                        }
                        //
                        lista_maquina_itens_report_model.Add(mirm);
                        //
                        lista_coletoralerta_model = db.ColetorAlerta.Where(x => x.Id_Coletor == coletor.Id).ToList();
                        //
                        string teste = @"
                                    SELECT Cast(datahora AS DATE)                   AS 'Data',
                                            DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
                                            Datepart(month, datahora)                AS Mes, 
                                            Datepart(day, datahora)                  AS Dia, 
                                            Temperatura
                                        FROM   ColetorTemperaturaHistorico 
                                    WHERE Id_Coletor = 47
                                        AND DataHora >= '2019-12-15 14:40:01.000'
                                        AND DataHora <= '2019-12-31 14:40:01.000'
                                        AND Temperatura IS NOT NULL 

                                        ORDER  BY Datepart(month, datahora), 
                                                Datepart(day, datahora)
                                    ";
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
                        DataTable data_temp = sqlcontroller.ExecutaSQL(teste);
                        //
                        if (data_temp != null && data_temp.Rows.Count > 0)
                        {
                            double[] valoresY = new double[data_temp.Rows.Count];
                            string[] valoresX = new string[data_temp.Rows.Count];
                            //
                            for (int k = 0; k < data_temp.Rows.Count; k++)
                            {
                                valoresX[k] = Convert.ToString(data_temp.Rows[k]["Dia"]) + "/" + Convert.ToString(data_temp.Rows[k]["Mes"]);
                                valoresY[k] = Convert.ToDouble(data_temp.Rows[k]["Temperatura"]);
                            }
                            //
                            List<Object> lista_itens = new List<Object>();
                            lista_itens.Add(valoresX);
                            lista_itens.Add(valoresY);
                            lista_itens.Add(coletor.Maquina.Descricao);
                            //
                            lista.Add(lista_itens);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                sFileName = "Erro: " + exc.Message;
                sreult = "nok";
            }
            //
            return Json(new
            {
                data = "OK",
                results = 1,
                success = true,
                errors = 0,
                dados = lista,
                maquinas = maq,
                periodo = data_filtro_ini.ToShortDateString() + " " + data_filtro_ini.ToShortTimeString() + " - " + data_filtro_fim.ToShortDateString() + " " + data_filtro_fim.ToShortTimeString()
            }, JsonRequestBehavior.AllowGet);
        }

        string RemTransp(string file, string filename)
        {
            Bitmap src = new Bitmap(file);
            Bitmap target = new Bitmap(src.Size.Width, src.Size.Height);
            Graphics g = Graphics.FromImage(target);
            g.DrawRectangle(new Pen(new SolidBrush(Color.White)), 0, 0, target.Width, target.Height);
            g.DrawImage(src, 0, 0);
            string app_path = ((HttpRequestWrapper)this.Request).PhysicalApplicationPath;
            filename = filename + "_temp.png";
            app_path += "Temp\\" + filename;
            target.Save(app_path);
            return filename;
        }

        public JsonResult RelatorioMaquinaInequil(int idempresa, string idmaquina, string periodo)
        {
            string sFileName = string.Empty;
            string serrors = string.Empty;
            //
            try
            {
                string sreult = string.Empty;
                string sProgramaNome = string.Empty;
                Empresa empresa = null;
                string date_ini = periodo.Split('-')[0];
                string date_end = periodo.Split('-')[1];
                int iidmaquina = Convert.ToInt32(idmaquina);
                //
                int day_ini = Convert.ToInt32(date_ini.Split('/')[1]);
                int month_ini = Convert.ToInt32(date_ini.Split('/')[0]);
                int year_ini = Convert.ToInt32(date_ini.Split('/')[2]);
                //
                int day_end = Convert.ToInt32(date_end.Split('/')[1]);
                int month_end = Convert.ToInt32(date_end.Split('/')[0]);
                int year_end = Convert.ToInt32(date_end.Split('/')[2]);
                //
                DateTime dtIni = new DateTime(year_ini, month_ini, day_ini);
                DateTime dtEnd = new DateTime(year_end, month_end, day_end);
                dtEnd = dtEnd.AddHours(23);
                dtEnd = dtEnd.AddMinutes(59);
                dtEnd = dtEnd.AddSeconds(59);
                //
                List<MaquinaLog> ml = db.MaquinaLog.Where(a => a.Id_Maquina == iidmaquina).ToList();
                List<MaquinaLogReport> maRepo_list = db.MaquinaLogReport.Where(a => a.Id_Maquina == iidmaquina && a.DataHora >= dtIni && a.DataHora <= dtEnd).ToList();
                List<MaquinaItensReportModel> lista_itens = new List<MaquinaItensReportModel>();
                //
                foreach (MaquinaLogReport item in maRepo_list)
                {
                    MaquinaItensReportModel mtrm = new MaquinaItensReportModel();
                    //
                    mtrm.Id = item.Id;
                    mtrm.Passo = item.Passo;
                    mtrm.Tempo = item.Tempo;
                    mtrm.DataHora = Convert.ToDateTime(item.DataHora);
                    mtrm.Kilos = string.IsNullOrEmpty(item.Kilos) ? "" : item.Kilos;
                    mtrm.ProdutoA = string.IsNullOrEmpty(item.ProdutoA) ? "" : item.ProdutoA;
                    mtrm.ProdutoB = string.IsNullOrEmpty(item.ProdutoB) ? "" : item.ProdutoB;
                    mtrm.ProdutoC = string.IsNullOrEmpty(item.ProdutoC) ? "" : item.ProdutoC;
                    mtrm.ProdutoD = string.IsNullOrEmpty(item.ProdutoD) ? "" : item.ProdutoD;
                    mtrm.ProdutoE = string.IsNullOrEmpty(item.ProdutoE) ? "" : item.ProdutoE;
                    mtrm.ProdutoF = string.IsNullOrEmpty(item.ProdutoF) ? "" : item.ProdutoF;
                    mtrm.ProdutoG = string.IsNullOrEmpty(item.ProdutoG) ? "" : item.ProdutoG;
                    mtrm.ProgramaExec = item.ProgramaExec;
                    mtrm.RPM = item.RPM;
                    mtrm.Temperatura = item.Temperatura;
                    mtrm.DescricaoColetor = "";
                    mtrm.DescricaoMaquina = "";
                    sProgramaNome = item.ProgramaExec;
                    //
                    lista_itens.Add(mtrm);
                }
                //
                List<EmpresaRelatorioModel> lista_empresa_model = new List<EmpresaRelatorioModel>();
                empresa = db.Empresa.Where(x => x.Id == idempresa).FirstOrDefault();
                int idmaquinaint = Convert.ToInt32(idmaquina);
                Maquina maq = db.Maquina.Where(a => a.ID == idmaquinaint).FirstOrDefault();
                int tot_list = lista_itens.Count;
                DateTime dt_inicial = lista_itens[0].DataHora;
                DateTime dt_final = lista_itens[tot_list -1].DataHora;
                int NumeroLote = lista_itens[0].Id;
                //
                TimeSpan dt_diff = (dt_final - dt_inicial);
                //
                if (empresa != null)
                {
                    EmpresaRelatorioModel tmp = new EmpresaRelatorioModel();
                    //
                    tmp.Id = empresa.Id;
                    tmp.Nome = empresa.Nome;
                    tmp.Numero = empresa.Numero;
                    tmp.Telefone = empresa.Telefone;
                    tmp.Site = empresa.Site;
                    tmp.EstadoSigla = PegaEstadoSigla(empresa.Estado);
                    tmp.Endereco = empresa.Endereco;
                    tmp.CEP = empresa.CEP;
                    tmp.Cidade = empresa.Cidade;
                    tmp.NomeFantasia = empresa.NomeFantasia;
                    tmp.Email = empresa.Email;
                    tmp.Bairro = empresa.Bairro;
                    tmp.Imagem = RetornaImagemLink("");
                    tmp.Periodo = periodo;
                    tmp.NomeMaquina = "";
                    //
                    if (maq != null)
                    {
                        tmp.Programa = sProgramaNome;
                        tmp.Maquina = maq.Descricao;
                        tmp.NumeroLote = NumeroLote.ToString();
                        tmp.DataCiclo = dt_inicial.ToShortDateString();
                        tmp.InicioCiclo = dt_inicial.ToShortTimeString();
                        tmp.FimCiclo = dt_final.ToShortTimeString();
                        tmp.TotalCiclo = dt_diff.ToString().Substring(0, 5);
                        tmp.Erros = "0";
                    }
                    //
                    lista_empresa_model.Add(tmp);
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
                    sFileName = "Relatorio_Maquinas" + DateTime.Today.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString() + "_.pdf";
                    sFileName = sFileName.Replace("/", "-");
                    sFileName = sFileName.Replace("\\", "-");
                    sFileName = sFileName.Replace(":", "-");
                    //
                    oRelatorioMaquina.Load(System.Web.HttpContext.Current.Server.MapPath("~/Relatorio/") + "RelatorioMaquina.rpt");
                    oRelatorioMaquina.SetDataSource(lista_empresa_model);
                    oRelatorioMaquina.Database.Tables[0].SetDataSource(lista_empresa_model);
                    //
                    oRelatorioMaquinaItem.Load(System.Web.HttpContext.Current.Server.MapPath("~/Relatorio/") + "RelatorioMaquinaItem.rpt");
                    oRelatorioMaquinaItem.SetDataSource(lista_itens);
                    //
                    oRelatorioMaquina.Subreports["RelatorioMaquinaItem.rpt"].Database.Tables[0].SetDataSource(lista_itens);
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
                    GC.Collect();
                }
                catch (Exception exc)
                {
                    serrors = "Erro: " + exc.Message;
                }
            }
            catch (Exception exce)
            {
                serrors = "Erro: " + exce.Message;
            }
            //
            return Json(new
            {
                data = serrors.Length > 0 ? serrors : "OK",
                results = 1,
                success = true,
                errors = serrors,
                relatorio = sFileName,
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RelatorioTemperaturaAtmosfera(int idempresa, string strimage1, string maquinas, string periodo)
        {
            //string data = strimage1 + strimage2;
            string img = strimage1.Split(',')[1];
            string sFileNameImgGrafico = "img" + DateTime.Today.ToShortDateString().Replace("/", "_") + "_" + DateTime.Now.ToLongTimeString().Replace(":", "_");
            string ext = ".png";
            string app_path = ((HttpRequestWrapper)this.Request).PhysicalApplicationPath;
            app_path += "Temp\\";
            //byte[] bytes = Convert.FromBase64String("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");

            //var imgbase64 = data.Split(',')[1];
            string img2 = img.Replace(" ", "+");
            byte[] bytes = Convert.FromBase64String(img2);

            //byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(bytes, 0, bytes.Length);
            ms.Write(bytes, 0, bytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            image.Save(app_path + sFileNameImgGrafico + ext);
            sFileNameImgGrafico = RemTransp(app_path + sFileNameImgGrafico + ext, sFileNameImgGrafico);
            //
            List<Object> lista = new List<Object>();           
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
                tmp.Imagem = RetornaImagemLink(sFileNameImgGrafico);
                tmp.Periodo = periodo;
                tmp.NomeMaquina = maquinas;
                lista_empresa_model.Add(tmp);
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
                sFileName = "Relatorio_Maquinas" + DateTime.Today.ToShortDateString() + "_" + DateTime.Now.ToLongTimeString() + "_.pdf";
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
                GC.Collect();
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
                data = "OK",
                results = 1,
                success = true,
                errors = 0,                
                relatorio = sFileName,
            }, JsonRequestBehavior.AllowGet);
        }

        private Byte[] RetornaImagemLink(string filename)
        {
            Byte[] bytes = null;
            //
            string app_path = ((HttpRequestWrapper)this.Request).PhysicalApplicationPath;
            app_path += "Temp\\";
            //
            string scaminho = app_path + "\\" + filename;
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