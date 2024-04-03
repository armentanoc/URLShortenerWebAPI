# Projeto URLShortener ⛓️
 
Gere um redirecionamento de URL breve e temporário para compartilhamento eficiente de links. Esse processo envolve criar um link conciso que redireciona para uma URL específica, mas permanece válido por um período limitado. Ao acessar a URL curta dentro desse intervalo de tempo, os usuários são direcionados para a URL original, oferecendo uma solução rápida e temporária para compartilhar conteúdo.

<div align="center" display="flex">
<img src="https://github.com/armentanoc/URLShortenerWebAPI/assets/88147887/e7bc600b-01b6-4e5e-9237-68177cd4e2a4" height="500px">
</div>

## Endpoints da API 🚀
A API oferece os seguintes endpoints:

### Url 🔗

```
GET /all: Obtém todas as URLs.
```

<div align="center" display="flex">
<img src="https://github.com/armentanoc/URLShortenerWebAPI/assets/88147887/1df2eb48-373c-4929-97eb-41f832a18e6b" height="500px">
</div>

```
GET /{slug}: Efetua o redirect para uma página válida através da URL encurtada.
```

<div align="center" display="flex">
<img src="https://github.com/armentanoc/URLShortenerWebAPI/assets/88147887/012eae15-12f2-4d0b-b095-8bfb7aa457f7" height="500px">
</div>

```
POST /makeUrlShort: Encurta a URL e retorna o objeto armazenado no Database.
```

<div align="center" display="flex">
<img src="https://github.com/armentanoc/URLShortenerWebAPI/assets/88147887/c4383253-700d-402d-8b86-c6f160f0f6e1" height="500px">
</div>

## Estrutura do Projeto :building_construction:

A pasta `/src` contém a solução `URLShortener` e os projetos que compõem a aplicação.

---

### 💻 `URLShortener.WebAPI` 
Projeto principal que contém a API e os controladores.

### 📦 `URLShortener.Domain` 
Projeto que contém as entidades de domínio da aplicação.

### 🗃️ `URLShortener.Infra` 
Projeto responsável pela camada de infraestrutura, incluindo o contexto do banco de dados e repositórios.

### 🚀 `URLShortener.Application` 
Projeto que implementa a lógica de aplicação e serviços.

### 👀 `URLShortener.ViewModels` 
Projeto que contém os modelos de visualização utilizados pelos controladores.

### 🐛 `URLShortener.CustomExceptions` 
Projeto que contém as exceções customizadas lançadas pela aplicação.

### 🧪 `URLShortener.Tests` 
Projeto que contém os testes unitários em xUnity da lógica de negócio da aplicação.

<!--dotnet tool install -g dotnet-reportgenerator-globaltool
dotnet tool install dotnet-stryker-->
```
dotnet test --collect:"XPlat Code Coverage"

reportgenerator "-reports:.\**\coverage.cobertura.xml" -reporttypes:Html -targetdir:output

dotnet-stryker
```
## Configurações da Aplicação Personalizadas 📁

O domínio da url curta gerada, o mínimo e máximo de minutos para expirar são parâmetros customizáveis no `appsettings.json`:

```
  "AppSettings": {
    "ShortenedUrlDomain": "http://localhost:5500",
    "MinMinutesToExpire": "20",
    "MaxMinutesToExpire": "50"
  }, 
```

## Política de Cors 🔐

Foi implementada uma Política de Cors que só autoriza o acesso a recursos da API através da rota `localhost:5000`: para visualização, é possível obter todos os produtos, vendas, reembolsos e devoluções, além de realizar o filtro por nome de produto.

Obs.: Garanta que as configurações do servidor que rode o FrontEnd de testes (ex.: Live Server), ignore os arquivos de log gerados para impedir que a página seja recarregada após uma requisição POST com sucesso. 

Por exemplo, no Live Server, os settings.json podem ignorar totalmente a pasta do BackEnd (`/src`) no `settings.json`:

<!--<div align="center" display="flex">
<img src="" height="500px">
</div>
-->

```
{
    (...)
    "liveServer.settings.ignoreFiles": [

        (...)
        "src/**"
    ]
}
```

## Middleware Customizado de Logging 🗞️ e Filtro Customizado de Exceção 🐛

Através do `Middlewares/LoggingMiddleware` é realizado o logging sempre no começo e no final de uma requisição, com detalhes sobre o status e eventuais erros de forma personalizada, que são capturados no Filtro Customizado de Exceção Global (`Filters/ExceptionFilter.cs`).

<div align="center" display="flex">
<img src="https://github.com/armentanoc/URLShortenerWebAPI/assets/88147887/91fd958c-f6a5-4c72-ad13-1369ab7b1194" height="500px">
</div>

## Configuração do Banco de Dados 🛢️

O projeto utiliza o SQLite como banco de dados, e as configurações podem ser encontradas no arquivo `appsettings.json` do projeto `URLShortener.WebAPI`. Certifique-se de ajustar as configurações conforme necessário.

```json
{
  "ConnectionStrings": {
    "URLShortenerSqlite": "Data Source=UrlShortenerDB.db"
  },
}
```
<!--<div align="center" display="flex">
<img src="" height="500px">
</div>
-->

## Execução do Projeto ▶️

1. Clone e abra a solução no Visual Studio.
2. Configure o projeto `URLShortener.Infra` como o projeto de inicialização no `Package Manager Console`.
3. Certifique-se de que as migrações do banco de dados foram realizadas pelo Entity Framework. Se não, execute os seguintes comandos:
```
Add-Migration CreateDatabaseInitial
Update-Database
```
4. Execute o projeto.

## Documentação da API 📚
A API está documentada usando Swagger. Após a execução do projeto, acesse a documentação em:

```
http://localhost:5500/swagger/v1/swagger.json
```

## Contribuições 🛠️

Aceitamos contribuições! Se encontrar um bug ou tiver uma solicitação de recurso, por favor, abra uma issue. 
