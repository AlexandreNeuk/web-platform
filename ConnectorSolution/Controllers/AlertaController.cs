using Connector.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Connector.Models.Enum;

namespace Connector.Controllers
{
    public class AlertaController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.AlertasAtivo = "active";
            List<Maquina> lista_maquina = new List<Maquina>();
            //
            if (Tipo_Empresa == 1)
            {
                List<EmpresaModel> list_empresas = PegaEmpresas();
                List<Maquina> lista_maquina_temp = new List<Maquina>();
                //
                foreach (EmpresaModel item in list_empresas)
                {
                    lista_maquina_temp = db.Maquina.Where(a => a.Id_Empresa == item.Id).ToList();
                    lista_maquina.AddRange(lista_maquina_temp);
                }
            }
            else
            {
                lista_maquina = db.Maquina.Where(a => a.Id_Empresa == Codigo_Empresa).ToList();
            }
            //
            ViewBag.ListaMaquinas = lista_maquina;
            ViewBag.ListaAlertas = PegaAlertas();
            return View();
        }
    

        public List<ColetorAlertaLogModel> PegaAlertas()
        {
            List<ColetorAlertaLogModel> lsta_retorno = new List<ColetorAlertaLogModel>();
            List<ColetorAlertaLog> lista_coletor_alerta = new List<ColetorAlertaLog>();
            //
            if (Tipo_Empresa == 1)
            {
                List<EmpresaModel> list_empresas = PegaEmpresas();
                List<ColetorAlertaLog> lista_coletor_alerta_temp = new List<ColetorAlertaLog>();
                //
                foreach (EmpresaModel item in list_empresas)
                {
                    lista_coletor_alerta_temp = db.ColetorAlertaLog.Where(a => a.Coletor.Id_Empresa  == item.Id).ToList();
                    lista_coletor_alerta.AddRange(lista_coletor_alerta_temp);
                }
            }
            else
            {
                lista_coletor_alerta = db.ColetorAlertaLog.Where(a => a.Coletor.Id_Empresa == Codigo_Empresa).ToList();
            }
            //
            foreach (ColetorAlertaLog item in lista_coletor_alerta)
            {
                ColetorAlertaLogModel calm = new ColetorAlertaLogModel();
                //
                calm.Id = item.Id;
                calm.NomeColetor = item.Coletor.Descricao;
                calm.NomeMaquina = item.Coletor.Maquina.Descricao;
                calm.NomeEmpresa = item.Coletor.Empresa.Nome;
                calm.DataHora = item.DataHora;
                calm.DataHoraDesc = FormataDataHoraTela(item.DataHora);
                string desc = string.Empty;
                string descregra = string.Empty;
                string descunidade = string.Empty;
                //
                if (item.ColetorAlerta.ColetorTipoAlerta != null)
                {
                    switch (item.ColetorAlerta.Regra)
                    {
                        case 1:
                            descregra = " maior que ";
                            break;
                        case 2:
                            descregra = " menor que ";
                            break;
                        case 3:
                            descregra = " igual a ";
                            break;
                        case 4:
                            descregra = " maior ou igual que ";
                            break;
                        case 5:
                            descregra = " menor ou igual que ";
                            break;
                    }
                    switch (Convert.ToInt32(item.ColetorAlerta.ColetorTipoAlerta.Tipo))
                    {
                        case 1: // temperatura
                            descunidade = "temperatura";
                            break;
                        case 2: // presão
                            descunidade = "pressão";
                            break;
                        case 3: // produção
                            descunidade = "produção";
                            break;
                    }
                    //
                    desc = "A " +
                            descunidade + 
                            " " +
                            item.ValorEnviado +
                            " " +
                            item.ColetorAlerta.ColetorTipoAlerta.UnidadeMedida +
                            " é " +
                            descregra +
                            item.ValorRegra +
                            " " +
                            item.ColetorAlerta.ColetorTipoAlerta.UnidadeMedida;
                }
                //
                calm.Descricao = desc;
                //
                lsta_retorno.Add(calm);
            }
            //
            return lsta_retorno;
        }
    }
}