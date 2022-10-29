using BusesControl.Data;
using BusesControl.Helper;
using BusesControl.Models;
using BusesControl.Models.Enums;
using Microsoft.EntityFrameworkCore;
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
        public List<Funcionario> ListarTodosMotoristasHab() {
            var buscar = _bancocontext.Funcionario.ToList();
            return buscar.Where(x => x.Status == StatuFuncionario.Habilitado && x.Cargos == CargoFuncionario.Motorista).ToList();
        }
        public Funcionario Adicionar(Funcionario funcionario) {
            try {
                if (Duplicata(funcionario)) {
                    throw new Exception("Funcionário já se encontra cadastrado!");
                }
                funcionario = TrimFuncionario(funcionario);
                if (funcionario.Cargos != CargoFuncionario.Motorista) {
                    funcionario.Senha = funcionario.GerarSenha();
                    bool emailEnviado = EnviarSenha(funcionario.Name, funcionario.Senha, funcionario.Email);
                    if (!emailEnviado) {
                        throw new Exception("Não conseguimos enviar o e-mail com a senha.");
                    }
                    else {
                        funcionario.setPasswordHash();
                        _bancocontext.Funcionario.Add(funcionario);
                        _bancocontext.SaveChanges();
                        return funcionario;
                    }
                }
                _bancocontext.Funcionario.Add(funcionario);
                _bancocontext.SaveChanges();
                return funcionario;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Funcionario EditarFuncionario(Funcionario funcionario) {
            try {
                Funcionario funcionarioDB = ListarPorId(funcionario.Id);
                if (funcionarioDB.Contratos.Any(x => x.StatusContrato == ContratoStatus.Ativo && x.Aprovacao != StatusAprovacao.Negado 
                    && funcionario.Cargos != CargoFuncionario.Motorista)) {
                    throw new Exception("Não é possível alterar o cargo de um motorista que possui contratos em andamento.");
                }
                if (DuplicataEditar(funcionario, funcionarioDB)) {
                    throw new Exception("Funcionário já se encontra cadastrado!");
                }
                if (funcionarioDB == null) {
                    throw new Exception("Desculpe, ID não foi encontrado.");
                }
                if (funcionario.Cargos == CargoFuncionario.Motorista) {
                    funcionarioDB.StatusUsuario = UsuarioStatus.Desativado;
                }
                funcionarioDB.Name = funcionario.Name.Trim();
                funcionarioDB.DataNascimento = funcionario.DataNascimento;
                funcionarioDB.Cpf = funcionario.Cpf;
                funcionarioDB.Email = funcionario.Email;
                funcionarioDB.Telefone = funcionario.Telefone.Trim();
                funcionarioDB.Cep = funcionario.Cep.Trim();
                funcionarioDB.Logradouro = funcionario.Logradouro.Trim();
                funcionarioDB.NumeroResidencial = funcionario.NumeroResidencial.Trim();
                funcionarioDB.ComplementoResidencial = funcionario.ComplementoResidencial.Trim();
                funcionarioDB.Ddd = funcionario.Ddd.Trim();
                funcionarioDB.Bairro = funcionario.Bairro.Trim();
                funcionarioDB.Cidade = funcionario.Cidade.Trim();
                funcionarioDB.Estado = funcionario.Estado.Trim();
                funcionarioDB.Cargos = funcionario.Cargos;

                if (funcionario.Cargos != CargoFuncionario.Motorista && string.IsNullOrEmpty(funcionarioDB.Senha)) {
                    funcionario.Senha = funcionario.GerarSenha();
                    bool emailEnviado = EnviarSenha(funcionario.Name, funcionario.Senha, funcionario.Email);
                    if (!emailEnviado) {
                        throw new Exception("Não conseguimos enviar o e-mail com a senha.");
                    }
                    else {
                        funcionarioDB.Senha = funcionario.Senha;
                        funcionarioDB.setPasswordHash();
                        _bancocontext.Update(funcionarioDB);
                        _bancocontext.SaveChanges();
                        return funcionario;
                    }
                }
                _bancocontext.Update(funcionarioDB);
                _bancocontext.SaveChanges();
                return funcionario;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public Funcionario ListarPorId(long id) {
            Funcionario funcionario = _bancocontext.Funcionario.AsNoTracking().Include("Contratos").FirstOrDefault(x => x.Id == id);
            return funcionario;
        }
        public Funcionario ListarPorIdNoJoin(long id) {
            return _bancocontext.Funcionario.FirstOrDefault(x => x.Id == id);
        }
        public Funcionario ListarPorlogin(string cpf) {
            return _bancocontext.Funcionario.FirstOrDefault(x => x.Cpf == cpf && x.StatusUsuario == UsuarioStatus.Ativado);
        }
        public Funcionario ListarPorloginAndEmail(string email, string login) {
            return _bancocontext.Funcionario.FirstOrDefault(x => x.Email == email && x.Cpf == login && x.StatusUsuario == UsuarioStatus.Ativado);
        }
        public Funcionario Desabilitar(Funcionario funcionario) {
            Funcionario funcionarioDesabilitado = ListarPorId(funcionario.Id);
            if (funcionarioDesabilitado == null) throw new System.Exception("Desculpe, ID não foi encontrado.");
            if (funcionarioDesabilitado.Contratos.Any(x => x.StatusContrato == ContratoStatus.Ativo && x.Aprovacao != StatusAprovacao.Negado)) {
                throw new Exception("Motorista vinculado em contrato em andamento!");
            }
            funcionarioDesabilitado.Status = StatuFuncionario.Desabilitado;
            funcionarioDesabilitado.StatusUsuario = UsuarioStatus.Desativado;
            _bancocontext.Update(funcionarioDesabilitado);
            _bancocontext.SaveChanges();
            return funcionario;
        }

        public Funcionario Habilitar(Funcionario funcionario) {
            Funcionario funcionarioHabilitado = ListarPorId(funcionario.Id);
            if (funcionarioHabilitado == null) throw new System.Exception("Desculpe, ID não foi encontrado.");
            funcionarioHabilitado.Status = StatuFuncionario.Habilitado;
            _bancocontext.Update(funcionarioHabilitado);
            _bancocontext.SaveChanges();
            return funcionario;
        }
        public Funcionario DesabilitarUsuario(Funcionario funcionario) {
            Funcionario usuarioDesabilitado = ListarPorId(funcionario.Id);
            if (usuarioDesabilitado == null) throw new System.Exception("Desculpe, ID não foi encontrado.");
            usuarioDesabilitado.StatusUsuario = UsuarioStatus.Desativado;
            _bancocontext.Update(usuarioDesabilitado);
            _bancocontext.SaveChanges();
            return funcionario;
        }
        public Funcionario HabilitarUsuario(Funcionario funcionario) {
            Funcionario usuarioHabilitado = ListarPorId(funcionario.Id);
            if (usuarioHabilitado == null) throw new System.Exception("Desculpe, ID não foi encontrado.");
            usuarioHabilitado.StatusUsuario = UsuarioStatus.Ativado;
            _bancocontext.Update(usuarioHabilitado);
            _bancocontext.SaveChanges();
            return funcionario;
        }
        public Funcionario AlterarSenha(MudarSenha mudarSenha) {
            Funcionario usuarioDB = ListarPorId(mudarSenha.Id);
            if (usuarioDB == null) throw new System.Exception("Desculpe, ID não foi encontrado.");
            if (!usuarioDB.ValidarSenha(mudarSenha.SenhaAtual)) throw new System.Exception("Senha atual inválida!");
            if (usuarioDB.ValidarDuplicataSenha(mudarSenha.NovaSenha)) throw new System.Exception("A nova senha não pode ser igual a atual!");
            usuarioDB.Senha = mudarSenha.NovaSenha;
            _bancocontext.Update(usuarioDB);
            _bancocontext.SaveChanges();
            return usuarioDB;
        }
        public Funcionario NovaSenha(Funcionario usuario) {
            try {
                Funcionario usuarioDB = ListarPorIdNoJoin(usuario.Id);
                if (usuarioDB == null) {
                    throw new System.Exception("Desculpe, ID não foi encontrado.");
                }
                usuarioDB.Senha = usuario.Senha;
                _bancocontext.Update(usuarioDB);
                _bancocontext.SaveChanges();
                return usuarioDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Funcionario RegistroApelido(Funcionario usuario) {
            Funcionario usuarioDB = ListarPorId(usuario.Id);
            if (usuarioDB == null) {
                throw new System.Exception("Desculpe, ID não foi encontrado.");
            }
            usuarioDB.Apelido = usuario.Apelido;
            _bancocontext.Update(usuarioDB);
            _bancocontext.SaveChanges();
            return usuarioDB;
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
        public Funcionario TrimFuncionario(Funcionario value) {
            value.Name = value.Name.Trim();
            value.Telefone = value.Telefone.Trim();
            value.Cep = value.Cep.Trim();
            value.Logradouro = value.Logradouro.Trim();
            value.NumeroResidencial = value.NumeroResidencial.Trim();
            value.ComplementoResidencial = value.ComplementoResidencial.Trim();
            value.Ddd = value.Ddd.Trim();
            value.Bairro = value.Bairro.Trim();
            value.Cidade = value.Cidade.Trim();
            value.Estado = value.Estado.Trim();
            return value;
        }
        public bool Duplicata(Funcionario funcionario) {
            if (_bancocontext.Funcionario.Any(x => x.Cpf == funcionario.Cpf || x.Telefone == funcionario.Telefone || x.Email == funcionario.Email)) {
                return true;
            }
            return false;
        }
        public bool DuplicataEditar(Funcionario funcionario, Funcionario funcionarioDB) {
            if (_bancocontext.Funcionario.Any(x => (x.Cpf == funcionario.Cpf && funcionario.Cpf != funcionarioDB.Cpf)
                || (x.Email == funcionario.Email && funcionario.Email != funcionarioDB.Email)
                || (x.Telefone == funcionario.Telefone && funcionario.Telefone != funcionarioDB.Telefone))) {
                return true;
            }
            return false;
        }
    }
}
