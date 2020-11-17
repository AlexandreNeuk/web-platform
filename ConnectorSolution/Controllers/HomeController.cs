using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System.Threading.Tasks;
using SendGrid.Helpers.Mail;
using SendGrid;
using System.Threading.Tasks;

namespace Connector.Controllers
{
    public class HomeController : BaseController
    {
        public JsonResult teste()
        {
            string sret = string.Empty;
            sret = "ok";
            try
            {
                //teste_envia().Wait();
            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
            //
            return Json(new { data = sret, results = 1, success = true }, JsonRequestBehavior.AllowGet);
        }

        static async Task teste_envia()
        {
            string sret = string.Empty;
            sret = "ok";
            try
            {
                //var apiKey = Environment.GetEnvironmentVariable("XarsTgaBQQ6iAFGVA2w3KQ");
                var client = new SendGridClient("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");
                var from = new EmailAddress("aneukirchen@hotmail.com", "Example User");
                var subject = "Sending with SendGrid is Fun";
                var to = new EmailAddress("aneuk3@gmail.com", "Example User");
                var plainTextContent = "and easy to do anywhere, even with C#";
                var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);

            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
        }

        public ActionResult Index()
        {
            //teste_envia().Wait();
            Session["logado"] = "0";
            return View();
        }

        #region Firebase
        //
        public async Task<JsonResult> Cha() // pega programação máquina
        {
            string pama = string.Empty;
            string pamb = string.Empty;

            IFirebaseClient client;
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "MCPZ3qTPh8YdCQxk8aFvM59tWGoLuZUQb3fakIqJ",
                BasePath = "https://notificacaohahaha.firebaseio.com/"
            };
            //
            var data = new Data
            {
                Id = "4",
                Medidor = "efefefef",
                Potencia = "44"
            };
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = await client.SetTaskAsync("Information/" + "1", data);
            Data result = response.ResultAs<Data>();

            if (!string.IsNullOrEmpty(result.Id))
            {

            }
            //object[] arrayObj = new object[] { pama, pamb };
            //FireConector fire_class = new FireConector();
            ////
            //new FireConector().GetType().GetMethod("InsertData").Invoke(fire_class, arrayObj);

            //if (arrayObj != null)
            //{

            //}

            return Json(new { data = "", results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        #endregion        

        #region Usuarios
        //
        public ActionResult Usuarios()
        {
            List<Usuario> lista_usuarios = new List<Usuario>();
            //
            if (Codigo_Empresa > 0)
            {
                lista_usuarios = db.Usuario.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            }
            //
            ViewBag.ListaUsuarios = lista_usuarios;
            //
            return View();
        }
        //
        public JsonResult UsuarioPost(string nome, string email, int idusuario)
        {
            string sret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                Usuario mm = new Usuario();
                //
                if (idusuario > 0)
                {
                    mm = db.Usuario.Where(a => a.Id_Empresa == Codigo_Empresa &&
                                                               a.ID == idusuario).FirstOrDefault();
                    //
                    if (mm != null)
                    {
                        mm.Nome = nome;
                        mm.Email = email;
                        db.Entry(mm).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                else
                {
                    mm.Nome = nome;
                    mm.Id_Empresa = Codigo_Empresa;
                    mm.Email = email;
                    mm.Pass = "v6YAd6R/UQtwlIRbCJKsVQ==";
                    mm.Ative = 1;
                    mm.Create = DateTime.Now;
                    //
                    db.Usuario.Add(mm);
                    db.SaveChanges();
                    db.Entry(mm).Reload();
                }
                //
                sret = "ok";
            }
            catch (Exception exc)
            {
                erro = exc.Message;
                sret = "nok";
            }
            //
            return Json(new { data = sret, results = 0, success = true, erro }, JsonRequestBehavior.AllowGet);
        }
        //
        #endregion

        public ActionResult DashGramado()
        {
            ViewBag.DashboardAtivo = "active";
            return View();
        }
        #region Dasboard
        //
        public ActionResult Dashboard()
        {            
            if (Session["cd_empresa"] != null && Session["cd_empresa"].ToString().Equals("10"))
            {
                return RedirectToAction("DashGramado", "Home");
            }
            ViewBag.DashboardAtivo = "active";
            ViewBag.CodigoEmpresa = Codigo_Empresa.ToString();
            if (Codigo_Empresa > 0)
            {
                if (Session["nome_usuario"] != null)
                {
                    ViewBag.NomeUsuario = Session["nome_usuario"].ToString();
                }
                Empresa me = db.Empresa.Where(a => a.Id == Codigo_Empresa).FirstOrDefault();
                //
                if (me != null)
                {
                    ViewBag.Empresa = me.Nome;
                    //
                    string s = @"
                                SELECT TOP 50 
		                                me.Descricao, 
		                                ma.Descricao as 'Máquina', 
		                                mm.Descricao as 'Medidor', 
		                                mm.MAC, 
		                                mmh.Id_Mac, 
		                                mmh.DataHora, 
		                                mmh.Potencia 
                                FROM MedidorHistorico mmh
                                inner join Medidor mm on  mm.Id = mmh.Id_Mac
                                inner join Maquina ma on  ma.ID = mm.Id_Maquina
                                inner join Empresa me on  me.Id = ma.Id_Empresa
                                where me.Id = " + Codigo_Empresa + @"
                                order by mmh.Id desc
                                ";
                    SQLController sqlcontroller = new SQLController();
                    //DataTable data = sqlcontroller.ExecutaSQL(s);
                    //
                    //if (data != null && data.Rows != null && data.Rows.Count > 0)
                    //{

                    //}
                }
                //
                ViewBag.Codigo_Empresa = Codigo_Empresa;
                ViewBag.Codigo_Usuario = Codigo_Usuario;
                ViewBag.Nome_Empresa = Nome_Empresa;
                ViewBag.Nome_Usuario = Nome_Usuario;
                ViewBag.Email_Usuario = Email_Usuario;
                ViewBag.Tipo_Empresa = Tipo_Empresa;                
                //
                return View();
            }
            else
            {
                Session["logado"] = "0";
                return Redirect("http://connector.ind.br/");
            }
        }
        //
        //public JsonResult getgraf()
        //{
        //    string sret = string.Empty;
        //    string horala = string.Empty;
        //    List<string> lista = new List<string>();
        //    List<DataGrid> lista_retorno = new List<DataGrid>();
        //    try
        //    {
        //        if (Codigo_Empresa > 0)
        //        {
        //            SQLController sqlcontroller = new SQLController();
        //            List<Medidor> lista_medidor = db.Medidor.Where(a => a.Maquina.Id_Empresa == Codigo_Empresa && a.Id_Maquina > 0).ToList();
        //            //
        //            for (int i = 1; i <= 11; i++)
        //            {
        //                string sql = string.Empty;
        //                int[] valores_media = new int[lista_medidor.Count];
        //                int j = 0;
        //                //
        //                foreach (Medidor item in lista_medidor)
        //                {
        //                    sql = @"select avg(CONVERT(int, potencia)) as media from MedidorHistorico 
        //                                where DataHora >= '" + DateTime.Now.Year + @"-" + i + @"-01 00:00:01.000' 
        //                                  and DataHora <= '" + DateTime.Now.Year + @"-" + (i + 1) + @"-01 00:00:00.001'
        //                                  and id_mac = " + item.Id_Maquina;
        //                    //
        //                    DataTable datatable = new DataTable();
        //                    //
        //                    if (sqlcontroller.ExecutaSQL(sql, out datatable))
        //                    {
        //                        if (!string.IsNullOrEmpty(datatable.Rows[0]["media"].ToString()))
        //                        {
        //                            valores_media[j] = Convert.ToInt32(datatable.Rows[0]["media"]);
        //                        }
        //                        else
        //                        {
        //                            valores_media[j] = 0;
        //                        }
        //                    }
        //                    //
        //                    j++;
        //                }
        //                //
        //                DataGrid ddd = new DataGrid();
        //                //
        //                switch (i)
        //                {
        //                    case 1: ddd.y = "Jan."; break;
        //                    case 2: ddd.y = "Fev."; break;
        //                    case 3: ddd.y = "Mar."; break;
        //                    case 4: ddd.y = "Abr."; break;
        //                    case 5: ddd.y = "Mai."; break;
        //                    case 6: ddd.y = "Jun."; break;
        //                    case 7: ddd.y = "Jul."; break;
        //                    case 8: ddd.y = "Ago."; break;
        //                    case 9: ddd.y = "Set."; break;
        //                    case 10: ddd.y = "Out."; break;
        //                    case 11: ddd.y = "Nov."; break;
        //                }
        //                //
        //                ddd.a = valores_media[0];
        //                ddd.b = valores_media[1];
        //                ddd.c = valores_media[2];
        //                lista_retorno.Add(ddd);
        //                //
        //                //DataTable datatablew = new DataTable();
        //                ////
        //                //if (sqlcontroller.ExecutaSQL(sql, out datatablew))
        //                //{

        //                //}
        //            }
        //            #region dadaos estaticos
        //            //DataGrid dd = new DataGrid();
        //            ////
        //            //dd = new DataGrid();
        //            //dd.y = "10 Set.";
        //            //dd.a = 1902;
        //            //dd.b = 1652;
        //            //dd.c = 1150;
        //            //lista_retorno.Add(dd);
        //            ////
        //            //dd = new DataGrid();
        //            //dd.y = "20 Set.";
        //            //dd.a = 1902;
        //            //dd.b = 1652;
        //            //dd.c = 1150;
        //            //lista_retorno.Add(dd);
        //            ////
        //            //dd = new DataGrid();
        //            //dd.y = "30 Set.";
        //            //dd.a = 1902;
        //            //dd.b = 1652;
        //            //dd.c = 1150;
        //            //lista_retorno.Add(dd);
        //            ////
        //            //dd = new DataGrid();
        //            //dd.y = "1 out.";
        //            //dd.a = 1902;
        //            //dd.b = 1652;
        //            //dd.c = 1150;
        //            //lista_retorno.Add(dd);
        //            ////
        //            //dd = new DataGrid();
        //            //dd.y = "10 Out.";
        //            //dd.a = 1750;
        //            //dd.b = 1730;
        //            //dd.c = 1520;
        //            //lista_retorno.Add(dd);
        //            ////
        //            //dd = new DataGrid();
        //            //dd.y = "1 Set.";
        //            //dd.a = 1902;
        //            //dd.b = 1652;
        //            //dd.c = 1150;
        //            //lista_retorno.Add(dd);
        //            ////
        //            //dd = new DataGrid();
        //            //dd.y = "1 Set.";
        //            //dd.a = 1902;
        //            //dd.b = 1652;
        //            //dd.c = 1150;
        //            //lista_retorno.Add(dd);
        //            ////
        //            //dd = new DataGrid();
        //            //dd.y = "1 Set.";
        //            //dd.a = 1902;
        //            //dd.b = 1652;
        //            //dd.c = 1150;
        //            //lista_retorno.Add(dd);
        //            #endregion
        //        }
        //        sret = "ok";
        //    }
        //    catch (Exception exc)
        //    {
        //        sret = exc.Message;
        //    }
        //    //
        //    return Json(new { data = sret, lista_retorno, results = 1, success = true }, JsonRequestBehavior.AllowGet);
        //}

        //
        #endregion

        #region Diversos
        //
        public ActionResult ConsumoKWMaquina()
        {
            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;
            return View();
        }

        public ActionResult ConsumoTRKWMaquina()
        {
            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;
            return View();
        }

        public ActionResult HistoricoMaquina()
        {
            return View();
        }

        public ActionResult Profileusuario()
        {
            return View();
        }

        public ActionResult Settinhgs()
        {
            return View();
        }
        //
        #endregion

        #region Sequel
        //
        public ActionResult Sequel()
        {
            if (Session != null && Session["sequel_colunas"] != null)
            {
                ViewBag.SequelColunas = Session["sequel_colunas"];
            }
            //
            if (Session != null && Session["sequel_dados"] != null)
            {
                ViewBag.SequelDados = Session["sequel_dados"];
            }
            //
            return View();
        }
        //
        public JsonResult SequelPost(string s)
        {
            string sret = string.Empty;
            sret = "ok";
            try
            {
                SQLController sqlcontroller = new SQLController();
                DataTable data = sqlcontroller.ExecutaSQL(s);
                List<string> lista_colunas = new List<string>();
                List<ListaTable> lista_rows = new List<ListaTable>();
                //
                if (data != null && data.Columns != null && data.Columns.Count > 0)
                {
                    foreach (var coluna in data.Columns)
                    {
                        lista_colunas.Add(coluna.ToString());
                    }
                }
                //
                if (data != null && data.Rows != null && data.Rows.Count > 0)
                {
                    foreach (DataRow row in data.Rows)
                    {
                        ListaTable item_lista = new ListaTable();
                        //
                        foreach (var item_array in row.ItemArray)
                        {
                            item_lista.row.Add(item_array.ToString());
                        }
                        //
                        lista_rows.Add(item_lista);
                    }
                }
                //
                Session["sequel_colunas"] = lista_colunas;
                Session["sequel_dados"] = lista_rows;
            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
            //
            return Json(new { data = sret, results = 1, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        #endregion

        #region POST - Maquina dados
        //
        //public JsonResult mpd(string m, int p)
        //{
        //    string sret = string.Empty;
        //    string horala = string.Empty;
        //    List<string> lista = new List<string>();
        //    try
        //    {
        //        Medidor medidor = db.Medidor.Where(a => a.MAC == m).FirstOrDefault();
        //        //
        //        if (medidor != null)
        //        {
        //            MedidorHistorico medidor_hist = new MedidorHistorico();
        //            //
        //            //foreach (TimeZoneInfo z in TimeZoneInfo.GetSystemTimeZones())
        //            //{
        //            //    if (z != null)
        //            //    {
        //            //        lista.Add(z.Id);
        //            //    }
        //            //}
        //            //
        //            DateTime timeUtc = DateTime.UtcNow;
        //            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");
        //            horala = String.Format("Hora Brasilia: {0}", TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).ToString());
        //            //
        //            int ano = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Year;
        //            int mes = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Month;
        //            int dia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Day;
        //            int hora = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Hour;
        //            int min = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Minute;
        //            int seg = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Second;
        //            //
        //            medidor_hist.Id_Mac = medidor.Id;
        //            medidor_hist.DataHora = new DateTime(ano, mes, dia, hora, min, seg).AddHours(1);
        //            //medidor_hist.DataHora = DateTime.Now;
        //            medidor_hist.Potencia = p.ToString();
        //            //
        //            db.MedidorHistorico.Add(medidor_hist);
        //            db.SaveChanges();
        //            //
        //            sret = "ok";
        //        }
        //        else
        //        {
        //            sret = "Medidor não encontrado!";
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        sret = exc.Message;
        //    }
        //    //
        //    return Json(new { data = sret, results = 1, success = true }, JsonRequestBehavior.AllowGet);
        //}
        ////
        //public async Task<JsonResult> mpd_teste(string m, int p)
        //{
        //    string sret = string.Empty;
        //    string horala = string.Empty;
        //    List<string> lista = new List<string>();
        //    try
        //    {
        //        Medidor medidor = db.Medidor.Where(a => a.MAC == m).FirstOrDefault();
        //        //
        //        if (medidor != null)
        //        {
        //            MedidorHistorico medidor_hist = new MedidorHistorico();
        //            //
        //            DateTime timeUtc = DateTime.UtcNow;
        //            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");
        //            horala = String.Format("Hora Brasilia: {0}", TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).ToString());
        //            //
        //            int ano = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Year;
        //            int mes = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Month;
        //            int dia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Day;
        //            int hora = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Hour;
        //            int min = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Minute;
        //            int seg = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Second;
        //            //
        //            medidor_hist.Id_Mac = medidor.Id;
        //            medidor_hist.DataHora = new DateTime(ano, mes, dia, hora, min, seg).AddHours(1);
        //            medidor_hist.Potencia = p.ToString();
        //            //
        //            db.MedidorHistorico.Add(medidor_hist);
        //            db.SaveChanges();
        //            db.Entry(medidor_hist).Reload();
        //            //
        //            sret = await teste_firebase(medidor_hist.Id.ToString(), medidor.Id.ToString(), m, medidor.Descricao, p.ToString(), medidor_hist.DataHora);
        //        }
        //        else
        //        {
        //            sret = "Medidor não encontrado!";
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        sret = exc.Message;
        //    }
        //    //
        //    return Json(new { data = sret, results = 1, success = true }, JsonRequestBehavior.AllowGet);
        //}
        //
        public async Task<string> teste_firebase(string id, string id_medidor, string mac_radiobase, string desc_medidor, string potencia, Nullable<System.DateTime> DataHora) // pega programação máquina
        {
            string sret = string.Empty;
            string pamb = string.Empty;

            IFirebaseClient client;
            IFirebaseConfig config = new FirebaseConfig
            {
                AuthSecret = "MCPZ3qTPh8YdCQxk8aFvM59tWGoLuZUQb3fakIqJ",
                BasePath = "https://notificacaohahaha.firebaseio.com/"
            };
            //
            var data = new Data
            {
                Id = id,
                Medidor = mac_radiobase,
                Descricao_Medidor = desc_medidor,
                Id_Medidor = id_medidor,
                Potencia = potencia,
                DataHora = DataHora.Value.ToShortDateString() + " " + DataHora.Value.ToShortTimeString()
            };
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = await client.SetTaskAsync("Alertas/" + id, data);
            Data result = response.ResultAs<Data>();
            //
            if (!string.IsNullOrEmpty(result.Id))
            {
                sret = "ok";
            }
            else
            {
                sret = "nok";
            }
            //
            return sret;
        }
        //
        #endregion

        #region Insert Data Maquina Medidor x Historico
        //
        //public void insertdata(int mes_inicio, int mes_fim, int id_maquina, int range_ini, int range_fim)
        //{
        //    DateTime dt = new DateTime(2018, mes_inicio, 1, 0, 0, 1);
        //    DateTime dt_fim = new DateTime(2018, mes_fim, 1, 0, 0, 1);
        //    Random rnd = new Random();
        //    //
        //    while (dt <= dt_fim)
        //    {
        //        MedidorHistorico mh = new MedidorHistorico();
        //        mh.Id_Mac = id_maquina;
        //        mh.Potencia = rnd.Next(range_ini, range_fim).ToString();
        //        mh.DataHora = dt;
        //        db.MedidorHistorico.Add(mh);
        //        dt = dt.AddMinutes(30);
        //    }
        //    //
        //    db.SaveChanges();
        //}

        //public JsonResult Insertdatahistorico()
        //{
        //    string sret = "";
        //    int mes_ini = 1;
        //    //
        //    for (int i = 1; i <= 11; i++)
        //    {
        //        insertdata(mes_ini, mes_ini + 1, 4, 60, 100);
        //        mes_ini++;
        //    }
        //    //
        //    return Json(new { data = sret, results = 1, success = true }, JsonRequestBehavior.AllowGet);
        //}
        //
        #endregion
    }

    internal class Data
    {
        public string Id { get; set; }
        public string Id_Medidor { get; set; }
        public string Descricao_Medidor { get; set; }
        public string Medidor { get; set; }
        public string Potencia { get; set; }
        public string DataHora { get; set; }
    }
}