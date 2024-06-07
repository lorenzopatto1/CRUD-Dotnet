using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ControleDeContatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;

        public LoginController(IUsuarioRepositorio usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Entrar(LoginModel loginModel)
        {
            try
            {
                if (ModelState.IsValid) 
                {
                UsuarioModel usuarioDb = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    if(usuarioDb != null)
                    {
                        if (usuarioDb.SenhaValida(loginModel.Senha))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    TempData["MensagemErro"] = "A senha está incorreta!";
                    }
                    TempData["MensagemErro"] = "Credenciais inválidas!";
                }
                return View("Index");
            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Não foi possivel realizar seu login, tente novamente! {error.Message}";
                return RedirectToAction("Index");
            }
        }
    }
}
