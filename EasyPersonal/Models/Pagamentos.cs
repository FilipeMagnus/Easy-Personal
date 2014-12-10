using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MySql.Data.MySqlClient;

namespace EasyPersonal.Models
{
    public class Pagamentos
    {
        #region Atributos

        public static List<Pagamentos> listaPagamentos = new List<Pagamentos>();

        public int idPagamento { get; set; }
        public int idTreinador { get; set; }
        public int idAluno { get; set; }    
        public double valor { get; set; }
        public int diaPagar{ get; set; }
        public int diaPagado { get; set; }
        public string diaPago { get; set; }
        public string status { get; set; }
        public int parcelas { get; set; }
        public DateTime comeca { get; set; }
        public int termina { get; set; }
        public string nome { get; set; }
        

        
            
        #endregion

        #region Métodos

        public static void getListaTreinador(Int32 idTreinador)
        {
            Pagamentos.listaPagamentos.Clear();

            StringBuilder sql = new StringBuilder();

            sql.Append("select * from pagamentos ");
            sql.Append("inner join alunos ");
            sql.Append("on pagamentos.idAluno = alunos.idAluno ");
            sql.Append("where pagamentos.idTreinador = @idTreinador and parcelas != 0");
            

            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@idTreinador", idTreinador);

            cmm.CommandText = sql.ToString();

            var dr = Connections.Mysql.MySql.getListaCmm(cmm);

            while (dr.Read())
            {
                Pagamentos novo = new Pagamentos();
                novo.idPagamento = (int)dr["idPagamento"];
                novo.idTreinador = (int)dr["idTreinador"];
                novo.idAluno = (int)dr["idAluno"];
                novo.status = (string)dr["status"];
                novo.nome = (string)dr["nome"];
                novo.valor = (double)dr["valor"];
                novo.diaPago = (string)dr["diaPago"];
                novo.diaPagar = (int)dr["diaPagar"];
                novo.comeca = (DateTime)dr["comeca"];
                novo.termina = (int)dr["termina"];
                novo.parcelas = (int)dr["parcelas"];
                novo.diaPagado = (int)dr["diaPagado"];

                Pagamentos.listaPagamentos.Add(novo);
                
            }

            dr.Dispose();
            Connections.Mysql.MySql.Desconectar();
        }
        public static void gerarParcelas(Int32 idTreinador, string senha)
        {
            Treinadores.getTreinadorEspecifico(idTreinador);

          
            Pagamentos.getListaTreinador(idTreinador);

            int x=0;
            while (x < Pagamentos.listaPagamentos.Count && Pagamentos.listaPagamentos.Count != 0)
            {
                if (Pagamentos.listaPagamentos[x].parcelas > 0)
                {
                    int parcela = Pagamentos.listaPagamentos[x].parcelas--;
                    Pagamentos.listaPagamentos[x].parcelas = 0;
                    Pagamentos.addPagamento(Pagamentos.listaPagamentos[x]);
                    Pagamentos.listaPagamentos[x].parcelas = parcela;
                    Pagamentos.listaPagamentos[x].editarPagamento();
                    x++;
                }
                else
                {
                    x++;
                }
            }
            Treinadores.listaTreinadores[0].parcelaGerada = DateTime.Now.Month;
            Treinadores.listaTreinadores[0].senha =  Treinadores.encripta(senha);            
            Treinadores.listaTreinadores[0].editarTreinador();
        }



        public static void getLista(Int32 idAluno)
        {
            Pagamentos.listaPagamentos.Clear();

            StringBuilder sql = new StringBuilder();

            sql.Append("select * from pagamentos ");
            sql.Append("where idAluno = @idAluno");

            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@idAluno", idAluno);
            cmm.CommandText = sql.ToString();
            
            var dr = Connections.Mysql.MySql.getListaCmm(cmm);

            while (dr.Read())
            {
                Pagamentos novo = new Pagamentos();
                novo.idPagamento = (int)dr["idPagamento"];
                novo.idTreinador = (int)dr["idTreinador"];
                novo.idAluno = (int)dr["idAluno"];
                novo.status = (string)dr["status"];
                novo.valor = (double)dr["valor"];
                novo.diaPago = (string)dr["diaPago"];
                novo.diaPagar = (int)dr["diaPagar"];
                novo.comeca = (DateTime)dr["comeca"];
                novo.termina = (int)dr["termina"];
                novo.diaPagado = (int)dr["diaPagado"];
                novo.parcelas = (int)dr["parcelas"];
                Pagamentos.listaPagamentos.Add(novo);
            }

            dr.Dispose();
            Connections.Mysql.MySql.Desconectar();
        }


