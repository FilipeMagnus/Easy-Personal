using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MySql.Data.MySqlClient;

namespace EasyPersonal.Models
{
    public class Faulkner:Avaliacoes
    {
        #region Atributos
        public static List<Faulkner> listaFaulkner = new List<Faulkner>();
            public int idFaulkner { get; set; }
            public int idAluno { get; set; }
            public double triceps { get; set; }
            public double subescapular { get; set; }
            public double supraIliaca { get; set; }
            public double abdominal { get; set; }
        #endregion

        #region Métodos

            public static void addFaulkner(Faulkner novo,string sexo, int idade)
            {

                novo.calculoFaulkner(sexo,idade);
                
                StringBuilder sql = new StringBuilder();

                novo.IMC = (novo.pesoAtual / (novo.altura * novo.altura));
                novo.arrumarDecimal();
                sql.Append("insert into faulkner(porcentagemGordura,porcentagemMuscular,pesoMinimo,pesoMaximo,excessoPeso,triceps,supraIliaca,subescapular,abdominal,dataAvaliacao,IMC,altura,pesoAtual,pesoGordo,pesoMagro,pesoOsseo,pesoMuscular,pesoResidual,idAluno) ");
                sql.Append("values(@porcentagemGordura,@porcentagemMuscular,@pesoMinimo,@pesoMaximo,@excessoPeso,@triceps,@supraIliaca,@subescapular,@abdominal,@dataAvaliacao,@IMC,@altura,@pesoAtual,@pesoGordo,@pesoMagro,@pesoOsseo,@pesoMuscular,@pesoResidual,@idAluno)");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@triceps", novo.triceps);
                cmm.Parameters.AddWithValue("@supraIliaca", novo.supraIliaca);
                cmm.Parameters.AddWithValue("@subescapular", novo.subescapular);
                cmm.Parameters.AddWithValue("@abdominal", novo.abdominal);
                cmm.Parameters.AddWithValue("@dataAvaliacao", novo.dataAvaliacao.Date);
                cmm.Parameters.AddWithValue("@IMC", novo.IMC);
                cmm.Parameters.AddWithValue("@altura", novo.altura);
                cmm.Parameters.AddWithValue("@pesoAtual", novo.pesoAtual);
                cmm.Parameters.AddWithValue("@pesoGordo", novo.pesoGordo);
                cmm.Parameters.AddWithValue("@pesoMagro", novo.pesoMagro);
                cmm.Parameters.AddWithValue("@pesoOsseo", novo.pesoOsseo);
                cmm.Parameters.AddWithValue("@pesoMuscular", novo.pesoMuscular);
                cmm.Parameters.AddWithValue("@pesoResidual", novo.pesoResidual);
                cmm.Parameters.AddWithValue("@idAluno", novo.idAluno);
                cmm.Parameters.AddWithValue("@porcentagemGordura", novo.porcentagemGordura);
                cmm.Parameters.AddWithValue("@porcentagemMuscular", novo.porcentagemMuscular);
                cmm.Parameters.AddWithValue("@pesoMinimo", novo.pesoMinimo);
                cmm.Parameters.AddWithValue("@pesoMaximo", novo.pesoMaximo);
                cmm.Parameters.AddWithValue("@excessoPeso", novo.excessoPeso);

                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            }
            public static List<Faulkner> getLista(Int32 idAluno, int aux)
            {
                Faulkner.listaFaulkner.Clear();

                StringBuilder sql = new StringBuilder();

                sql.Append("select * from faulkner ");
                sql.Append("where idAluno = @idAluno");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idAluno", idAluno);
                cmm.CommandText = sql.ToString();

                var dr = Connections.Mysql.MySql.getListaCmm(cmm);

                while (dr.Read())
                {
                    Faulkner novo = new Faulkner();
                    novo.idFaulkner = (int)dr["idFaulkner"];
                    novo.abdominal = (double)dr["abdominal"];
                    novo.subescapular = (double)dr["subescapular"];
                    novo.triceps = (double)dr["triceps"];
                    novo.supraIliaca = (double)dr["supraIliaca"];
                    novo.altura = (double)dr["altura"];
                    novo.dataAvaliacao = (DateTime)dr["dataAvaliacao"];
                    novo.IMC = (double)dr["IMC"];
                    novo.dataGrafico = Convert.ToString(novo.dataAvaliacao.Date);
                    novo.dataGrafico = novo.dataGrafico.Substring(0, 10);
                    novo.aux = aux;
                    novo.pesoAtual = (double)dr["pesoAtual"];
                    novo.pesoGordo = (double)dr["pesoGordo"];
                    novo.pesoMagro = (double)dr["pesoMagro"];
                    novo.pesoOsseo = (double)dr["pesoOsseo"];
                    novo.pesoMinimo = (double)dr["pesoMinimo"];
                    novo.pesoMaximo = (double)dr["pesoMaximo"];
                    novo.porcentagemGordura = (double)dr["porcentagemGordura"];
                    novo.excessoPeso = (double)dr["excessoPeso"];
                    novo.porcentagemMuscular = (double)dr["porcentagemMuscular"];
                    novo.pesoMuscular = (double)dr["pesoMuscular"];
                    novo.pesoResidual = (double)dr["pesoResidual"];
                    novo.idAluno = (int)dr["idAluno"];

                    Faulkner.listaFaulkner.Add(novo);
                }

                dr.Dispose();
                Connections.Mysql.MySql.Desconectar();
                return Faulkner.listaFaulkner;
            }

