# AI Context - React Native App

## Objetivo
Este arquivo serve como contexto completo para qualquer IA gerar código consistente com a arquitetura do projeto.

## Estrutura do Projeto

```
src/
    background/     => Funcionalidades que são executadas em segundo plano, sem a 
    components/     => Componentes genéricos
    contexts/       => Camada mais externa da aplicação que envelopam as depais funcionalidades
    controller/     => Implementação das regras de negócio e orquestração para outras camadas como repository e services 
    enums/          
    interfaces/     => Definições de modelos de domínios
    reduces/
    repository/     => Acesso e gerenciamento do banco de dados
    screens/        => Telas e tudo que for relacionado a interface
    services/       => Camanda de comunicação externa (APIs)
    stacks/
    styles/         => Arquivos de estilos genéricos (utilizados por toda a aplicação)
    constants.js    => Constants e dicionários
    utils.js        => Funções genéricas (utilizadas por toda a aplicação)
App.tsx
index.js => Barrel export
```

## Regras de Nomenclatura (obrigatórias)

- Use **PascalCase** para pasta de componentes e telas e **minúsculo** para qualquer arquivo;
- Todo componente deve ter um arquivo "index.tsx" e "styles.js" (se tiver estilos próprios);
- Em *repository* os arquivos deverão ter o nome `{feature}.repository.ts`; 
- Em *controller* os arquivos deverão ter o nome `{feature}.controller.ts`;
- Em *services* os arquivos deverão ter o nome `{feature}.api.ts`;
- Caso a feature/componente tiver mais de uma palavra no nome, cada palavra deve ser separada por ponto (.), exemplo: "account.role.api.ts".
- Nome de tabelas, índices e campos no banco de dados deve ser minǘsculo separado por "_";

## Padrões de Código

- Idioma: Português nos comentários, inglês nos nomes de variáveis/funções/componentes.
- TypeScript obrigatório.
- Preferir **functional components**.
- Preferir **Arrow function**
- Services devem retornar `Promise<T>` e tratar erros.
- Evitar lógica de negócio dentro de componentes/screens (extrair para controller).
- Usar `StyleSheet.create` para estilos, recebendo theme (src/styles/themes.ts) como prop.
- Toda estilização de cores deve vir de "src/styles/themes.ts".
- Navegação: usar o padrão já existente no projeto (React Navigation).
- Exportar tudo pelo `index.ts` da feature (barrel export).
- Não deixar `console.log` em código final.
- Usar `async/await` em services.
- Nunca usar `map` assíncrono incorretamente (preferir `for...of`).

## Como a IA deve responder

1. Criar a pasta da feature + todos os arquivos listados.
2. Colocar código base limpo e tipado.
3. Manter a estrutura de pastas rigorosamente.
4. Se o usuário pedir apenas uma parte (ex: só o service), gerar somente o que foi solicitado.

    