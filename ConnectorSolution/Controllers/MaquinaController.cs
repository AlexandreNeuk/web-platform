using Connector.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class MaquinaController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.FabricaAtivo = "active";
            ViewBag.FabricaMaquinaAtivo = "active";
            ViewBag.FabricaShow = "show";
            List<EmpresaModel> lista_empresas = new List<EmpresaModel>();
            List<Maquina> lista_maquinas = new List<Maquina>();
            List<Medidor_MaquinaModels> lista_maquinas_models = new List<Medidor_MaquinaModels>();
            List<Coletor> lista_coletor = new List<Coletor>();
            //
            if (Codigo_Empresa > 0)
            {
                
                lista_maquinas = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
                //
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
                            List<Maquina> lista_sub_maquinas = new List<Maquina>();
                            foreach (Empresa sub_empresa in lista_sub_empresas)
                            {
                                empresa_model = new EmpresaModel();
                                empresa_model.Id = sub_empresa.Id;
                                empresa_model.Nome = sub_empresa.Nome;
                                lista_empresas.Add(empresa_model);

                                lista_sub_maquinas = db.Maquina.Where(x => x.Id_Empresa == sub_empresa.Id).ToList();
                                lista_maquinas.AddRange(lista_sub_maquinas);
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
                foreach (Maquina item in lista_maquinas)
                {
                    Medidor_MaquinaModels novo = new Medidor_MaquinaModels();
                    novo.ID = item.ID;
                    novo.Id_Empresa = item.Id_Empresa;
                    novo.Descricao = item.Descricao;
                    novo.DescricaoEmpresa = item.Empresa.Nome;
                    //
                    Coletor oColetor = db.Coletor.Where(a => a.Maquina.Id_Empresa == item.Empresa.Id && a.Id_Maquina == item.ID).FirstOrDefault();
                    //
                    if (oColetor != null)
                    {
                        novo.DescricaoMedidor = oColetor.Descricao;
                        novo.Id_Coletor = oColetor.Id;
                    }
                    //
                    lista_maquinas_models.Add(novo);
                }
                //
                lista_coletor = db.Coletor.Where(a => a.Id_Empresa == Codigo_Empresa && a.Id_Maquina == null).ToList();
                //
                Coletor coletor_vazio = new Coletor();
                coletor_vazio.Id = 0;
                coletor_vazio.Descricao = "N/A";
                lista_coletor.Add(coletor_vazio);
                lista_coletor = lista_coletor.OrderBy(a => a.Id).ToList();
                lista_maquinas_models = lista_maquinas_models.OrderBy(x => x.ID).ToList();
                //
                ViewBag.ListaEmpresas = lista_empresas;
                ViewBag.ListaMaquinas = lista_maquinas_models;
                ViewBag.ListaColetor = lista_coletor;
            }
            //
            return View();
        }
        //
        public JsonResult CarregaDados()
        {
            string sret = string.Empty;
            List<Medidor_MaquinaModels> lista_retorno = new List<Medidor_MaquinaModels>();
            List<EmpresaModel> lista_empresas = new List<EmpresaModel>();
            List<Maquina> lista_maquinas = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            //
            try
            {
                if (Tipo_Empresa == 1)
                {
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
                            List<Maquina> lista_sub_maquinas = new List<Maquina>();
                            foreach (Empresa sub_empresa in lista_sub_empresas)
                            {
                                empresa_model = new EmpresaModel();
                                empresa_model.Id = sub_empresa.Id;
                                empresa_model.Nome = sub_empresa.Nome;
                                lista_empresas.Add(empresa_model);

                                lista_sub_maquinas = db.Maquina.Where(x => x.Id_Empresa == sub_empresa.Id).ToList();
                                lista_maquinas.AddRange(lista_sub_maquinas);
                            }
                        }
                    }
                }
                //
                foreach (Maquina item in lista_maquinas)
                {
                    Medidor_MaquinaModels novo = new Medidor_MaquinaModels();
                    novo.ID = item.ID;
                    novo.Id_Empresa = item.Id_Empresa;
                    novo.Descricao = item.Descricao;
                    novo.DescricaoEmpresa = item.Empresa.Nome;
                    //
                    Coletor oColetor = db.Coletor.Where(a => a.Maquina.Id_Empresa == item.Empresa.Id && a.Id_Maquina == item.ID).FirstOrDefault();
                    //
                    if (oColetor != null)
                    {
                        novo.DescricaoMedidor = oColetor.Descricao;
                        novo.Id_Coletor = oColetor.Id;
                    }
                    //
                    lista_retorno.Add(novo);
                }
                //
                sret = "ok";
            }
            catch (Exception exc)
            {
                sret = exc.Message;
            }
            //
            lista_retorno = lista_retorno.OrderBy(x => x.ID).ToList();
            //
            return Json(new { data = sret, lista_retorno, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult PegaColetores(int empresa)
        {
            string ret = string.Empty;
            List<ColetorModel> lista_retorno = new List<ColetorModel>();
            //
            try
            {
                List<Coletor> lista_coletores = db.Coletor.Where(a => a.Id_Empresa == empresa && a.Id_Maquina == null).ToList();
                //
                foreach (Coletor item in lista_coletores)
                {
                    ColetorModel cm = new ColetorModel();
                    //
                    cm.Id = item.Id;
                    cm.Descricao = item.Descricao;
                    //
                    lista_retorno.Add(cm);
                }
                //
                ret = "ok";
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            //
            return Json(new { ret, lista_retorno, results = 1, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult ExcluiMaquinaPost(int idmaquina)
        {
            string ret = string.Empty;
            try
            {
                Maquina maquina = db.Maquina.Where(a => a.ID == idmaquina).FirstOrDefault();
                db.Entry(maquina).State = EntityState.Deleted;
                Coletor oColetor = db.Coletor.Where(a => a.Id_Maquina == idmaquina).FirstOrDefault();
                //
                if (oColetor != null)
                {
                    oColetor.Id_Maquina = null;
                    db.Entry(oColetor).State = EntityState.Modified;
                }
                //
                db.SaveChanges();
                ret = "ok";
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }

            return Json(new { ret, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        public JsonResult MaquinaPost(string descricao, int id_coletor, int id_maquina, int id_empresa_nova, int id_empresa_ant)
        {
            string ret = string.Empty;
            string erro = string.Empty;
            //
            try
            {
                Maquina mm = new Maquina();
                //
                if (id_maquina > 0)
                {
                    mm = db.Maquina.Where(a => a.Id_Empresa == id_empresa_ant && a.ID == id_maquina).FirstOrDefault();
                    //
                    if (mm != null)
                    {
                        mm.Id_Empresa = id_empresa_nova;
                        mm.Descricao = descricao;
                        db.Entry(mm).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        ret = "Máquina não encontrada: Empresa (" + id_empresa_ant  + ") - Máquina (" + id_maquina + ")";
                    }
                }
                else
                {
                    mm.Descricao = descricao;
                    mm.Id_Empresa = id_empresa_nova;
                    db.Maquina.Add(mm);
                    db.SaveChanges();
                    db.Entry(mm).Reload();
                }             
                //
                try
                {
                    Coletor coletor_temp = db.Coletor.Where(a => a.Id_Maquina == mm.ID && a.Id_Empresa == id_empresa_ant).FirstOrDefault();
                    //
                    if (coletor_temp != null)
                    {
                        coletor_temp.Id_Maquina = null;
                        db.Entry(coletor_temp).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    //
                    if (id_coletor > 0) // 0 = N/A não se aplica
                    {
                        Coletor oColetor = null;

                        if (id_empresa_nova == id_empresa_ant)
                        {
                            oColetor = db.Coletor.Where(a => a.Id == id_coletor && a.Id_Empresa == id_empresa_ant).FirstOrDefault();
                        }
                        else
                        {
                            oColetor = db.Coletor.Where(a => a.Id == id_coletor && a.Id_Empresa == id_empresa_nova).FirstOrDefault();
                        }                        
                        //
                        if (oColetor != null)
                        {
                            oColetor.Id_Maquina = mm.ID;
                            db.Entry(oColetor).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    //
                    ret = "ok";
                }
                catch (Exception exc3)
                {
                    erro = exc3.Message;
                    ret = "nok";
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
        public JsonResult PegaColetorMaquina(int idmaquina, int idempresa)
        {
            string data = string.Empty;
            int id = 0;
            string desc = string.Empty;
            Coletor oColetor = new Coletor();
            //
            try
            {
                if (idempresa > 0)
                {
                    oColetor = db.Coletor.Where(a => a.Maquina.Id_Empresa == idempresa && a.Id_Maquina == idmaquina).FirstOrDefault();
                    //
                    if (oColetor != null)
                    {
                        desc = oColetor.Descricao;
                        id = oColetor.Id;
                    }
                    data = "ok";
                }
            }
            catch (Exception exc)
            {
                data = "nok";
            }
            //
            return Json(new { data = data, desc = desc, id = id, results = 0, success = true }, JsonRequestBehavior.AllowGet);
        }
        //
        //public JsonResult pegadadosmaquina(int idmaquina, string periodo)
        //{
        //    List<data_novo> datas = new List<data_novo>();
        //    DateTime data_filtro_um = new DateTime();
        //    DateTime data_filtro_dois = new DateTime();
        //    //
        //    if (!string.IsNullOrEmpty(periodo))
        //    {
        //        string[] datas_filro = periodo.Split('-');
        //        string[] data_um = datas_filro[0].ToString().Split('/');
        //        string[] data_dois = datas_filro[1].ToString().Split('/');
        //        //
        //        data_filtro_um = new DateTime(Convert.ToInt32(data_um[2]), Convert.ToInt32(data_um[0]), Convert.ToInt32(data_um[1]), 0, 0, 1);
        //        data_filtro_dois = new DateTime(Convert.ToInt32(data_dois[2]), Convert.ToInt32(data_dois[0]), Convert.ToInt32(data_dois[1]), 23, 59, 59);
        //    }
        //    //
        //    DateTime dd = DateTime.Now;
        //    List<MedidorHistorico> lista_med = db.MedidorHistorico.Where(a => a.Id_Mac == idmaquina &&
        //                                                                              a.DataHora.Value >= data_filtro_um &&
        //                                                                              a.DataHora.Value <= data_filtro_dois)
        //                                                                              .ToList();
        //    //
        //    if (lista_med.Count > 0)
        //    {
        //        data_novo data_novo = new data_novo();
        //        foreach (MedidorHistorico item in lista_med)
        //        {
        //            data_novo = new data_novo();
        //            data_novo.data = item.DataHora.Value.ToShortDateString();
        //            if (!string.IsNullOrEmpty(item.Potencia))
        //            {
        //                data_novo.valor = Convert.ToInt32(item.Potencia);
        //            }
        //            //
        //            datas.Add(data_novo);
        //        }

        //    }
        //    //
        //    return Json(new { data = datas, results = 1, success = true }, JsonRequestBehavior.AllowGet);
        //}
    }
}