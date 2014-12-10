using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyPersonal.Models;

namespace EasyPersonal.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Treinadores loginTre)
        {
            if (loginTre.email == null || loginTre.senha == null)
            {
                ViewBag.Alerta = "E-mail ou senha incorretos!";
                return View();
            }
            else
            {

                Treinadores treinadorLogado = loginTre.loginTreinador(loginTre.email, loginTre.senha);
                if (treinadorLogado != null)
                {
                     Session["idAluno"] = null;
                     Session["idTreinador"] = treinadorLogado.idTreinador;
                     Session["nome"] = treinadorLogado.nome;
                     Session["telefone"] = treinadorLogado.telefone;
                     Session["email"] = treinadorLogado.email;
                     Session["senha"] = treinadorLogado.senha;
                     Session["idade"] = treinadorLogado.idade;
                     Session["cidade"] = treinadorLogado.cidade;
                     Session["sexo"] = treinadorLogado.sexo;
                    return RedirectToAction("mainTreinador", "Treinadores");
                }
                else
                {
                    Alunos alunoLogado = new Alunos();
                    alunoLogado = alunoLogado.loginAluno(loginTre.email, loginTre.senha);
                    if (alunoLogado != null)
                    {
                         Session["idAluno"] = alunoLogado.idAluno;
                         Session["idTreinador"] = alunoLogado.idTreinador;
                         Session["nome"] = alunoLogado.nome;
                         Session["telefone"] = alunoLogado.telefone;
                         Session["email"] = alunoLogado.email;
                         Session["senha"] = alunoLogado.senha;
                         Session["idade"] = alunoLogado.idade;
                         Session["cidade"] = alunoLogado.cidade;
                         Session["sexo"] = alunoLogado.sexo;
                    return RedirectToAction("mainAluno", "Alunos");
                    }
                    else
                    {
                        ViewBag.Alerta = "E-mail ou senha incorretos!";
                        return View();
                    }
                }
            }
       }

        public ActionResult errorLogin()
        {
            return View();
        }
    }
}