using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class TipoAlertaController : BaseController
    {
        // GET: TioAlerta
        public ActionResult Index()
        {
            ViewBag.FabricaAtivo = "active";
            ViewBag.FabricaTipoAlertaAtivo = "active";
            ViewBag.FabricaShow = "show";
            //
            if (Tipo_Empresa == 1)
            {
                ViewBag.ShowCrudIncluir = "inline";
                ViewBag.ShowCrudEditar = "inline";
                ViewBag.ShowCrudExcluir = "inline";
            }
            else
            {
                ViewBag.ShowCrudIncluir = "none";
                ViewBag.ShowCrudEditar = "none";
                ViewBag.ShowCrudExcluir = "none";
            }
            //
            List<ColetorTipoAlertaModel> lista_tipoalerta_model = new List<ColetorTipoAlertaModel>();
            List<EmpresaModel> lista_empresa_model = PegaEmpresas();
            //
            foreach (EmpresaModel item in lista_empresa_model)
            {
                List<ColetorTipoAlerta> list_coletor_tipo_alerta = db.ColetorTipoAlerta.Where(x => x.Id_Empresa == item.Id).ToList();
                //
                foreach (ColetorTipoAlerta item_ca in list_coletor_tipo_alerta)
                {
                    ColetorTipoAlertaModel ca = new ColetorTipoAlertaModel();
                    //
                    ca.Id = item_ca.Id;
                    ca.Id_Empresa = item_ca.Id_Empresa;
                    ca.Descricao = item_ca.Descricao;
                    ca.UnidadeMedida = item_ca.UnidadeMedida;
                    ca.AtivoGrid = item_ca.Ativo.HasValue ? item_ca.Ativo.Value ? "Sim" : "Não" : "Não";
                    ca.DescricaoEmpresa = item.Nome;
                    //
                    if (item_ca.Tipo != null)
                    {
                        switch (item_ca.Tipo.Value)
                        {
                            case 1:
                                ca.Tipo = "Temperatura";
                                ca.Id_Tipo = 1;
                                break;
                            case 2:
                                ca.Tipo = "Pressão";
                                ca.Id_Tipo = 2;
                                break;
                            case 3:
                                ca.Tipo = "Produção";
                                ca.Id_Tipo = 3;
                                break;
                        }
                    }
                    else
                    {
                        ca.Tipo = "N/A";
                    }
                    //
                    lista_tipoalerta_model.Add(ca);
                }
            }
            //
            List<TipoAlertaOpcaoesModel> lista_tipo_alerta_op_model = new List<TipoAlertaOpcaoesModel>();
            TipoAlertaOpcaoesModel tao = new TipoAlertaOpcaoesModel();
            tao.Id = 1;
            tao.Descricao = "Temperatura";
            lista_tipo_alerta_op_model.Add(tao);
            //
            tao = new TipoAlertaOpcaoesModel();
            tao.Id = 2;
            tao.Descricao = "Pressão";
            lista_tipo_alerta_op_model.Add(tao);
            //
            tao = new TipoAlertaOpcaoesModel();
            tao.Id = 3;
            tao.Descricao = "Produção";
            lista_tipo_alerta_op_model.Add(tao);
            //
            //tao = new TipoAlertaOpcaoesModel();
            //tao.Id = 4;
            //tao.Descricao = "Movimentação";
            //lista_tipo_alerta_op_model.Add(tao);
            //
            ViewBag.ListaEmpresas = lista_empresa_model;
            ViewBag.ListaTipoAlerta = lista_tipoalerta_model;
            ViewBag.ListaTipoAlertaOpcoes = lista_tipo_alerta_op_model;
            //
            return View();
        }
        //
        public JsonResult TipoAlertaPost(int id_tipoalerta, int id_empresa, string descricao, string unidade_medida, int ativo, int tipo)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                ColetorTipoAlerta cta = new ColetorTipoAlerta();
                //
                if (id_tipoalerta > 0)
                {
                    cta = db.ColetorTipoAlerta.Where(a => a.Id == id_tipoalerta && a.Id_Empresa == id_empresa).FirstOrDefault();
                    //
                    if (cta != null)
                    {
                        cta.Descricao = descricao;
                        cta.UnidadeMedida = unidade_medida;
                        cta.Ativo = ativo == 1 ? true : false;
                        cta.Tipo = tipo;
                        db.Entry(cta).State = EntityState.Modified;
                        db.SaveChanges();
                        ret = "ok";
                    }
                    else
                    {
                        ret = "Tipo Alerta não encontrado: TipoAlerta (" + id_tipoalerta + ") - Empresa (" + id_empresa + ")";
                    }
                }
                else
                {
                    cta.Id_Empresa = id_empresa;
                    cta.Descricao = descricao;
                    cta.UnidadeMedida = unidade_medida;
                    cta.Ativo = ativo == 1 ? true : false;
                    cta.Tipo = tipo;
                    db.ColetorTipoAlerta.Add(cta);
                    db.SaveChanges();
                    db.Entry(cta).Reload();
                    ret = "ok";
                }
            }
            catch (Exception exc)
            {
                erro = exc.Message;
                ret = "nok";
            }
            //
            return Json(new { ret, results = 0, success = true, erro }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult ExcluiTipoAlerta(int idtipoalerta, int idempresa)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                ColetorAlerta ca = db.ColetorAlerta.Where(x => x.Id_TipoAlerta == idtipoalerta && x.Id_Empresa == idempresa).FirstOrDefault();
                if (ca != null)
                {
                    ret = "coletoralerta";
                }
                else
                {
                    ColetorTipoAlerta ctp = db.ColetorTipoAlerta.Where(x => x.Id == idtipoalerta && x.Id_Empresa == idempresa).FirstOrDefault();
                    if (ctp != null)
                    {
                        db.Entry(ctp).State = EntityState.Deleted;
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
                ret = "erro";
                erro = exc.Message;
            }
            //
            return Json(new { ret, results = 0, success = true, erro }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult PegaTipoasAlertas()
        {
            string ret = string.Empty;
            string erro = string.Empty;
            List<ColetorTipoAlertaModel> lista_tipoalerta_model = new List<ColetorTipoAlertaModel>();
            //
            try
            {
                List<EmpresaModel> lista_empresa_model = PegaEmpresas();
                //
                foreach (EmpresaModel item in lista_empresa_model)
                {
                    List<ColetorTipoAlerta> list_coletor_tipo_alerta = db.ColetorTipoAlerta.Where(x => x.Id_Empresa == item.Id).ToList();
                    //
                    foreach (ColetorTipoAlerta item_ca in list_coletor_tipo_alerta)
                    {
                        ColetorTipoAlertaModel ctam = new ColetorTipoAlertaModel();
                        //
                        ctam.Id = item_ca.Id;
                        ctam.Id_Empresa = item_ca.Id_Empresa;
                        ctam.Descricao = item_ca.Descricao;
                        ctam.UnidadeMedida = item_ca.UnidadeMedida;
                        ctam.AtivoGrid = item_ca.Ativo.HasValue ? item_ca.Ativo.Value ? "Sim" : "Não" : "Não";
                        ctam.DescricaoEmpresa = item.Nome;
                        //
                        if (item_ca.Tipo != null)
                        {
                            switch (item_ca.Tipo.Value)
                            {
                                case 1: ctam.Tipo = "Temperatura"; break;
                                case 2: ctam.Tipo = "Pressão"; break;
                                case 3: ctam.Tipo = "Produção"; break;
                            }
                        }
                        else
                        {
                            ctam.Descricao = "N/A";
                        }
                        //
                        lista_tipoalerta_model.Add(ctam);
                    }
                }
                //
                ret = "ok";
            }
            catch (Exception exc)
            {
                ret = "nok";
                erro = exc.Message;
            }
            //
            return Json(new { lista_retonro = lista_tipoalerta_model, ret, results = 0, success = true, erro }, JsonRequestBehavior.AllowGet);
        }
    }
}