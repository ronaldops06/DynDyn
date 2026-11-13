---
name: rn-sqlite-repository
description: Cria repositórios SQLite no padrão do projeto. Use quando o usuário pedir para criar, gerar ou implementar um repository (simples, 1:N ou N:N).
---

# React Native SQLite Repository

## Fluxo obrigatório

1. Leia nesta ordem:
    - `.ai/repository/CONTEXT.md`
    - `.ai/repository/patterns.md`
    - `.ai/repository/conventions.md`

2. Identifique o tipo de relacionamento e carregue o template correspondente:

| Tipo     | Template |
|----------|----------|
| Simples  | `.ai/templates/repository/simple.repository.md` |
| 1:N      | `.ai/templates/repository/one-to-many.repository.md` |
| N:N      | `.ai/templates/repository/many-to-many.repository.md` + `many-to-many-link.repository.md` |

- Se o tipo de relacionamento (simples / 1:N / N:N) não estiver claro, pergunte antes de gerar

3. Siga **rigorosamente** o template escolhido + as regras dos arquivos `patterns.md` e `conventions.md`.
4. Substitua apenas os placeholders (`Xxx`, `Yyy`, `xxx`, `yyy`...).
5. Gere todos os arquivos necessários (pai + filho + link quando for o caso).