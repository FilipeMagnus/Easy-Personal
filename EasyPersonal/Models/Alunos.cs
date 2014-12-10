using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;

namespace EasyPersonal.Models
{
    public class Alunos:Pessoa
    {
        #region Atributos
        
        public static List<Alunos> listaAlunos = new List<Alunos>();

        public int idAluno{get;set;}
        public int idTreinador { get; set; }

        #endregion

        #region Métodos
        public void editarAluno()
        {
          
            StringBuilder sql = new StringBuilder();

            sql.Append("update alunos ");
            sql.Append("set nome = @nome, email = @email, cidade = @cidade, idade = @idade, telefone = @telefone, sexo = @sexo,idAluno = @idAluno,idTreinador = @idTreinador ");
            sql.Append("where idAluno = @idAluno ");

            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@nome",this.nome);
            cmm.Parameters.AddWithValue("@email",this.email);
            cmm.Parameters.AddWithValue("@cidade",this.cidade);
            cmm.Parameters.AddWithValue("@idade",this.idade);
            cmm.Parameters.AddWithValue("@telefone",this.telefone);
            cmm.Parameters.AddWithValue("@sexo",this.sexo);
            cmm.Parameters.AddWithValue("@idAluno",this.idAluno);
            cmm.Parameters.AddWithValue("@idTreinador",this.idTreinador);

            cmm.CommandText = sql.ToString();

            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);

        }
        public void deletarAluno(Int32 idAlunoo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("delete from alunos ");
            sql.Append("where idAluno = @idAluno");

            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@idAluno", idAlunoo);
            cmm.CommandText = sql.ToString();

            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);

            //Connections.Mysql.MySql.ExecutarComando("delete from alunos where idAluno = '" + idAlunoo + "'");
        }
        public static Alunos getAlunoEspecifico(int idAluno)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select * from alunos ");
            sql.Append("where idAluno = @idAluno");

            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@idAluno", idAluno);
            cmm.CommandText = sql.ToString();

            var n = Connections.Mysql.MySql.getListaCmm(cmm);

            //var n = Connections.Mysql.MySql.getLista("select * from alunos where '" + idAluno + "' = idAluno");
            
            if (!n.IsClosed && n.Read())
            {
                Alunos aluno = new Alunos();
                aluno.idTreinador = n.GetInt32("idTreinador");
                aluno.idAluno = n.GetInt32("idAluno");
                aluno.nome = n.GetString("nome");
                aluno.email = n.GetString("email");
                aluno.cidade = n.GetString("cidade");
                aluno.idade = n.GetInt16("idade");
                aluno.telefone = n.GetString("telefone");
                aluno.sexo = (string)n["sexo"];
                aluno.senha = n.GetString("senha");
                aluno.senha = Alunos.encripta(aluno.senha);

                Connections.Mysql.MySql.Desconectar();
                return aluno;

            }
            else
            {
                Connections.Mysql.MySql.Desconectar();
                return null;
            }
        }
        public static void getAlunosTreinador(int idTreinador)
        {
            Alunos.listaAlunos.Clear();

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from alunos ");
            sql.Append("where idTreinador = @idTreinador");

            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@idTreinador", idTreinador);
            cmm.CommandText = sql.ToString();

            var n = Connections.Mysql.MySql.getListaCmm(cmm);

            //var n = Connections.Mysql.MySql.getLista("select * from alunos where '"+ idTreinador+"' = idTreinador");

            while (!n.IsClosed && n.Read())
            {
                Alunos aluno = new Alunos();
                aluno.idTreinador = n.GetInt32("idTreinador");
                aluno.idAluno = n.GetInt32("idAluno");
                aluno.nome = n.GetString("nome");
                aluno.email = n.GetString("email");
                aluno.cidade = n.GetString("cidade");
                aluno.idade = n.GetInt16("idade");
                aluno.telefone = n.GetString("telefone");
                aluno.sexo = n.GetString("sexo");
                aluno.senha = n.GetString("senha");
                aluno.senha = Alunos.encripta(aluno.senha);

                Alunos.listaAlunos.Add(aluno);

            }
            Connections.Mysql.MySql.Desconectar();
        }

        public Alunos loginAluno(string email, string senha)
        {
            Alunos alunoLogado = new Alunos();
            senha = Alunos.encripta(senha);

            StringBuilder sql = new StringBuilder();
            sql.Append("select * from alunos ");
            sql.Append("where email = @email && senha = @senha");

            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@email", email);
            cmm.Parameters.AddWithValue("@senha", senha);
            cmm.CommandText = sql.ToString();

            var n = Connections.Mysql.MySql.getListaCmm(cmm);

            //var n = Connections.Mysql.MySql.getLista("select * from alunos where '" + email + "' = email && '" + senha + "' = senha");
            
            if (!n.IsClosed && n.Read())
            {
                alunoLogado.idAluno = n.GetInt32("idAluno");
                alunoLogado.nome = n.GetString("nome");
                alunoLogado.idTreinador = n.GetInt32("idTreinador");
                alunoLogado.email = n.GetString("email");
                alunoLogado.cidade = n.GetString("cidade");
                alunoLogado.idade = n.GetInt16("idade");
                alunoLogado.telefone = n.GetString("telefone");
                alunoLogado.sexo = n.GetString("sexo");
                alunoLogado.senha = n.GetString("senha");
                alunoLogado.senha = Alunos.encripta(alunoLogado.senha);
                Connections.Mysql.MySql.Desconectar();
                return alunoLogado;
            }
            else
            {
                Connections.Mysql.MySql.Desconectar(); 
                return null;
            }

        }

        public static void addAluno(Alunos novo)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("insert into alunos (idTreinador,nome,telefone,email,senha,idade,cidade,sexo) ");
            sql.Append("values (@idTreinador,@nome,@telefone,@email,@senha,@idade,@cidade,@sexo)");
            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@idTreinador", novo.idTreinador);
            cmm.Parameters.AddWithValue("@nome", novo.nome);
            cmm.Parameters.AddWithValue("@telefone", novo.telefone);
            cmm.Parameters.AddWithValue("@email", novo.email);
            cmm.Parameters.AddWithValue("@senha", Treinadores.encripta(novo.senha));
            cmm.Parameters.AddWithValue("@idade", novo.idade);
            cmm.Parameters.AddWithValue("@cidade", novo.cidade);
            cmm.Parameters.AddWithValue("@sexo", novo.sexo);

            cmm.CommandText = sql.ToString();

            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            cmm.Dispose();

        }

        #endregion
    }
}