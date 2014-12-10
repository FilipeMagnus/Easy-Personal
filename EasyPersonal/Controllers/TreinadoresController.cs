using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyPersonal.Models;
using MySql.Data.MySqlClient;
using Connections.Mysql;

namespace EasyPersonal.Controllers
{
    public class TreinadoresController : Controller
    {
        // GET: Treinadores
       
        public ActionResult ListaTreinadores()
        {
            Treinadores.getTreinadores();
            return View(Treinadores.listaTreinadores);
        }

        
        #region cadastraTreinador
        
        public ActionResult cadastraTreinador()
        {
            return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult cadastraTreinador(Treinadores novo)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                if (novo.senha != novo.confirmaSenha)
                {
                    ViewBag.AlertaCorretamente = "Informe a seha corretamente!";
                    return View();
                }

                Treinadores.addTreinador(novo);
                Treinadores treinador = Treinadores.getUltimoTreinador();
               // ExerciciosTreinadores.addExerciciosTreinador(treinador.idTreinador);
                Session["idAluno"] = null;
                Session["idTreinador"] = treinador.idTreinador;
                Session["nome"] = treinador.nome;
                Session["telefone"] = treinador.telefone;
                Session["email"] = treinador.email;
                Session["senha"] = treinador.senha;
                Session["idade"] = treinador.idade;
                Session["cidade"] = treinador.cidade;
                Session["sexo"] = treinador.sexo;
                return RedirectToAction("mainTreinador", "Treinadores");
            }
        }

        #endregion

