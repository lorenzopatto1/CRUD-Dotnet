using ControleDeContatos.Helper;
using ControleDeContatos.Models;
using ControleDeContatos.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeContatos.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISessao _sessao;

        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
        }
        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Sair()
        {
            _sessao.RemoverSessaoDoUsuario();
            return RedirectToAction("Index", "Login");
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
                            _sessao.CriarSessaoDoUsuario(usuarioDb);
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
