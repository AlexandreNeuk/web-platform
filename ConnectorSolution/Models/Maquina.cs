//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Connector.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Maquina
    {
        public Maquina()
        {
            this.MaquinaHorario = new HashSet<MaquinaHorario>();
            this.Coletor = new HashSet<Coletor>();
        }
    
        public int ID { get; set; }
        public int Id_Empresa { get; set; }
        public string Descricao { get; set; }
    
        public virtual ICollection<MaquinaHorario> MaquinaHorario { get; set; }
        public virtual ICollection<Coletor> Coletor { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}
