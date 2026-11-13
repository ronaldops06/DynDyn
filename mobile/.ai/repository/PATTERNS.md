# Padrões de Relacionamento – Repositórios

## 1. Padrão Simples (1 tabela)
**Exemplo real:** `category.repository.ts` → tabela `categories`

- Uma única tabela
- CRUD completo
- Filtros opcionais (`type`, `status`/`activated`)
- Paginação opcional
- Sem filhos

**Quando usar:** Entidades independentes (Category, etc.)

---

## 2. Padrão 1:N com Dependência Forte
**Exemplo real:** `attribute.repository.ts`
- Tabela pai: `attributes`
- Tabela filha: `attribute_option`
- Relação: 1 Attribute → 0..N AttributeOption
- A Option **não existe** sem o Attribute

### Regras específicas
- No `insert` do pai → inserir também todas as Options
- No `update` do pai:
    1. Buscar Options atuais
    2. Calcular `remove`, `add` e `update`
    3. Executar as 3 operações
- Função auxiliar: `selectOptionByAttributeInternalId(internalId)`
- Funções de Option recebem a conexão `db` já aberta (para transação implícita)

**Quando usar:** Quando o filho só faz sentido dentro do pai (AttributeOption, etc.)

---

## 3. Padrão N:N (Many-to-Many)
**Exemplo real:**
- `operation.repository.tsx` → `operations` + `operations_roles_link`
- `operation.role.repository.ts` → `operation_roles`

### Estrutura
- Tabela A: `operations`
- Tabela B: `operation_roles` (pode ser usada por outras entidades)
- Tabela de ligação: `operations_roles_link` (operation_id + operation_role_id + reference)

### Regras específicas
- A tabela de ligação tem `FOREIGN KEY ... ON DELETE CASCADE`
- No `insert`/`update` de Operation → sincronizar os links
- Função de busca: `selectOperationRoleByOperationInternalId`
- Funções de verificação de relacionamento:
    - `existsOperationRelationshipCategory`
    - `existsOperationRelationshipOperationRole`
- O repositório da entidade “Role” é independente e pode ser referenciado por várias tabelas de ligação

**Quando usar:** Quando duas entidades se relacionam de forma N:N e a entidade do lado B pode ser compartilhada.