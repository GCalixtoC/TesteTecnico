# TesteFullStack — Documentação de lógica e regras de negócio

Este documento descreve as entidades, DTOs, repositórios, serviços, validações, regras de negócio e endpoints presentes no projeto `TesteFullStack`.

## Visão geral

O projeto é uma API ASP.NET Core (.NET 8) que gerencia Pessoas, Categorias e Transações. A persistência usa Entity Framework Core com um `AppDbContext` configurado para PostgreSQL.

## Entidades (Domain)

- `Person`
  - `Id` (Guid)
  - `Name` (string)
  - `Age` (int) — no mapeamento EF `Age` é obrigatório

- `Category`
  - `Id` (Guid)
  - `Description` (string)
  - `Purpose` (string) — propósito da categoria, usado para compatibilidade com o tipo da transação (ex. receita ou despesa). No mapeamento EF `Purpose` é obrigatório e tem max length 20.

- `Transaction`
  - `Id` (Guid)
  - `Description` (string)
  - `Value` (decimal) — obrigatório
  - `Type` (string) — obrigatório, max length 10. Valores esperados receita ou despesa.
  - `CategoryId` (Guid)
  - `Category` (Category)
  - `PersonId` (Guid)
  - `Person` (Person)

Relações e comportamentos do banco
- `Transaction` tem `Person` com `OnDelete(DeleteBehavior.Cascade)` — remover uma pessoa removerá as transações relacionadas via cascade.
- `Transaction` tem `Category` com `OnDelete(DeleteBehavior.Restrict)` — não permite exclusão de categoria referenciada por transações.
- Restrições de tamanho de campos e obrigatoriedade são definidas em `AppDbContext`.

## DTOs

- Requests
  - `CreatePersonRequest` (nome, idade)
  - `UpdatePersonRequest` (id, nome, idade)
  - `CreateCategoryRequest` (description, purpose)
  - `CreateTransactionRequest` (description, value, type, personId, categoryId)

- Responses
  - `PersonResponse` (id, name, age)
  - `CategoryResponse` (id, description, purpose)
  - `TransactionResponse` (id, description, value, type, personName, categoryPurpose)
  - Totais `PersonTotalsResponse`, `TotalGeralResponse`, `PersonTotalsListResponse`

## Validação (FluentValidation)

`CreateTransactionRequestValidator` (regras aplicadas antes de chegar ao Service)
- `Description` deve existir e ter até 400 caracteres.
- `Value` deve ser maior que zero.
- `Type` é obrigatório e deve ser despesa ou receita (comparação case-insensitive).
- `PersonId` e `CategoryId` são obrigatórios.

Outros validators estão registrados no projeto (`CreatePersonRequestValidator`, etc.). A validação automática do FluentValidation está habilitada em `Program.cs`.

## Repositórios (Infra)

- `RepositoryBaseT` operações genéricas `AddAsync`, `UpdateAsync`, `DeleteAsync`, `GetByIdAsync`, `GetAllAsync` usando EF Core.
- `PersonRepository` estende `RepositoryBasePerson` e adiciona `DeletePersonAndTransactionsAsync` (remove a pessoa e `SaveChanges`). Observação o `AppDbContext` já tem cascade configurado para transações relacionadas.
- `CategoryRepository` estende `RepositoryBaseCategory`.
- `TransactionRepository` estende `RepositoryBaseTransaction` e adiciona `GetWithIncludesAsync()` que retorna transações incluindo `Person` e `Category`.

## Services e Regras de Negócio (Application)

### `PersonService`
- `GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`.
- `UpdateAsync` mantém valores anteriores quando campos opcionais chegam nulos (atribuições condicionais `request.Name  entity.Name`).
- `DeleteAsync` busca a pessoa e lança exceção se não existir, então remove via repositório.

### `CategoryService`
- `GetAllAsync`, `CreateAsync` — cria categoria com `Description` e `Purpose`.

### `TransactionService` (regras principais)
- `GetAllAsync` retorna transações com `PersonName` e `CategoryPurpose` via `GetWithIncludesAsync()` do repositório.

