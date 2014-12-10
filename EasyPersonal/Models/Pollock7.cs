using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;

namespace EasyPersonal.Models
{
    public class Pollock7:Avaliacoes
    {
        #region Atributos
            public static List<Pollock7> listaPollock7 = new List<Pollock7>();
            public int idPollock7 { get; set; }
            public int idAluno { get; set; }
            public double triceps { get; set; }
            public double subescapular { get; set; }
            public double supraIliaca { get; set; }
            public double abdominal { get; set; }
            public double axiliarMedial { get; set; }
            public double peito { get; set; }
            public double coxa { get; set; }
        #endregion


            #region Métodos

            public static void addPollock7(Pollock7 novo, string sexo, int idade)
            {
                novo.calculoPollock7(sexo,idade);
                novo.IMC = (novo.pesoAtual / (novo.altura * novo.altura));

                novo.arrumarDecimal();
                StringBuilder sql = new StringBuilder();

                
                sql.Append("insert into pollock7(pesoResidual,pesoMuscular,densidadeCorporal,porcentagemGordura,porcentagemMuscular,pesoMinimo,pesoMaximo,excessoPeso,triceps,subescapular,supraIliaca,abdominal,axiliarMedial,peito,coxa,dataAvaliacao,IMC,altura,pesoAtual,pesoGordo,pesoMagro,pesoOsseo,idAluno) ");
                sql.Append("values(@pesoResidual,@pesoMuscular,@densidadeCorporal,@porcentagemGordura,@porcentagemMuscular,@pesoMinimo,@pesoMaximo,@excessoPeso,@triceps,@subescapular,@supraIliaca,@abdominal,@axiliarMedial,@peito,@coxa,@dataAvaliacao,@IMC,@altura,@pesoAtual,@pesoGordo,@pesoMagro,@pesoOsseo,@idAluno)");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@triceps", novo.triceps);
                cmm.Parameters.AddWithValue("@subescapular", novo.subescapular);
                cmm.Parameters.AddWithValue("@supraIliaca", novo.supraIliaca);
                cmm.Parameters.AddWithValue("@abdominal", novo.abdominal);
                cmm.Parameters.AddWithValue("@axiliarMedial", novo.axiliarMedial);
                cmm.Parameters.AddWithValue("@peito", novo.peito);
                cmm.Parameters.AddWithValue("@densidadeCorporal", novo.densidadeCorporal);
                cmm.Parameters.AddWithValue("@pesoMuscular", novo.pesoMuscular);
                cmm.Parameters.AddWithValue("@pesoResidual", novo.pesoResidual);
                cmm.Parameters.AddWithValue("@coxa", novo.coxa);
                cmm.Parameters.AddWithValue("@dataAvaliacao", novo.dataAvaliacao.Date);
                cmm.Parameters.AddWithValue("@IMC", novo.IMC);
                cmm.Parameters.AddWithValue("@altura", novo.altura);
                cmm.Parameters.AddWithValue("@pesoAtual", novo.pesoAtual);
                cmm.Parameters.AddWithValue("@pesoGordo", novo.pesoGordo);
                cmm.Parameters.AddWithValue("@pesoMagro", novo.pesoMagro);
                cmm.Parameters.AddWithValue("@pesoOsseo", novo.pesoOsseo);
                cmm.Parameters.AddWithValue("@idAluno", novo.idAluno);
                cmm.Parameters.AddWithValue("@porcentagemGordura", novo.porcentagemGordura);
                cmm.Parameters.AddWithValue("@porcentagemMuscular", novo.porcentagemMuscular);
                cmm.Parameters.AddWithValue("@pesoMinimo", novo.pesoMinimo);
                cmm.Parameters.AddWithValue("@pesoMaximo", novo.pesoMaximo);
                cmm.Parameters.AddWithValue("@excessoPeso", novo.excessoPeso);

                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            }

            public static List<Pollock7> getLista(Int32 idAluno, int aux)
            {
                Pollock7.listaPollock7.Clear();

                StringBuilder sql = new StringBuilder();

                sql.Append("select * from pollock7 ");
                sql.Append("where idAluno = @idAluno ");
                sql.Append("order by dataAvaliacao");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idAluno", idAluno);
                cmm.CommandText = sql.ToString();

                var dr = Connections.Mysql.MySql.getListaCmm(cmm);

                while (dr.Read())
                {
                    Pollock7 novo = new Pollock7();
                    novo.idPollock7 = (int)dr["idPollock7"];
                    novo.coxa = (double)dr["coxa"];
                    novo.abdominal = (double)dr["abdominal"];
                    novo.axiliarMedial = (double)dr["axiliarMedial"];
                    novo.subescapular = (double)dr["subescapular"];
                    novo.peito = (double)dr["peito"];
                    novo.triceps = (double)dr["triceps"];
                    novo.supraIliaca = (double)dr["supraIliaca"];
                    novo.altura = (double)dr["altura"];
                    novo.dataAvaliacao = (DateTime)dr["dataAvaliacao"];
                    novo.dataGrafico = Convert.ToString(novo.dataAvaliacao.Date);
                    novo.dataGrafico = novo.dataGrafico.Substring(0, 10);
                    novo.aux = aux;
                    novo.IMC = (double)dr["IMC"];
                    novo.pesoAtual = (double)dr["pesoAtual"];
                    novo.pesoGordo = (double)dr["pesoGordo"];
                    novo.pesoMagro = (double)dr["pesoMagro"];
                    novo.pesoOsseo = (double)dr["pesoOsseo"];
                    novo.pesoMuscular = (double)dr["pesoMuscular"];
                    novo.pesoResidual = (double)dr["pesoResidual"];
                    novo.densidadeCorporal = (double)dr["densidadeCorporal"];
                    novo.idAluno = (int)dr["idAluno"];
                    novo.pesoMinimo = (double)dr["pesoMinimo"];
                    novo.pesoMaximo = (double)dr["pesoMaximo"];
                    novo.porcentagemGordura = (double)dr["porcentagemGordura"];
                    novo.excessoPeso = (double)dr["excessoPeso"];
                    novo.porcentagemMuscular = (double)dr["porcentagemMuscular"];

                    Pollock7.listaPollock7.Add(novo);
                }

                dr.Dispose();
                Connections.Mysql.MySql.Desconectar();
                return Pollock7.listaPollock7;
            }

