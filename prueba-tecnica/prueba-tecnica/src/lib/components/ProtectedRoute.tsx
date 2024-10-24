
import { Navigate } from "react-router-dom";
import {useAuth} from "../context/AuthProvider.tsx";

export const ProtectedRoute = ({ children }: { children: React.ReactNode }) => {
    const { token } = useAuth();
    if (!token) {
        // user is not authenticated
        return <Navigate to="/login" />;
    }
    return children;
};
