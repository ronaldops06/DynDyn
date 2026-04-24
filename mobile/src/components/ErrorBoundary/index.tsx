import React from 'react';
import crashlytics from '@react-native-firebase/crashlytics';

type Props = {
    children: React.ReactNode;
};

type State = {
    hasError: boolean;
};

export class ErrorBoundary extends React.Component<Props, State> {
    state = { hasError: false };

    static getDerivedStateFromError() {
        return { hasError: true };
    }

    componentDidCatch(error: Error) {
        crashlytics().recordError(error);
    }

    render() {
        if (this.state.hasError) {
            return null; // ou tela de fallback
        }

        return this.props.children;
    }
}
