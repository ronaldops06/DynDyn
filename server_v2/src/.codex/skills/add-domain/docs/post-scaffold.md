# Alterações Necessárias

## Api.Domain
### Models
- Inclua os campos solicitados pelo usuário nos locais sugeridos pelos comentários do código.
- Se o campo tiver o nome de algum dos domínios ou subdomínios (do tipo model), inclua o tipo do campo conforme ele. Nesse caso, também inclua o campo de referência com o sufixo "Id" do tipo "int".
- Se for subdomínio e este for incluído como uma lista no domínio, não incluir campo de referência para o domínio no subdomínio.

### Entities
- Inclua os campos solicitados pelo usuário nos locais sugeridos pelos comentários do código.
- Se o campo tiver o nome de algum dos domínios ou subdomínios (do tipo entity), inclua o tipo do campo conforme ele. Nesse caso, também inclua o campo de referência com o sufixo "Id" do tipo "int".
- Se for subdomínio e este for incluído como uma lista no domínio, não incluir campo de referência para o domínio no subdomínio.

### Dtos
- Inclua os campos solicitados pelo usuário nos locais sugeridos pelos comentários do código.
- Se o campo tiver o nome de algum dos domínios ou subdomínios (do tipo ResponseDto ou RequestDto, conforme a classe que está sendo alterada), inclua o tipo do campo conforme ele. Aqui não é incluído campos de referência (int).
- Campos que são "bool" devem ser incluídos como "int".

## Api.Data
### Mapping
- Inclua os campos solicitados pelo usuário nos locais sugeridos pelos comentários do código.

### Repository
- Faça os tratamentos sugeridos pelos comentários do código.

### Context
Na classe já existente "SomniaContext":
- Incluir a propriedade: public DbSet<{{Dominio}}Entity> Dominio { get; set; }
- No método OnModelCreating adicionar: modelBuilder.Entity<{{Dominio}}Entity>(new {{Dominio}}Map().Configure);

## Api.CrossCutting
### DependencyInjection
Na classe já existente "ConfigureRepository" incluir a injeção de dependência para a interface e classe "repository" criada para o domínio. Mapear também de ICleanupRepository para a classe concreta criada para o domínio.
Na classe já existente "ConfigureService" incluir a injeção de dependência para a interface e classe "servide" criada para o domínio.

### Mappings
Na classe já existente "EntityToModelProfile", incluir o mapeamento do Model para Entity para o respectivo domínio. Quanto aos campos, mapear somente o que for necessário, principalmente os que tiverem tipos diferentes.
Na classe já existente "DtoToModelProfile", incluir o mapeamento do Dto para Model para o respectivo domínio. Quanto aos campos, mapear somente o que for necessário, principalmente os que tiverem tipos diferentes.