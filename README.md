# 💈 Barbearia API

API REST para gerenciamento de serviços e agendamentos de uma barbearia, desenvolvida em .NET 10.

## 👤 Autor

**Gustavo Purkoot Ferreira**

## 📋 Descrição

Sistema de gerenciamento de barbearia com cadastro de serviços e agendamentos. Permite criar e gerenciar os serviços oferecidos pela barbearia, além de realizar agendamentos vinculados a esses serviços com validações de regras de negócio.

## 🚀 Instruções de Execução

### Pré-requisitos
- .NET 10 instalado
- dotnet-ef instalado globalmente

### Passos

1. Clone o repositório:
```bash
git clone https://github.com/gustavopurkoot/Barbearia_CSharp.git
cd Barbearia_CSharp/Barbearia
```

2. Restaure os pacotes:
```bash
dotnet restore
```

3. Crie o banco de dados:
```bash
dotnet ef database update
```

4. Execute o projeto:
```bash
dotnet run
```

A API estará disponível em `http://localhost:5144`.

## 🛠️ Tecnologias Utilizadas

- **Minimal API** — endpoints sem controllers
- **Entity Framework Core** — ORM para acesso ao banco
- **SQLite** — banco de dados local
- **JSON** — formato de comunicação

## 📦 Endpoints

### Serviços
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/Servicos` | Lista todos os serviços |
| GET | `/servicos/{id}` | Busca serviço por ID |
| POST | `/Servicos` | Cadastra novo serviço |
| PUT | `/servicos/{id}` | Atualiza serviço existente |
| DELETE | `/servicos/{id}` | Remove serviço |

### Agendamentos
| Método | Rota | Descrição |
|--------|------|-----------|
| GET | `/Agendamentos` | Lista todos os agendamentos |
| GET | `/agendamentos/{id}` | Busca agendamento por ID |
| POST | `/Agendamentos` | Cria novo agendamento |
| PUT | `/agendamentos/{id}` | Atualiza agendamento existente |
| DELETE | `/agendamentos/{id}` | Remove agendamento |

## ✅ Regras de Negócio

- Preço do serviço deve ser maior que zero
- Não é permitido agendar em data/hora passada
- Validação de conflito de horário: verifica se o novo agendamento se sobrepõe a algum já existente, levando em conta a duração de cada serviço (ex: um corte de 20 min às 14h bloqueia agendamentos entre 14h e 14h20, não só o horário exato)

## 📝 Exemplos de JSON

### Cadastrar Serviço
```json
{
  "nome": "Corte Social",
  "preco": 25.00,
  "duracaoMinutos": 20
}
```

### Cadastrar Agendamento
```json
{
  "clienteNome": "João Silva",
  "dataHora": "2026-11-06T16:00:00",
  "status": "Pendente",
  "servicoId": 1
}
```

## 🗂️ Estrutura do Projeto

```
Barbearia/
├── Models/
│   ├── Servico.cs
│   └── Agendamento.cs
├── Data/
│   └── AppDataContext.cs
├── Migrations/
├── Program.cs
└── README.md
```