import React, {useEffect, useState} from "react";
import {View} from "react-native";
import Picker from "../CustomPicker";
import {constants} from "../../constants";
import {useTheme} from "../../contexts/ThemeContext.tsx";
import {getStyle} from "../../styles/styles.ts";
import TextItem from "../CustomTextInput";
import {ValueFilter} from "../../interfaces/interfaces.tsx";

interface FieldFilterValueProps {
    setValue: (ValueFilter) => void;
    value: ValueFilter;
}

const FieldFilterValue = (props: FieldFilterValueProps) => {
    const {theme} = useTheme();
    const style = getStyle(theme);
    
    const [operator, setOperator] = useState("");
    const [value, setValue] = useState("");

    useEffect(() => {
        let operatorId = constants.operators.find(item => item.Key === props.value?.Operator?.toString())?.Id;
        setOperator(operatorId);
        setValue(props.value?.Value?.toString() ?? "");
    }, [props.value]);
    
    const handleSetValue = (field: string, valueField: string) => {
        let valueFilter = props.value ?? {} as ValueFilter;
        let operatorKey = null;
        
        if (field === 'operator'){
            operatorKey = constants.operators.find(item => item.Id === parseInt(valueField))?.Key;
            valueFilter.Operator = operatorKey;
            setOperator(valueField);
            
            if (parseInt(valueField) === 0)
                setValue("")
        } else {
            valueFilter.Value = parseFloat(valueField);
            setValue(valueField)
        }
        
        props.setValue(valueFilter);
    }
    
    return(
        <View style={style.row}>
            <Picker
                data={constants.operators.map(item => {return { Id: item.Id, Name: item.Name}})}
                text={"Condição"}
                value={parseInt(operator)}
                setValue={(value) => handleSetValue('operator', value)}
                width={"56%"}
                isMoveText={false}
            />
            <TextItem
                text="Valor"
                value={value}
                setValue={(value) => handleSetValue('value', value)}
                width="42%"
                keyboardType={"decimal-pad"}
                isMoveText={false}
                editable={operator != undefined && operator != 0 && operator != ""}
            />
        </View>
    );
}

export default FieldFilterValue;