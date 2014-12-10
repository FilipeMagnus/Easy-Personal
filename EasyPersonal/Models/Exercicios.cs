using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EasyPersonal.Models
{
    public class Exercicios
    {
        #region Atributos
            public int idExercicio { get; set; }

            [Required(ErrorMessage = "Obrigatório informar o nome")]
            [StringLength(50, ErrorMessage = "O nome deve possuir no máximo 50 caracteres")]
            public string nome { get; set; }

            [StringLength(50, ErrorMessage = "O grupo muscular deve possuir no máximo 50 caracteres")]
            public string grupo { get; set; }

            [StringLength(50, ErrorMessage = "O equipamento deve possuir no máximo 50 caracteres")]
            public string equipamento { get; set; }

        #endregion

    }
}