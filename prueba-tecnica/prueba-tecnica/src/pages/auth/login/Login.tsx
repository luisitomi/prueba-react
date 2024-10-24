import {useState} from 'react'
import './styles.css'
import {useAuth} from "../../../lib/context/AuthProvider.tsx";
import {useLocalStorage} from "../../../lib/hooks/useLocalStorage.ts";

export default function LoginPage() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const {login} = useAuth();
    const [_, setToken] = useLocalStorage('token', '');

    const manejarEnvio = async (e: React.FormEvent) => {
        e.preventDefault()
        if (!email || !password) {
            setError('Por favor, rellene todos los campos')
        } else {
            setError('')
            try {
                const response = await fetch('https://localhost:44356/api/User/Login',
                    {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify({user: email, password}),
                    });


                console.log(response);

                if (!response.ok) {
                    throw new Error('Login failed');
                }

                const data = await response.json();
                setToken(data.data)
                login(data.data)
                // Handle successful login, e.g., store token, redirect to dashboard
                console.log('Login successful:', data);
            } catch (error: any) {
                setError(error.message);
            }
        }
    }

    return (
        <div className="contenedor-formulario">
            <div className="tarjeta">
                <div className="encabezado-tarjeta">
                    <h2 className="titulo-tarjeta">Iniciar Sesión</h2>
                    <p className="descripcion-tarjeta">Ingrese sus credenciales para acceder a su cuenta</p>
                </div>
                <div className="contenido-tarjeta">
                    <form onSubmit={manejarEnvio}>
                        <div className="grupo-campo">
                            <label htmlFor="email">Correo Electrónico</label>
                            <div className="input-container">
                                <input
                                    id="email"
                                    type="text"
                                    placeholder="su@correo.com"
                                    value={email}
                                    onChange={(e) => setEmail(e.target.value)}
                                    required
                                />
                            </div>
                        </div>
                        <div className="grupo-campo">
                            <label htmlFor="password">Contraseña</label>
                            <div className="input-container">
                                <input
                                    id="password"
                                    type="password"
                                    value={password}
                                    onChange={(e) => setPassword(e.target.value)}
                                    required
                                />
                            </div>
                        </div>
                        {error && (
                            <div className="alerta-error">
                                <p>{error}</p>
                            </div>
                        )}
                        <button type="submit" className="boton-enviar">
                            Iniciar Sesión
                        </button>
                    </form>
                </div>
                <div className="pie-tarjeta">
                    <a href="#" className="enlace-olvidar-contrasena">
                        ¿Olvidó su contraseña?
                    </a>
                </div>
            </div>
        </div>
    )
}
