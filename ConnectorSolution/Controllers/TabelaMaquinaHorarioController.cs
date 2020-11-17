using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class TabelaMaquinaHorarioController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.FabricaAtivo = "active";
            ViewBag.FabricaProgramacaoMaquinaAtivo = "active";
            ViewBag.FabricaShow = "show";

            List<Maquina> lista_mdidor_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            ViewBag.ListaMaquinas = lista_mdidor_maquina;
            return View();
        }
        //
        //public JsonResult ppm(int b) // pega programação máquina
        //{
        //    string erro = string.Empty;
        //    List<MaquinaHorario> list = db.MaquinaHorario.Where(a => a.Id_Maquina == b).OrderBy(a => a.Descricao).ToList();
        //    List<maquina_horaios> lista_retonro = new List<maquina_horaios>();
        //    //
        //    if (list != null && list.Count > 0)
        //    {
        //        DateTime timeUtc = DateTime.UtcNow;
        //        var brasilia = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");
        //        //
        //        int ano = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Year;
        //        int mes = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Month;
        //        int dia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Day;
        //        int hora = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Hour;
        //        int min = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Minute;
        //        int seg = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Second;
        //        //
        //        DateTime dt_now = new DateTime(2019, 1, 1, hora, min, seg).AddHours(1);
        //        DateTime dt_fim = new DateTime(2019, 1, 1, hora, min, seg).AddHours(1);
        //        //
        //        foreach (MaquinaHorario item in list)
        //        {
        //            maquina_horaios novo = new maquina_horaios();
        //            //
        //            novo.dia = item.Descricao;
        //            //
        //            novo.horaon = item.DataHoraInicio.Value.ToString("HH:mm");
        //            novo.horafim = item.DataHoraFim.Value.ToString("HH:mm");
        //            //
        //            string sRet = string.Empty;
        //            string dia_semana = string.Empty;
        //            //
        //            switch (DateTime.Now.DayOfWeek)
        //            {
        //                case DayOfWeek.Monday:
        //                    dia_semana = "Segunda";
        //                    break;
        //                case DayOfWeek.Tuesday:
        //                    dia_semana = "Terça";
        //                    break;
        //                case DayOfWeek.Wednesday:
        //                    dia_semana = "Quarta";
        //                    break;
        //                case DayOfWeek.Thursday:
        //                    dia_semana = "Quinta";
        //                    break;
        //                case DayOfWeek.Friday:
        //                    dia_semana = "Sexta";
        //                    break;
        //                case DayOfWeek.Saturday:
        //                    dia_semana = "Sábado";
        //                    break;
        //                case DayOfWeek.Sunday:
        //                    dia_semana = "Domingo";
        //                    break;
        //            }
        //            //
        //            if (item.Descricao.Equals(dia_semana))
        //            {
        //                DateTime dt_fim_banco = item.DataHoraFim.Value;
        //                if (item.DataHoraInicio.Value.Hour > 20 && item.DataHoraFim.Value.Hour < 6) // debugar esse trecho durante a noite no intervalo de 23 e 00:30
        //                {
        //                    if (dt_now.Ticks >= item.DataHoraInicio.Value.Ticks) // && dt_fim.Ticks <= dt_fim_banco.Ticks)
        //                    {
        //                        if (dt_now.Ticks <= item.DataHoraFim.Value.AddDays(1).Ticks)
        //                        {
        //                            novo.ativo = true;
        //                        }
        //                        else
        //                        {
        //                            novo.ativo = false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        novo.ativo = false;
        //                    }
        //                }
        //                else
        //                {
        //                    if ((dt_now.Ticks >= item.DataHoraInicio.Value.Ticks && dt_fim.Ticks <= item.DataHoraFim.Value.Ticks))
        //                    {
        //                        novo.ativo = true;
        //                    }
        //                    else
        //                    {
        //                        novo.ativo = false;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                DateTime dt_fim_banco = item.DataHoraFim.Value;
        //                if (item.DataHoraInicio.Value.Hour > 20 && item.DataHoraFim.Value.Hour < 6)
        //                {
        //                    dt_fim_banco = new DateTime(item.DataHoraFim.Value.Year,
        //                                          item.DataHoraFim.Value.Month,
        //                                          item.DataHoraFim.Value.Day + 1,
        //                                          item.DataHoraFim.Value.Hour,
        //                                          item.DataHoraFim.Value.Minute,
        //                                          item.DataHoraFim.Value.Second);
        //                    //
        //                    if (item.Descricao != dia_semana)
        //                    {
        //                        dt_fim = new DateTime(2019, 1, 2, hora, min, seg).AddHours(1);
        //                    }
        //                    else
        //                    {
        //                        dt_fim = new DateTime(2019, 1, 1, hora, min, seg).AddHours(1);
        //                    }
        //                }
        //                //
        //                //if ((dt_now.Ticks >= item.DataHoraInicio.Value.Ticks && dt_fim.Ticks <= dt_fim_banco.Ticks))
        //                //{
        //                //    novo.ativo = true;
        //                //}
        //                else
        //                {
        //                    if (ValidaTrocaDia(item.Descricao, dia_semana))
        //                    {
        //                        if (item.DataHoraInicio.Value.Hour > 20 && item.DataHoraFim.Value.Hour < 6) // intervalo está entre dois dias
        //                        {
        //                            if (dt_now.Hour <= 6) // valida se é depois da meia noite
        //                            {
        //                                dt_now = new DateTime(dt_now.Year, dt_now.Month, item.DataHoraFim.Value.Day, dt_now.Hour, dt_now.Minute, dt_now.Second);
        //                                if (dt_now.Ticks <= item.DataHoraFim.Value.Ticks)
        //                                {
        //                                    novo.ativo = true;
        //                                }
        //                                else
        //                                {
        //                                    novo.ativo = false;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                if (dt_now.Ticks >= item.DataHoraInicio.Value.Ticks)
        //                                {
        //                                    novo.ativo = true;
        //                                }
        //                                else
        //                                {
        //                                    novo.ativo = false;
        //                                }
        //                            }
        //                        }
        //                        else
        //                        {
        //                            novo.ativo = false;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        novo.ativo = false;
        //                    }
        //                }
        //            }
        //            //
        //            lista_retonro.Add(novo);
        //        }
        //        ViewBag.ListaMaquinas = lista_retonro;
        //    }
        //    else
        //    {
        //        Maquina maquina = db.Maquina.Where(a => a.ID == b).FirstOrDefault();
        //        //
        //        if (maquina != null)
        //        {
        //            DateTime timeUtc = DateTime.UtcNow;
        //            var brasilia = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");
        //            //
        //            int ano = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Year;
        //            int mes = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Month;
        //            int dia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Day;
        //            int hora = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Hour;
        //            int min = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Minute;
        //            int seg = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Second;
        //            //
        //            DateTime now = new DateTime(ano, mes, dia, hora, min, seg).AddHours(1);
        //            //
        //            MaquinaHorario mmh = new MaquinaHorario();
        //            //
        //            string[] dias = new[] { "Domingo", "Segunda", "Terça", "Quarta", "Quinta", "Sexta", "Sábado" };
        //            //
        //            DateTime dt = DateTime.Now;
        //            DateTime dt_hrini = new DateTime(2019, 1, 1, 13, 30, 0);
        //            DateTime dt_hrfim = new DateTime(2019, 1, 1, 14, 30, 0);
        //            for (int i = 0; i < dias.Length; i++)
        //            {
        //                try
        //                {
        //                    mmh.Id_Maquina = b;
        //                    mmh.Descricao = dias[i];
        //                    mmh.DataHoraInicio = dt_hrini;
        //                    mmh.DataHoraFim = dt_hrfim;
        //                    //
        //                    db.MaquinaHorario.Add(mmh);
        //                    db.SaveChanges();
        //                    //
        //                    maquina_horaios novo = new maquina_horaios();
        //                    novo.dia = dias[i];
        //                    novo.horaon = dt_hrini.ToString("HH:mm");
        //                    novo.horafim = dt_hrfim.ToString("HH:mm");
        //                    //
        //                    lista_retonro.Add(novo);
        //                }
        //                catch (Exception exc)
        //                {
        //                    //sRet = exc.Message;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            erro = "1";
        //        }
        //    }
        //    //
        //    lista_retonro = AjustaDias(lista_retonro);
        //    //
        //    return Json(new { data = lista_retonro, erro, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        //}
        ////
        public List<maquina_horaios> AjustaDias(List<maquina_horaios> lista)
        {
            List<maquina_horaios> lista_retonro = new List<maquina_horaios>();
            lista_retonro.Add(lista.Where(a => a.dia.ToLower().Contains("domi")).FirstOrDefault());
            lista_retonro.Add(lista.Where(a => a.dia.ToLower().Contains("seg")).FirstOrDefault());
            lista_retonro.Add(lista.Where(a => a.dia.ToLower().Contains("ter")).FirstOrDefault());
            lista_retonro.Add(lista.Where(a => a.dia.ToLower().Contains("qua")).FirstOrDefault());
            lista_retonro.Add(lista.Where(a => a.dia.ToLower().Contains("qui")).FirstOrDefault());
            lista_retonro.Add(lista.Where(a => a.dia.ToLower().Contains("sex")).FirstOrDefault());
            lista_retonro.Add(lista.Where(a => a.dia.ToLower().Contains("sáb")).FirstOrDefault());
            return lista_retonro;
        }
        //
        public bool ValidaTrocaDia(string dia_semana_um, string dia_semana_dois)
        {
            bool bRet = false;
            //
            if (dia_semana_dois.Equals("Segunda") && dia_semana_um.Equals("Domingo"))
            {
                bRet = true;
            }
            else if (dia_semana_dois.Equals("Terça") && dia_semana_um.Equals("Segunda"))
            {
                bRet = true;
            }
            else if (dia_semana_dois.Equals("Quarta") && dia_semana_um.Equals("Terça"))
            {
                bRet = true;
            }
            else if (dia_semana_dois.Equals("Quinta") && dia_semana_um.Equals("Quarta"))
            {
                bRet = true;
            }
            else if (dia_semana_dois.Equals("Sexta") && dia_semana_um.Equals("Quinta"))
            {
                bRet = true;
            }
            else if (dia_semana_dois.Equals("Sábado") && dia_semana_um.Equals("Sexta"))
            {
                bRet = true;
            }
            else if (dia_semana_dois.Equals("Domingo") && dia_semana_um.Equals("Sábado"))
            {
                bRet = true;
            }
            //
            return bRet;
        }
        //
        public JsonResult GetMaquinaHorario(string mac, int id)
        {
            string sRet = string.Empty;
            string dia_semana = string.Empty;
            //
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    dia_semana = "Segunda";
                    break;
                case DayOfWeek.Tuesday:
                    dia_semana = "Terça";
                    break;
                case DayOfWeek.Wednesday:
                    dia_semana = "Quarta";
                    break;
                case DayOfWeek.Thursday:
                    dia_semana = "Quinta";
                    break;
                case DayOfWeek.Friday:
                    dia_semana = "Sexta";
                    break;
                case DayOfWeek.Saturday:
                    dia_semana = "Sábado";
                    break;
                case DayOfWeek.Sunday:
                    dia_semana = "Domingo";
                    break;
            }
            //
            //MaquinaHorario mmh = db.MaquinaHorario.Where(a => a.Id_Maquina == id && a.Descricao.ToLower() == dia_semana).FirstOrDefault();

            ////Medidor mm = db.Medidor.Where( a => a.Id == )
            ////
            //if (mmh != null)
            //{
            //    DateTime timeUtc = DateTime.UtcNow;
            //    var brasilia = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");
            //    //
            //    int ano = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Year;
            //    int mes = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Month;
            //    int dia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Day;
            //    int hora = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Hour;
            //    int min = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Minute;
            //    int seg = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Second;
            //    //
            //    DateTime now = new DateTime(ano, mes, dia, hora, min, seg).AddHours(1);

            //    if (now.Hour > mmh.DataHoraInicio.Value.Hour)
            //    {
            //        if (now.Hour < mmh.DataHoraFim.Value.Hour)
            //        {
            //            sRet = "1";
            //        }
            //        else
            //        {
            //            int hora2 = mmh.DataHoraFim.Value.Hour;
            //            int minu = mmh.DataHoraFim.Value.Minute;
            //            int segun = mmh.DataHoraFim.Value.Second;
            //            //
            //            DateTime data_hoje_hora_banco = new DateTime(now.Year, now.Month, now.Day, hora2, minu, segun);
            //            if (now <= data_hoje_hora_banco)
            //            {
            //                sRet = "1";
            //            }
            //            else
            //            {
            //                sRet = "0";
            //            }
            //        }
            //    }
            //    else if (now.Hour == mmh.DataHoraInicio.Value.Hour)
            //    {
            //        sRet = "1";
            //    }
            //    else if (now.Hour < mmh.DataHoraInicio.Value.Hour)
            //    {
            //        sRet = "0";
            //    }
            //}
            //else
            //{
            //    sRet = "O id da máquna não foi encontrado: " + id;
            //}
            //
            return Json(
                new
                {
                    data = sRet,
                    id
                },
                JsonRequestBehavior.AllowGet
            );
        }
        //
        public JsonResult pmp(int idmaquina, string horarios) // Prorgamacao Máquina Horário Post
        {
            string sRet = string.Empty;
            //
            try
            {
                string[] horarios_array = horarios.Split('*');
                //
                for (int i = 0; i < horarios_array.Length; i++)
                {
                    string[] linha = horarios_array[i].Split('$');
                    string dia = linha[0];
                    //
                    //MaquinaHorario mmh = db.MaquinaHorario.Where(a => a.Id_Maquina == idmaquina && a.Descricao == dia).FirstOrDefault();
                    ////
                    //if (mmh != null)
                    //{
                    //    int hora_inicio = Convert.ToInt32(linha[1].Split(':')[0]);
                    //    int minuto_inicio = Convert.ToInt32(linha[1].Split(':')[1]);
                    //    int hora_fim = Convert.ToInt32(linha[2].Split(':')[0]);
                    //    int minuto_fim = Convert.ToInt32(linha[2].Split(':')[1]);
                    //    //
                    //    DateTime dt = DateTime.Now;
                    //    mmh.DataHoraInicio = new DateTime(2019, 1, 1, hora_inicio, minuto_inicio, 0);
                    //    mmh.DataHoraFim = new DateTime(2019, 1, 1, hora_fim, minuto_fim, 0);
                    //    //
                    //    db.Entry(mmh).State = EntityState.Modified;
                    //    db.SaveChanges();
                    //}
                }
            }
            catch (Exception exc)
            {
                sRet = exc.Message;
            }
            //
            return Json(new { data = sRet }, JsonRequestBehavior.AllowGet);
        }
    }
}