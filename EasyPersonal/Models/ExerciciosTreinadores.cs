using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Connections.Mysql;
using System.Data;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace EasyPersonal.Models
{
    public class ExerciciosTreinadores
    {
        #region Atributos
        public static List<ExerciciosTreinadores> listaExerciciosTreinadores = new List<ExerciciosTreinadores>();
            public int idExerciciosTreinadores { get; set; }
            public int idExercicio { get; set; }
            public int idTreinador { get; set; }

            [Required(ErrorMessage = "Obrigatório informar o nome")]
            [StringLength(50, ErrorMessage = "O nome deve possuir no máximo 50 caracteres")]
            public string nome { get; set; }

            [StringLength(50, ErrorMessage = "O grupo muscular deve possuir no máximo 50 caracteres")]
            public string grupo { get; set; }

            [StringLength(50, ErrorMessage = "O nome equipamento possuir no máximo 50 caracteres")]
            public string equipamento { get; set; }
        #endregion

        #region Metodos

            public static void buscaExercicio(string buscando, Int32 idTreinador)
            {
                ExerciciosTreinadores.listaExerciciosTreinadores.Clear();
                string sql = "SELECT * FROM exerciciostreinadores WHERE nome LIKE  @buscando  and idTreinador = @idTreinador order by nome;";
                MySqlCommand comand = new MySqlCommand();
                comand.CommandText = sql;
                comand.Parameters.AddWithValue("@buscando", "%" + buscando + "%");
                comand.Parameters.AddWithValue("@idTreinador", idTreinador);
                var n = Connections.Mysql.MySql.getListaCmm(comand);
                while (n.Read())
                {
                    ExerciciosTreinadores exercicio = new ExerciciosTreinadores();
                    exercicio.idExerciciosTreinadores = n.GetInt32("idExerciciosTreinadores");
                    exercicio.idExercicio = n.GetInt32("idExercicio");
                    exercicio.nome = n.GetString("nome");
                    exercicio.idTreinador = n.GetInt32("idTreinador");
                    exercicio.equipamento = n.GetString("equipamento");
                    exercicio.grupo = n.GetString("grupo");
                    ExerciciosTreinadores.listaExerciciosTreinadores.Add(exercicio);
                }
                n.Dispose();
                Connections.Mysql.MySql.Desconectar();
            }


       
            public static void getLista(Int32 idTreinadorr)
            {
                ExerciciosTreinadores.listaExerciciosTreinadores.Clear();

                StringBuilder sql = new StringBuilder();
                sql.Append("select  * from exerciciostreinadores ");
                sql.Append("where idTreinador = @idTreinador ");
                sql.Append("order by nome, idExercicio");

                MySqlCommand cmm = new MySqlCommand();
                cmm.Parameters.AddWithValue("@idTreinador", idTreinadorr);
                cmm.CommandText = sql.ToString();
                var n = Connections.Mysql.MySql.getListaCmm(cmm);

                //var n = Connections.Mysql.MySql.getLista("select * from exerciciostreinadores where idTreinador = '" + idTreinadorr + "' order by nome");
               
                while (!n.IsClosed && n.Read())
                {
                    ExerciciosTreinadores exercicio = new ExerciciosTreinadores();
                    exercicio.idExerciciosTreinadores = n.GetInt32("idExerciciosTreinadores");
                    exercicio.idExercicio = n.GetInt32("idExercicio");
                    exercicio.nome = n.GetString("nome");
                    exercicio.idTreinador = n.GetInt32("idTreinador");
                    exercicio.equipamento = n.GetString("equipamento");
                    exercicio.grupo = n.GetString("grupo");
                    ExerciciosTreinadores.listaExerciciosTreinadores.Add(exercicio);
                }
                n.Dispose();
                Connections.Mysql.MySql.Desconectar();
            }

            public static ExerciciosTreinadores getExercicioEspecifico(Int32 idExercicio, Int32 idTreinador)
            {
                ExerciciosTreinadores exercicio = new ExerciciosTreinadores();

                StringBuilder sql = new StringBuilder();
                sql.Append("select * from exerciciostreinadores ");
                sql.Append("where idTreinador = @idTreinador and idExercicio = @idExercicio ");
                sql.Append("order by nome");

                MySqlCommand cmm = new MySqlCommand();
                cmm.Parameters.AddWithValue("@idTreinador", idTreinador);
                cmm.Parameters.AddWithValue("@idExercicio", idExercicio);
                cmm.CommandText = sql.ToString();
                var n = Connections.Mysql.MySql.getListaCmm(cmm);

                
                //var n = Connections.Mysql.MySql.getLista("select * from exerciciostreinadores where idTreinador = '" + idTreinador + "' and idExercicio = '"+ idExercicio + "'");

                if (!n.IsClosed && n.Read())
                {
                    exercicio.idExerciciosTreinadores = n.GetInt32("idExerciciosTreinadores");
                    exercicio.idExercicio = n.GetInt32("idExercicio");
                    exercicio.nome = n.GetString("nome");
                    exercicio.idTreinador = n.GetInt32("idTreinador");
                    exercicio.equipamento = n.GetString("equipamento");
                    exercicio.grupo = n.GetString("grupo");
                }
                Connections.Mysql.MySql.Desconectar();
                return exercicio;
            }
            
            public void editarExercicio()
            {
                if (this.equipamento == null)
                {
                    this.equipamento = "";
                }
                if (this.grupo == null)
                {
                    this.grupo = "";
                }
                if (this.nome == null)
                {
                    this.nome = "";
                }
                StringBuilder sql = new StringBuilder();
                sql.Append("update exerciciostreinadores ");
                sql.Append("set  nome = @nome, grupo = @grupo, equipamento = @equipamento ");
                sql.Append("where idTreinador = @idTreinador and idExercicio = @idExercicio");
                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@nome", this.nome);
                cmm.Parameters.AddWithValue("@grupo", this.grupo);
                cmm.Parameters.AddWithValue("@equipamento", this.equipamento);
                cmm.Parameters.AddWithValue("@idTreinador", this.idTreinador);
                cmm.Parameters.AddWithValue("@idExercicio", this.idExercicio);
                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
                cmm.Dispose();
            
            }
            public static void deletarExercicio(Int32 idTreinador, Int32 idExercicio)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("delete from exerciciostreinadores ");
                sql.Append("where idTreinador = @idTreinador and idExercicio = @idExercicio");
                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idTreinador", idTreinador);
                cmm.Parameters.AddWithValue("@idExercicio", idExercicio);
                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
                cmm.Dispose();

            }

            public static Int32 getUltimoID(Int32 idTreinador)
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("select * from exerciciostreinadores ");
                sql.Append("where idTreinador = @idTreinador ");
                sql.Append("order by idExercicio desc limit 0,1");
                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idTreinador", idTreinador);
                cmm.CommandText = sql.ToString();

                var n = Connections.Mysql.MySql.getListaCmm(cmm);

                //var n = Connections.Mysql.MySql.getLista("select * from exerciciostreinadores where idTreinador = '" + idTreinador + "' order by idExercicio desc limit 0,1");

                if (!n.IsClosed && n.Read())
                {
                    Int32 idExercicio = n.GetInt32("idExercicio") + 1;
                    n.Dispose();
                    return  idExercicio;

                }
                else
                {
                    n.Dispose();
                    return 0;
                }
                
            }

            public void addExercicio()
            {
                StringBuilder sql = new StringBuilder();
                sql.Append("insert into exerciciostreinadores (idExercicio,idTreinador,nome,grupo,equipamento)");
                sql.Append("values (@idExercicio,@idTreinador,@nome,@grupo,@equipamento)"); 
                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idExercicio", idExercicio);
                cmm.Parameters.AddWithValue("@idTreinador", idTreinador);
                cmm.Parameters.AddWithValue("@nome", nome);
                cmm.Parameters.AddWithValue("@grupo", grupo);
                cmm.Parameters.AddWithValue("@equipamento", equipamento);
                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
                cmm.Dispose();
            }

        #endregion


    }
}