            private void arrumarDecimal()
            {
                this.pesoMagro = Math.Round(this.pesoMagro, 2);
                this.pesoGordo = Math.Round(this.pesoGordo, 2);
                this.pesoResidual = Math.Round(this.pesoResidual, 2);
                this.porcentagemGordura = Math.Round(this.porcentagemGordura, 2);
                this.porcentagemMuscular = Math.Round(this.porcentagemMuscular, 2);
                this.excessoPeso = Math.Round(this.excessoPeso, 2);
                this.densidadeCorporal = Math.Round(this.densidadeCorporal, 2);
                this.pesoAtual = Math.Round(this.pesoAtual, 2);
                this.altura = Math.Round(this.altura, 2);
                this.IMC = Math.Round(this.IMC, 2);
                this.pesoMaximo = Math.Round(this.pesoMaximo, 2);
                this.pesoMinimo = Math.Round(this.pesoMinimo, 2);
                this.pesoOsseo = Math.Round(this.pesoOsseo, 2);
                this.supraIliaca = Math.Round(this.supraIliaca, 2);
                this.triceps = Math.Round(this.triceps, 2);
                this.pesoMuscular = Math.Round(this.pesoMuscular, 2);
                this.subescapular = Math.Round(this.subescapular, 2);
                this.abdominal = Math.Round(this.abdominal, 2);
            }
    


            public static Faulkner getEspecifico(Int32 idFaulkner)
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("select * from faulkner ");
                sql.Append("where idFaulkner = @idFaulkner");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idFaulkner", idFaulkner);

                cmm.CommandText = sql.ToString();

