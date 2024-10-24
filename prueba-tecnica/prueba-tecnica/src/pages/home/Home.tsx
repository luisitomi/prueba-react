import {useState, useEffect} from 'react';
import './styles.css';
import { useLocalStorage } from '../../lib/hooks/useLocalStorage';
import deleteIcon from '../../assets/delete-icon.jpg';

type Row = {
    id: number;
    nombres: string;
    apellidos: string;
    user: string;
    password: string;
}

export default function HomePage() {
    const [searchTerm, setSearchTerm] = useState('');
    const [data, setData] = useState<Row[]>([]);
    const [loading, setLoading] = useState<boolean>(false);
    const [deleting, setDeleting] = useState<boolean>(false);
    const [token] = useLocalStorage('token', '');
    const [showModal, setShowModal] = useState<boolean>(false);
    const [currentId, setCurrentId] = useState<string | number>(0)

    useEffect(() => {
        getUsers();
    }, []);

    async function getUsers() {
        setLoading(true);
        try {
            const response = await fetch('https://localhost:44356/api/User/Search',
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    },
                    body: JSON.stringify({
                        page: 0,
                        total: 10,
                        search: searchTerm
                    }),
                });


            if (!response.ok) {
                throw new Error('Login failed');
            }

            const json = await response.json();
            setData(json.data.users);
        } catch (error: any) {}
        finally {
            setLoading(false);
        }
    }

    async function handleDelete() {
        setDeleting(true)
        try {
            const response = await fetch('https://localhost:44356/api/User/Delete',
                {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`
                    },
                    body: JSON.stringify({
                        id: currentId,
                    }),
                });


            if (!response.ok) {
                throw new Error('Login failed');
            }

            const json = await response.json();
            if (json.valid) {
                getUsers()
                setShowModal(false);
            }
        } catch(error) {

        } finally {
            setDeleting(false)
        }
    }

    function onSelectForDeleting(id: number | string) {
        setCurrentId(id);
        setShowModal(true)
    }


    return (
        <>
            <div className="block"/>
            <div className="table-container">
                <div className="search-container">
                    <input
                        type="text"
                        placeholder="Buscar..."
                        value={searchTerm}
                        onChange={(e) => setSearchTerm(e.target.value)}
                        className="search-input"
                    />
                    <button onClick={getUsers}>Buscar</button>
                </div>
                <table className={`data-table ${loading ? 'min-height' : ''}`}>
                    <thead>
                    <tr>
                        <th>ID</th>
                        <th>User</th>
                        <th>Nombres</th>
                        <th>Apellidos</th>
                        <th>Password</th>
                        <th>Accion</th>
                    </tr>
                    </thead>
                    {
                        loading &&
                        <tbody className="loading">
                            <div className="spinner"></div>
                            <p>Cargando...</p>
                        </tbody>
                    }
                    {
                    !loading &&
                        <tbody>
                        {data.map((item) => (
                            <tr key={item.id}>
                                <td>{item.id}</td>
                                <td>{item.user}</td>
                                <td>{item.nombres}</td>
                                <td>{item.apellidos}</td>
                                <td>{item.password}</td>
                                <td>
                                    <button onClick={() => onSelectForDeleting(item.id)}>
                                        <img src={deleteIcon} alt="icono de tacho de basura" width="18px" />
                                    </button>
                                </td>
                            </tr>
                        ))}
                        </tbody>
                    }
                </table>
                {data.length === 0 && !loading && (
                    <p className="no-results">No se encontraron resultados.</p>
                )}
            </div>
            {showModal && (
                <div className="modal">
                <div className="modal-content">
                    <p>Eliminar usuario?</p>
                    <div className='buttons-container'>
                        <button onClick={() => setShowModal(false)}>No</button>
                        <button onClick={handleDelete}>Si</button>
                    </div>
                </div>
                </div>
            )}
        </>
    );
}
