import { getFocusedRouteNameFromRoute, RouteProp } from '@react-navigation/native';
import { BottomTabNavigationOptions } from '@react-navigation/bottom-tabs';

export function hideTabOnScreens(screensToHide: string[]) {
    return ({ route }: { route: RouteProp<Record<string, object | undefined>, string> }): BottomTabNavigationOptions => {
        const routeName = getFocusedRouteNameFromRoute(route) ?? '';

        const shouldHide = screensToHide.includes(routeName);

        return {
            tabBarStyle: shouldHide
                ? { display: 'none' }
                : { display: 'flex' },
        };
    };
}
