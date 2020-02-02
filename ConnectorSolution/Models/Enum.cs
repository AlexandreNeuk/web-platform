using Connector.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connector.Models
{
    public class Enum : BaseController
    {
        public enum TipoRegraAlerta
        {
            [Description("Maior")]
            Maior = 1,
            [Description("Menor")]
            Menor = 2,
            [Description("Igual")]
            Igual = 3,
            [Description("Maior ou Igual")]
            MaiorOuIgual = 4,
            [Description("Menor ou Igual")]
            MenorOuIgual = 5
        }

        public enum TipoColetorAlerta
        {
            [Description("Temperatura")]
            Temperatura = 1,
            [Description("Pressão")]
            Pressao = 2,
            [Description("Produção")]
            Producao = 3,
            [Description("Movimentaçãoo")]
            Movimentacao = 4
        }
    }
}