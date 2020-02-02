using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Connector.Controllers.UtilController;

namespace Connector.Controllers
{
    public class UsuarioController : BaseController
    {
        // GET: Usuario
        public ActionResult Index()
        {
            ViewBag.GeralAtivo = "active";
            ViewBag.GeralUsuariosAtivo = "active";
            ViewBag.GeralShow = "show";
            //
            List<Usuario> lista_usuarios = new List<Usuario>();
            List<UsuarioModel> lista_retorno = new List<UsuarioModel>();
            List<EmpresaModel> lista_empresas = new List<EmpresaModel>();
            //
            lista_empresas = PegaEmpresas();
            foreach (EmpresaModel item in lista_empresas)
            {
                lista_usuarios = db.Usuario.Where(x => x.Id_Empresa == item.Id).ToList();
                foreach (var item_usuario in lista_usuarios)
                {
                    UsuarioModel um = new UsuarioModel();
                    um.ID = item_usuario.ID;
                    um.Id_Empresa = item_usuario.Id_Empresa;
                    um.Ative = item_usuario.Ative;
                    um.Email = item_usuario.Email;
                    um.Nome = item_usuario.Nome;
                    um.Tipo = item_usuario.Tipo;
                    um.EmpresaNome = item.Nome;
                    lista_retorno.Add(um);
                }
            }
            //
            ViewBag.ListaEmpresas = lista_empresas;
            ViewBag.ListaUsuarios = lista_retorno;
            //
            return View();
        }
        //
        public List<UsuarioModel> CarregaListaUsuarios()
        {
            List<Usuario> lista_usuarios = new List<Usuario>();
            List<UsuarioModel> lista_retorno = new List<UsuarioModel>();
            List<EmpresaModel> lista_empresas = new List<EmpresaModel>();
            //
            lista_empresas = PegaEmpresas();
            foreach (EmpresaModel item in lista_empresas)
            {
                lista_usuarios = db.Usuario.Where(x => x.Id_Empresa == item.Id).ToList();
                foreach (var item_usuario in lista_usuarios)
                {
                    UsuarioModel um = new UsuarioModel();
                    um.ID = item_usuario.ID;
                    um.Id_Empresa = item_usuario.Id_Empresa;
                    um.Ative = item_usuario.Ative;
                    um.Email = item_usuario.Email;
                    um.Nome = item_usuario.Nome;
                    um.Tipo = item_usuario.Tipo;
                    um.EmpresaNome = item.Nome;
                    lista_retorno.Add(um);
                }
            }
            //
            return lista_retorno;
        }
        //
        public JsonResult CarregaUsuarios()
        {
            string sret = string.Empty;
            string erro = string.Empty;
            List<UsuarioModel> lista_retorno = new List<UsuarioModel>();
            //
            try
            {
                lista_retorno = CarregaListaUsuarios();
                sret = "ok";
            }
            catch (Exception exc)
            {
                sret = "nok";
                erro = exc.Message;
            }
            //
            return Json(new { data = sret, lista_retorno, results = 0, success = true, erro }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult UsuarioPost(int idusuario, int idempresa, string nome, string email, string pass)
        {
            string sret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                Usuario oUsuarioModel = new Usuario();
                //
                if (idusuario > 0)
                {
                    oUsuarioModel = db.Usuario.Where(a => a.ID == idusuario && a.Id_Empresa == idempresa).FirstOrDefault();
                    //
                    if (oUsuarioModel != null)
                    {
                        oUsuarioModel.Nome = nome;
                        oUsuarioModel.Email = email;
                        oUsuarioModel.Ative = 1;
                        //
                        if (!string.IsNullOrEmpty(pass))
                        {
                            Encryption cryp = new Encryption();
                            pass = cryp.Encrypt(pass);
                            oUsuarioModel.Pass = pass;
                        }                        
                        //
                        db.Entry(oUsuarioModel).State = EntityState.Modified;
                    }
                }
                else
                {
                    oUsuarioModel.Nome = nome;
                    oUsuarioModel.Email = email;
                    oUsuarioModel.Ative = 1;
                    oUsuarioModel.Id_Empresa = idempresa;
                    //
                    if (!string.IsNullOrEmpty(pass))
                    {
                        Encryption cryp = new Encryption();
                        pass = cryp.Encrypt(pass);
                        oUsuarioModel.Pass = pass;
                    }
                    //
                    db.Usuario.Add(oUsuarioModel);
                }
                //
                db.SaveChanges();
                db.Entry(oUsuarioModel).Reload();
                //
                sret = "ok";
            }
            catch (DbEntityValidationException e)
            {
                erro = e.Message;
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        erro += ve.ErrorMessage + " - ";
                    }
                }
                //
                sret = "nok";
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
        public JsonResult ExcluiUsuarioPost(int idempresa, int idusuario)
        {
            string ret = string.Empty;
            try
            {
                if (Codigo_Usuario == idusuario && idempresa == Codigo_Empresa)
                {
                    ret = "usu";
                }
                else
                {
                    Usuario oUsuarioModel = db.Usuario.Where(a => a.ID == idusuario && a.Id_Empresa == idempresa).FirstOrDefault();
                    //
                    if (oUsuarioModel != null)
                    {
                        string erro = string.Empty;
                        string sql = "DELETE UsuarioLogin WHERE Id_MedidorUsuario = " + idusuario;
                        SQLController sqlcontroller = new SQLController();
                        int total_excl = sqlcontroller.ExecutaSQLNonQuery(sql, out erro);
                        //
                        db.Entry(oUsuarioModel).State = EntityState.Deleted;
                        db.SaveChanges();
                        ret = "ok";
                    }
                    else
                    {
                        ret = "nok";
                    }
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }

            return Json(new { ret, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}