            public static Pollock7 getEspecifico(Int32 idPollock7)
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("select * from pollock7 ");
                sql.Append("where idPollock7 = @idPollock7");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idPollock7", idPollock7);

                cmm.CommandText = sql.ToString();

                var dr = Connections.Mysql.MySql.getListaCmm(cmm);
                Pollock7 novo = new Pollock7();
                if (dr.Read())
                {
                    novo.idPollock7 = (int)dr["idPollock7"];
                    novo.coxa = (double)dr["coxa"];
                    novo.abdominal = (double)dr["abdominal"];
                    novo.axiliarMedial = (double)dr["axiliarMedial"];
                    novo.subescapular = (double)dr["subescapular"];
                    novo.peito = (double)dr["peito"];
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
                    novo.densidadeCorporal = (double)dr["densidadeCorporal"];
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
                this.coxa = Math.Round(this.coxa, 2);
                this.pesoMaximo = Math.Round(this.pesoMaximo, 2);
                this.pesoMinimo = Math.Round(this.pesoMinimo, 2);
                this.pesoOsseo = Math.Round(this.pesoOsseo, 2);
                this.supraIliaca = Math.Round(this.supraIliaca, 2);
                this.triceps = Math.Round(this.triceps, 2);
                this.pesoMuscular = Math.Round(this.pesoMuscular, 2);
                this.peito = Math.Round(this.peito, 2);
                this.abdominal = Math.Round(this.abdominal, 2);
                this.axiliarMedial = Math.Round(this.axiliarMedial, 2);
                this.subescapular = Math.Round(this.subescapular, 2);
            }

            private void calculoPollock7(string sexo, int idade)
            {
                if (this.coxa == null || this.triceps == null || this.peito == null || this.supraIliaca == null || this.abdominal == null || this.axiliarMedial == null || this.subescapular == null)
                {
                    this.densidadeCorporal = 0;
                    this.porcentagemGordura = 0;
                }
                else
                {
                    double st = 0;
                    double st3 = 0;
                    st = this.triceps + this.supraIliaca + this.coxa + this.peito + this.abdominal + this.axiliarMedial + this.subescapular;
                    st3 = this.triceps + this.supraIliaca + this.coxa;
                    if (sexo == "m")
                    {
                        this.densidadeCorporal = (1.112 - (0.00043499 * st) + (0.00000055 * (st3 * st3) - (0.00012882 * idade))); 
                    }
                    else
                    {
                        this.densidadeCorporal = (1.097-(0.0004697 * st)+(0.00000056 * (st3 * st3) - (0.00012828 * idade))); 
                    }

                    this.porcentagemGordura = ((4.95 / this.densidadeCorporal) - 4.5) * 100;
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
                if (sexo=="m")
                {
                    this.pesoResidual = (this.pesoAtual * 24.1)/100;
                }
                else
                {
                    this.pesoResidual = (this.pesoAtual * 20.9)/100; 
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

                sql.Append("update pollock7 ");
                sql.Append("set idPollock7 = @idPollock7, pesoResidual = @pesoResidual, pesoMuscular = @pesoMuscular, densidadeCorporal = @densidadeCorporal,porcentagemMuscular = @porcentagemMuscular, porcentagemGordura = @porcentagemGordura, pesoMinimo = @pesoMinimo, pesoMaximo = @pesoMaximo, excessoPeso = @excessoPeso,subescapular = @subescapular, abdominal = @abdominal, peito = @peito, axiliarMedial = @axiliarMedial ,coxa = @coxa, triceps = @triceps, supraIliaca = @supraIliaca, altura = @altura, dataAvaliacao = @dataAvaliacao, IMC = @IMC, pesoAtual = @pesoAtual, pesoGordo = @pesoGordo, pesoMagro = @pesoMagro, pesoOsseo = @pesoOsseo, idAluno = @idAluno ");
                sql.Append("where idPollock7 = @idPollock7");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idPollock7", this.idPollock7);
                cmm.Parameters.AddWithValue("@triceps", this.triceps);
                cmm.Parameters.AddWithValue("@subescapular", this.subescapular);
                cmm.Parameters.AddWithValue("@supraIliaca", this.supraIliaca);
                cmm.Parameters.AddWithValue("@abdominal", this.abdominal);
                cmm.Parameters.AddWithValue("@axiliarMedial", this.axiliarMedial);
                cmm.Parameters.AddWithValue("@peito", this.peito);
                cmm.Parameters.AddWithValue("@coxa", this.coxa);
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
                cmm.Parameters.AddWithValue("@densidadeCorporal", this.densidadeCorporal);

                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            }

            public static void deletarPollock7(Int32 idPollock7)
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("delete from pollock7 ");
                sql.Append("where idPollock7 = @idPollock7");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idPollock7", idPollock7);

                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            }



            #endregion


    }


}