# Convenções da Camada de Repositório

## Nomenclatura de Arquivos
- `xxx.repository.ts` (ou `.tsx` se tiver JSX – evite)
- Exemplos: `category.repository.ts`, `attribute.repository.ts`, `operation.repository.ts`

## Nomenclatura de Funções
| Ação                        | Padrão de nome                                      |
|-----------------------------|-----------------------------------------------------|
| Criar tabela                | `createTableXxx`                                    |
| Inserir                     | `insertXxx`                                         |
| Atualizar                   | `updateXxx`                                         |
| Deletar por InternalId      | `deleteInternalXxx`                                 |
| Deletar por Id externo      | `deleteInternalXxxByExternalId`                     |
| Deletar todos do usuário    | `deleteAllXxx`                                      |
| Listar com filtros          | `selectAllXxx`                                      |
| Contar                      | `selectContAllXxx`                                  |
| Buscar por Id               | `selectXxxById`                                     |
| Buscar filhos               | `selectYyyByXxxInternalId`                          |
| Verificar relacionamento    | `existsXxxRelationshipYyy`                          |

## Estilo de Código
- Sempre `async/await`
- Preferir `for...of` em vez de `.map(async ...)`
- `formatResult` e `formatResultOption` devem ser funções privadas (`const`)
- Tipagem forte: nunca retornar `any`
- Parâmetros de filtro opcionais usam `| null`
- Comentários em português apenas quando a lógica for complexa (ex: sincronização de filhos)

## Banco de Dados
- Sempre abrir com `const db = await openDatabase();`
- Em funções internas de filhos, receber `db: SQLite.SQLiteDatabase` como parâmetro
- Sempre criar índices relevantes
- Usar `CREATE TABLE IF NOT EXISTS`