        public ActionResult mainTreinador()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }


            Treinadores.getTreinadorEspecifico(Convert.ToInt32(Session["idTreinador"]));
            if (Session["idTreinador"] != null)
            {
                if (DateTime.Now.Day == 1 || Treinadores.listaTreinadores[0].parcelaGerada == (DateTime.Now.Month-1))
                {
                    Pagamentos.gerarParcelas(Convert.ToInt32(Session["idTreinador"]), Convert.ToString(Session["senha"]));

                }
            }


            Pagamentos.getListaMain(Convert.ToInt32(Session["idTreinador"]));
            ExerciciosTreinadores.getLista(Convert.ToInt32(Session["idTreinador"]));
            Alunos.getAlunosTreinador(Convert.ToInt32(Session["idTreinador"]));
            return View();
        }

        #region addAluno
        
        public ActionResult addAluno()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        [HttpPost]
        public ActionResult addAluno(Alunos novo)
        {
            
                novo.senha = novo.email;
                novo.idTreinador = Convert.ToInt32(Session["idTreinador"]);
                Alunos.addAluno(novo);
                return RedirectToAction("listaAluno", "Treinadores");
            
        }

        #endregion


        public ActionResult listaAluno()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            int idTreinador = Convert.ToInt32(Session["idTreinador"]); 
            Alunos.getAlunosTreinador(idTreinador);
            return View(Alunos.listaAlunos);
        }



        #region editarAluno
        
        public ActionResult editarAluno()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idAlunoEditado"] = Convert.ToInt32(RouteData.Values["id"]);
            }
            Alunos edit = Alunos.getAlunoEspecifico(Convert.ToInt32(Session["idAlunoEditado"]));
            if (edit != null)
            {
                Session["idTreinadorAluno"] = edit.idTreinador;
            }
            edit.idAluno = Convert.ToInt32(Session["idAlunoEditado"]);
            edit.idTreinador = Convert.ToInt32(Session["idTreinador"]);
            return View(edit);
        }
        [HttpPost]
        public ActionResult editarAluno(Alunos editado)
        {


            editado.editarAluno();
            Session["idAlunoEditado"] = null;
            Session["idTreinadorAluno"] = null;
            return RedirectToAction("listaAluno");
        }
        #endregion

        #region deletarAluno
        
        public ActionResult deletarAluno()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idAlunoDeletado"] = Convert.ToInt32(RouteData.Values["id"]);
            }
            Alunos delet = Alunos.getAlunoEspecifico(Convert.ToInt32(Session["idAlunoDeletado"]));
           
            return View(delet);
        }
        [HttpPost]
        public ActionResult deletarAluno(Alunos del)
        {
            del.deletarAluno(Convert.ToInt32(Session["idAlunoDeletado"]));
            Session["idAlunoDeletado"] = null;
            return RedirectToAction("listaAluno");
        }

        #endregion

        public ActionResult treinosAluno()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idAlunoTreinos"] = Convert.ToInt32(RouteData.Values["id"]);
            }
            Alunos novo = Alunos.getAlunoEspecifico(Convert.ToInt32(Session["idAlunoTreinos"]));
            Session["SexoAluno"] = novo.sexo;
            Session["NomeAluno"] = novo.nome;
            Treinos.getTreinoAluno(Convert.ToInt32(Session["idAlunoTreinos"]), Convert.ToInt32(Session["idTreinador"]));
            ExerciciosTreinadores.getLista(Convert.ToInt32(Session["idTreinador"]));
            return View(Treinos.listaTreinos);
        }


        [HttpPost]
        public ActionResult treinosAluno(int dia)
        {
            Treinos.getTreinoFiltrado(Convert.ToInt32(Session["idAlunoTreinos"]), Convert.ToInt32(Session["idTreinador"]), dia);
            return View(Treinos.listaTreinos);
        }


        #region addTreino
        
        public ActionResult addTreino()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            ExerciciosTreinadores.getLista(Convert.ToInt32(Session["idTreinador"]));
            return View();
        }
        [HttpPost]
        public ActionResult addTreino(Treinos novo, int? seg, int? ter, int? qua, int? qui, int? sex, int? sab, int? dom)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                if (seg != null)
                {
                    novo.diaSemana = Convert.ToInt32(seg);
                    Treinos.addTreino(novo);
                } 
                if (ter != null)
                {
                    novo.diaSemana = Convert.ToInt32(ter);
                    Treinos.addTreino(novo);
                }
                if (qua != null)
                {
                    novo.diaSemana = Convert.ToInt32(qua);
                    Treinos.addTreino(novo);
                }
                if (qui != null)
                {
                    novo.diaSemana = Convert.ToInt32(qui);
                    Treinos.addTreino(novo);
                }
                if (sex != null)
                {
                    novo.diaSemana = Convert.ToInt32(sex);
                    Treinos.addTreino(novo);
                }
                if (sab != null)
                {
                    novo.diaSemana = Convert.ToInt32(sab);
                    Treinos.addTreino(novo);
                }
                if (dom != null)
                {
                    novo.diaSemana = Convert.ToInt32(dom);
                    Treinos.addTreino(novo);
                }


               
                return RedirectToAction("treinosAluno");
            }

        }

        #endregion

        #region editarTreino
        
        public ActionResult editarTreino()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idTreinoEditar"] = Convert.ToInt32(RouteData.Values["id"]);
            }
            Treinos editar = Treinos.getTreinoEspecifico(Convert.ToInt32(Session["idTreinoEditar"]));
            Session["idExercicioEditar"] = editar.idExerciciosTreinadores;
            Session["idAlunoEditar"] = editar.idAluno;
            ExerciciosTreinadores.getLista(Convert.ToInt32(Session["idTreinador"]));
            return View(editar);
        }

        [HttpPost]
        public ActionResult editarTreino(Treinos editado)
        {
            editado.idTreino = Convert.ToInt32(Session["idTreinoEditar"]);
            editado.editarTreino();
            return RedirectToAction("treinosAluno", "Treinadores");
        }

        #endregion

        #region deletarTreino
        
        public ActionResult deletarTreino()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idTreinoAlunoDeletado"] = Convert.ToInt32(RouteData.Values["id"]);
            }
            Treinos deletar = Treinos.getTreinoEspecifico(Convert.ToInt32(Session["idTreinoAlunoDeletado"]));
            return View(deletar);
        }

        [HttpPost]
        public ActionResult deletarTreino(Treinos deletar)
        {
            Treinos.deletarTreino(Convert.ToInt32(Session["idTreinoAlunoDeletado"]));
            Session["idTreinoAlunoDeletado"] = null;
            return RedirectToAction("treinosAluno", "Treinadores");
        }

        #endregion
  

        public ActionResult editarTreinador()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            Treinadores.getTreinadorEspecifico(Convert.ToInt32(Session["idTreinador"]));
            return View(Treinadores.listaTreinadores[0]);
        }

        [HttpPost]
        public ActionResult editarTreinador(Treinadores novo)
        {
            novo.senha = Convert.ToString(Session["senha"]);
            novo.senha = Treinadores.encripta(novo.senha);
            novo.editarTreinador();
            Session["idTreinador"] = novo.idTreinador;
            Session["nome"] = novo.nome;
            Session["telefone"] = novo.telefone;
            Session["email"] = novo.email;
            Session["senha"] = novo.senha;
            Session["idade"] = novo.idade;
            Session["cidade"] = novo.cidade;
            Session["sexo"] = novo.sexo;

            return RedirectToAction("mainTreinador");
        }

        public ActionResult alterarSenha()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            Treinadores.getTreinadorEspecifico(Convert.ToInt32(Session["idTreinador"]));
            return View(Treinadores.listaTreinadores[0]);
        }

        [HttpPost]
        public ActionResult alterarSenha(Treinadores novo)
        {
            if (novo.senha != novo.confirmaSenha)
            {
                ViewBag.AlertaSenhaIncorreta = "Senha incorreta!";
                return View();
            }
            else
            {
                ViewBag.AlertaSenhaSucesso = "Senha alterada com sucesso!";
            }
            novo.senha = Treinadores.encripta(novo.senha);
            novo.editarTreinador();
            return RedirectToAction("mainTreinador");
        }

        public ActionResult listaExercicios()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            Int32 idTreinador = Convert.ToInt32(Session["idTreinador"]);
            ExerciciosTreinadores.getLista(idTreinador);
            return View(ExerciciosTreinadores.listaExerciciosTreinadores);
        }

        [HttpPost]
        public ActionResult listaExercicios(string buscando)
        {
            ExerciciosTreinadores.buscaExercicio(buscando, Convert.ToInt32(Session["idTreinador"]));
             return View(ExerciciosTreinadores.listaExerciciosTreinadores);
        }

        #region editarExercicio
        

        public ActionResult editarExercicio()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idExercicioEditar"] = Convert.ToInt32(RouteData.Values["id"]);
            } ExerciciosTreinadores edit = ExerciciosTreinadores.getExercicioEspecifico(Convert.ToInt32(Session["idExercicioEditar"]), Convert.ToInt32(Session["idTreinador"]));
            return View(edit);
        }
        [HttpPost]
        public ActionResult editarExercicio(ExerciciosTreinadores editado)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                editado.editarExercicio();
                return RedirectToAction("listaExercicios", "Treinadores");
            }
        }

        #endregion

        #region deletarExercicio
        
        public ActionResult deletarExercicio()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idExercicioDeletar"] = Convert.ToInt32(RouteData.Values["id"]);
            } ExerciciosTreinadores delet = ExerciciosTreinadores.getExercicioEspecifico(Convert.ToInt32(Session["idExercicioDeletar"]), Convert.ToInt32(Session["idTreinador"]));
            Session["idTreinadorDeletar"] = Convert.ToInt32(delet.idTreinador);
            return View(delet);
        }
        [HttpPost]
        public ActionResult deletarExercicio(ExerciciosTreinadores delet)
        {
            ExerciciosTreinadores.deletarExercicio(Convert.ToInt32(Session["idTreinadorDeletar"]), Convert.ToInt32(Session["idExercicioDeletar"]));
            return RedirectToAction("listaExercicios","Treinadores");
        }

        #endregion

        #region addExercicio
        
        public ActionResult addExercicio()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            Session["idExercicioAdd"] = ExerciciosTreinadores.getUltimoID(Convert.ToInt32(Session["idTreinador"]));
            return View();
        }
        [HttpPost]
        public ActionResult addExercicio(ExerciciosTreinadores novo)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                novo.addExercicio();
                return RedirectToAction("listaExercicios", "Treinadores");
            }
        }

        #endregion
        public ActionResult pagamentoAluno()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idPagamentoAluno5"] = Convert.ToInt32(RouteData.Values["id"]);
                Alunos novo = Alunos.getAlunoEspecifico(Convert.ToInt32(Session["idPagamentoAluno5"]));
                Session["SexoAluno"] = novo.sexo;
                Session["NomeAluno"] = novo.nome;
            } Pagamentos.getLista(Convert.ToInt32(Session["idPagamentoAluno5"]));
            return View(Pagamentos.listaPagamentos);
        }

        public ActionResult quitarPagamento()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idPagamentoAluno2"] = Convert.ToInt32(RouteData.Values["id"]);
            }
            Pagamentos editar = Pagamentos.getEspecifico(Convert.ToInt32(Session["idPagamentoAluno2"]));
            editar.status = "Pago";
            editar.diaPagado = DateTime.Now.Day;
            editar.diaPago = Convert.ToString(DateTime.Now.Day) +"/"+ Convert.ToString(DateTime.Now.Month) + "/" + Convert.ToString(DateTime.Now.Year);
            editar.editarPagamento();
            return RedirectToAction("pagamentoAluno");
        }

        public ActionResult listaTodosPagamentos()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            Pagamentos.getTodos(Convert.ToInt32(Session["idTreinador"]));
            return View(Pagamentos.listaPagamentos);
        }


        #region addPagamento
            public ActionResult addPagamento()
            {
                if (Session["idTreinador"] == null && Session["idAluno"] == null)
                {
                    ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                    return RedirectToAction("Index", "Login");
                }
                return View();
            }

            [HttpPost]
            public ActionResult addPagamento(Pagamentos novo)
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                else
                {
                    novo.idAluno = Convert.ToInt32(Session["idPagamentoAluno5"]);
                    novo.parcelas--;
                    novo.diaPago = "";
                    novo.comeca = DateTime.Now;
                    novo.termina = Convert.ToInt32(DateTime.Now.Month) + novo.parcelas;
                    Pagamentos.addPagamento(novo);
                    return RedirectToAction("pagamentoAluno");
                }
            }

        #endregion

        #region editarPagamento
            public ActionResult editarPagamento()
            {
                if (Session["idTreinador"] == null && Session["idAluno"] == null)
                {
                    ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                    return RedirectToAction("Index", "Login");
                }
                if (RouteData.Values["id"] != null)
                {
                    Session["idPagamentoAluno"] = Convert.ToInt32(RouteData.Values["id"]);
                } 
                Pagamentos editar = Pagamentos.getEspecifico(Convert.ToInt32(Session["idPagamentoAluno"]));
                return View(editar);
            }

            [HttpPost]
            public ActionResult editarPagamento(Pagamentos editar)
            {

                if (!ModelState.IsValid)
                {
                    return View();
                }
                else
                {
                   string aux = "";
                   if (editar.diaPagado > 0 && editar.diaPagado < 10)
                    {
                        aux =  editar.diaPago.Substring(1,8);
                        editar.diaPago = editar.diaPagado + aux;
                    }
                    else
                    {
                        aux = editar.diaPago.Substring(2, 8);
                        editar.diaPago = editar.diaPagado + aux;
                    }


                    editar.editarPagamento();
                    return RedirectToAction("pagamentoAluno");
                }
            }
        #endregion

        #region deletarPagamento
            public ActionResult deletarPagamento()
            {
                if (Session["idTreinador"] == null && Session["idAluno"] == null)
                {
                    ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                    return RedirectToAction("Index", "Login");
                }
                if (RouteData.Values["id"] != null)
                {
                    Session["idPagamentoDeletar"] = Convert.ToInt32(RouteData.Values["id"]);
                } 
                Pagamentos delet = Pagamentos.getEspecifico(Convert.ToInt32(Session["idPagamentoDeletar"]));
                return View(delet);
            }
            
            [HttpPost]
            public ActionResult deletarPagamento(Pagamentos delet)
            {
                Pagamentos.deletarPagamento(Convert.ToInt32(Session["idPagamentoAluno"]));
                return RedirectToAction("pagamentoAluno");
            }

        #endregion

        #region AvaliaçãoAluno

            public ActionResult avaliacaoAluno()
            {
                if (Session["idTreinador"] == null && Session["idAluno"] == null)
                {
                    ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                    return RedirectToAction("Index", "Login");
                }
                if (RouteData.Values["id"] != null)
                {
                    Session["idAlunoAvaliacao"] = Convert.ToInt32(RouteData.Values["id"]);
                    Alunos novo = Alunos.getAlunoEspecifico(Convert.ToInt32(Session["idAlunoAvaliacao"]));
                    Session["SexoAluno"] = novo.sexo;
                    Session["NomeAluno"] = novo.nome;
           
                }
                return View();
            }
            #region Pollock3
                public ActionResult listaPollock3()
                {
                    if (Session["idTreinador"] == null && Session["idAluno"] == null)
                    {
                        ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                        return RedirectToAction("Index", "Login");
                    }
                    var lista = Pollock3.getLista(Convert.ToInt32(Session["idAlunoAvaliacao"]), 0);
                    return View(lista);
                }
                
                [HttpPost]
                public ActionResult listaPollock3(int grafico)
                {
                    Session["grafico"] = grafico;
                    return RedirectToAction("graficoPollock3");
                }
                public ActionResult addPollock3()
                {
                    if (Session["idTreinador"] == null && Session["idAluno"] == null)
                    {
                        ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                        return RedirectToAction("Index", "Login");
                    }
                    return View();
                }
            
            [HttpPost]
                public ActionResult addPollock3(Pollock3 novo)
                {
                    novo.idAluno = Convert.ToInt32(Session["idAlunoAvaliacao"]);
                    Alunos aluno = Alunos.getAlunoEspecifico(Convert.ToInt32(Session["idAlunoAvaliacao"]));
                    novo.addPollock3(Convert.ToString(aluno.sexo),Convert.ToInt16(aluno.idade));
                    return RedirectToAction("listaPollock3");
                }

            public ActionResult editarPollock3()
            {
                if (Session["idTreinador"] == null && Session["idAluno"] == null)
                {
                    ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                    return RedirectToAction("Index", "Login");
                }
                if (RouteData.Values["id"] != null)
                {
                    Session["idPollock3"] = Convert.ToInt32(RouteData.Values["id"]);
                } 
                Pollock3 edit = Pollock3.getEspecifico(Convert.ToInt32(Session["idPollock3"]));
                return View(edit);
            }

        [HttpPost]
            public ActionResult editarPollock3(Pollock3 editado)
            {
                editado.editar();
                return RedirectToAction("listaPollock3");
            }

            public ActionResult deletarPollock3()
            {
                if (Session["idTreinador"] == null && Session["idAluno"] == null)
                {
                    ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                    return RedirectToAction("Index", "Login");
                }
                if (RouteData.Values["id"] != null)
                {
                    Session["idPollock3Deletar"] = Convert.ToInt32(RouteData.Values["id"]);
                }
                Pollock3 delet = Pollock3.getEspecifico(Convert.ToInt32(Session["idPollock3Deletar"]));
                return View(delet);        
            }
            
        [HttpPost]
            public ActionResult deletarPollock3(Pollock3 delet)
            {
                Pollock3.deletarPollock3(Convert.ToInt32(Session["idPollock3"]));
                return RedirectToAction("listaPollock3");
            }
            #endregion

           #region Pollock7
        public ActionResult listaPollock7()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            var lista = Pollock7.getLista(Convert.ToInt32(Session["idAlunoAvaliacao"]),0);
            return View(lista);
        }
        [HttpPost]
        public ActionResult listaPollock7(int grafico)
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            Session["grafico"] = grafico;
            return RedirectToAction("graficoPollock7");
        }

        public ActionResult addPollock7()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult addPollock7(Pollock7 novo)
        {
            novo.idAluno = Convert.ToInt32(Session["idAlunoAvaliacao"]);
            Alunos aluno = Alunos.getAlunoEspecifico(novo.idAluno);
            Pollock7.addPollock7(novo,aluno.sexo,aluno.idade);
            return RedirectToAction("listaPollock7");
        }

        public ActionResult editarPollock7()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idPollock7Editar"] = Convert.ToInt32(RouteData.Values["id"]);
            }
            Pollock7 edit = Pollock7.getEspecifico(Convert.ToInt32(Session["idPollock7Editar"]));
            return View(edit);
        }

        [HttpPost]
        public ActionResult editarPollock7(Pollock7 editado)
        {
            editado.editar();
            return RedirectToAction("listaPollock7");
        }

        public ActionResult deletarPollock7()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idPollock7Deletar"] = Convert.ToInt32(RouteData.Values["id"]);
            }
            Pollock7 delet = Pollock7.getEspecifico(Convert.ToInt32(Session["idPollock7Deletar"]));
            return View(delet);
        }

        [HttpPost]
        public ActionResult deletarPollock7(Pollock7 delet)
        {
            Pollock7.deletarPollock7(Convert.ToInt32(Session["idPollock7Deletar"]));
            return RedirectToAction("listaPollock7");
        }
        #endregion

        #region Faulkner
        public ActionResult listaFaulkner()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            var lista = Faulkner.getLista(Convert.ToInt32(Session["idAlunoAvaliacao"]),0);
            return View(lista);
        }
        [HttpPost]
        public ActionResult listaFaulkner(int grafico)
        {
            Session["grafico"] = grafico;
            return RedirectToAction("graficoFaulkner");
        }
        public ActionResult addFaulkner()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult addFaulkner(Faulkner novo)
        {
            novo.idAluno = Convert.ToInt32(Session["idAlunoAvaliacao"]);
            Alunos aluno = Alunos.getAlunoEspecifico(Convert.ToInt32(Session["idAlunoAvaliacao"]));
            Faulkner.addFaulkner(novo,Convert.ToString(aluno.sexo), Convert.ToInt16(aluno.idade));
            return RedirectToAction("listaFaulkner");
        }

        public ActionResult editarFaulkner()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idFaulknerEditar"] = Convert.ToInt32(RouteData.Values["id"]);
            }
            Faulkner edit = Faulkner.getEspecifico(Convert.ToInt32(Session["idFaulknerEditar"]));
            return View(edit);
        }

        [HttpPost]
        public ActionResult editarFaulkner(Faulkner editado)
        {
            editado.idFaulkner = Convert.ToInt32(Session["idFaulknerEditar"]);
            editado.idAluno = Convert.ToInt32(Session["idAlunoAvaliacao"]);
            editado.editar();
            return RedirectToAction("listaFaulkner");
        }

        public ActionResult deletarFaulkner()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            if (RouteData.Values["id"] != null)
            {
                Session["idFaulknerDeletar"] = Convert.ToInt32(RouteData.Values["id"]);
            }
            Faulkner delet = Faulkner.getEspecifico(Convert.ToInt32(Session["idFaulknerDeletar"]));
            return View(delet);
        }

        [HttpPost]
        public ActionResult deletarFaulkner(Faulkner delet)
        {
            Faulkner.deletarFaulkner(Convert.ToInt32(Session["idFaulknerDeletar"]));
            return RedirectToAction("listaFaulkner");
        }
        #endregion

        public ActionResult graficoPollock3()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            return View();
           
        }

        public JsonResult preencherJsonGraficoP3()
        {
            Pollock3.getLista(Convert.ToInt32(Session["idAlunoAvaliacao"]), Convert.ToInt32(Session["grafico"]));
            return Json(Pollock3.listaPollock3, JsonRequestBehavior.AllowGet);
        }

        public ActionResult graficoPollock7()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            return View();

        }
        
        public JsonResult preencherJsonGraficoP7()
        {
            Pollock7.getLista(Convert.ToInt32(Session["idAlunoAvaliacao"]), Convert.ToInt32(Session["grafico"]));
            return Json(Pollock7.listaPollock7, JsonRequestBehavior.AllowGet);
        }

        public ActionResult graficoFaulkner()
        {
            if (Session["idTreinador"] == null && Session["idAluno"] == null)
            {
                ViewBag.AlertaNecessitaLogin = "Por favor, realize o login!";
                return RedirectToAction("Index", "Login");
            }
            return View();

        }

        public JsonResult preencherJsonFaulkner()
        {
            Faulkner.getLista(Convert.ToInt32(Session["idAlunoAvaliacao"]), Convert.ToInt32(Session["grafico"]));
            return Json(Faulkner.listaFaulkner, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}