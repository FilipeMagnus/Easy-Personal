using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using Connections.Mysql;

namespace EasyPersonal.Models
{
    public class Treinos
    {
        #region Atributos
        
        public static List<Treinos> listaTreinos = new List<Treinos>();
        public int idTreino { get; set; }
        public int idExerciciosTreinadores { get; set; }
        public int idAluno { get; set; }


        [StringLength(50, ErrorMessage = "O nome do exercício deve possuir no máximo 50 caracteres")]
        public string nomeExercicio { get; set; }


        [StringLength(50, ErrorMessage = "O nome do equipamento deve possuir no máximo 50 caracteres")]
        public string equipamento { get; set; }


        [StringLength(50, ErrorMessage = "O grupo muscular deve possuir no máximo 50 caracteres")]
        public string grupo { get; set; }
        public double carga { get; set; }
        public int series { get; set; }
        public int repeticoes { get; set; }


        [StringLength(50, ErrorMessage = "O tempo de intervalo deve possuir no máximo 50 caracteres")]
        public string tempo { get; set; }



        [Required(ErrorMessage = "Obrigatório informar o dia da semana")]
        public int diaSemana { get; set; }


        #endregion

        #region Métodos
        
        public static void addTreino(Treinos novo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into treinos(idExerciciosTreinadores,idAluno,carga,series,repeticoes,tempo,diaSemana) ");
            sql.Append("values(@idExerciciosTreinadores,@idAluno,@carga,@series,@repeticoes,@tempo,@diaSemana)");

            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@idExerciciosTreinadores",novo.idExerciciosTreinadores);
            cmm.Parameters.AddWithValue("@idAluno", novo.idAluno);
            cmm.Parameters.AddWithValue("@carga", novo.carga);
            cmm.Parameters.AddWithValue("@series", novo.series);
            cmm.Parameters.AddWithValue("@repeticoes", novo.repeticoes);
            cmm.Parameters.AddWithValue("@tempo", novo.tempo);
            cmm.Parameters.AddWithValue("@diaSemana", novo.diaSemana);

            cmm.CommandText = sql.ToString();
            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);

         //   Connections.Mysql.MySql.ExecutarComando("insert into treinos(idExerciciosTreinadores,idAluno,carga,series,repeticoes,tempo,diaSemana) values (" + novo.idExerciciosTreinadores + "," + novo.idAluno + "," + novo.carga + "," + novo.series + "," + novo.repeticoes + ",'" + novo.tempo + "','" + novo.diaSemana + "')");
        }

        public void editarTreino()
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("update treinos ");
            sql.Append("set idExerciciosTreinadores = @idExerciciosTreinadores,idAluno = @idAluno,carga = @carga,series = @series,repeticoes = @repeticoes,tempo = @tempo,diaSemana = @diaSemana ");
            sql.Append("where idTreino = @idTreino");
            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@idExerciciosTreinadores", this.idExerciciosTreinadores);
            cmm.Parameters.AddWithValue("@idAluno", this.idAluno);
            cmm.Parameters.AddWithValue("@carga", this.carga);
            cmm.Parameters.AddWithValue("@series", this.series);
            cmm.Parameters.AddWithValue("@repeticoes", this.repeticoes);
            cmm.Parameters.AddWithValue("@tempo", this.tempo);
            cmm.Parameters.AddWithValue("@diaSemana", this.diaSemana);
            cmm.Parameters.AddWithValue("@idTreino", this.idTreino);

