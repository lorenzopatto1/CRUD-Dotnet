using System.ComponentModel.DataAnnotations;

namespace ControleDeContatos.Models
{
    public class AlterarSenhaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Por favor, digite a senha atual")]
        public string SenhaAtual { get; set; }
        [Required(ErrorMessage = "Por favor, digite a nova senha")]
        public string NovaSenha { get; set; }
        [Required(ErrorMessage = "Por favor, confirme a nova senha")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmarNovaSenha { get; set; }
    }
}
