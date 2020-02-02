using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class EmpresaController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.GeralAtivo = "active";
            ViewBag.GeralEmpresasAtivo = "active";
            ViewBag.GeralShow = "show";

            List<Empresa> lista_empresas = new List<Empresa>();
            List<EmpresaModel> lista_empresas_model = new List<EmpresaModel>();
            //
            if (Codigo_Empresa > 0)
            {
                lista_empresas = db.Empresa.Where(a => a.Id == Codigo_Empresa).ToList();
                //
                foreach (Empresa item in lista_empresas)
                {
                    EmpresaModel oEmpresaModel = EmpresaToModel(item);
                    lista_empresas_model.Add(oEmpresaModel);
                    //
                    if (item.Tipo.HasValue && item.Tipo.Value == 1)
                    {
                        string empresas = item.Empresas;
                        if (!string.IsNullOrEmpty(empresas))
                        {
                            string[] empresas_array = empresas.Split(',');
                            int[] ids = Array.ConvertAll(empresas_array, s => int.Parse(s));
                            var lista_sub_empresas = db.Empresa.Where(r => ids.Contains(r.Id));
                            if (lista_sub_empresas.Any())
                            {
                                foreach (Empresa sub_empresa in lista_sub_empresas)
                                {
                                    oEmpresaModel = EmpresaToModel(sub_empresa);
                                    lista_empresas_model.Add(oEmpresaModel);
                                }
                            }
                        }
                    }
                }
            }
            //
            ViewBag.ListaEmpresas = lista_empresas_model;
            //
            return View();
        }
        //
        public EmpresaModel EmpresaToModel(Empresa empresa)
        {
            EmpresaModel model = new EmpresaModel();
            //
            model.Id = empresa.Id;
            model.AnalitycCode = empresa.AnalitycCode;
            model.Nome = empresa.Nome;
            model.Resumo = empresa.Resumo;
            model.Tipo = empresa.Tipo;
            model.URL = empresa.URL;
            model.Email = empresa.Email;
            model.Cidade = empresa.Cidade;
            model.Estado = empresa.Estado;
            model.EstadoSigla = PegaEstadoSigla(empresa.Estado);
            model.Telefone = empresa.Telefone;
            model.Numero = empresa.Numero;
            model.CEP = empresa.CEP;
            model.NomeFantasia = empresa.NomeFantasia;
            model.Endereco = empresa.Endereco;
            model.Bairro = empresa.Bairro;
            model.Site = empresa.Site;
            model.UsuarioAtivo = empresa.Usuario.Any();
            model.ColetorAtivo = empresa.Coletor.Any();
            model.MaquinaAtiva = empresa.Maquina.Any();
            //
            return model;
        }
        //
        public List<EmpresaModel> CarregaListaEmpresas()
        {
            List<Empresa> lista_empresas = new List<Empresa>();
            List<EmpresaModel> lista_empresas_model = new List<EmpresaModel>();
            //
            if (Codigo_Empresa > 0)
            {
                lista_empresas = db.Empresa.Where(a => a.Id == Codigo_Empresa).ToList();
                //
                foreach (Empresa item in lista_empresas)
                {
                    EmpresaModel oEmpresaModel = EmpresaToModel(item);
                    lista_empresas_model.Add(oEmpresaModel);
                    //
                    if (item.Tipo.HasValue && item.Tipo.Value == 1)
                    {
                        string empresas = item.Empresas;
                        if (!string.IsNullOrEmpty(empresas))
                        {
                            string[] empresas_array = empresas.Split(',');
                            int[] ids = Array.ConvertAll(empresas_array, s => int.Parse(s));
                            var lista_sub_empresas = db.Empresa.Where(r => ids.Contains(r.Id));
                            if (lista_sub_empresas.Any())
                            {
                                foreach (Empresa sub_empresa in lista_sub_empresas)
                                {
                                    oEmpresaModel = EmpresaToModel(sub_empresa);
                                    lista_empresas_model.Add(oEmpresaModel);
                                }
                            }
                        }
                    }
                }
            }
            //
            return lista_empresas_model;
        }
        //
        public JsonResult CarregaEmpresas()
        {
            string sret = string.Empty;
            string erro = string.Empty;
            List<EmpresaModel> lista_retorno = new List<EmpresaModel>();
            //
            try
            {
                lista_retorno = CarregaListaEmpresas();
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
        public JsonResult EmpresaPost(int idempresa, string nome, string telefone, string endereco, string bairro, string numero, string cidade, string cep, string site, string email, string estado)
        {
            string sret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                Empresa oEmpresaModel = new Empresa();
                //
                if (idempresa > 0)
                {
                    oEmpresaModel = db.Empresa.Where(a => a.Id == idempresa).FirstOrDefault();
                    //
                    if (oEmpresaModel != null)
                    {
                        oEmpresaModel.Nome = nome;
                        oEmpresaModel.Telefone = telefone;
                        oEmpresaModel.Endereco = endereco;
                        oEmpresaModel.Bairro = bairro;
                        oEmpresaModel.Numero = numero;
                        oEmpresaModel.Cidade = cidade;
                        oEmpresaModel.CEP = cep;
                        oEmpresaModel.Site = site;
                        oEmpresaModel.Email = email;
                        oEmpresaModel.Estado = estado;
                        //
                        db.Entry(oEmpresaModel).State = EntityState.Modified;
                    }
                }
                else
                {
                    if (Tipo_Empresa == 1)
                    {
                        oEmpresaModel.Tipo = 2;
                    }
                    else
                    {
                        oEmpresaModel.Tipo = 1;
                    }
                    //
                    oEmpresaModel.Nome = nome;
                    oEmpresaModel.Telefone = telefone;
                    oEmpresaModel.Endereco = endereco;
                    oEmpresaModel.Bairro = bairro;
                    oEmpresaModel.Numero = numero;
                    oEmpresaModel.Cidade = cidade;
                    oEmpresaModel.CEP = cep;
                    oEmpresaModel.Site = site;
                    oEmpresaModel.Email = email;
                    oEmpresaModel.Estado = estado;
                    //
                    db.Empresa.Add(oEmpresaModel);
                }
                //
                db.SaveChanges();
                db.Entry(oEmpresaModel).Reload();
                //
                if (Tipo_Empresa == 1 && idempresa == 0)
                {
                    Empresa empresa_logado = db.Empresa.Where(x => x.Id == Codigo_Empresa).FirstOrDefault();
                    //
                    if (empresa_logado != null)
                    {
                        string empresas = empresa_logado.Empresas;
                        //
                        if (string.IsNullOrEmpty(empresas))
                        {
                            empresas = oEmpresaModel.Id.ToString();
                        }
                        else
                        {
                            empresas += "," + oEmpresaModel.Id.ToString();
                        }
                        //
                        empresa_logado.Empresas = empresas;
                        //
                        db.Entry(empresa_logado).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
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
        public JsonResult ExcluiEmpresaPost(int idempresa)
        {
            string ret = string.Empty;
            try
            {
                Empresa oEmpresaModel = db.Empresa.Where(a => a.Id == idempresa).FirstOrDefault();
                if (Tipo_Empresa == 1)
                {
                    Empresa empresa_logado = db.Empresa.Where(x => x.Id == Codigo_Empresa).FirstOrDefault();
                    //
                    if (empresa_logado != null)
                    {
                        if (!string.IsNullOrEmpty(empresa_logado.Empresas))
                        {
                            string[] empresas = empresa_logado.Empresas.Split(',');
                            string empresasids = string.Empty;
                            //
                            for (int i = 0; i < empresas.Length; i++)
                            {
                                if (!empresas[i].Equals(idempresa.ToString()))
                                {
                                    if (string.IsNullOrEmpty(empresasids))
                                    {
                                        empresasids = empresas[i];
                                    }
                                    else
                                    {
                                        empresasids += "," + empresas[i];
                                    }
                                }
                            }
                            //
                            empresa_logado.Empresas = empresasids;
                        }
                        //
                        db.Entry(empresa_logado).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {

                    }
                }
                //
                if (oEmpresaModel != null)
                {
                    db.Entry(oEmpresaModel).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = "ok";
                }
                else
                {
                    ret = "nok";
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