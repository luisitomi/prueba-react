
import {createContext, useContext, useMemo, useState} from 'react';
import {useNavigate} from "react-router-dom";
import { useLocalStorage } from '../hooks/useLocalStorage';

export const AuthContext = createContext<AuthContextValue>({
    isAuthenticated: false,
    token: '',
    login: () => {},
    logout: () => {},
});

interface AuthContextValue {
    isAuthenticated: boolean;
    token: string;
    login: (token: string) => void;
    logout: () => void;
}

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
    const [localStorageToken] = useLocalStorage('token', '');
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [token, setToken] = useState<string>(localStorageToken);
    const navigate = useNavigate();

    const login = (token: string) => {
        // Replace with your actual authentication logic
        setIsAuthenticated(true);
        setToken(token);
        navigate("/", { replace: true });
    };

    const logout = () => {
        setIsAuthenticated(false);
        setToken('');
        navigate("/login", { replace: true });
    };

    const value = useMemo(
        () => ({
            isAuthenticated,
            token,
            login,
            logout
        }),
        [token]
    )

    return (
        <AuthContext.Provider value={value}>
            {children}
        </AuthContext.Provider>
    );
};


export const useAuth = () => {
    return useContext(AuthContext);
};
