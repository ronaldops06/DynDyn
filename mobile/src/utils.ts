import * as I from "./interfaces/interfaces.tsx";

export function isEndScroll(event: any) {
    let mHeight = event.nativeEvent.layoutMeasurement.height;
    let cSize = event.nativeEvent.contentSize.height;
    let Y = event.nativeEvent.contentOffset.y;

    if (Math.ceil(mHeight + Y) >= cSize) return true;
    return false;
}

export const validateLogin = (response: I.Response, navigation: any) => {
    if (!response.isLogged)
        navigation.navigate("SignIn");
}

export const validateSuccess = (response: I.Response, navigation: any) => {
    if (response.success)
        navigation.goBack();
}