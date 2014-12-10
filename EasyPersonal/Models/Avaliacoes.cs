using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EasyPersonal.Models
{
    public abstract class Avaliacoes
    {
        #region Atributos

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public DateTime dataAvaliacao { get; set; }
            public double densidadeCorporal { get; set; }
            public double porcentagemGordura { get; set; }
            public double porcentagemMuscular { get; set; }
            public double altura { get; set; }
            public double pesoAtual { get; set; }
            public double pesoGordo { get; set; }
            public double pesoMagro { get; set; }
            public double pesoOsseo { get; set; }
            public double pesoMuscular { get; set; }
            public double pesoResidual { get; set; }
            public double pesoMinimo { get; set; }
            public double pesoMaximo { get; set; }
            public double excessoPeso { get; set; }
            public double IMC { get; set; }
            public string dataGrafico { get; set; }
            public int aux { get; set; }
        #endregion
    }
}