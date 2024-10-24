import {createBrowserRouter} from "react-router-dom";
import LoginPage from "../../pages/auth/login/Login.tsx";
import HomePage from "../../pages/home/Home.tsx";
import {ProtectedRoute} from "../components/ProtectedRoute.tsx";

const router =  createBrowserRouter([
    {
        path: "/",
        element: (
            <ProtectedRoute>
                <HomePage />
            </ProtectedRoute>
        )
    },
    {
        path: "login",
        element: <LoginPage />
    },
]);

export default router;