            cmm.CommandText = sql.ToString();
            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);


            //Connections.Mysql.MySql.ExecutarComando("update treinos set idExerciciosTreinadores = '" + this.idExerciciosTreinadores + "', carga = '" + this.carga + "', series = '" + this.series + "', repeticoes = '" + this.repeticoes + "', tempo = '" + this.tempo + "', diaSemana = '" + this.diaSemana + "' where idTreino = '" + this.idTreino + "'");
        }

        public static void deletarTreino(Int32 idTreinoo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("delete from treinos ");
            sql.Append("where idTreino = @idTreino");

            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@idTreino", idTreinoo);
            cmm.CommandText = sql.ToString();

            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);

            //Connections.Mysql.MySql.ExecutarComando("delete from treinos where idTreino = '" + idTreinoo + "'");
        }


        public static Treinos getTreinoEspecifico(Int32 idTreinoo)
        {
            Treinos treino = new Treinos();

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from treinos ");
            sql.Append("inner join exercicios ");
            sql.Append("on exercicios.idExercicio = treinos.idExerciciosTreinadores ");
            sql.Append("where idTreino = @idTreino ");
            sql.Append("order by treinos.diaSemana, exercicios.grupo desc");

            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@idTreino", idTreinoo);
            cmm.CommandText = sql.ToString();
            var n = Connections.Mysql.MySql.getListaCmm(cmm);

            //var n = Connections.Mysql.MySql.getLista("select * from treinos inner join exercicios on exercicios.idExercicio = treinos.idExerciciosTreinadores where idTreino = '" + idTreinoo + "' order by treinos.diaSemana, exercicios.grupo desc");

            if (!n.IsClosed && n.Read())
            {
                treino.idTreino = n.GetInt32("idTreino");
                treino.idExerciciosTreinadores = n.GetInt32("idExerciciosTreinadores");
                treino.idAluno = n.GetInt32("idAluno");
                treino.carga = n.GetDouble("carga");
                treino.series = n.GetInt32("series");
                treino.repeticoes = n.GetInt32("repeticoes");
                treino.tempo = n.GetString("tempo");
                treino.diaSemana = n.GetInt16("diaSemana");
                treino.equipamento = n.GetString("equipamento");
                treino.nomeExercicio = n.GetString("nome");
                treino.grupo = n.GetString("grupo");
                          
                
            }
            Connections.Mysql.MySql.Desconectar();     
            return treino;

        }
        public static void getTreinoAluno(Int32 idAlunoo, Int32 idTreinador)
        {
            Treinos.listaTreinos.Clear();

            StringBuilder sql = new StringBuilder();
            sql.Append("select distinct treinos.idTreino, treinos.idExerciciosTreinadores,treinos.idAluno,treinos.carga,treinos.series,treinos.repeticoes,treinos.tempo,treinos.diaSemana,exerciciostreinadores.equipamento,exerciciostreinadores.nome,exerciciostreinadores.grupo ");
            sql.Append("from treinos inner join exerciciostreinadores ");
            sql.Append("on exerciciostreinadores.idExercicio = treinos.idExerciciosTreinadores ");
            sql.Append("where idAluno = @idAluno && idTreinador = @idTreinador ");
            sql.Append("order by treinos.diaSemana, exerciciostreinadores.grupo desc");

            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@idAluno", idAlunoo);
            cmm.Parameters.AddWithValue("@idTreinador", idTreinador);
            cmm.CommandText = sql.ToString();
            var n = Connections.Mysql.MySql.getListaCmm(cmm);

            //var n = Connections.Mysql.MySql.getLista("select distinct treinos.idTreino, treinos.idExerciciosTreinadores,treinos.idAluno,treinos.carga,treinos.series,treinos.repeticoes,treinos.tempo,treinos.diaSemana,exerciciostreinadores.equipamento,exerciciostreinadores.nome,exerciciostreinadores.grupo from treinos inner join exerciciostreinadores on exerciciostreinadores.idExercicio = treinos.idExerciciosTreinadores where idAluno = '"+ idAlunoo + "' order by treinos.diaSemana, exerciciostreinadores.grupo desc");

            while (!n.IsClosed && n.Read())
            {
                Treinos treino = new Treinos();
                treino.idTreino = n.GetInt32("idTreino");
                treino.idExerciciosTreinadores = n.GetInt32("idExerciciosTreinadores");
                treino.idAluno = n.GetInt32("idAluno");
                treino.carga = n.GetDouble("carga");
                treino.series = n.GetInt32("series");
                treino.repeticoes = n.GetInt32("repeticoes");
                treino.tempo = n.GetString("tempo");
                treino.diaSemana = n.GetInt16("diaSemana");
                treino.equipamento = n.GetString("equipamento");
                treino.nomeExercicio = n.GetString("nome");
                treino.grupo = n.GetString("grupo");
                Treinos.listaTreinos.Add(treino);

            }

            Connections.Mysql.MySql.Desconectar();

        }

        public static void getTreinoFiltrado(Int32 idAlunoo, Int32 idTreinador, int dia)
        {
            Treinos.listaTreinos.Clear();

            StringBuilder sql = new StringBuilder();
            sql.Append("select distinct treinos.idTreino, treinos.idExerciciosTreinadores,treinos.idAluno,treinos.carga,treinos.series,treinos.repeticoes,treinos.tempo,treinos.diaSemana,exerciciostreinadores.equipamento,exerciciostreinadores.nome,exerciciostreinadores.grupo ");
            sql.Append("from treinos inner join exerciciostreinadores ");
            sql.Append("on exerciciostreinadores.idExercicio = treinos.idExerciciosTreinadores ");
            sql.Append("where idAluno = @idAluno && idTreinador = @idTreinador && treinos.diaSemana = @diaSemana ");
            sql.Append("order by treinos.diaSemana, exerciciostreinadores.grupo desc");

            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@idAluno", idAlunoo);
            cmm.Parameters.AddWithValue("@idTreinador", idTreinador);
            cmm.Parameters.AddWithValue("@diaSemana", dia);
            cmm.CommandText = sql.ToString();
            var n = Connections.Mysql.MySql.getListaCmm(cmm);

            //var n = Connections.Mysql.MySql.getLista("select distinct treinos.idTreino, treinos.idExerciciosTreinadores,treinos.idAluno,treinos.carga,treinos.series,treinos.repeticoes,treinos.tempo,treinos.diaSemana,exerciciostreinadores.equipamento,exerciciostreinadores.nome,exerciciostreinadores.grupo from treinos inner join exerciciostreinadores on exerciciostreinadores.idExercicio = treinos.idExerciciosTreinadores where idAluno = '"+ idAlunoo + "' order by treinos.diaSemana, exerciciostreinadores.grupo desc");

            while (!n.IsClosed && n.Read())
            {
                Treinos treino = new Treinos();
                treino.idTreino = n.GetInt32("idTreino");
                treino.idExerciciosTreinadores = n.GetInt32("idExerciciosTreinadores");
                treino.idAluno = n.GetInt32("idAluno");
                treino.carga = n.GetDouble("carga");
                treino.series = n.GetInt32("series");
                treino.repeticoes = n.GetInt32("repeticoes");
                treino.tempo = n.GetString("tempo");
                treino.diaSemana = n.GetInt16("diaSemana");
                treino.equipamento = n.GetString("equipamento");
                treino.nomeExercicio = n.GetString("nome");
                treino.grupo = n.GetString("grupo");
                Treinos.listaTreinos.Add(treino);

            }

            Connections.Mysql.MySql.Desconectar();

        }
        #endregion
    }
}