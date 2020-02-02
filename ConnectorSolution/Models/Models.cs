using System;
using System.Collections.Generic;

namespace Connector.Models
{
    public class Medidor_MaquinaModels
    {
        public int ID { get; set; }
        public int Id_Empresa { get; set; }
        public int Id_Coletor { get; set; }
        public string Descricao { get; set; }
        public string DescricaoMedidor { get; set; }
        public string DescricaoEmpresa { get; set; }
    }

    public class ret
    {
        public int draw = 0;
        public int recordsTotal = 0;
        public int recordsFiltered = 0;
        public List<Medidor_MaquinaModels> data = new List<Medidor_MaquinaModels>();
    }

    public class data
    {
        public string Age = "";
        public string Name = "";
        public string DoB = "";
    }

    public class maquina_horaios
    {
        public string dia = "";
        public string horaon = "";
        public string horafim = "";
        public bool ativo = false;
    }

    public class data_novo
    {
        public string data = "";
        public int valor = 0;
    }

    public class DataGrid
    {
        public string y { get; set; }
        public int a { get; set; }
        public int b { get; set; }
        public int c { get; set; }
    }

    public class ListaTable
    {
        public List<string> row = new List<string>();
    }

    public class LogAtividadeModel
    {
        public int Id { get; set; }
        public int? Id_Empresa { get; set; }
        public string Id_Dispositivo { get; set; }
        public string Descricao { get; set; }
        public string NomeMaquina { get; set; }
        public string NomeColetor { get; set; }
        public string NomeEmpresa { get; set; }
        public string Tipo { get; set; }
        public string DataHoraDesc { get; set; }
        public string Imagem { get; set; }
        public DateTime DataHora { get; set; }
    }

    public class ColetorModel
    {
        public int Id { get; set; }
        public int Id_Empresa { get; set; }
        public string Descricao { get; set; }
        public string Empresa { get; set; }
        public string Resumo { get; set; }
        public Nullable<bool> Ativo { get; set; }
        public string MAC { get; set; }
        public string Maquina { get; set; }
        public string TotalAlertas { get; set; }
        public string PossuiAlerta { get; set; }
        public Nullable<int> Id_Maquina { get; set; }
        public Nullable<int> Alerta { get; set; }
    }

    public class ProgramaModel
    {
        public int Id { get; set; }
        public Nullable<int> Id_Empresa { get; set; }
        public Nullable<int> Id_Processo { get; set; }
        public string Descricao { get; set; }
        public string EmpresaDesc { get; set; }


        public Empresa Empresa { get; set; }
        public Processo Processo { get; set; }
    }

    public class EmpresaModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Resumo { get; set; }
        public Nullable<int> Tipo { get; set; }
        public string Empresas { get; set; }
        public string URL { get; set; }
        public string AnalitycCode { get; set; }
        public string Site { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string EstadoSigla { get; set; }
        public string Telefone { get; set; }
        public string NomeFantasia { get; set; }
        public string Email { get; set; }
        public bool UsuarioAtivo { get; set; }
        public bool ColetorAtivo { get; set; }
        public bool MaquinaAtiva { get; set; }
        public string Bairro { get; set; }
    }

    public class EmpresaRelatorioModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Site { get; set; }
        public string Endereco { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public string Cidade { get; set; }
        public string EstadoSigla { get; set; }
        public string Telefone { get; set; }
        public string NomeFantasia { get; set; }
        public string Email { get; set; }
        public string Bairro { get; set; }
    }

    public class GatewayModel
    {
        public int Id { get; set; }
        public int Id_Empresa { get; set; }
        public string Descricao { get; set; }
        public string Resumo { get; set; }
        public Nullable<bool> Ativa { get; set; }
        public string MAC { get; set; }
        public string Maquina { get; set; }
        public Nullable<int> Id_Maquina { get; set; }
    }

    public class MedidorHistoricoModel
    {
        public int Id { get; set; }
        public int Id_Mac { get; set; }
        public Nullable<System.DateTime> DataHora { get; set; }
        public string Pressao { get; set; }
    }

    public class ColetorTipoAlertaModel
    {
        public int Id { get; set; }
        public int Id_Empresa { get; set; }
        public string Descricao { get; set; }
        public string UnidadeMedida { get; set; }
        public string DescricaoEmpresa { get; set; }
        public string AtivoGrid { get; set; }
        public string Tipo { get; set; }
        public int Id_Tipo { get; set; }
        public Nullable<bool> Ativo { get; set; }
        public virtual Empresa Empresa { get; set; }
    }

    public class ColetorTipoAlertaGridModel
    {
        public int Id { get; set; }
        public int Id_Empresa { get; set; }
        public string Descricao { get; set; }
        public string UnidadeMedida { get; set; }
        public Nullable<bool> Ativo { get; set; }
        public virtual Empresa Empresa { get; set; }
    }

    public class MaquinaItensReportModel
    {
        public string DescricaoMaquina { get; set; }
        public string DescricaoColetor { get; set; }
    }

    //public partial class ColetorAlertaLogModel
    //{
    //    public int Id { get; set; }
    //    public int Id_Coletor { get; set; }
    //    public int Id_ColetorAlerta { get; set; }
    //    public Nullable<System.DateTime> DataHora { get; set; }
    //    public string ValorRegra { get; set; }
    //    public string ValorEnviado { get; set; }

    //    public virtual Coletor Coletor { get; set; }
    //    public virtual ColetorAlerta ColetorAlerta { get; set; }
    //}

    public class ColetorAlertaModel
    {
        public int Id { get; set; }
        public int Id_Empresa { get; set; }
        public int Id_Coletor { get; set; }
        public int Id_TipoAlerta { get; set; }
        public string DescricaoTipoAlerta { get; set; }
        public int Prioridade { get; set; }
        public string Descricao { get; set; }
        public string DescricaoColetor { get; set; }
        public string AtivoDescricao { get; set; }
        public string Email { get; set; }
        public int Regra { get; set; }
        public string Valor { get; set; }
        public int Ativo { get; set; }
    }

    public class UsuarioModel
    {
        public int ID { get; set; }
        public int Id_Empresa { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string EmpresaNome { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<System.DateTime> Last { get; set; }
        public Nullable<System.DateTime> Create { get; set; }
        public Nullable<int> Ative { get; set; }
        public string Hash { get; set; }
        public string Nome { get; set; }
        public Nullable<int> Tipo { get; set; }
    }

    public class TipoAlertaOpcaoesModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
    }

    public partial class ColetorAlertaLogModel
    {
        public int Id { get; set; }
        public int Id_Coletor { get; set; }
        public int Id_ColetorAlerta { get; set; }
        public Nullable<System.DateTime> DataHora { get; set; }
        public string ValorRegra { get; set; }
        public string ValorEnviado { get; set; }
        public string NomeMaquina { get; set; }
        public string NomeColetor { get; set; }
        public string Descricao { get; set; }
        public string DataHoraDesc { get; set; }
        public string NomeEmpresa { get; set; }
        public virtual Coletor Coletor { get; set; }
        public virtual ColetorAlerta ColetorAlerta { get; set; }
    }
}