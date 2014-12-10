using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyPersonal.Models;
using Connections;
using MySql.Data.MySqlClient;

namespace EasyPersonal.Controllers
{
    public class AlunosController : Controller
    {
        // GET: Alunos
        public ActionResult mainAluno()
        {
            if(Session["idAluno"] == null)
            {
               return RedirectToAction("Index", "Login");
            }
            Treinadores.getTreinadorEspecifico(Convert.ToInt32(Session["idTreinador"]));
            Treinos.getTreinoAluno(Convert.ToInt32(Session["idAluno"]), Convert.ToInt32(Session["idTreinador"]));
            return View();
        }
        public ActionResult listaTreino()
        {
            if (Session["idAluno"] == null)
            {
                RedirectToAction("Index", "Login");
            }
            Treinos.getTreinoAluno(Convert.ToInt32(Session["idAluno"]), Convert.ToInt32(Session["idTreinador"]));
            return View(Treinos.listaTreinos);
        }
        public ActionResult editarAluno()
        {
            if (Session["idAluno"] == null)
            {
                RedirectToAction("Index", "Login");
            }
            Alunos edit = Alunos.getAlunoEspecifico(Convert.ToInt32(Session["idAluno"]));
            return View(edit);
        }
        [HttpPost]
        public ActionResult editarAluno(Alunos novo)
        {
            novo.editarAluno();
            return RedirectToAction("mainAluno");
        }
        public ActionResult alterarSenha()
        {
            if (Session["idAluno"] == null)
            {
                RedirectToAction("Index", "Login");
            }
            Alunos edit = Alunos.getAlunoEspecifico(Convert.ToInt32(Session["idAluno"]));
            return View(edit);
        }
        [HttpPost]
        public ActionResult alterarSenha(Alunos novo)
        {
            if (novo.senha == novo.confirmaSenha)
            {
                novo.idAluno = Convert.ToInt32(Session["idAluno"]);
                novo.senha = Alunos.encripta(novo.senha);
                novo.editarAluno();
            }
            else
            {
                ViewBag.AlertaSenha = "As senhas devem ser iguais !!";
                return View();
            }
            ViewBag.AlertaSenha = "Senha alterada com sucesso !!";
            return RedirectToAction("mainAluno");
        }

    }
}