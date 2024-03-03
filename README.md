# URLShortener

## Configuração do Banco de Dados 🛢️

O projeto utiliza o SQLite como banco de dados, e as configurações podem ser encontradas no arquivo `appsettings.json` do projeto `ECommerce.WebAPI`. Certifique-se de ajustar as configurações conforme necessário.

```json
{
  "ConnectionStrings": {
    "ECommerceSqlite": "Data Source=UrlShortenerDB.db"
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
