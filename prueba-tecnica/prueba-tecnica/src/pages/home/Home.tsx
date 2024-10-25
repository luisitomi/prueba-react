import {useState, useEffect} from 'react';
import './styles.css';
import deleteIcon from '../../assets/delete-icon.jpg';
import saveIcon from '../../assets/save-icon.webp';
import closeIcon from '../../assets/close-icon.webp';
import Pagination from '../../lib/components/Pagination';
import useFetch from '../../lib/hooks/useFetch';

type UserRow = {
    id: number;
    nombres: string;
    apellidos: string;
    user: string;
    password: string;
}

export default function HomePage() {
    const [searchTerm, setSearchTerm] = useState('');
    const [newUser, setNewUser] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [newNames, setNewNames] = useState('');
    const [newLastname, setNewLastname] = useState('');
    const [showModal, setShowModal] = useState<boolean>(false);
    const [page, setPage] = useState<number>(1);
    const [currentId, setCurrentId] = useState<string | number>(0)
    const [isCreating, setIsCreating] = useState<boolean>(false);
    const [ usersList, usersLoading, usersError, getUsers ] = useFetch('https://localhost:44356/api/User/Search', {
        authorization: true,
        method: 'POST',
        body: JSON.stringify({
            page,
            total: 5,
            search: searchTerm
        })
    })

    const [ createdUser, creationLoading, creationError, createUser ] = useFetch('https://localhost:44356/api/User/add', {
        authorization: true,
        method: 'POST',
        body: JSON.stringify({
            id: 0,
            user: newUser,
            nombres: newNames,
            apellidos: newLastname,
            password: newPassword
        })
    })

    const [ deletedResponse, deleting, deleteError, deleteUser ] = useFetch('https://localhost:44356/api/User/delete', {
        authorization: true,
        method: 'POST',
        body: JSON.stringify({
            id: currentId,
        })
    })

    useEffect(() => {
        getUsers();
        setIsCreating(false)
        setNewLastname('');
        setNewNames('');
        setNewPassword('')
        setNewUser('');
        setShowModal(false);
    }, [deletedResponse, createdUser]);


    function onSelectForDeleting(id: number | string) {
        setCurrentId(id);
        setShowModal(true)
    }

    function toggleCreating() {
        if (isCreating) {
            setIsCreating(false)
            setNewLastname('');
            setNewNames('');
            setNewPassword('')
            setNewUser('');
        } else {
            setIsCreating(true)
        }
    }

    function onPaginationChange(pageNumber: number) {
        setPage(pageNumber);
        getUsers();
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
                <button onClick={toggleCreating}>{isCreating ? 'Cancelar registro' : 'Nuevo usuario'}</button>
                <table className={`data-table ${usersLoading ? 'min-height' : ''}`}>
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
                        usersLoading &&
                        <tbody className="loading">
                            <div className="spinner"></div>
                            <p>Cargando...</p>
                        </tbody>
                    }
                    {
                    !usersLoading &&
                        <tbody>
                            {
                                isCreating &&
                                <tr>
                                    <td>--</td>
                                    <td>
                                        <input
                                            type="text"
                                            placeholder="User"
                                            value={newUser}
                                            onChange={(e) => setNewUser(e.target.value)}
                                            className="search-input"
                                        />
                                    </td>
                                    <td>
                                        <input
                                            type="text"
                                            value={newNames}
                                            onChange={(e) => setNewNames(e.target.value)}
                                            placeholder="Nombres"
                                            className="search-input"
                                        />
                                    </td>
                                    <td>
                                        <input
                                            type="text"
                                            value={newLastname}
                                            onChange={(e) => setNewLastname(e.target.value)}
                                            placeholder="Apellidos"
                                            className="search-input"
                                        />
                                    </td>
                                    <td>
                                        <input
                                            type="text"
                                            value={newPassword}
                                            onChange={(e) => setNewPassword(e.target.value)}
                                            placeholder="Contrasena"
                                            className="search-input"
                                        />
                                    </td>
                                    <td>
                                    <button onClick={toggleCreating} disabled={creationLoading}>
                                            <img src={closeIcon} alt="save button" width="20px" />
                                        </button>
                                        <button onClick={createUser} disabled={creationLoading}>
                                            <img src={saveIcon} alt="save button" width="20px" />
                                        </button>
                                    </td>
                                </tr>
                            }
                        {usersList?.users?.map((item: UserRow) => (
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
                {usersList?.users?.length === 0 && !usersLoading && (
                    <p className="no-results">No se encontraron resultados.</p>
                )}
                <Pagination itemsPerPage={5} totalItems={usersList?.totalElementos} onPageChange={onPaginationChange}  />
            </div>
            {showModal && (
                <div className="modal">
                <div className="modal-content">
                    <p>Eliminar usuario?</p>
                    <div className='buttons-container'>
                        <button disabled={deleting} onClick={() => setShowModal(false)}>No</button>
                        <button disabled={deleting} onClick={deleteUser}>{deleting ? 'Cargando...' : 'Si'}</button>
                    </div>
                </div>
                </div>
            )}
        </>
    );
}