- `CreateAsync(CreateTransactionRequest request)` — regras de negócio aplicadas
  1. Verifica se `Person` existe; caso contrário lança `Exception(Pessoa não encontrada)`.
  2. Verifica se `Category` existe; caso contrário lança `Exception(Categoria não encontrada)`.
  3. Regra de idade se `person.Age  18` então só é permitido criar transações do tipo despesa.
     - Implementação converte `request.Type.ToLower()`, e se `person.Age  18 && type != despesa` lança `Exception(Menores de idade só podem registrar despesas.)`.
  4. Regra de compatibilidade tipo - categoria a `Category.Purpose` (ex. receitadespesa) deve ser compatível com o `request.Type`.
     - Implementação atual pega `category.Purpose.ToLower()` e impede criação quando `type == despesa && purpose == receita` (lança `Exception(Categoria incompatível com o tipo da transação.)`).
     - Observação a verificação atual só cobre o caso despesa vs categoria receita. Se necessário, pode-se ampliar para cobrir o caso inverso e garantir correspondência estrita.
  5. Se passar nas validações, cria `Transaction`, salva via repositório e retorna `TransactionResponse` preenchido.

- `GetTotalsByPersonAsync()` — cálcula totais por pessoa
  - Carrega todas as pessoas e todas as transações.
  - Para cada pessoa filtra as transações por `PersonId`.
  - Somas
    - `TotalReceitas` = soma `Value` onde `Type` == receita (case-insensitive).
    - `TotalDespesas` = soma `Value` onde `Type` == despesa (case-insensitive).
    - `Saldo` calculado em `PersonTotalsResponse` (receitas - despesas).
  - Também calcula `TotalGeral` somando os totais individuais.

## Endpoints (API Controllers)

- `PersonController` (`apiperson`)
  - GET `apiperson` — lista pessoas.
  - GET `apiperson{id}` — obtém pessoa por id.
  - POST `apiperson` — cria pessoa.
  - PUT `apiperson` — atualiza pessoa (recebe `UpdatePersonRequest`).
  - DELETE `apiperson{id}` — deleta pessoa.

- `CategoryController` (`apicategory`) — expõe operações para categorias (GetAll, Create) via `CategoryService`.

- `TransactionController` (`apitransaction`)
  - GET `apitransaction` — lista transações com informações agregadas (nome da pessoa, propósito da categoria).
  - GET `apitransactiontotais-por-pessoa` — retorna totais por pessoa e total geral.
  - POST `apitransaction` — cria transação (aplica validações e regras de negócio descritas acima).

## Injeção de dependência

Registrada em `TesteFullStack.Infra.IoC.DependencyInjection`
- DbContext `AppDbContext` com `UseNpgsql` (connection string `DefaultConnection`).
- Repositórios `IPersonRepository`, `ICategoryRepository`, `ITransactionRepository`.
- Services `IPersonService`, `ICategoryService`, `ITransactionService`.
- FluentValidation registrado em `Program.cs` com `AddValidatorsFromAssemblyContainingCreatePersonRequestValidator()`.

## Regras e Observações importantes

- Tipos válidos de transação somente `receita` e `despesa` (validação feita no validator de request).
- Menores de 18 anos só podem registrar transações do tipo `despesa` (regra de negócio no `TransactionService.CreateAsync`).
- Compatibilidade entre `Transaction.Type` e `Category.Purpose` implementação atual bloqueia transação do tipo `despesa` quando a categoria tem propósito `receita`. Pode-se estender para validação bidirecional mais estrita.
- Exclusão de pessoa existe tratamento via cascade no EF Core para remover transações associadas; `PersonRepository` também tem `DeletePersonAndTransactionsAsync` como utilitário.
- Mensagens de erro os services lançam `Exception` com mensagens em português (ex. Pessoa não encontrada, Categoria não encontrada, etc.). Em ambiente real recomenda-se usar exceções customizadas e tratamento centralizado para traduzir em códigos HTTP apropriados.

## Exemplos de requests

- Criar transação válida (despesa)
{
  description Compra supermercado,
  value 150.50,
  type despesa,
  personId GUID-DA-PESSOA,
  categoryId GUID-DA-CATEGORIA
}

- Criar transação inválida para menor de idade (receita) resultado esperado erro com mensagem Menores de idade só podem registrar despesas..

## Possíveis melhorias

- Tornar as mensagens de erro tipos específicos (ex. `NotFoundException`, `BusinessRuleException`) e mapear para respostas HTTP adequadas.
- Ampliar validação de compatibilidade entre tipo e `Category.Purpose` para cobrir todos os casos e normalizar valores armazenados (ex. enum ou constantes).
- Mover mensagens e textos para recursos (i18n) se houver multiplos idiomas.
- Testes unitários e de integração para `TransactionService` e validações.
