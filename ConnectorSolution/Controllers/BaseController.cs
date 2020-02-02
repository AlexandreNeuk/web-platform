using Connector.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Connector.Controllers
{
    public class BaseController : Controller
    {
        private int _cd_empresa;
        private int _cd_usuario;
        private int _tipo_empresa;
        private string _nome_empresa;
        private string _nome_usuario;
        private string _email_usuario;
        public dbConnectorEntities db = new dbConnectorEntities();
        //
        public int Codigo_Empresa
        {
            get
            {
                if (Session != null && Session["cd_empresa"] != null)
                {
                    try
                    {
                        this._cd_empresa = Convert.ToInt32(Session["cd_empresa"]);
                    }
                    catch (Exception exc)
                    {

                    }
                }
                return this._cd_empresa;
            }
            set
            {
                this._cd_empresa = value;
            }
        }
        //
        public int Codigo_Usuario
        {
            get
            {
                if (Session != null && Session["cd_usuario"] != null)
                {
                    try
                    {
                        this._cd_usuario = Convert.ToInt32(Session["cd_usuario"]);
                    }
                    catch (Exception exc)
                    {

                    }
                }
                return this._cd_usuario;
            }
            set
            {
                this._cd_usuario = value;
            }
        }
        //
        public int Tipo_Empresa
        {
            get
            {
                if (Session != null && Session["tipo_empresa"] != null)
                {
                    try
                    {
                        this._tipo_empresa = Convert.ToInt32(Session["tipo_empresa"]);
                    }
                    catch (Exception exc)
                    {

                    }
                }
                return this._tipo_empresa;
            }
            set
            {
                this._tipo_empresa = value;
            }
        }
        //
        public string Nome_Empresa
        {
            get
            {
                if (Session != null && Session["nome_empresa"] != null)
                {
                    try
                    {
                        this._nome_empresa = Session["nome_empresa"].ToString();
                    }
                    catch (Exception exc)
                    {

                    }
                }
                return this._nome_empresa;
            }
            set
            {
                this._nome_empresa = value;
            }
        }
        //
        public string Nome_Usuario
        {
            get
            {
                if (Session != null && Session["nome_usuario"] != null)
                {
                    try
                    {
                        this._nome_usuario = Session["nome_usuario"].ToString();
                    }
                    catch (Exception exc)
                    {

                    }
                }
                return this._nome_usuario;
            }
            set
            {
                this._nome_usuario = value;
            }
        }
        //
        public string Email_Usuario
        {
            get
            {
                if (Session != null && Session["email"] != null)
                {
                    try
                    {
                        this._email_usuario = Session["email"].ToString();
                    }
                    catch (Exception exc)
                    {
                
                    }
                }
                return this._email_usuario;
            }
            set
            {
                this._email_usuario = value;
            }
        }
        //
        public DateTime getData()
        {
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
            return new DateTime(ano, mes, dia, hora, min, seg).AddHours(1);
        }
        //
        public string getConString()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnectorEntities"].ConnectionString;
            string constr = "server=" + connectionString.Split('=')[4].Split(';')[0] +
                ";database=" + connectionString.Split('=')[5].Split(';')[0] +
                ";user id=" + connectionString.Split('=')[7].Split(';')[0] +
                ";password=" + connectionString.Split('=')[8].Split(';')[0];
            //
            return constr;
        }
        //
        public List<EmpresaModel> PegaEmpresas()
        {
            Empresa empresa = db.Empresa.Where(x => x.Id == Codigo_Empresa).FirstOrDefault();
            List<EmpresaModel> lista_empresas_model = new List<EmpresaModel>();
            //
            if (empresa != null)
            {
                EmpresaModel empresa_model = new EmpresaModel();
                empresa_model.Id = empresa.Id;
                empresa_model.Nome = empresa.Nome;
                lista_empresas_model.Add(empresa_model);
                //
                string empresas = empresa.Empresas;
                //
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
                            lista_empresas_model.Add(empresa_model);

                            lista_sub_maquinas = db.Maquina.Where(x => x.Id_Empresa == sub_empresa.Id).ToList();
                        }
                    }
                }
            }
            //
            return lista_empresas_model;
        }
        //
        public string FormataDataHoraTela(DateTime? dt)
        {
            string ret = string.Empty;
            //
            try
            {
                DateTime data = Convert.ToDateTime(dt);
                //
                ret = (data.Day > 9 ? data.Day.ToString() : "0" + data.Day) + "/" +
                    (data.Month > 9 ? data.Month.ToString() : "0" + data.Month) + "/" +
                    data.Year + " - " +
                    (data.Hour > 9 ? data.Hour.ToString() : "0" + data.Hour) + ":" +
                    (data.Minute > 9 ? data.Minute.ToString() : "0" + data.Minute) + ":" +
                    (data.Second > 9 ? data.Second.ToString() : "0" + data.Second);
            }
            catch (Exception exc)
            {
                ret = "Err: Dt Convert";
            }
            //
            return ret;
        }
        //
        public string PegaEstadoSigla(string estado)
        {
            string ret = string.Empty;
            //
            switch (estado)
            {
                case "1": ret = "AC"; break;
                case "2": ret = "AL"; break;
                case "3": ret = "AP"; break;
                case "4": ret = "AM"; break;
                case "5": ret = "BA"; break;
                case "6": ret = "CE"; break;
                case "7": ret = "DF"; break;
                case "8": ret = "ES"; break;
                case "9": ret = "GO"; break;
                case "10": ret = "MA"; break;
                case "11": ret = "MT"; break;
                case "12": ret = "MS"; break;
                case "13": ret = "MG"; break;
                case "14": ret = "PA"; break;
                case "15": ret = "PB"; break;
                case "16": ret = "PR"; break;
                case "17": ret = "PE"; break;
                case "18": ret = "PI"; break;
                case "19": ret = "RJ"; break;
                case "20": ret = "RN"; break;
                case "21": ret = "RS"; break;
                case "22": ret = "RO"; break;
                case "23": ret = "RR"; break;
                case "24": ret = "SC"; break;
                case "25": ret = "SP"; break;
                case "26": ret = "SE"; break;
                case "27": ret = "TO"; break;
            }
            //
            return ret;
        }
    }
}