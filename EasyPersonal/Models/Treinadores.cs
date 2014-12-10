using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EasyPersonal.Models;
using MySql.Data.MySqlClient;
using Connections.Mysql;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Text;



namespace EasyPersonal.Models
{
    public class Treinadores:Pessoa
    {
        #region Atributos

        public static List<Treinadores> listaTreinadores = new List<Treinadores>();
        public int idTreinador { get; set; }
        public int parcelaGerada { get; set; }
        #endregion

        #region Metodos

        public void editarTreinador()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("update treinadores ");
            sql.Append("set nome = @nome, email = @email, cidade = @cidade, idade = @idade, senha = @senha, telefone = @telefone, sexo = @sexo, parcelaGerada = @parcelaGerada, idTreinador = @idTreinador ");
            sql.Append("where idTreinador = @idTreinador ");

            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@nome", this.nome);
            cmm.Parameters.AddWithValue("@email", this.email);
            cmm.Parameters.AddWithValue("@cidade", this.cidade);
            cmm.Parameters.AddWithValue("@idade", this.idade);
            cmm.Parameters.AddWithValue("@senha", this.senha);
            cmm.Parameters.AddWithValue("@telefone", this.telefone);
            cmm.Parameters.AddWithValue("@sexo", this.sexo);
            cmm.Parameters.AddWithValue("@parcelaGerada", this.parcelaGerada);
            cmm.Parameters.AddWithValue("@idTreinador", this.idTreinador);

            cmm.CommandText = sql.ToString();

            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
        }

        public static void getTreinadorEspecifico(Int32 idTreinador)
        {
            Treinadores.listaTreinadores.Clear();
            StringBuilder sql = new StringBuilder();

            sql.Append("select * from treinadores ");
            sql.Append("where idTreinador = @idTreinador ");
            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@idTreinador", idTreinador);

            cmm.CommandText = sql.ToString();

            var dr = Connections.Mysql.MySql.getListaCmm(cmm);

            if (dr.Read())
            {
                Treinadores treinador = new Treinadores();
                treinador.nome = dr.GetString("nome");
                treinador.idTreinador = dr.GetInt32("idTreinador");
                treinador.email = dr.GetString("email");
                treinador.cidade = dr.GetString("cidade");
                treinador.idade = dr.GetInt16("idade");
                treinador.telefone = dr.GetString("telefone");
                treinador.sexo = dr.GetString("sexo");
                treinador.senha = dr.GetString("senha");
                treinador.senha = Treinadores.encripta(treinador.senha);
                treinador.parcelaGerada = dr.GetInt16("parcelaGerada");
               
                Treinadores.listaTreinadores.Add(treinador);
                dr.Dispose();
                Connections.Mysql.MySql.Desconectar();
            }
            else
            {
                Connections.Mysql.MySql.Desconectar();
                dr.Dispose();
            }
        }


        public Treinadores loginTreinador(string email, string senha)
        {
            Treinadores treinadorLogado = new Treinadores();
            string senhaa = senha;
            senha = Treinadores.encripta(senha);
              StringBuilder sql = new StringBuilder();
            sql.Append("select * from treinadores ");
            sql.Append("where email = @email && senha = @senha");
            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@email", email);
            cmm.Parameters.AddWithValue("@senha", senha);
            cmm.CommandText = sql.ToString();
            var n = Connections.Mysql.MySql.getListaCmm(cmm);

        //    var n = Connections.Mysql.MySql.getLista("select * from treinadores where '" + email + "' = email && '" + senha + "' = senha");
            
            if (!n.IsClosed && n.Read())
            {
                treinadorLogado.nome = n.GetString("nome");
                treinadorLogado.idTreinador = n.GetInt32("idTreinador");
                treinadorLogado.email = n.GetString("email");
                treinadorLogado.cidade = n.GetString("cidade");
                treinadorLogado.idade = n.GetInt16("idade");
                treinadorLogado.telefone = n.GetString("telefone");
                treinadorLogado.sexo = n.GetString("sexo");
                treinadorLogado.senha = senhaa;
                treinadorLogado.parcelaGerada = n.GetInt16("parcelaGerada");
            
                Connections.Mysql.MySql.Desconectar(); 
                return treinadorLogado;       
            }
            else
            {
                Connections.Mysql.MySql.Desconectar(); 
                return null;
            }
            
        }

