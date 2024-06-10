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
        private readonly IEmail _email;

        public LoginController(IUsuarioRepositorio usuarioRepositorio, ISessao sessao, IEmail email)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _sessao = sessao;
            _email = email;
        }
        public IActionResult Index()
        {
            if (_sessao.BuscarSessaoDoUsuario() != null) return RedirectToAction("Index", "Home");

            return View();
        }
        public IActionResult RedefinirSenha()
        {
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
                UsuarioModel usuario = _usuarioRepositorio.BuscarPorLogin(loginModel.Login);

                    if(usuario != null)
                    {
                        if (usuario.SenhaValida(loginModel.Senha))
                        {
                            _sessao.CriarSessaoDoUsuario(usuario);
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
        [HttpPost]
        public IActionResult EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                if (ModelState.IsValid) 
                {
                UsuarioModel usuario = _usuarioRepositorio.BuscarPorEmailELogin(redefinirSenhaModel);

                    if(usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();
                        string mensagem = $"Sua nova senha é: {novaSenha}";

                        bool emailEnviado = _email.Enviar(redefinirSenhaModel.Email, "Sistema de Contatos - Nova Senha", mensagem);

                        if (emailEnviado)
                        {
                            _usuarioRepositorio.Atualizar(usuario);
                            TempData["MensagemSucesso"] = "Enviamos no seu email um link para redefinição de senha!";
                        }
                        else
                        {
                            TempData["MensagemErro"] = "Nao foi possivel enviar o email. Por favor, tente novamente.";
                        }

                        return RedirectToAction("Index", "Login");
                    }
                    TempData["MensagemErro"] = "Usuário não encontrado. Por favor, verifique os dados informados";
                }
                return View("RedefinirSenha");
            }
            catch (Exception error)
            {
                TempData["MensagemErro"] = $"Não foi possivel redefinir sua senha, tente novamente! {error.Message}";
                return View("RedefinirSenha");
            }
        }
    }
}
