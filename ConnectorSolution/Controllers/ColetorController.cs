using Connector.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using static Connector.Models.Enum;

namespace Connector.Controllers
{
    public class ColetorController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.FabricaAtivo = "active";
            ViewBag.FabricaColetorAtivo = "active";
            ViewBag.FabricaShow = "show";
            ViewBag.ListaColetores = CarregaDadosColetores();
            ViewBag.ListaEmpresas = PegaEmpresas();
            return View();
        }
        //
        public List<ColetorModel> CarregaDadosColetores()
        {
            List<Coletor> lista_coletores = new List<Coletor>();
            List<EmpresaModel> lista_empresas = new List<EmpresaModel>();
            List<ColetorModel> lista_coletor_models = new List<ColetorModel>();
            //
            if (Codigo_Empresa > 0)
            {
                lista_coletores = db.Coletor.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
                if (Tipo_Empresa == 1)
                {
                    ViewBag.ShowCrudIncluir = "inline";
                    ViewBag.ShowCrudEditar = "inline";
                    ViewBag.ShowCrudExcluir = "inline";

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
                            List<Coletor> lista_sub_coletores = new List<Coletor>();
                            foreach (Empresa sub_empresa in lista_sub_empresas)
                            {
                                empresa_model = new EmpresaModel();
                                empresa_model.Id = sub_empresa.Id;
                                empresa_model.Nome = sub_empresa.Nome;
                                lista_empresas.Add(empresa_model);

                                lista_sub_coletores = db.Coletor.Where(x => x.Id_Empresa == sub_empresa.Id).ToList();
                                lista_coletores.AddRange(lista_sub_coletores);
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.ShowCrudIncluir = "none";
                    ViewBag.ShowCrudEditar = "none";
                    ViewBag.ShowCrudExcluir = "none";
                }
                //
                string spossuiAlerta = string.Empty;
                foreach (Coletor item in lista_coletores)
                {
                    ColetorModel oColetor = new ColetorModel();
                    //
                    oColetor.Id = item.Id;
                    oColetor.Id_Empresa = item.Id_Empresa.HasValue ? item.Id_Empresa.Value : Codigo_Empresa;
                    oColetor.Id_Maquina = item.Id_Maquina;
                    //
                    oColetor.Empresa = item.Empresa.Nome;
                    oColetor.Ativo = item.Ativo;
                    oColetor.Descricao = item.Descricao;
                    oColetor.MAC = item.MAC.ToUpper();
                    oColetor.Alerta = item.Alerta;
                    int tot_alertas = db.ColetorAlerta.Where(x => x.Id_Coletor == item.Id && x.Id_Empresa == oColetor.Id_Empresa).ToList().Count;
                    //
                    if (item.Alerta.HasValue && item.Alerta.Value == 1)
                    {
                        spossuiAlerta = "Sim (" + tot_alertas + ")";
                    }
                    else
                    {
                        spossuiAlerta = "Não (" + tot_alertas + ")";
                    }
                    oColetor.PossuiAlerta = spossuiAlerta;
                    //
                    if (oColetor.Id_Maquina != null && oColetor.Id_Maquina > 0)
                    {
                        oColetor.Maquina = db.Maquina.Where(a => a.ID == oColetor.Id_Maquina).FirstOrDefault().Descricao;
                    }
                    //
                    lista_coletor_models.Add(oColetor);
                }
            }
            //
            return lista_coletor_models;
        }
        //
        public JsonResult CarregaDados()
        {
            string sret = string.Empty;
            List<ColetorModel> lista_coletor = new List<ColetorModel>();
            //
            try
            {
                lista_coletor = CarregaDadosColetores();
                //
                sret = "ok";
            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
            //
            return Json(new { data = sret, lista_coletor, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult ColetorPost(string descricao, string mac, int idcoletor, int empresa_anterior, int empresa_nova, int alerta)
        {
            string sret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                Coletor oColetor = new Coletor();
                //
                if (idcoletor > 0)
                {
                    oColetor = db.Coletor.Where(a => a.Id_Empresa == empresa_anterior && a.Id == idcoletor).FirstOrDefault();
                    //
                    if (oColetor != null)
                    {
                        oColetor.Descricao = descricao;
                        oColetor.MAC = mac;
                        //
                        if (oColetor.Id_Empresa != empresa_nova)
                        {
                            List<ColetorAlerta> lista_coletor_alerta = db.ColetorAlerta.Where(x => x.Id_Coletor == idcoletor && x.Id_Empresa == oColetor.Id_Empresa).ToList();
                            //
                            foreach (ColetorAlerta item in lista_coletor_alerta)
                            {
                                item.Id_Empresa = empresa_nova;
                                db.Entry(item).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        //
                        oColetor.Id_Empresa = empresa_nova;
                        oColetor.Alerta = alerta;
                        db.Entry(oColetor).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        sret = "Coletor não encontrado!";
                    }
                }
                else
                {
                    oColetor.Descricao = descricao;
                    oColetor.MAC = mac;
                    oColetor.Ativo = true;
                    oColetor.Id_Empresa = empresa_nova;
                    //
                    db.Coletor.Add(oColetor);
                    db.SaveChanges();
                    db.Entry(oColetor).Reload();
                }
                //
                sret = "ok";
            }
            catch (Exception exc)
            {
                erro = exc.Message;
                sret = "erro";
            }
            //
            return Json(new { data = sret, results = 0, success = true, erro }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult ExcluiColetorPost(int idcoletor, int idempresa)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            try
            {
                Coletor oColetor = db.Coletor.Where(a => a.Id == idcoletor && a.Id_Empresa == idempresa).FirstOrDefault();
                //
                if (oColetor != null)
                {
                    var historico_pressao = db.ColetorPressaoHistorico.Where(x => x.Id_Coletor == idcoletor).Any();
                    if (historico_pressao)
                    {
                        ret = "hpres";
                    }
                    var historico_temperatura = db.ColetorTemperaturaHistorico.Where(x => x.Id_Coletor == idcoletor).Any();
                    if (historico_temperatura)
                    {
                        if (string.IsNullOrEmpty(ret))
                            ret = "htemp";
                        else
                            ret += ",htemp";                     
                    }
                    var historico_producao = db.ColetorProducaoHistorico.Where(x => x.Id_Coletor == idcoletor).Any();
                    if (historico_producao)
                    {
                        if (string.IsNullOrEmpty(ret))
                            ret = "hpro";
                        else
                            ret += ",hpro";
                    }
                    //
                    if (string.IsNullOrEmpty(ret))
                    {
                        List<ColetorAlerta> lista_coletor_alerta = db.ColetorAlerta.Where(x => x.Id_Coletor == idcoletor && x.Id_Empresa == oColetor.Id_Empresa).ToList();
                        //
                        foreach (ColetorAlerta item in lista_coletor_alerta)
                        {
                            db.Entry(item).State = EntityState.Deleted;
                            db.SaveChanges();
                        }
                        //
                        db.Entry(oColetor).State = EntityState.Deleted;
                        db.SaveChanges();
                        ret = "ok";
                    }
                }
                else
                {
                    ret = "nao_encontrada";
                }
            }
            catch (Exception exc)
            {
                ret = "erro";
                erro = exc.Message;
            }

            return Json(new { ret, results = 0, erro, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult ExcluiColetorPostHistorico(int idcoletor, int idempresa)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            int total_excl = 0;
            //
            try
            {
                Coletor oColetor = db.Coletor.Where(a => a.Id == idcoletor && a.Id_Empresa == idempresa).FirstOrDefault();
                //
                if (oColetor != null)
                {
                    var historico_pressao = db.ColetorPressaoHistorico.Where(x => x.Id_Coletor == idcoletor).Any();
                    if (historico_pressao)
                    {                        
                        string sql = "DELETE ColetorPressaoHistorico WHERE Id_Coletor = " + idcoletor;
                        SQLController sqlcontroller = new SQLController();
                        total_excl = sqlcontroller.ExecutaSQLNonQuery(sql, out erro);

                    }
                    var historico_temperatura = db.ColetorTemperaturaHistorico.Where(x => x.Id_Coletor == idcoletor).Any();
                    if (historico_temperatura)
                    {
                        string sql = "DELETE ColetorTemperaturaHistorico WHERE Id_Coletor = " + idcoletor;
                        SQLController sqlcontroller = new SQLController();
                        total_excl += sqlcontroller.ExecutaSQLNonQuery(sql, out erro);
                    }
                    var historico_producao = db.ColetorProducaoHistorico.Where(x => x.Id_Coletor == idcoletor).Any();
                    if (historico_producao)
                    {
                        string sql = "DELETE ColetorProducaoHistorico WHERE Id_Coletor = " + idcoletor;
                        SQLController sqlcontroller = new SQLController();
                        total_excl += sqlcontroller.ExecutaSQLNonQuery(sql, out erro);
                    }
                    //
                    if (string.IsNullOrEmpty(erro))
                    {
                        List<ColetorAlerta> lista_coletor_alerta = db.ColetorAlerta.Where(x => x.Id_Coletor == idcoletor && x.Id_Empresa == oColetor.Id_Empresa).ToList();
                        //
                        foreach (ColetorAlerta item in lista_coletor_alerta)
                        {
                            db.Entry(item).State = EntityState.Deleted;
                            db.SaveChanges();
                        }
                        //
                        db.Entry(oColetor).State = EntityState.Deleted;
                        db.SaveChanges();
                        ret = "ok";
                    }
                }
                else
                {
                    ret = "nao_encontrada";
                }
            }
            catch (Exception exc)
            {
                ret = "erro";
                erro = exc.Message;
            }

            return Json(new { ret, results = 0, total_excl, erro, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult PegaColetorTipoAlerta(int idempresa)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            List<ColetorTipoAlertaModel> lista_retorno = new List<ColetorTipoAlertaModel>();
            //
            try
            {
                List<ColetorTipoAlerta> lista_coletor_tipo_alerta = db.ColetorTipoAlerta.Where(a => a.Id_Empresa == idempresa && a.Ativo == true).ToList();
                //
                foreach (var item in lista_coletor_tipo_alerta)
                {
                    ColetorTipoAlertaModel ctam = new ColetorTipoAlertaModel();
                    //
                    ctam.Id = item.Id;
                    ctam.Id_Empresa = item.Id_Empresa;
                    ctam.Ativo = item.Ativo;
                    ctam.UnidadeMedida = item.UnidadeMedida;
                    //
                    if (item.Tipo != null)
                    {
                        switch (item.Tipo.Value)
                        {
                            case 1:
                                ctam.Descricao = "Temperatura (" + item.UnidadeMedida + ")";
                                ctam.Id_Tipo = 1;
                                break;
                            case 2:
                                ctam.Descricao = "Pressão (" + item.UnidadeMedida + ")";
                                ctam.Id_Tipo = 2;
                                break;
                            case 3:
                                ctam.Descricao = "Produção (" + item.UnidadeMedida + ")";
                                ctam.Id_Tipo = 3;
                                break;
                        }
                    }
                    else
                    {
                        ctam.Descricao = "N/A";
                    }
                    //
                    lista_retorno.Add(ctam);
                }
                //
                ret = "ok";
            }
            catch (Exception exc)
            {
                erro = exc.Message;
                ret = "nok";
            }
            //
            return Json(new { ret, lista_retorno, erro, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult PegaColetorAlerta(int idempresa, int idcoletor)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            List<ColetorAlertaModel> lista_retorno = new List<ColetorAlertaModel>();
            //
            try
            {
                List<ColetorAlerta> lista_coletor_alerta = db.ColetorAlerta.Where(a => a.Id_Empresa == idempresa && a.Id_Coletor == idcoletor).ToList();
                //
                foreach (ColetorAlerta item in lista_coletor_alerta)
                {
                    ColetorAlertaModel cam = new ColetorAlertaModel();
                    //
                    cam.Id = item.Id;
                    cam.Id_Empresa = item.Id_Empresa;
                    cam.Id_TipoAlerta = item.Id_TipoAlerta;
                    cam.Ativo = item.Ativo.Value;
                    cam.Id_Coletor = item.Id_Coletor;
                    cam.Regra = item.Regra.Value;
                    cam.Descricao = item.Descricao;
                    cam.Valor = item.Valor;
                    cam.Email = item.Email;
                    //
                    /*
                     
                    switch (item.ColetorTipoAlerta.Tipo.Value)
                    {
                        case 1:
                            cam.Descricao = "Temperatura (" + item.ColetorTipoAlerta.UnidadeMedida + ")";
                            break;
                        case 2:
                            cam.Descricao = "Pressão (" + item.ColetorTipoAlerta.UnidadeMedida + ")";
                            break;
                        case 3:
                            cam.Descricao = "Produção (" + item.ColetorTipoAlerta.UnidadeMedida + ")";
                            break;
                    }                     
                     */
                    //
                    if (item.Ativo.HasValue && item.Ativo.Value == 1)
                    {
                        cam.AtivoDescricao = "Sim";
                    }
                    else
                    {
                        cam.AtivoDescricao = "Não";
                    }
                    //
                    lista_retorno.Add(cam);
                }
                //
                ret = "ok";
            }
            catch (Exception exc)
            {
                erro = exc.Message;
                ret = "nok";
            }
            //
            return Json(new { data = ret, lista_retorno, results = 0, erro, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult SalvaColetorAlerta(int idcoletoralerta, int idempresa, int idcoletor, int idtipoalerta, int idregra, string valor, string email, int ativo)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            string tipoRegraAlerta = string.Empty;
            string stipoalerta = string.Empty;
            string stipoalerta_unidade = string.Empty;
            ColetorTipoAlerta oColetorTipoAlerta = new ColetorTipoAlerta();
            //
            try
            {
                oColetorTipoAlerta = db.ColetorTipoAlerta.Where(x => x.Id_Empresa == idempresa && x.Id == idtipoalerta).FirstOrDefault();
                stipoalerta = oColetorTipoAlerta.Descricao;
                //
                if (oColetorTipoAlerta.Tipo != null)
                {
                    switch (oColetorTipoAlerta.Tipo.Value)
                    {
                        case 1:
                            stipoalerta = "Temperatura ";
                            break;
                        case 2:
                            stipoalerta = "Pressão ";
                            break;
                        case 3:
                            stipoalerta = "Produção ";
                            break;
                    }
                }
                else
                {
                    oColetorTipoAlerta.Descricao = "N/A";
                }
                //
                stipoalerta_unidade = oColetorTipoAlerta.UnidadeMedida;
                //
                switch (idregra)
                {
                    case 1:
                        tipoRegraAlerta = "Maior que ";
                        break;
                    case 2:
                        tipoRegraAlerta = "Menor que ";
                        break;
                    case 3:
                        tipoRegraAlerta = "Igual a ";
                        break;
                    case 4:
                        tipoRegraAlerta = "Maior ou Igual a ";
                        break;
                    case 5:
                        tipoRegraAlerta = "Manor ou Igual a ";
                        break;
                }
                //
                if (idcoletoralerta > 0)
                {
                    ColetorAlerta ca = db.ColetorAlerta.Where(x => x.Id == idcoletoralerta && x.Id_Empresa == idempresa && x.Id_Coletor == idcoletor).FirstOrDefault();
                    //
                    if (ca != null)
                    {
                        ca.Id_TipoAlerta = idtipoalerta;
                        ca.Email = email;
                        ca.Valor = valor;
                        ca.Ativo = ativo;
                        ca.Regra = idregra;
                        ca.Descricao = stipoalerta + " (" + stipoalerta_unidade + ") " + tipoRegraAlerta + valor;
                        //
                        db.Entry(ca).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        ret = "Não foi possível encontrar o alerta.";
                    }
                }
                else
                {
                    ColetorAlerta ca = new ColetorAlerta();
                    //
                    ca.Id_Empresa = idempresa;
                    ca.Id_Coletor = idcoletor;
                    ca.Id_TipoAlerta = idtipoalerta;
                    ca.Email = email;
                    ca.Valor = valor;
                    ca.Ativo = ativo;
                    ca.Regra = idregra;
                    ca.Descricao = stipoalerta + " (" + stipoalerta_unidade + ") " + tipoRegraAlerta + " a " + valor;
                    //
                    db.ColetorAlerta.Add(ca);
                    db.SaveChanges();
                }
                //
                ret = "ok";
            }
            catch (Exception exc)
            {
                erro = exc.Message;
                ret = "nok";
            }
            //
            return Json(new { ret, erro, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult ExcluiColetorAlertaPost(int idalerta, int idcoletor, int idempresa)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            try
            {
                ColetorAlerta oColetorAlerta = db.ColetorAlerta.Where(x => x.Id == idalerta && x.Id_Coletor == idcoletor && x.Id_Empresa == idempresa).FirstOrDefault();
                //
                if (oColetorAlerta != null)
                {
                    string sql = "DELETE ColetorAlertaLog WHERE Id_Coletor = " + idcoletor + " AND Id_ColetorAlerta = " + oColetorAlerta.Id;
                    SQLController sqlcontroller = new SQLController();
                    sqlcontroller.ExecutaSQLNonQuery(sql, out erro);
                    //
                    db.Entry(oColetorAlerta).State = EntityState.Deleted;
                    db.SaveChanges();
                    ret = "ok";
                }
                else
                {
                    ret = "nao_encontrada";
                }
            }
            catch (Exception exc)
            {
                ret = "erro";
                erro = exc.Message;
            }

            return Json(new { ret, results = 0, erro, success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}