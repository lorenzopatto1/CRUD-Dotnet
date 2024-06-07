using ControleDeContatos.Enums;
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

        public Boolean SenhaValida(string senha)
        {
            return senha == Senha;
        }
    }
}
