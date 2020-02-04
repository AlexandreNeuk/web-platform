using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Connector.Models
{
    public class ImagemGraficoModel
    {
        public string Descricao { get; set; }
        public string Temp { get; set; }
        public Byte[] Imagem { get; set; }
    }
}