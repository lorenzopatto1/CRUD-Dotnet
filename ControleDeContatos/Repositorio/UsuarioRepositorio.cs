using ControleDeContatos.Data;
using ControleDeContatos.Models;
using Microsoft.EntityFrameworkCore;

namespace ControleDeContatos.Repositorio
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly BancoContext _context;
        public UsuarioRepositorio(BancoContext bancoContext)
        {
            _context = bancoContext;
        }
        public UsuarioModel BuscarPorLogin(string login)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.Login.ToUpper() == login.ToUpper());
        }
        public UsuarioModel BuscarPorEmailELogin(RedefinirSenhaModel redefinirSenhaModel)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.Email.ToUpper() == redefinirSenhaModel.Email.ToUpper() && usuario.Login.ToUpper() == redefinirSenhaModel.Login.ToUpper());
        }
        public UsuarioModel BuscarPorId(int id)
        {
            return _context.Usuarios.FirstOrDefault(usuario => usuario.Id == id);
        }
        public List<UsuarioModel> BuscarTodos()
        {
            return _context.Usuarios
                .Include(x => x.Contatos)
                .ToList();
        }
        public UsuarioModel Adicionar(UsuarioModel usuario)
        {
            usuario.DataCadastro = DateTime.Now;
            usuario.SetSenhaHash();
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return usuario;
        }
        public UsuarioModel Atualizar(UsuarioModel usuario)
        {
            UsuarioModel usuarioDb = BuscarPorId(usuario.Id);
            if (usuarioDb == null)
            {
                throw new Exception("Houve um erro na atualização do usuario");
            }
            usuarioDb.Nome = usuario.Nome;
            usuarioDb.Email = usuario.Email;
            usuarioDb.Login = usuario.Login;
            usuarioDb.Perfil = usuario.Perfil;
            usuarioDb.DataCadastroAlterado = DateTime.Now;

            _context.Usuarios.Update(usuarioDb);
            _context.SaveChanges();

            return usuarioDb;
        }
        public UsuarioModel AlterarSenha(AlterarSenhaModel alterarSenhaModel)
        {
            UsuarioModel usuarioDb = BuscarPorId(alterarSenhaModel.Id);
            if (usuarioDb == null) throw new Exception("Houve um erro na atualização da senha, usuário não encontrado");

            if (!usuarioDb.SenhaValida(alterarSenhaModel.SenhaAtual)) throw new Exception("Senha atual não confere");

            if (usuarioDb.SenhaValida(alterarSenhaModel.NovaSenha)) throw new Exception("Sua nova senha deve ser diferente da senha atual");

            usuarioDb.SetNovaSenha(alterarSenhaModel.NovaSenha);
            usuarioDb.DataCadastroAlterado = DateTime.Now;

            _context.Usuarios.Update(usuarioDb);
            _context.SaveChanges();

            return usuarioDb;

        }
        public bool Apagar(int id)
        {
            UsuarioModel usuarioDb = BuscarPorId(id);
            if (usuarioDb == null)
            {
                throw new Exception("Houve um erro na deleção do usuario");
            }
            _context.Remove(usuarioDb);
            _context.SaveChanges();
            return true;
        }
    }
}
