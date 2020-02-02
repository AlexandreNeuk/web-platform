using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class GraficoController : BaseController
    {
        // GET: Grafico
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Grafico()
        {
            ViewBag.GraficoAtivo = "active";

            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;
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
            return View();
        }

        public ActionResult Pressao()
        {
            ViewBag.GraficoAtivo = "active";
            ViewBag.GraficoPressao = "active";
            ViewBag.GraficoShow = "show";
            
            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;
            return View();
        }

        public JsonResult GraficoPresTempProd(int idmaquina, string periodo)
        {

            List<data_novo> datas = new List<data_novo>();
            DateTime data_filtro_um = new DateTime();
            DateTime data_filtro_dois = new DateTime();
            //
            string sRet = "ok";
            string sErro = string.Empty;
            string sql_temp = string.Empty;
            string sql_pressao = string.Empty;
            string sql_producao = string.Empty;
            int iperiodo = -1;
            //
            string[] lista_labels = null;
            string[] lista_labels_compara = null;
            List<string[]> lista_temperatura = new List<string[]>();
            List<string[]> lista_pressao = new List<string[]>();
            List<string[]> lista_producao = new List<string[]>();
            //
            try
            {
                if (!string.IsNullOrEmpty(periodo))
                {
                    string[] datas_filro = periodo.Split('-');
                    string[] data_um = datas_filro[0].ToString().Split('/');
                    string[] data_dois = datas_filro[1].ToString().Split('/');
                    //
                    data_filtro_um = new DateTime(Convert.ToInt32(data_um[2]), Convert.ToInt32(data_um[0]), Convert.ToInt32(data_um[1]), 0, 0, 1);
                    data_filtro_dois = new DateTime(Convert.ToInt32(data_dois[2]), Convert.ToInt32(data_dois[0]), Convert.ToInt32(data_dois[1]), 23, 59, 59);
                }
                //
                int empresa = 0;
                if (Session["cd_empresa"] != null)
                {
                    empresa = Convert.ToInt32(Session["cd_empresa"]);
                }
                //
                int dias = Convert.ToInt32((data_filtro_dois - data_filtro_um).TotalDays);
                var horas = (data_filtro_dois - data_filtro_um).TotalHours;
                //
                Coletor coletor = db.Coletor.Where(x => x.Maquina != null && x.Maquina.ID == idmaquina && x.Id_Empresa == empresa).FirstOrDefault();
                //
                if (dias <= 123)
                {
                    if (dias <= 31) // até 31 dias um mês
                    {
                        iperiodo = 1;
                    }
                    else if (dias <= 62) // até 62 dias dois meses
                    {
                        iperiodo = 2;
                    }
                    else if (dias <= 123) // até 123 dias 4 meses
                    {
                        iperiodo = 3;
                    }
                    //
                    sql_temp = @"SELECT Cast(datahora AS DATE)                   AS 'Data',
                                        DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
                                        Datepart(month, datahora)                AS Mes, 
                                        Datepart(day, datahora)                  AS Dia, 
                                        ROUND((Avg(Cast(temperatura AS FLOAT)) / 10 ), 2) AS 'TemperaturaMedia' 
                                 FROM   ColetorTemperaturaHistorico 
                                WHERE Id_Coletor = " + coletor.Id + @"
                                    AND DataHora >= '" + data_filtro_um.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                    AND DataHora <= '" + data_filtro_dois.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                    AND Temperatura IS NOT NULL 
                                 GROUP  BY Cast(datahora AS DATE), 
                                           Datepart(day, datahora), 
                                           Datepart(month, datahora) 
                                 ORDER  BY Datepart(month, datahora), 
                                           Datepart(day, datahora)";
                    //
                    sql_pressao = @"SELECT Cast(datahora AS DATE)                   AS 'Data',
                                        DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
                                        Datepart(month, datahora)                AS Mes, 
                                        Datepart(day, datahora)                  AS Dia, 
                                        ROUND((Avg(Cast(pressao AS FLOAT)) / 10 ), 2) AS 'PressaoMedia' 
                                 FROM   coletorpressaohistorico 
                                WHERE Id_Coletor = " + coletor.Id + @"
                                    AND DataHora >= '" + data_filtro_um.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                    AND DataHora <= '" + data_filtro_dois.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                    AND pressao IS NOT NULL 
                                 GROUP  BY Cast(datahora AS DATE), 
                                           Datepart(day, datahora), 
                                           Datepart(month, datahora) 
                                 ORDER  BY Datepart(month, datahora), 
                                           Datepart(day, datahora)";
                    //
                    sql_producao = @"SELECT Cast(datahora AS DATE)                   AS 'Data', 
	                                       DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
                                           Datepart(month, datahora)                AS Mes, 
                                           Datepart(day, datahora)                  AS Dia, 
                                           Sum(Cast(valor AS INT))   AS ValorTotal 
                                    FROM   coletorproducaohistorico 
                                  WHERE Id_Coletor = " + coletor.Id + @"
                                        AND DataHora >= '" + data_filtro_um.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                        AND DataHora <= '" + data_filtro_dois.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                           AND valor IS NOT NULL 
                                    GROUP  BY Cast(datahora AS DATE), 
                                              Datepart(day, datahora), 
                                              Datepart(month, datahora) 
                                    ORDER  BY Datepart(month, datahora), 
                                              Datepart(day, datahora)";
                }
                //else if (dias <= 62)
                //{
                //    iperiodo = 2;
                //    //
                //    sql_temp = @"SELECT Cast(datahora AS DATE)                   AS 'Data',
                //                        DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
                //                        Datepart(month, datahora)                AS Mes, 
                //                        Datepart(day, datahora)                  AS Dia, 
                //                        ROUND((Avg(Cast(temperatura AS FLOAT)) / 10 ), 2) AS 'TemperaturaMedia' 
                //                 FROM   ColetorTemperaturaHistorico 
                //                WHERE Id_Coletor = " + coletor.Id + @"
                //                    AND DataHora >= '" + data_filtro_um.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                //                    AND DataHora <= '" + data_filtro_dois.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                //                    AND Temperatura IS NOT NULL 
                //                 GROUP  BY Cast(datahora AS DATE), 
                //                           Datepart(day, datahora), 
                //                           Datepart(month, datahora) 
                //                 ORDER  BY Datepart(month, datahora), 
                //                           Datepart(day, datahora)";
                //    //
                //    sql_pressao = @"SELECT Cast(datahora AS DATE)                   AS 'Data',
                //                        DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
                //                        Datepart(month, datahora)                AS Mes, 
                //                        Datepart(day, datahora)                  AS Dia, 
                //                        ROUND((Avg(Cast(pressao AS FLOAT)) / 10 ), 2) AS 'PressaoMedia' 
                //                 FROM   coletorpressaohistorico 
                //                WHERE Id_Coletor = " + coletor.Id + @"
                //                    AND DataHora >= '" + data_filtro_um.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                //                    AND DataHora <= '" + data_filtro_dois.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                //                    AND pressao IS NOT NULL 
                //                 GROUP  BY Cast(datahora AS DATE), 
                //                           Datepart(day, datahora), 
                //                           Datepart(month, datahora) 
                //                 ORDER  BY Datepart(month, datahora), 
                //                           Datepart(day, datahora)";
                //    //
                //    sql_producao = @"SELECT Cast(datahora AS DATE)                   AS 'Data', 
	               //                        DATENAME(weekday, Cast(datahora AS DATE)) AS 'DiaSemana',
                //                           Datepart(month, datahora)                AS Mes, 
                //                           Datepart(day, datahora)                  AS Dia, 
                //                           Sum(Cast(valor AS INT))   AS ValorTotal 
                //                    FROM   coletorproducaohistorico 
                //                  WHERE Id_Coletor = " + coletor.Id + @"
                //                        AND DataHora >= '" + data_filtro_um.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                //                        AND DataHora <= '" + data_filtro_dois.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                //                           AND valor IS NOT NULL 
                //                    GROUP  BY Cast(datahora AS DATE), 
                //                              Datepart(day, datahora), 
                //                              Datepart(month, datahora) 
                //                    ORDER  BY Datepart(month, datahora), 
                //                              Datepart(day, datahora)";
                //}
                else if (dias > 360) // ano
                {
                    iperiodo = 5;
                    sql_temp = @"SELECT Month(DataHora) as 'Mes', 
                                                sum(CAST(Temperatura AS INT)) AS 'TemperaturaTotal', 
	                                            AVG(CAST(Temperatura AS INT)) AS 'TemperaturaMedia', 
	                                            count(*) AS 'Total' 
                                        FROM ColetorTemperaturaHistorico
                                            where Id_Coletor = " + coletor.Id + @"
                                            and DataHora >= '" + data_filtro_um.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                            and DataHora <= '" + data_filtro_dois.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                            and Temperatura is not null
                                        GROUP BY Year([DataHora]), MONTH([DataHora])
                                        ORDER BY Year([DataHora]), MONTH([DataHora])";
                    //
                    sql_pressao = @"SELECT Month(DataHora) as 'Mes', 
                                                sum(CAST(Pressao AS INT)) AS 'PressaoTotal', 
	                                            (AVG(CAST(Pressao AS INT)) / 10) AS 'PressaoMedia', 
	                                            count(*) AS 'Total' 
                                        FROM ColetorPressaoHistorico
                                            where Id_Coletor = " + coletor.Id + @"
                                            and DataHora >= '" + data_filtro_um.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                            and DataHora <= '" + data_filtro_dois.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                            and Pressao is not null
                                        GROUP BY Year([DataHora]), MONTH([DataHora])
                                        ORDER BY Year([DataHora]), MONTH([DataHora])";
                    //
                    sql_producao = @"SELECT Month(DataHora) as 'Mes', 
                                                sum(CAST(Valor AS INT)) AS 'ValorTotal', 
	                                            (AVG(CAST(Valor AS INT)) / 10) AS 'ValorMedia', 
	                                            count(*) AS 'Total' 
                                        FROM ColetorProducaoHistorico
                                            where Id_Coletor = " + coletor.Id + @"
                                            and DataHora >= '" + data_filtro_um.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                            and DataHora <= '" + data_filtro_dois.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                            and Valor is not null
                                        GROUP BY Year([DataHora]), MONTH([DataHora])
                                        ORDER BY Year([DataHora]), MONTH([DataHora])";

                }
                //
                //lista_labels_compara
                PegaRotuloGrafico(iperiodo, data_filtro_dois, ref lista_labels_compara, ref lista_labels, dias);
                string[] lista_dados_temp = new string[lista_labels.Length];
                string[] lista_dados_pressao = new string[lista_labels.Length];
                string[] lista_dados_producao = new string[lista_labels.Length];
                //
                SQLController sqlcontroller = new SQLController();
                //
                DataTable data_temp = sqlcontroller.ExecutaSQL(sql_temp);
                if (data_temp != null && data_temp.Rows != null && data_temp.Rows.Count > 0)
                {
                    lista_dados_temp = MontaDados(lista_dados_temp, lista_labels_compara, lista_labels, iperiodo, data_temp, "TemperaturaMedia");
                }
                //
                DataTable data_pressao = sqlcontroller.ExecutaSQL(sql_pressao);
                if (data_pressao != null && data_pressao.Rows != null && data_pressao.Rows.Count > 0)
                {
                    lista_dados_pressao = MontaDados(lista_dados_pressao, lista_labels_compara, lista_labels, iperiodo, data_pressao, "PressaoMedia");
                }
                //
                DataTable data_producao = sqlcontroller.ExecutaSQL(sql_producao);
                if (data_producao != null && data_producao.Rows != null && data_producao.Rows.Count > 0)
                {
                    lista_dados_producao = MontaDados(lista_dados_producao, lista_labels_compara, lista_labels, iperiodo, data_producao, "ValorTotal");
                }
                //
                if (iperiodo == 2 || iperiodo == 3)
                {
                    string[] lista_labels_tmp = lista_labels;
                    string[] lista_dados_temp_tmp = lista_dados_temp;
                    string[] lista_dados_pressao_tmp = lista_dados_pressao;
                    string[] lista_dados_producao_tmp = lista_dados_producao;
                    //
                    lista_labels = new string[lista_labels.Length / iperiodo];
                    lista_dados_temp = new string[lista_dados_temp.Length / iperiodo];
                    lista_dados_pressao = new string[lista_dados_pressao.Length / iperiodo];
                    lista_dados_producao = new string[lista_dados_producao.Length / iperiodo];
                    int j = 0;
                    //
                    if (iperiodo == 2)
                    {
                        for (int i = 0; i < lista_labels_tmp.Length; i++)
                        {
                            if (i % 2 == 0)
                            {
                                if (lista_dados_temp.Length > j)
                                {
                                    lista_dados_temp[j] = lista_dados_temp_tmp[i];
                                    lista_dados_pressao[j] = lista_dados_pressao_tmp[i];
                                    lista_dados_producao[j] = lista_dados_producao_tmp[i];
                                    lista_labels[j] = lista_labels_tmp[i];
                                }
                                j++;
                            }
                        }
                    }
                    else if (iperiodo == 3)
                    {
                        int indice = 0;
                        int cont = 0;
                        for (int i = 0; i < lista_labels_tmp.Length; i++)
                        {
                            if (i == 0 || j == 3)
                            {
                                if (lista_dados_temp.Length > indice)
                                {
                                    lista_dados_temp[indice] = lista_dados_temp_tmp[i];
                                    lista_dados_pressao[indice] = lista_dados_pressao_tmp[i];
                                    lista_dados_producao[indice] = lista_dados_producao_tmp[i];
                                    lista_labels[indice] = lista_labels_tmp[i];
                                    indice++;
                                }
                                j = 0;
                            }
                            j++;
                        }
                    }
                }
                //
                lista_temperatura.Add(lista_dados_temp);
                lista_pressao.Add(lista_dados_pressao);
                lista_producao.Add(lista_dados_producao);
            }
            catch (Exception exc)
            {
                sRet = "nok";
                sErro = exc.Message;
            }
            //
            return Json(new { data = sRet, lista_temperatura, lista_pressao, lista_producao, lista_labels, results = 1, success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Temperatura()
        {
            ViewBag.GraficoAtivo = "active";
            ViewBag.GraficoTemperatura = "active";
            ViewBag.GraficoShow = "show";

            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;
            return View();
        }

        public ActionResult Producao()
        {
            ViewBag.GraficoAtivo = "active";
            ViewBag.GraficoProducao = "active";
            ViewBag.GraficoShow = "show";

            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;
            return View();
        }

        public ActionResult Atmosfera()
        {
            ViewBag.GraficoAtivo = "active";
            ViewBag.GraficoAtmosfera = "active";
            ViewBag.GraficoShow = "show";

            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;
            return View();
        }


        public void PegaRotuloGrafico(int periodo, DateTime dt, ref string[] listaRotulosGrafico_comp, ref string[] listaRotulosGrafico, int dias)
        {
            switch (periodo)
            {
                case 0: // 0 diário
                    listaRotulosGrafico = PegaUltimasVinteQuatroHoras();
                    break;
                case 1: // 1 semanal
                case 2: // Últimos 62 dias: periodo dias > 32 e <= 62
                case 3: // Últimos 123 dias: periodo dias > 62 e <= 123
                    PegaUltimosDias(dt, ref listaRotulosGrafico_comp, ref listaRotulosGrafico, dias);
                    break;
                case 4: // 6 mes
                    listaRotulosGrafico = PegaUltimosMeses(6, dt);
                    break;
                case 5: // 12 mes
                    listaRotulosGrafico = PegaUltimosMeses(12, dt);
                    break;
                case 6: // Tempo real
                    //listaRotulosGrafico = PegaUltimosMeses(12);
                    break;
            }
        }

        public void PegaUltimosDias(DateTime dt, ref string[] listaRotulosGrafico_comp, ref string[] listaRotulosGrafico, int dias)
        {
            listaRotulosGrafico_comp = new string[dias];
            listaRotulosGrafico = new string[dias];
            //
            for (int i = 0; i < dias; i++)
            {
                DateTime dt_tmp = dt.AddDays(-i);
                string day_lbl = dt_tmp.Day > 9 ? dt_tmp.Day.ToString() : "0" + dt_tmp.Day;
                string month_lbl = dt_tmp.Month > 9 ? dt_tmp.Month.ToString() : "0" + dt_tmp.Month;
                string day = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dt_tmp.DayOfWeek);
                listaRotulosGrafico_comp[i] = char.ToUpper(day[0]) + day.Substring(1);
                listaRotulosGrafico[i] = (char.ToUpper(day[0]) + day.Substring(1)).ToString().Split('-')[0] + " - " + day_lbl + "/" + month_lbl;
            }
        }

        public void PegaUltimos60Dias(DateTime dt, ref string[] listaRotulosGrafico_comp, ref string[] listaRotulosGrafico, int dias)
        {
            listaRotulosGrafico_comp = new string[dias];
            listaRotulosGrafico = new string[dias];
            //
            for (int i = 0; i < dias; i++)
            {
                DateTime dt_tmp = dt.AddDays(-i);
                string day_lbl = dt_tmp.Day > 9 ? dt_tmp.Day.ToString() : "0" + dt_tmp.Day;
                string month_lbl = dt_tmp.Month > 9 ? dt_tmp.Month.ToString() : "0" + dt_tmp.Month;
                string day = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(dt_tmp.DayOfWeek);
                listaRotulosGrafico_comp[i] = char.ToUpper(day[0]) + day.Substring(1);
                listaRotulosGrafico[i] = (char.ToUpper(day[0]) + day.Substring(1)).ToString().Split('-')[0] + " - " + day_lbl + "/" + month_lbl;
            }
        }

        public string[] PegaUltimasVinteQuatroHoras()
        {
            string[] listaUltimasVinteQuatroHoras = new string[23];
            int hora = DateTime.Now.Hour;
            //
            for (int i = 0; i < 23; i++)
            {
                string hora_str = string.Empty;
                if (hora <= 9)
                {
                    hora_str = "0" + hora.ToString() + ":00";
                }
                else
                {
                    hora_str = hora.ToString() + ":00";
                }
                //
                listaUltimasVinteQuatroHoras[i] = hora_str;
                hora--;
                if (hora == 0)
                {
                    hora = 23;
                }
            }
            //
            return listaUltimasVinteQuatroHoras;
        }

        public string[] PegaUltimosMeses(int meses, DateTime dt)
        {
            string[] listaUltimosMeses = new string[12];
            //
            switch (meses)
            {
                case 1 :
                    int dias_mes = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        if (i > 0)
                        {
                            if (i % 2 == 0)
                            {
                                dias_mes = 2;
                            }
                            else
                            {
                                dias_mes = 3;
                            }
                            //
                            dt = dt.AddDays(-dias_mes);
                        }                        
                        listaUltimosMeses[i] = (dt.Day > 9 ? dt.Day.ToString() : "0" + dt.Day.ToString()) + "/" +
                                               (dt.Month > 9 ? dt.Month.ToString() : "0" + dt.Month.ToString());
                    }
                    break;
                case 3:
                    int dias_trimestre = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        if (i > 0)
                        {
                            if (i % 2 == 0)
                            {
                                dias_trimestre = 4;
                            }
                            else
                            {
                                dias_trimestre = 3;
                            }
                            //
                            dt = dt.AddDays(-dias_trimestre);
                        }
                        listaUltimosMeses[i] = (dt.Day > 9 ? dt.Day.ToString() : "0" + dt.Day.ToString()) + "/" +
                                               (dt.Month > 9 ? dt.Month.ToString() : "0" + dt.Month.ToString());
                    }
                    break;
                case 6:
                    int dias_semestre = 0;
                    for (int i = 0; i < 12; i++)
                    {
                        if (i > 0)
                        {
                            dias_semestre = 15;
                            dt = dt.AddDays(-dias_semestre);
                        }
                        listaUltimosMeses[i] = (dt.Day > 9 ? dt.Day.ToString() : "0" + dt.Day.ToString()) + "/" +
                                               (dt.Month > 9 ? dt.Month.ToString() : "0" + dt.Month.ToString());
                    }
                    break;
                case 12:
                    //
                    for (int i = 0; i < 12; i++)
                    {                        
                        string mes = dt.AddMonths(-i).ToString("MMMM", new CultureInfo("pt-BR"));
                        listaUltimosMeses[i] = char.ToUpper(mes[0]) + mes.Substring(1);
                    }
                    break;
            }
            //
            return listaUltimosMeses;
        }

        //
        public JsonResult PressaoMaquina2(string lista_ids, int periodo)
        {
            string sRet = string.Empty;
            string sql = string.Empty;
            List<MedidorHistoricoModel> list = new List<MedidorHistoricoModel>();
            DateTime dt = getData();
            DateTime dt_fim = getData();
            string[] listaids = lista_ids.Split(',');
            string[] lista_labels = null;
            string[] lista_labels_compara = null;
            PegaRotuloGrafico(periodo, dt_fim, ref lista_labels_compara, ref lista_labels, 0);
            List<string[]> lista_dados_retorno = new List<string[]>();            
            //
            for (int i = 0; i < listaids.Length; i++)
            {
                string[] lista_dados = new string[lista_labels.Length];
                int idmaquina = Convert.ToInt32(listaids[i]);
                Coletor coletor = db.Coletor.Where(x => x.Maquina != null && x.Maquina.ID == idmaquina).FirstOrDefault();
                //
                try
                {
                    if (coletor != null)
                    {
                        if (periodo >= 2)
                        {
                            switch (periodo)
                            {
                                case 2: // 1 mes
                                    dt = dt.AddDays(-30);
                                    sql = @"SELECT Day(DataHora) as 'Dia', 
                                                sum(CAST(pressao AS INT)) AS 'Pressão Total', 
	                                            AVG(CAST(pressao AS INT)) AS 'Pressão Média', 
	                                            count(*) AS 'Total' 
                                        FROM ColetorPressaoHistorico
                                            where id_coletor = " + coletor.Id + @"
                                            and DataHora > '" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                            and pressao is not null
                                        GROUP BY Day([DataHora])";
                                    break;
                                case 3: // 3 mes
                                    dt = dt.AddDays(-90);
                                    break;
                                case 4: // 6 mes
                                case 5: // 12 mes
                                    if (periodo == 4)
                                        dt = dt.AddDays(-180);
                                    else
                                        dt = dt.AddDays(-365);
                                    //
                                    sql = @"SELECT Month(DataHora) as 'Mes', 
                                                sum(CAST(pressao AS INT)) AS 'PressaoTotal', 
	                                            AVG(CAST(pressao AS INT)) AS 'PressaoMedia', 
	                                            count(*) AS 'Total' 
                                        FROM ColetorPressaoHistorico
                                            where id_coletor = " + coletor.Id + @"
                                            and DataHora > '" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                            and Pressao is not null
                                        GROUP BY Month([DataHora])";
                                    break;
                                case 6: // Tempo real
                                    sql = @"SELECT TOP (24) [Id]
                                              ,[Id_Coletor]
                                              ,[DataHora]
                                              ,[Pressao]
                                          FROM [DB_A4925A_connector].[dbo].[ColetorPressaoHistorico]
                                          where id_coletor = " + coletor.Id + @"
                                          order by id desc";
                                    break;
                            }
                        }
                        else
                        {
                            if (periodo == 0)
                            {
                                sql = @"SELECT CAST(datahora as date) AS ForDate,
                                               DATEPART(minute,datahora) AS OnHour,
                                               COUNT(*) AS Totals,
	                                           AVG(CAST(pressao AS FLOAT)) AS 'media'
                                        FROM coletorpressaohistorico
                                        WHERE  id_coletor = 13 
                                               AND datahora >= '2019/07/15 11:00:00.000' 
	                                           AND datahora <= '2019/07/15 12:00:00.000' 
                                               AND pressao IS NOT NULL 
                                        GROUP BY CAST(datahora as date),
                                               DATEPART(minute, datahora)";
                                //
                                SQLController sqlcontroller = new SQLController();
                                DataTable datatable = sqlcontroller.ExecutaSQL(sql);
                                lista_dados = new string[datatable.Rows.Count];
                                //
                                if (datatable != null && datatable.Rows != null && datatable.Rows.Count > 0)
                                {
                                    for (int h = 0; h < datatable.Rows.Count; h++)
                                    {
                                        lista_dados[h] = Math.Round(Convert.ToDouble(datatable.Rows[h]["media"])).ToString();
                                    }
                                    //
                                    lista_dados_retorno.Add(lista_dados);
                                }
                                //
                                lista_labels = new string[datatable.Rows.Count];
                                for (int l = 0; l < lista_dados.Length; l++)
                                {
                                    lista_labels[l] = (l + 1).ToString(); ;
                                }
                            }
                            //if (periodo == 0) // Diário
                            //{
                            //    dt = dt.AddDays(-1);
                            //    dt_fim = dt_fim.AddDays(-1);
                            //    dt_fim = dt_fim.AddHours(1);


                            //    for (int j = 0; j < 23; j++)
                            //    {
                            //        sql = @"SELECT DAY(DataHora) as 'Dia', 
                            //           sum(CAST(pressao AS FLOAT)) AS 'Pressão Total', 
                            //           AVG(CAST(pressao AS FLOAT)) AS 'media', 
                            //           count(*) AS 'Total' 
                            //         FROM ColetorPressaoHistorico
                            //         where id_coletor =  " + coletor.Id + @"
                            //          and DataHora >= '" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                            //          and DataHora <= '" + dt_fim.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                            //          and pressao is not null
                            //         GROUP BY DAY([DataHora])";
                            //        //
                            //        SQLController sqlcontrollerss222 = new SQLController();
                            //        DataTable datatable = sqlcontrollerss222.ExecutaSQL(sql);
                            //        //
                            //        if (datatable != null && datatable.Rows != null && datatable.Rows.Count > 0)
                            //        {
                            //            lista_dados[j] = Math.Round(Convert.ToDouble(datatable.Rows[0]["media"])).ToString();
                            //        }
                            //        else
                            //        {
                            //            lista_dados[j] = "0";
                            //        }
                            //        //
                            //        dt = dt.AddHours(1);
                            //        dt_fim = dt_fim.AddHours(1);
                            //    }

                            //    lista_dados_retorno.Add(lista_dados);
                            //}
                            else if (periodo == 1) // última semanal
                            {
                                dt = dt.AddDays(-7);
                                sql = @"SELECT DAY(DataHora) as 'Mes', 
                                       sum(CAST(pressao AS INT)) AS 'Pressão Total', 
	                                   AVG(CAST(pressao AS INT)) AS 'Pressão Média', 
	                                   count(*) AS 'Total' 
                               FROM ColetorPressaoHistorico
                                  where id_coletor =  " + coletor.Id + @"
                                   and DataHora > '" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                   and pressao is not null
                               GROUP BY DAY([DataHora])";
                            }
                        }
                        //
                        if (periodo != 1 && periodo != 0)
                        {
                            SQLController sqlcontroller = new SQLController();
                            DataTable data = sqlcontroller.ExecutaSQL(sql);
                            //
                            if (data != null && data.Rows != null && data.Rows.Count > 0)
                            {
                                lista_dados = MontaDados(lista_dados, lista_labels, lista_labels_compara, periodo, data, "");
                            }
                            lista_dados_retorno.Add(lista_dados);
                        }
                        //
                        sRet = "ok";
                    }
                    else
                    {
                        lista_dados = new string[lista_labels.Length];
                        lista_dados_retorno.Add(lista_dados);
                        sRet = "Máquina (ID:" + idmaquina + ") não encontrada!";
                    }
                }
                catch (Exception exc)
                {
                    sRet = exc.Message;

                }
            }
            //
            return Json(new { data = sRet, lista_dados_retorno, lista_labels, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult PressaoMaquina(string lista_ids, int periodo)
        {
            string sRet = string.Empty;
            string sql = string.Empty;
            List<MedidorHistoricoModel> list = new List<MedidorHistoricoModel>();
            DateTime dt = getData();
            DateTime dt_fim = getData();
            string[] listaids = lista_ids.Split(',');
            string[] lista_labels = null;
            string[] lista_labels_compara = null;
            PegaRotuloGrafico(periodo, dt_fim, ref lista_labels_compara, ref lista_labels, 0);

            List<string[]> lista_dados_retorno = new List<string[]>();
            //
            for (int i = 0; i < listaids.Length; i++)
            {
                string[] lista_dados = new string[lista_labels.Length];
                int idmaquina = Convert.ToInt32(listaids[i]);
                Coletor coletor = db.Coletor.Where(x => x.Maquina != null && x.Maquina.ID == idmaquina).FirstOrDefault();
                //
                try
                {
                    if (coletor != null)
                    {
                        if (periodo >= 2)
                        {
                            switch (periodo)
                            {
                                case 2: // 1 mes
                                    dt = dt.AddDays(-30);
                                    sql = @"SELECT Day(DataHora) as 'Dia', 
                                                sum(CAST(pressao AS INT)) AS 'Pressão Total', 
	                                            AVG(CAST(pressao AS INT)) AS 'Pressão Média', 
	                                            count(*) AS 'Total' 
                                        FROM ColetorPressaoHistorico
                                            where id_coletor = " + coletor.Id + @"
                                            and DataHora > '" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                            and pressao is not null
                                        GROUP BY Day([DataHora])";
                                    break;
                                case 3: // 3 mes
                                    dt = dt.AddDays(-90);
                                    break;
                                case 4: // 6 mes
                                case 5: // 12 mes
                                    if (periodo == 4)
                                        dt = dt.AddDays(-180);
                                    else
                                        dt = dt.AddDays(-365);
                                    //
                                    sql = @"SELECT Month(DataHora) as 'Mes', 
                                                sum(CAST(pressao AS INT)) AS 'PressaoTotal', 
	                                            AVG(CAST(pressao AS INT)) AS 'PressaoMedia', 
	                                            count(*) AS 'Total' 
                                        FROM ColetorPressaoHistorico
                                            where id_coletor = " + coletor.Id + @"
                                            and DataHora > '" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                            and Pressao is not null
                                        GROUP BY Month([DataHora])";
                                    break;
                                case 6: // Tempo real
                                    sql = @"SELECT TOP (24) [Id]
                                              ,[Id_Coletor]
                                              ,[DataHora]
                                              ,[Pressao]
                                          FROM [DB_A4925A_connector].[dbo].[ColetorPressaoHistorico]
                                          where id_coletor = " + coletor.Id + @"
                                          order by id desc";
                                    break;
                            }
                        }
                        else
                        {
                            if (periodo == 0)
                            {
                                sql = @"SELECT CAST(datahora as date) AS ForDate,
                                               DATEPART(minute,datahora) AS OnHour,
                                               COUNT(*) AS Totals,
	                                           AVG(CAST(pressao AS FLOAT)) AS 'media'
                                        FROM coletorpressaohistorico
                                        WHERE  id_coletor = 13 
                                               AND datahora >= '2019/07/15 11:00:00.000' 
	                                           AND datahora <= '2019/07/15 12:00:00.000' 
                                               AND pressao IS NOT NULL 
                                        GROUP BY CAST(datahora as date),
                                               DATEPART(minute, datahora)";
                                //
                                SQLController sqlcontroller = new SQLController();
                                DataTable datatable = sqlcontroller.ExecutaSQL(sql);
                                lista_dados = new string[datatable.Rows.Count];
                                //
                                if (datatable != null && datatable.Rows != null && datatable.Rows.Count > 0)
                                {
                                    for (int h = 0; h < datatable.Rows.Count; h++)
                                    {
                                        lista_dados[h] = Math.Round(Convert.ToDouble(datatable.Rows[h]["media"])).ToString();
                                    }
                                    //
                                    lista_dados_retorno.Add(lista_dados);
                                }
                                //
                                lista_labels = new string[datatable.Rows.Count];
                                for (int l = 0; l < lista_dados.Length; l++)
                                {
                                    lista_labels[l] = (l + 1).ToString(); ;
                                }
                            }
                            else if (periodo == 1) // última semanal
                            {
                                dt = dt.AddDays(-7);
                                sql = @"SELECT DAY(DataHora) as 'Mes', 
                                       sum(CAST(pressao AS INT)) AS 'Pressão Total', 
	                                   AVG(CAST(pressao AS INT)) AS 'Pressão Média', 
	                                   count(*) AS 'Total' 
                               FROM ColetorPressaoHistorico
                                  where id_coletor =  " + coletor.Id + @"
                                   and DataHora > '" + dt.ToString("yyyy/MM/dd HH:mm:ss.fff", CultureInfo.InvariantCulture) + @"'
                                   and pressao is not null
                               GROUP BY DAY([DataHora])";
                            }
                        }
                        //
                        if (periodo != 1 && periodo != 0)
                        {
                            SQLController sqlcontroller = new SQLController();
                            DataTable data = sqlcontroller.ExecutaSQL(sql);
                            //
                            if (data != null && data.Rows != null && data.Rows.Count > 0)
                            {
                                lista_dados = MontaDados(lista_dados, lista_labels, lista_labels_compara, periodo, data, "");
                            }
                            lista_dados_retorno.Add(lista_dados);
                        }
                        //
                        sRet = "ok";
                    }
                    else
                    {
                        lista_dados = new string[lista_labels.Length];
                        lista_dados_retorno.Add(lista_dados);
                        sRet = "Máquina (ID:" + idmaquina + ") não encontrada!";
                    }
                }
                catch (Exception exc)
                {
                    sRet = exc.Message;

                }
            }
            //
            return Json(new { data = sRet, lista_dados_retorno, lista_labels, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public string[] MontaDados(string[] lista_dados, string[] lista_labels, string[] lista_dias, int periodo, DataTable table, string campo)
        {
            switch (periodo)
            {
                case 0: break;
                case 1:
                    foreach (DataRow item in table.Rows)
                    {
                        string dia_dois = item["Data"].ToString().Substring(0, 5);
                        //
                        for (int i = 0; i < lista_dias.Length; i++)
                        {
                            string dia_um = lista_dias[i].Split('-')[1].ToString().Trim();
                            if (dia_dois.Equals(dia_um))
                            {
                                lista_dados[i] = item[campo].ToString();
                                break;
                            }
                        }
                    }
                    break;
                case 2:
                case 3:
                    foreach (DataRow item in table.Rows)
                    {
                        string dia_dois = item["Data"].ToString().Substring(0, 5);
                        //
                        for (int i = 0; i < lista_dias.Length; i++)
                        {
                            string dia_um = lista_dias[i].Split('-')[1].ToString().Trim();
                            if (dia_dois.Equals(dia_um))
                            {
                                lista_dados[i] = item[campo].ToString();
                                break;
                            }
                        }
                    }
                    break;
                case 4: break;
                case 5:
                    for (int i = 0; i < lista_labels.Length; i++)
                    {
                        foreach (DataRow item in table.Rows)
                        {
                            int mes_numero = Convert.ToInt32(item["Mes"]);
                            if (ValidaMesDescricaoMesNumero(lista_labels[i], mes_numero))
                            {
                                lista_dados[i] = item[campo].ToString();
                                break;
                            }
                        }
                    }
                    break;
            }
            return lista_dados;
        }

        public bool ValidaMesDescricaoDia(string dia, string data)
        {
            bool bResult = false;
            //
            if (dia.ToLower().Equals("domingo") && data.ToLower().Equals("sunday"))
                return true;
            else if (dia.ToLower().Equals("segunda-feira") && data.ToLower().Equals("monday"))
                return true;
            else if (dia.ToLower().Equals("terça-feira") && data.ToLower().Equals("tuesday"))
                return true;
            else if (dia.ToLower().Equals("quarta-feira") && data.ToLower().Equals("wednesday"))
                return true;
            else if (dia.ToLower().Equals("quinta-feira") && data.ToLower().Equals("thursday"))
                return true;
            else if (dia.ToLower().Equals("sexta-feira") && data.ToLower().Equals("friday"))
                return true;
            else if (dia.ToLower().Equals("sábado") && data.ToLower().Equals("saturday"))
                return true;
            //
            return bResult;
        }

        public bool ValidaMesDescricaoMesNumero(string mes_descricao, int mes_numero)
        {
            bool bResult = false;
            //
            if (mes_descricao.ToLower().Equals("janeiro") && mes_numero == 1)
                return true;
            else if (mes_descricao.ToLower().Equals("fevereiro") && mes_numero == 2)
                return true;
            else if (mes_descricao.ToLower().Equals("março") && mes_numero == 3)
                return true;
            else if (mes_descricao.ToLower().Equals("abril") && mes_numero == 4)
                return true;
            else if (mes_descricao.ToLower().Equals("maio") && mes_numero == 5)
                return true;
            else if (mes_descricao.ToLower().Equals("junho") && mes_numero == 6)
                return true;
            else if (mes_descricao.ToLower().Equals("julho") && mes_numero == 7)
                return true;
            else if (mes_descricao.ToLower().Equals("agosto") && mes_numero == 8)
                return true;
            else if (mes_descricao.ToLower().Equals("setembro") && mes_numero == 9)
                return true;
            else if (mes_descricao.ToLower().Equals("outubro") && mes_numero == 10)
                return true;
            else if (mes_descricao.ToLower().Equals("novembro") && mes_numero == 11)
                return true;
            else if (mes_descricao.ToLower().Equals("dezembro") && mes_numero == 12)
                return true;
            //
            return bResult;
        }
    }
}


//DateTime dt = DateTime.Now;

////Mês INT
//int mes = 12;
//string mesExtenso;
//string diaExtenso;

////Mês em português por extenso
//mesExtenso = dt.ToString("MMMM", new CultureInfo("pt-BR"));
////Mês abreviado em português também.
//mesExtenso = new CultureInfo("pt-BR").DateTimeFormat.GetAbbreviatedMonthName(mes);
////Mês (int) por extenso com primeira letra maiúscula.
//string month = new CultureInfo("pt-BR").DateTimeFormat.GetMonthName(mes);
//mesExtenso = char.ToUpper(month[0]) + month.Substring(1);

////Dia da semana (int) por extenso em português (segunda-feira)
//diaExtenso = new CultureInfo("pt-BR").DateTimeFormat.GetDayName((DayOfWeek)1);
////Dia da semana abreviado (seg)
//diaExtenso = new CultureInfo("pt-BR").DateTimeFormat.GetAbbreviatedDayName(DayOfWeek.Monday);
////Dia atual por extenso
//diaExtenso = DateTime.Now.ToString("dddd", new CultureInfo("pt-BR"));
////Dia por extenso com primeira letra maiúscula.
//string day = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(DateTime.Now.DayOfWeek);
//diaExtenso = char.ToUpper(day[0]) + day.Substring(1);

//string[] listaMesesAno = new string[12] { "Janeiro", "Fevereiro", "Marco", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
//string[] listaSemana = new string[12] { "Domingo", "Segunda", "Terça", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
////var  = [];
////
//List<String> listLabels = new List<string>();


//switch (periodo)
//{
//    case 0: // 0 diário
//        dt = dt.AddDays(-1);
//        break;
//    case 1: // 1 semanal
//        dt = dt.AddDays(-7);
//        break;
//    case 2: // 1 mes
//        dt = dt.AddDays(-30);
//        break;
//    case 3: // 3 mes
//        dt = dt.AddDays(-90);
//        break;
//    case 4: // 6 mes
//        dt = dt.AddDays(-180);
//        break;
//    case 5: // 12 mes
//        dt = dt.AddDays(-365);
//        break;
//}


//List<ColetorPressaoHistorico> lista_press_hist = db.ColetorPressaoHistorico.Where(a => a.Id_Coletor == coletor.Id &&
//                                                                          a.DataHora.Value >= data_filtro_um &&
//                                                                          a.DataHora.Value <= data_filtro_dois)
//                                                                          .ToList();
////
//List<ColetorTemperaturaHistorico> lista_temp_hist = db.ColetorTemperaturaHistorico.Where(a => a.Id_Coletor == coletor.Id &&
//                                                                          a.DataHora.Value >= data_filtro_um &&
//                                                                          a.DataHora.Value <= data_filtro_dois)
//                                                                          .ToList();
////
//List<ColetorPressaoHistorico> lista_med = db.ColetorPressaoHistorico.Where(a => a.Id_Coletor == coletor.Id &&
//                                                                          a.DataHora.Value >= data_filtro_um &&
//                                                                          a.DataHora.Value <= data_filtro_dois)
//                                                                          .ToList();

////
//if (lista_med.Count > 0)
//{
//    data_novo data_novo = new data_novo();
//    foreach (Medidor_MedidorHistorico item in lista_med)
//    {
//        data_novo = new data_novo();
//        data_novo.data = item.DataHora.Value.ToShortDateString();
//        if (!string.IsNullOrEmpty(item.Potencia))
//        {
//            data_novo.valor = Convert.ToInt32(item.Potencia);
//        }
//        //
//        datas.Add(data_novo);
//    }

//}