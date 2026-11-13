# Template – Estilo para modal utilizada para cadastros auxiliares

Use este template de estilo quando for solicitado a criação de uma modal para cadastro auxiliar

```js
import {StyleSheet} from "react-native";

export const getXxxStyle = (theme) => StyleSheet.create({
    areaFields: {
        marginBottom: 8,
    },
    areaButtonApply: {
        marginBottom: 2,
        flexDirection: "row",
        alignItems: "center",
        justifyContent: "space-around",
    }
});
```    