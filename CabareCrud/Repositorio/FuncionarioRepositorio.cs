using BusesControl.Data;
using BusesControl.Helper;
using BusesControl.Models;
using BusesControl.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusesControl.Repositorio {
    public class FuncionarioRepositorio : IFuncionarioRepositorio {
        private readonly BancoContext _bancocontext;
        private readonly IEmail _email;
        public FuncionarioRepositorio(BancoContext bancocontext, IEmail email) {
            _bancocontext = bancocontext;
            _email = email;
        }

        public List<Funcionario> ListarTodosHab() {
            var buscar = _bancocontext.Funcionario.ToList();
            return buscar.Where(x => x.Status == StatuFuncionario.Habilitado).ToList();
        }
        public List<Funcionario> ListarTodosDesa() {
            var buscar = _bancocontext.Funcionario.ToList();
            return buscar.Where(x => x.Status == StatuFuncionario.Desabilitado).ToList();
        }

        public Funcionario Adicionar(Funcionario funcionario) {
            try {
                _bancocontext.Funcionario.Add(funcionario);
                _bancocontext.SaveChanges();
                return funcionario;
            }
            catch (Exception erro) {
                TratarErro(funcionario, erro);
                return null;
            }
        }
        public Funcionario EditarFuncionario(Funcionario funcionario) {
            try {
                Funcionario funcionarioDB = ListarPorId(funcionario.Id);
                if (funcionarioDB == null) {
                    throw new System.Exception("Desculpe, houve alguma falha na aplicação.");
                }
                if (funcionario.Cargos != CargoFuncionario.Motorista && string.IsNullOrEmpty(funcionarioDB.Senha)) {
                    funcionarioDB.Senha = funcionario.GerarSenha();
                    bool emailEnviado = EnviarSenha(funcionario.Name, funcionarioDB.Senha, funcionario.Email);
                    if (!emailEnviado) {
                        throw new System.Exception("Não conseguimos enviar o e-mail com a senha, " +
                                "valide se ele é existente.");
                    }
                }
                if (funcionario.Cargos == CargoFuncionario.Motorista) {
                    funcionarioDB.StatusUsuario = UsuarioStatus.Desativado;
                }

                funcionarioDB.Name = funcionario.Name;
                funcionarioDB.DataNascimento = funcionario.DataNascimento;
                funcionarioDB.Cpf = funcionario.Cpf;
                funcionarioDB.Email = funcionario.Email;
                funcionarioDB.Telefone = funcionario.Telefone;
                funcionarioDB.Cep = funcionario.Cep;
                funcionarioDB.Logradouro = funcionario.Logradouro;
                funcionarioDB.NumeroResidencial = funcionario.NumeroResidencial;
                funcionarioDB.ComplementoResidencial = funcionario.ComplementoResidencial;
                funcionarioDB.Ddd = funcionario.Ddd;
                funcionarioDB.Bairro = funcionario.Bairro;
                funcionarioDB.Cidade = funcionario.Cidade;
                funcionarioDB.Estado = funcionario.Estado;
                funcionarioDB.Cargos = funcionario.Cargos;
                _bancocontext.Update(funcionarioDB);
                _bancocontext.SaveChanges();
                return funcionario;
            }
            catch (Exception erro) {
                TratarErro(funcionario, erro);
                return null;
            }
        }

        public Funcionario ListarPorId(long id) {
            Funcionario funcionario = _bancocontext.Funcionario.FirstOrDefault(x => x.Id == id);
            return funcionario;
        }
        public Funcionario ListarPorlogin(string cpf) {
            return _bancocontext.Funcionario.FirstOrDefault(x => x.Cpf == cpf && x.StatusUsuario == UsuarioStatus.Ativado);
        }
        public Funcionario ListarPorloginAndEmail(string email, string login) {
            return _bancocontext.Funcionario.FirstOrDefault(x => x.Email == email && x.Cpf == login && x.StatusUsuario == UsuarioStatus.Ativado);
        }
        public Funcionario Desabilitar(Funcionario funcionario) {
            Funcionario funcionarioDesabilitado = ListarPorId(funcionario.Id);
            if (funcionarioDesabilitado == null) throw new System.Exception("Desculpe, houve um erro ao desabilitar.");
            funcionarioDesabilitado.Status = StatuFuncionario.Desabilitado;
            funcionarioDesabilitado.StatusUsuario = UsuarioStatus.Desativado;
            _bancocontext.Update(funcionarioDesabilitado);
            _bancocontext.SaveChanges();
            return funcionario;
        }

        public Funcionario Habilitar(Funcionario funcionario) {
            Funcionario funcionarioHabilitado = ListarPorId(funcionario.Id);
            if (funcionarioHabilitado == null) throw new System.Exception("Desculpe, houve um erro ao habilitar");
            funcionarioHabilitado.Status = StatuFuncionario.Habilitado;
            _bancocontext.Update(funcionarioHabilitado);
            _bancocontext.SaveChanges();
            return funcionario;
        }
        public Funcionario DesabilitarUsuario(Funcionario funcionario) {
            Funcionario usuarioDesabilitado = ListarPorId(funcionario.Id);
            if (usuarioDesabilitado == null) throw new System.Exception("Desculpe, houve um erro ao desabilitar.");
            usuarioDesabilitado.StatusUsuario = UsuarioStatus.Desativado;
            _bancocontext.Update(usuarioDesabilitado);
            _bancocontext.SaveChanges();
            return funcionario;
        }
        public Funcionario HabilitarUsuario(Funcionario funcionario) {
            Funcionario usuarioHabilitado = ListarPorId(funcionario.Id);
            if (usuarioHabilitado == null) throw new System.Exception("Desculpe, houve um erro ao habilitar.");
            usuarioHabilitado.StatusUsuario = UsuarioStatus.Ativado;
            _bancocontext.Update(usuarioHabilitado);
            _bancocontext.SaveChanges();
            return funcionario;
        }
        public Funcionario AlterarSenha(MudarSenha mudarSenha) {
            Funcionario usuarioDB = ListarPorId(mudarSenha.Id);
            if (usuarioDB == null) throw new System.Exception("Desculpe, houve um erro ao trocar a senha.");
            if (!usuarioDB.ValidarSenha(mudarSenha.SenhaAtual)) throw new System.Exception("Senha atual inválida!");
            if (usuarioDB.ValidarDuplicataSenha(mudarSenha.NovaSenha)) throw new System.Exception("A nova senha não pode ser igual a atual!");
            usuarioDB.Senha = mudarSenha.NovaSenha;
            _bancocontext.Update(usuarioDB);
            _bancocontext.SaveChanges();
            return usuarioDB;
        }
        public Funcionario NovaSenha(Funcionario usuario) {
            Funcionario usuarioDB = ListarPorId(usuario.Id);
            if (usuarioDB == null) {
                throw new System.Exception("Desculpe, houve um erro ao trocar a senha.");
            }
            usuarioDB.Senha = usuario.Senha;
            _bancocontext.Update(usuarioDB);
            _bancocontext.SaveChanges();
            return usuarioDB;
        }
        public Exception TratarErro(Funcionario funcionario, Exception erro) {
            if (erro.InnerException.Message.Contains(funcionario.Cpf)) {
                throw new System.Exception("Funcionário já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(funcionario.Email)) {
                throw new System.Exception("Funcionário já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(funcionario.Telefone)) {
                throw new System.Exception("Funcionário já se encontra cadastrado!");
            }
            throw new System.Exception($"Desculpe, houve alguma falha na aplicação!");
        }

        public bool EnviarSenha(string name, string senha, string email) {
            string mensagem = $"Informamos que foi gerado uma senha para o usuário {name}. <br> A senha gerada para o usuário é: <strong>{senha}<strong/>";
            if (_email.Enviar(email, "Buses Control - Gerador de senhas", mensagem)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
