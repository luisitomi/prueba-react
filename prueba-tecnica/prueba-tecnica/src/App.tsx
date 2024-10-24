
import { Routes, Route } from "react-router-dom";
import {AuthProvider} from "./lib/context/AuthProvider.tsx";
import HomePage from "./pages/home/Home.tsx";
import LoginPage from "./pages/auth/login/Login.tsx";
import {ProtectedRoute} from "./lib/components/ProtectedRoute.tsx";

function App() {
    return (
        <AuthProvider>
            <Routes>
                <Route path="/login" element={<LoginPage />} />
                <Route
                    path="/"
                    element={
                        <ProtectedRoute>
                            <HomePage />
                        </ProtectedRoute>
                    }
                />
            </Routes>
        </AuthProvider>
    );
}

export default App;
