# Gestão de Alunos e Turmas

Aplicação que envolve API e front-end com o Razor.
![Front-end com Razor](https://github.com/user-attachments/assets/9f09ef08-87aa-4b8c-bbd2-9235dd53935f)

![API](https://github.com/user-attachments/assets/5690b702-0c55-41b5-9cbb-6d83ae9a0488)

----

## Tecnologias:

- .NET 8
- Entity Framework
- Dapper
- Razor
- SQL Server
- xUnit

----

## Objetivos

Criar:   
- Formulário de cadastro e edição de Aluno
- Lista de Alunos
- Inativar Aluno
- Formulário de cadastro e edição de Turma
- Lista de Turmas
- Inativar Turma
- Formulário de Associação com dois Combos Aluno e Turma (cadastro e edição)
- Lista de Turmas onde possa acessar Alunos relacionados.
- Inativar Relação

----

## Estrutura

A aplicação está estruturada pela separação de responsabilidades. Dentro do `src` estão separadas a API e o Front, enquanto na pasta `tests` estão concentrados os testes realizados.

- A comunicação entre API x Front foi feita utilizando o HTTPClient para a integração de dados, deixando-os separados.

### API

A API está organizada por camadas com a seguinte estrutura:

- `Controllers`: Controladores de rotas e endpoints
- `Migration`: Migrações para criação de tabelas e configuração de banco
- `Services`: Serviços que implementam as regras de negócio
- `Repositories`: Manipulação de dados e acesso ao banco
- `Models`: Representações das entidades de domínio
- `DTOs`: Objetos de transferência de dados para comunicação entre camadas

### Front

O front-end é um projeto MVC e consiste nas seguintes camadas:

- `Controllers`: Manipulação das requisições HTTP para as Views
- `Views`: Páginas Razor (.cshtml) com a lógica de interface
- `Models`: Representações de dados para o front-end
- `Services`: Manipulação de dados de API usando HttpClient

----

## Como rodar

**1. Pré-requisitos**

  - .NET SDK 8.0 instalado
  - SQL Server configurado
  - Visual Studio ou outra IDE
  - Clonar o repositório

**2. Configurar o banco de dados**

- Configure a string de conexão no arquivo `appsettings.json` da API para o SQL Server:
  ```json
  "ConnectionStrings": {
      "DatabaseConnection": "Server=SEU_SERVIDOR;Database=GestaoAlunosDB;User Id=SEU_USUARIO;Password=SUA_SENHA;"
  }
  ```

**3. Restaurar as dependências do nuget**

   - `dotnet restore`

**4. Gerar o banco**

  - `Update-Database` para gerar o banco de dados no SQL Server
  
**5. Buildar a aplicação**

   - `dotnet build`

**6. Rodar a aplicação**

   - Você pode rodar separadamente:
     
   **API:**
   - Vá para a pasta API
   - `dotnet run`
   - `https://localhost:7162/swagger`

   **Front:**
   - Vá para a pasta Front
   - `dotnet run`
   - `https://localhost:7292/`

  **Para rodar simultaneamente, você pode**:
  - Ir no **Solution Explorer** (`.sln`)
  - Escolher a opção **Set Startup Projects**
  - Definir o projeto API e Front como "Start" no **Action**
  - Clicar em aplicar e em ok
  - Após isso é só clicar em **Start** para rodar a aplicação

----

## Melhorias a serem feitas

- Ampliar a cobertura de testes para incluir mais cenários, sem ser de regras de negócio, e aumentar a profundidade.
- Implementar outras funcionalidades e refinar validações de segurança e integridade de dados.
- Padronizar as repostas com o Result Pattern.
- Padronizar as nomenclaturas das classes.
- Adicionar um middleware para o tratamento de exceções.
- Adicionar cache para evitar o excesso uso do banco de dados.
- Migrar a aplicação pra nuvem.
