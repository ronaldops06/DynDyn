# AI Context – Camada de Repositório SQLite (React Native)

## Objetivo
Este arquivo ensina qualquer IA a criar, manter e estender repositórios SQLite seguindo **exatamente** o padrão já existente no projeto.

## Tecnologias
- `react-native-sqlite-storage`
- Banco aberto via `openDatabase()` (arquivo `./database`)
- Constantes em `../constants` (ex: `constants.pageSize`)
- Interfaces em `../interfaces/interfaces`

## Conceitos Fundamentais (obrigatórios)

| Conceito              | Explicação                                                                 | Campo no banco      |
|-----------------------|----------------------------------------------------------------------------|---------------------|
| InternalId            | Chave primária local (AUTOINCREMENT)                                       | `internal_id`       |
| Id                    | ID vindo do servidor / API                                                 | `id`                |
| Reference / userLogin | Isolamento multi-tenant (cada usuário só vê seus dados)                    | `reference`         |
| DataCriacao           | Data de criação                                                            | `data_criacao`      |
| DataAlteracao         | Data de alteração                                                          | `data_alteracao`    |
| Status                | Ativo/Inativo (geralmente 1/0 ou boolean)                                  | `status`            |

## Mapeamento de Campos

Os campos para a tabela devem seguir a nomenclatura e tipo definido na respectiva "interface" (`../interfaces/interfaces`), fornecida pelo usuário

## Estrutura de um Repositório

Todo repositório deve exportar as seguintes funções (quando fizer sentido):

### Obrigatórias
- `createTableXxx()`
- `insertXxx(userLogin: string, entity: T): Promise<T>`
- `updateXxx(...)`
- `deleteInternalXxx(internalId: number)`
- `deleteInternalXxxByExternalId(userLogin: string, id: number)`
- `deleteAllXxx(userLogin: string)`
- `selectAllXxx(userLogin: string, ...filtros, pageNumber?: number | null)`
- `selectContAllXxx(userLogin: string, ...filtros): Promise<number>`
- `selectXxxById(userLogin: string, id: number): Promise<T | undefined>`

### Auxiliares
- `formatResult(item: any): T` (sempre privado)
- Funções de relacionamento (ex: `selectOptionByAttributeInternalId`)

## Padrões de Relacionamento existentes

1. **Simples** (1 tabela) → veja `category.repository.ts`
2. **1:N com dependência** (filho não existe sem o pai) → veja `attribute.repository.ts`
3. **N:N via tabela de ligação** → veja `operation.repository.tsx` + `operation.role.repository.ts`

Consulte o arquivo `PATTERNS.md` para detalhes de cada um.

## Regras de Ouro

1. Sempre filtrar por `reference = userLogin`.
2. Sempre criar índices em: `id`, `reference` e FKs.
3. `insert` deve retornar a entidade com o `InternalId` preenchido.
4. Em relacionamentos 1:N e N:N, o `insert` e o `update` devem sincronizar os filhos.
5. Paginação: `LIMIT constants.pageSize OFFSET (pageNumber - 1) * constants.pageSize`.
6. Funções de existência de relacionamento (`existsXxxRelationshipYyy`) devem retornar `boolean`.