        public static Treinadores getUltimoTreinador()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from treinadores ");
            sql.Append("order by idTreinador desc limit 0,1");
            MySqlCommand cmm = new MySqlCommand();
            cmm.CommandText = sql.ToString();
            var n = Connections.Mysql.MySql.getListaCmm(cmm);
            //var n = Connections.Mysql.MySql.getLista("select * from treinadores order by idTreinador desc limit 0,1");
            if (!n.IsClosed && n.Read())
            {
                Treinadores treinador = new Treinadores();
                treinador.nome = n.GetString("nome");
                treinador.idTreinador = n.GetInt32("idTreinador");
                treinador.email = n.GetString("email");
                treinador.cidade = n.GetString("cidade");
                treinador.idade = n.GetInt16("idade");
                treinador.telefone = n.GetString("telefone");
                treinador.sexo = n.GetString("sexo");
                treinador.senha = n.GetString("senha");
                treinador.parcelaGerada = n.GetInt16("parcelaGerada");
                treinador.senha = Treinadores.encripta(treinador.senha);
                n.Dispose();
                Connections.Mysql.MySql.Desconectar();
               return treinador;
            }
            else
            {
                n.Dispose();
                Connections.Mysql.MySql.Desconectar();
                return null;
            }
           
        }

        public static void addTreinador(Treinadores novo)
        {
            novo.parcelaGerada = DateTime.Now.Month;
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into treinadores (nome,telefone,email,senha,idade,cidade,sexo,parcelaGerada) ");
            sql.Append("values (@nome,@telefone,@email,@senha,@idade,@cidade,@sexo,@parcelaGerada)");
            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@nome", novo.nome);
            cmm.Parameters.AddWithValue("@telefone", novo.telefone);
            cmm.Parameters.AddWithValue("@email", novo.email);
            cmm.Parameters.AddWithValue("@senha", Treinadores.encripta(novo.senha));
            cmm.Parameters.AddWithValue("@idade", novo.idade);
            cmm.Parameters.AddWithValue("@cidade", novo.cidade);
            cmm.Parameters.AddWithValue("@sexo", novo.sexo);
            cmm.Parameters.AddWithValue("@parcelaGerada", novo.parcelaGerada);
            cmm.CommandText = sql.ToString();

            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            cmm.Dispose();

        }

        public static void getTreinadores()
        {
            
            Treinadores.listaTreinadores.Clear();
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from treinadores");
            MySqlCommand cmm = new MySqlCommand();
            cmm.CommandText = sql.ToString();
            var n = Connections.Mysql.MySql.getListaCmm(cmm);
            //var n = Connections.Mysql.MySql.getLista("select * from treinadores");
            
                while (!n.IsClosed && n.Read())
                {
                    Treinadores treinador = new Treinadores();
                    treinador.nome = n.GetString("nome");
                    treinador.idTreinador = n.GetInt32("idTreinador");
                    treinador.email = n.GetString("email");
                    treinador.cidade = n.GetString("cidade");
                    treinador.idade = n.GetInt16("idade");
                    treinador.telefone = n.GetString("telefone");
                    treinador.sexo = n.GetString("sexo");
                    treinador.senha = n.GetString("senha");
                    treinador.parcelaGerada = n.GetInt16("GetInt16");
                    treinador.senha = Treinadores.encripta(treinador.senha);
                    Treinadores.listaTreinadores.Add(treinador);
             
                }
                Connections.Mysql.MySql.Desconectar();
        }
    }
        #endregion
}