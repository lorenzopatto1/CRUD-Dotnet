using ControleDeContatos.Enums;
using ControleDeContatos.Helper;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ControleDeContatos.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Porfavor, preencha seu nome")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Porfavor, preencha seu login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Porfavor, preencha seu email")]
        [EmailAddress(ErrorMessage = "O email informado não está correto")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Porfavor, preencha seu perfil")]
        public PerfilEnum? Perfil { get; set; }
        [Required(ErrorMessage = "Porfavor, preencha sua senha")]
        public string Senha { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataCadastroAlterado { get; set; }
        public virtual List<ContatoModel> Contatos { get; set; }

        public Boolean SenhaValida(string senha)
        {
            return Senha == senha.GerarHash();
        }

        public void SetSenhaHash()
        {
            Senha = Senha.GerarHash();
        }

        public void SetNovaSenha(string novaSenha)
        {
            Senha = novaSenha.GerarHash();
        }

        public string GerarNovaSenha()
        {
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
            Senha = novaSenha.GerarHash();
            return novaSenha;
        }
    }
}