        public static void getListaMain(Int32 idTreinador)
        {
            Pagamentos.listaPagamentos.Clear();

            StringBuilder sql = new StringBuilder();

            sql.Append("select * from pagamentos ");
            sql.Append("inner join alunos ");
            sql.Append("on pagamentos.idAluno = alunos.idAluno ");
            sql.Append("where status = @Pendente and pagamentos.idTreinador = @idTreinador ");
            sql.Append("order by diaPagar");

            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@Pendente", "Pendente");
            cmm.Parameters.AddWithValue("@idTreinador", idTreinador);

            cmm.CommandText = sql.ToString();

            var dr = Connections.Mysql.MySql.getListaCmm(cmm);
            
            while (dr.Read())
            {
                Pagamentos novo = new Pagamentos();
                novo.idPagamento = (int)dr["idPagamento"];
                novo.idTreinador = (int)dr["idTreinador"];
                novo.idAluno = (int)dr["idAluno"];
                novo.status = (string)dr["status"];
                novo.nome = (string)dr["nome"];
                novo.diaPagado = (int)dr["diaPagado"];
                novo.valor = (double)dr["valor"];
                novo.diaPago = (string)dr["diaPago"];
                novo.diaPagar = (int)dr["diaPagar"];
                novo.comeca = (DateTime)dr["comeca"];
                novo.termina = (int)dr["termina"];
                novo.parcelas = (int)dr["parcelas"];

                    Pagamentos.listaPagamentos.Add(novo);
                
            }

            dr.Dispose();
            Connections.Mysql.MySql.Desconectar();
        }

        public static void getTodos(Int32 idTreinador)
        {
            Pagamentos.listaPagamentos.Clear();

            StringBuilder sql = new StringBuilder();

            sql.Append("select * from pagamentos ");
            sql.Append("inner join alunos ");
            sql.Append("on pagamentos.idAluno = alunos.idAluno ");
            sql.Append("where status = @Pendente and pagamentos.idTreinador = @idTreinador ");
            sql.Append("order by diaPagar");

            MySqlCommand cmm = new MySqlCommand();
            cmm.Parameters.AddWithValue("@Pendente", "Pendente");
            cmm.Parameters.AddWithValue("@idTreinador", idTreinador);

            cmm.CommandText = sql.ToString();

            var dr = Connections.Mysql.MySql.getListaCmm(cmm);

            while (dr.Read())
            {
                Pagamentos novo = new Pagamentos();
                novo.idPagamento = (int)dr["idPagamento"];
                novo.idTreinador = (int)dr["idTreinador"];
                novo.idAluno = (int)dr["idAluno"];
                novo.status = (string)dr["status"];
                novo.nome = (string)dr["nome"];
                novo.diaPagado = (int)dr["diaPagado"];
                novo.valor = (double)dr["valor"];
                novo.diaPago = (string)dr["diaPago"];
                novo.diaPagar = (int)dr["diaPagar"];
                novo.comeca = (DateTime)dr["comeca"];
                novo.termina = (int)dr["termina"];
                novo.parcelas = (int)dr["parcelas"];
                Pagamentos.listaPagamentos.Add(novo);
            }

            dr.Dispose();
            Connections.Mysql.MySql.Desconectar();
        }
        public static void addPagamento(Pagamentos novo)
        {



            StringBuilder sql = new StringBuilder();

            sql.Append("insert into pagamentos(diaPagado,valor,diaPagar,diaPago,status,idTreinador,idAluno,comeca,termina,parcelas) ");
            sql.Append("values(@diaPagado,@valor,@diaPagar,@diaPago,@status,@idTreinador,@idAluno,@comeca,@termina,@parcelas)");

            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@diaPagado", novo.diaPagado);
            cmm.Parameters.AddWithValue("@valor", novo.valor);
            cmm.Parameters.AddWithValue("@diaPagar", novo.diaPagar);
            cmm.Parameters.AddWithValue("@diaPago", novo.diaPago);
            cmm.Parameters.AddWithValue("@status", novo.status);
            cmm.Parameters.AddWithValue("@idTreinador", novo.idTreinador);
            cmm.Parameters.AddWithValue("@idAluno", novo.idAluno);
            cmm.Parameters.AddWithValue("@comeca", novo.comeca);
            cmm.Parameters.AddWithValue("@termina", novo.termina);
            cmm.Parameters.AddWithValue("@parcelas", novo.parcelas);

            cmm.CommandText = sql.ToString();

            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            Connections.Mysql.MySql.Desconectar();
        }