                var dr = Connections.Mysql.MySql.getListaCmm(cmm);
                Faulkner novo = new Faulkner();
                if (dr.Read())
                {
                    novo.idFaulkner = (int)dr["idFaulkner"];
                    novo.abdominal = (double)dr["abdominal"];
                    novo.subescapular = (double)dr["subescapular"];
                    novo.triceps = (double)dr["triceps"];
                    novo.supraIliaca = (double)dr["supraIliaca"];
                    novo.altura = (double)dr["altura"];
                    novo.dataAvaliacao = (DateTime)dr["dataAvaliacao"];
                    novo.IMC = (double)dr["IMC"];
                    novo.pesoAtual = (double)dr["pesoAtual"];
                    novo.pesoGordo = (double)dr["pesoGordo"];
                    novo.pesoMagro = (double)dr["pesoMagro"];
                    novo.pesoOsseo = (double)dr["pesoOsseo"];
                    novo.pesoMuscular = (double)dr["pesoMuscular"];
                    novo.pesoResidual = (double)dr["pesoResidual"];
                    novo.idAluno = (int)dr["idAluno"];
                    novo.pesoMinimo = (double)dr["pesoMinimo"];
                    novo.pesoMaximo = (double)dr["pesoMaximo"];
                    novo.porcentagemGordura = (double)dr["porcentagemGordura"];
                    novo.excessoPeso = (double)dr["excessoPeso"];
                    novo.porcentagemMuscular = (double)dr["porcentagemMuscular"];
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

            private void calculoFaulkner(string sexo, int idade)
            {
                if (this.triceps == null || this.supraIliaca == null || this.subescapular == null || this.abdominal == null)
                {
                    this.densidadeCorporal = 0;
                    this.porcentagemGordura = 0;
                }
                else
                {
                    double st = 0;

                    st = this.triceps + this.supraIliaca + this.abdominal + this.subescapular;
                    this.densidadeCorporal = st * 0.153 + 5.783 / 100;
                    this.porcentagemGordura = this.densidadeCorporal;
                }
                if (this.pesoAtual == 0 || this.altura == 0)
                {
                    this.IMC = 0;
                }
                else
                {
                    this.IMC = (this.pesoAtual / (this.altura * this.altura));
                }
                this.pesoGordo = (this.pesoAtual * this.porcentagemGordura) / 100;
                this.pesoMagro = this.pesoAtual - this.pesoGordo;
                if (sexo == "m")
                {
                    this.pesoResidual = (this.pesoAtual * 24.1) / 100;
                }
                else
                {
                    this.pesoResidual = (this.pesoAtual * 20.9) / 100;
                }
                this.pesoMaximo = this.pesoMagro / 0.895;
                this.pesoMinimo = this.pesoMagro / 0.91;
                this.excessoPeso = this.pesoAtual - this.pesoMinimo;
                this.pesoOsseo = 15;
                this.pesoMuscular = (this.pesoAtual - (this.pesoGordo + this.pesoOsseo + this.pesoResidual));
                this.porcentagemMuscular = this.pesoMuscular / this.pesoAtual;
 
            }

            public void editar()
            {
                StringBuilder sql = new StringBuilder();
                this.IMC = (this.pesoAtual / (this.altura * this.altura));

                sql.Append("update faulkner ");
                sql.Append("set idFaulkner = @idFaulkner, porcentagemMuscular = @porcentagemMuscular, porcentagemGordura = @porcentagemGordura, pesoMinimo = @pesoMinimo, pesoMaximo = @pesoMaximo, excessoPeso = @excessoPeso ,abdominal = @abdominal, subescapular = @subescapular, triceps = @triceps, supraIliaca = @supraIliaca, altura = @altura, dataAvaliacao = @dataAvaliacao, IMC = @IMC, pesoAtual = @pesoAtual, pesoGordo = @pesoGordo, pesoMagro = @pesoMagro, pesoOsseo = @pesoOsseo, pesoMuscular = @pesoMuscular, pesoResidual = @pesoResidual, idAluno = @idAluno ");
                sql.Append("where idFaulkner = @idFaulkner");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idFaulkner", this.idFaulkner);
                cmm.Parameters.AddWithValue("@abdominal", this.abdominal);
                cmm.Parameters.AddWithValue("@subescapular", this.subescapular);
                cmm.Parameters.AddWithValue("@triceps", this.triceps);
                cmm.Parameters.AddWithValue("@supraIliaca", this.supraIliaca);
                cmm.Parameters.AddWithValue("@dataAvaliacao", this.dataAvaliacao.Date);
                cmm.Parameters.AddWithValue("@IMC", this.IMC);
                cmm.Parameters.AddWithValue("@altura", this.altura);
                cmm.Parameters.AddWithValue("@pesoAtual", this.pesoAtual);
                cmm.Parameters.AddWithValue("@pesoGordo", this.pesoGordo);
                cmm.Parameters.AddWithValue("@pesoMagro", this.pesoMagro);
                cmm.Parameters.AddWithValue("@pesoOsseo", this.pesoOsseo);
                cmm.Parameters.AddWithValue("@pesoMuscular", this.pesoMuscular);
                cmm.Parameters.AddWithValue("@pesoResidual", this.pesoResidual);
                cmm.Parameters.AddWithValue("@idAluno", this.idAluno);
                cmm.Parameters.AddWithValue("@porcentagemGordura", this.porcentagemGordura);
                cmm.Parameters.AddWithValue("@porcentagemMuscular", this.porcentagemMuscular);
                cmm.Parameters.AddWithValue("@pesoMinimo", this.pesoMinimo);
                cmm.Parameters.AddWithValue("@pesoMaximo", this.pesoMaximo);
                cmm.Parameters.AddWithValue("@excessoPeso", this.excessoPeso);
                

                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            }

            public static void deletarFaulkner(Int32 idFaulkner)
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("delete from faulkner ");
                sql.Append("where idFaulkner = @idFaulkner");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idFaulkner", idFaulkner);

                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            }


        #endregion



    }
}