# Add Domain

Use esta skill quando o usuário solicitar:
- criar um novo domínio ou subdomínio;
- adicionar uma nova entidade/domínio à solução;
- gerar a estrutura de um novo domínio ou subdomínio.

Para adicionar um novo domínio ou subdomínio ao projeto:

1. Execute o scaffolding conforme `docs/scaffold.md`.
2. Faça os ajustes conforme `docs/post-scaffold.md`. 
3. Se for um subdominio, inclua o método no domínio principal conforme `docs/sub-domain.md`. 
4. Nos projetos de testes, considere os comentários existentes nos templates como instruções. Nos outros projetos (principais),
isso é definido em `docs/post-scaffold.md`.
5. Após a geração, implemente somente o código solicitado pelo usuário.
6. Não execute o build da aplicação, não faça testes e nem corrija nada.
7. Este processo deve ser executado ao criar um novo domínio, não sendo possível utilizar para domínios já existentes, em uma possível alteração, por exemplo.
