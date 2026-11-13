# Scaffolding

O scaffolder é o projeto ApiScaffolder, existente na solution (projeto .Net do tipo command line).

Para executar basta seguir o procedimento abaixo:

1. Via linha de comando, navegue para a pasta do projeto ApiScaffolder;
2. Execute o comando: dotnet run <NomeDominio> <Alias> <Nome> <Pasta>
    * NomeDominio: é o nome do domínio ou subdomínio. É de fato o nome que será utilizado na classe e arquivo (Pascal Case);
    * Alias: é utilizado no nome das variáveis onde este domínio será utilizado (Camel Case);
    * Nome: é utilizado no nome em português do domínio, aplicado em comentários
    * Pasta: referente a pasta onde o objeto gerado será colocado (Pascal Case). Informar somente quando for um subdomínio; A pasta é o nome do Domínio.

