# Portfolio View Screen

Tela de detalhes de um item do portfólio implementada seguindo o design fornecido.

## Estrutura

### Arquivos
- `index.tsx` - Componente principal da tela
- `styles.ts` - Estilos específicos da tela

## Componentes Principais

### PageSpecial
Base da tela que fornece:
- Header com botão voltar
- Título
- Layout com scroll

### Seções da Tela

1. **Header Card** - Informações básicas do imóvel
   - Ícone
   - Nome
   - Tipo
   - Status (tag)

2. **Value Section** - Valor atual
   - Valor principal
   - Data de atualização
   - Percentual de crescimento

3. **Acquisition Section** - Valores de aquisição e resultado
   - Valor de aquisição
   - Data de aquisição
   - Resultado (ganho/perda)

4. **Action Items** - Ações disponíveis
   - Documentos
   - Nova transação

5. **General Info** - Informações gerais
   - Tipo
   - Natureza
   - Status
   - Datas
   - Descrição

6. **Custom Attributes** - Atributos personalizados
   - Tipo do imóvel
   - Área útil
   - Localização
   - Andar
   - Número de quartos
   - Vagas de garagem
   - IPTU
   - Condomínio

7. **Financial Summary** - Resumo financeiro
   - Saldo atual
   - Total investido
   - Valorização
   - Rentabilidade

8. **Transaction History** - Histórico de transações
   - Data
   - Tipo de transação
   - Valor

## Dados Mock

Atualmente, a tela utiliza dados mock no estado do componente. Para integrar com dados reais:

1. Remova o `useState` e receba dados via `route.params` ou de um hook de contexto
2. Conecte os botões de ação (Documentos, Nova transação) com as telas correspondentes
3. Implemente o handler do botão "Edit"

## Ícones Utilizados

- `home` - Ícone de casa
- `copy` - Documentos
- `plus` - Nova transação
- `next` - Navegação
- `rule` - Informações genéricas
- `date` - Datas
- `text` - Texto
- `number` - Números
- `listoptions` - Listas

## Integração

### 1. Adicionar ao Stack de Navegação

Edite `src/stacks/MainStack.tsx`:

```typescript
import PortfolioView from "../screens/Portfolio/View";

// Dentro da MainStack, adicione:
<Stack.Screen name="PortfolioView" component={PortfolioView}/>
```

### 2. Usar em uma Navegação

Para navegar para a tela:

```typescript
navigation.navigate('PortfolioView', { portfolioId: 1 });
```

### 3. Exemplo de Integração com a Lista de Portfolio

No arquivo `src/screens/Portfolio/index.tsx`, adicione um handler ao clicar em um item:

```typescript
const handleItemClick = (item: Portfolio) => {
    navigation.navigate('PortfolioView', { portfolioId: item.Id, data: item });
};
```

## Notas

- Utilize `route.params` para passar dados reais do portfólio
- Implemente os handlers dos botões de ação conforme necessário
- Os dados mock podem ser substituídos por chamadas a APIs/controladores
