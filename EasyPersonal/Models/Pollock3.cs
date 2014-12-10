using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MySql.Data.MySqlClient;

namespace EasyPersonal.Models
{
    public class Pollock3:Avaliacoes
    {
        #region Atributos
            public static List<Pollock3> listaPollock3 = new List<Pollock3>();
            public int idPollock3 { get; set; }
            public int idAluno { get; set; }
            public double coxa { get; set; }
            public double triceps { get; set; } 
            public double supraIliaca { get; set; }
           
        #endregion

        #region Métodos

         

            public void addPollock3( string sexo, int idade)
            {

                this.calculoPollock3(sexo,idade);

                this.arrumarDecimal();

                StringBuilder sql = new StringBuilder();

                
                sql.Append("insert into pollock3(excessoPeso,pesoMinimo,pesoMaximo,densidadeCorporal,porcentagemGordura,porcentagemMuscular,coxa,triceps,supraIliaca,dataAvaliacao,IMC,altura,pesoAtual,pesoGordo,pesoMagro,pesoOsseo,pesoMuscular,pesoResidual,idAluno) ");
                sql.Append("values(@excessoPeso,@pesoMinimo,@pesoMaximo,@densidadeCorporal,@porcentagemGordura,@porcentagemMuscular,@coxa,@triceps,@supraIliaca,@dataAvaliacao,@IMC,@altura,@pesoAtual,@pesoGordo,@pesoMagro,@pesoOsseo,@pesoMuscular,@pesoResidual,@idAluno)");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@excessoPeso", this.excessoPeso);
                cmm.Parameters.AddWithValue("@pesoMinimo", this.pesoMinimo);
                cmm.Parameters.AddWithValue("@pesoMaximo", this.pesoMaximo);
                cmm.Parameters.AddWithValue("@densidadeCorporal", this.densidadeCorporal);
                cmm.Parameters.AddWithValue("@porcentagemGordura", this.porcentagemGordura);
                cmm.Parameters.AddWithValue("@porcentagemMuscular", this.porcentagemMuscular);
                cmm.Parameters.AddWithValue("@coxa", this.coxa);
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

                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
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
            }
    

            public static List<Pollock3> getLista(Int32 idAluno, int aux)
            {
                
                Pollock3.listaPollock3.Clear();
              
                
                StringBuilder sql = new StringBuilder();

                sql.Append("select * from pollock3 ");
                sql.Append("where idAluno = @idAluno ");
                sql.Append("order by dataAvaliacao");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idAluno", idAluno);
                cmm.CommandText = sql.ToString();

                var dr = Connections.Mysql.MySql.getListaCmm(cmm);

                while (dr.Read())
                {
                    Pollock3 novo = new Pollock3();
                    novo.idPollock3 = (int)dr["idPollock3"];
                    novo.pesoMinimo = (double)dr["pesoMinimo"];
                    novo.excessoPeso = (double)dr["excessoPeso"];
                    novo.pesoMaximo = (double)dr["pesoMaximo"];
                    novo.porcentagemMuscular = (double)dr["porcentagemMuscular"];
                    novo.porcentagemGordura = (double)dr["porcentagemGordura"];
                    novo.densidadeCorporal = (double)dr["densidadeCorporal"];
                    novo.coxa = (double)dr["coxa"];
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
                    novo.idAluno = (int)dr["idAluno"];
                    Pollock3.listaPollock3.Add(novo);
                }

                dr.Dispose();
                Connections.Mysql.MySql.Desconectar();
                return Pollock3.listaPollock3;
            }
          
       

            public static Pollock3 getEspecifico(Int32 idPollock3)
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("select * from pollock3 ");
                sql.Append("where idPollock3 = @idPollock3");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idPollock3", idPollock3);

                cmm.CommandText = sql.ToString();

                var dr = Connections.Mysql.MySql.getListaCmm(cmm);
                Pollock3 novo = new Pollock3();
                if (dr.Read())
                {
                    novo.excessoPeso = (double)dr["excessoPeso"];
                    novo.pesoMinimo = (double)dr["pesoMinimo"];
                    novo.pesoMaximo = (double)dr["pesoMaximo"];
                    novo.porcentagemMuscular = (double)dr["porcentagemMuscular"];
                    novo.porcentagemGordura = (double)dr["porcentagemGordura"];
                    novo.densidadeCorporal = (double)dr["densidadeCorporal"];
                    novo.idPollock3 = (int)dr["idPollock3"];
                    novo.coxa = (double)dr["coxa"];
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
            
            private void calculoPollock3(string sexo, int idade)
            {
                if (this.triceps == null || this.supraIliaca == null || this.coxa == null)
                {
                    this.densidadeCorporal = 0;
                    this.porcentagemGordura = 0;
                }
                else
                {
                    double st = 0;

                    st = this.triceps + this.supraIliaca + this.coxa;

                    if (sexo == "m")
                    {
                        this.densidadeCorporal = (1.10938 - (0.0008267 * st) + (0.0000016 * (st * st) - (0.0002574 * idade)));
                    }
                    else
                    {
                        this.densidadeCorporal = (1.0994921 - (0.0009929 * st ) + (0.0000023 * (st * st) - (0.0001393 * idade)));
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

                sql.Append("update pollock3 ");
                sql.Append("set  excessoPeso = @excessoPeso, pesoMinimo= @pesoMinimo, pesoMaximo= @pesoMaximo, densidadeCorporal = @densidadeCorporal, porcentagemGordura = @porcentagemGordura, porcentagemMuscular = @porcentagemMuscular, idPollock3 = @idPollock3, coxa = @coxa, triceps = @triceps, supraIliaca = @supraIliaca, altura = @altura, dataAvaliacao = @dataAvaliacao, IMC = @IMC, pesoAtual = @pesoAtual, pesoGordo = @pesoGordo, pesoMagro = @pesoMagro, pesoOsseo = @pesoOsseo, pesoMuscular = @pesoMuscular, pesoResidual = @pesoResidual, idAluno = @idAluno ");
                sql.Append("where idPollock3 = @idPollock3");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@excessoPeso", this.excessoPeso);
                cmm.Parameters.AddWithValue("@pesoMinimo", this.pesoMinimo);
                cmm.Parameters.AddWithValue("@pesoMaximo", this.pesoMaximo);
                cmm.Parameters.AddWithValue("@densidadeCorporal", this.densidadeCorporal);
                cmm.Parameters.AddWithValue("@porcentagemGordura", this.porcentagemGordura);
                cmm.Parameters.AddWithValue("@porcentagemMuscular", this.porcentagemMuscular);
                cmm.Parameters.AddWithValue("@idPollock3", this.idPollock3);
                cmm.Parameters.AddWithValue("@coxa", this.coxa);
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

                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            }

            public static void deletarPollock3(Int32 idPollock3)
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("delete from pollock3 ");
                sql.Append("where idPollock3 = @idPollock3");

                MySqlCommand cmm = new MySqlCommand();

                cmm.Parameters.AddWithValue("@idPollock3", idPollock3);

                cmm.CommandText = sql.ToString();

                Connections.Mysql.MySql.ExecutarComandoCmm(cmm);
            }

        #endregion
    }
}