using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Security.Cryptography;

namespace EasyPersonal.Models
{
    public abstract class Pessoa
    {
        #region Atributos
        [Required(ErrorMessage = "Obrigatório informar o nome")]
        [StringLength(50, ErrorMessage = "O nome deve possuir no máximo 50 caracteres")]
        public string nome { get; set; }


        [Required(ErrorMessage = "Obrigatório informar o telefone")]
        [StringLength(50, ErrorMessage = "O telefone deve possuir no máximo 50 caracteres")]
        public string telefone { get; set; }


        [Required(ErrorMessage = "Obrigatório informar o e-mail")]
        [StringLength(50, ErrorMessage = "O e-mail deve possuir no máximo 50 caracteres")]
        public string email { get; set; }


        [Required(ErrorMessage = "Obrigatório informar a senha")]
        [StringLength(50, ErrorMessage = "A senha deve possuir no máximo 50 caracteres")]
        [DataType(DataType.Password)]
        public string senha { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a senha")]
        [StringLength(50, ErrorMessage = "A senha deve possuir no máximo 50 caracteres")]
        [DataType(DataType.Password)]
        public string confirmaSenha { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a idade")]
        public int idade { get; set; }


        [Required(ErrorMessage = "Obrigatório informar o cidade")]
        [StringLength(50, ErrorMessage = "O cidade deve possuir no máximo 50 caracteres")]
        public string cidade { get; set; }


        [Required(ErrorMessage = "Obrigatório informar o sexo")]
        public string sexo { get; set; }

        #endregion

        #region Métodos

        public static string encripta(string _senha)
        {
            StringBuilder senha = new StringBuilder();

            MD5 md5 = MD5.Create();
            byte[] entrada = Encoding.ASCII.GetBytes(_senha);
            byte[] hash = md5.ComputeHash(entrada);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                senha.Append(hash[i].ToString("X2"));
            }
            return senha.ToString();
        }
        #endregion
    }

   

}