        public static Pagamentos getEspecifico(Int32 idPagamento)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("select * from pagamentos ");
            sql.Append("where idPagamento = @idPagamento");

            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@idPagamento", idPagamento);

            cmm.CommandText = sql.ToString();

            var dr = Connections.Mysql.MySql.getListaCmm(cmm);
            Pagamentos novo = new Pagamentos();
            if (dr.Read())
            {
                novo.idPagamento = (int)dr["idPagamento"];
                novo.idAluno = (int)dr["idAluno"];
                novo.idTreinador = (int)dr["idTreinador"];
                novo.status = (string)dr["status"];
                novo.valor = (double)dr["valor"];
                novo.diaPago = (string)dr["diaPago"];
                novo.diaPagar = (int)dr["diaPagar"];
                novo.comeca = (DateTime)dr["comeca"];
                novo.diaPagado = (int)dr["diaPagado"];
                novo.termina = (int)dr["termina"];
                novo.parcelas = (int)dr["parcelas"];
                dr.Dispose();
                Connections.Mysql.MySql.Desconectar();
                return novo;
            }
            else
            {
                Connections.Mysql.MySql.Desconectar();
                dr.Dispose();
                return null;
            }
        }

        public void editarPagamento()
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("update pagamentos ");
            sql.Append("set idPagamento = @idPagamento, diaPagado = @diaPagado ,valor = @valor, status = @status, diaPago = @diaPago, diaPagar = @diaPagar, idTreinador = @idTreinador, idAluno = @idAluno, parcelas = @parcelas, comeca = @comeca, termina = @termina ");
            sql.Append("where idPagamento = @idPagamento");

            MySqlCommand cmm = new MySqlCommand();


            cmm.Parameters.AddWithValue("@idPagamento", this.idPagamento);
            cmm.Parameters.AddWithValue("@diaPagado", this.diaPagado);
            cmm.Parameters.AddWithValue("@valor", this.valor);
            cmm.Parameters.AddWithValue("@status", this.status);
            cmm.Parameters.AddWithValue("@diaPago", this.diaPago);
            cmm.Parameters.AddWithValue("@diaPagar", this.diaPagar);
            cmm.Parameters.AddWithValue("@idTreinador", this.idTreinador);
            cmm.Parameters.AddWithValue("@idAluno", this.idAluno);
            cmm.Parameters.AddWithValue("@comeca", this.comeca);
            cmm.Parameters.AddWithValue("@termina", this.termina);
            cmm.Parameters.AddWithValue("@parcelas", this.parcelas);

            cmm.CommandText = sql.ToString();

            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
        }
        
        public static void deletarPagamento(Int32 idPagamento)
        {
            StringBuilder sql = new StringBuilder();

            sql.Append("delete from pagamentos ");
            sql.Append("where idPagamento = @idPagamento");

            MySqlCommand cmm = new MySqlCommand();

            cmm.Parameters.AddWithValue("@idPagamento", idPagamento);

            cmm.CommandText = sql.ToString();

            Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
        }

        #endregion
    }
}