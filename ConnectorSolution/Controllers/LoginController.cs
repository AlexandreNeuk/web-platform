using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Connector.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Index()
        {
            Session["logado"] = "0";
            return View();
        }
        //
        public JsonResult Login(string email, string chave)
        {
            FormsAuthentication.SetAuthCookie(email, false);
            string data = string.Empty;
            //
            Usuario user = new Usuario();
            string sRet = new UtilController().CheckLogin(email, chave, out user);
            //
            if (sRet.Equals("ok"))
            {
                if (!((HttpRequestWrapper)((HttpContextWrapper)HttpContext).Request).UserHostName.Equals("::1"))
                {
                    Thread myNewThread = new Thread(() => LogUsuarioLogin(user.ID, email));
                    myNewThread.Start();
                }
                //
                if (user.Empresa.Id != 1)
                {
                    Session["mapa"] = 1;
                }
                else
                {
                    Session["mapa"] = 0;
                }
                
                Session["cd_tipo"] = user.Tipo.HasValue ? user.Tipo.Value.ToString() : "";
                Session["cd_usuario"] = user.ID;
                Session["nome_usuario"] = user.Nome;
                Session["email"] = email;
                Session["cd_empresa"] = user.Id_Empresa;
                Session["nome_empresa"] = user.Empresa.Nome;
                Session["tipo_empresa"] = db.Empresa.Where(x => x.Id == user.Id_Empresa).FirstOrDefault().Tipo;
                Session["logado"] = "1";
                //
                FormsAuthentication.SetAuthCookie(email, false);
                //
                return Json(new { data = sRet, results = 0, success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                data = "nok";
                SetSessionNull();
            }
            //
            return Json(new { data, erro = sRet, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public void LogUsuarioLogin(int idusuario, string email)
        {
            Usuario usuario = db.Usuario.Where(a => a.Email.Equals(email)).FirstOrDefault();
            if (usuario.Count.HasValue && usuario.Count.Value > 0)
            {
                usuario.Count = usuario.Count++;
            }
            else
            {
                usuario.Count = 1;
            }
            //
            try
            {
                UsuarioLogin mul = new UsuarioLogin();
                //
                DateTime timeUtc = DateTime.UtcNow;
                var brasilia = TimeZoneInfo.FindSystemTimeZoneById("Central Brazilian Standard Time");
                //
                int ano = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Year;
                int mes = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Month;
                int dia = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Day;
                int hora = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Hour;
                int min = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Minute;
                int seg = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, brasilia).Second;
                //
                DateTime now = new DateTime(ano, mes, dia, hora, min, seg).AddHours(1);
                mul.DataHora = now;
                mul.Id_MedidorUsuario = idusuario;
                db.UsuarioLogin.Add(mul);
                db.SaveChanges();
                //
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception exc)
            {
            }
        }
        //
        private void SetSessionNull()
        {
            Session["cd_usuario"] = null;
            Session["nome_usuario"] = null;
            Session["email"] = null;
            Session["cd_empresa"] = null;
            Session["nome_empresa"] = null;
            Session["tipo_empresa"] = null;
            Session["logado"] = "0";
            Session["mapa"] = null;
        }
        //
        public JsonResult Logout()
        {
            SetSessionNull();
            //
            return Json(new { results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}