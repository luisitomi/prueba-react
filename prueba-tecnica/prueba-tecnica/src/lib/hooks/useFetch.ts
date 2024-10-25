import { useState } from 'react';
import { useLocalStorage } from './useLocalStorage';
import { useAuth } from '../context/AuthProvider';

type Payload = {
    body: BodyInit | null | undefined;
    method: 'POST' | 'GET' | 'PUT' | 'DELETE',
    authorization: boolean
}

const useFetch = (url: string, payload: Payload) => {
  const [data, setData] = useState(null);
  const [error, setError] = useState<any>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [token] = useLocalStorage('token', '');
  const {logout} = useAuth()


    const fetchData = async () => {
      try {
        const response = await fetch(url, {
            method: payload.method,
            headers: {
                'Content-Type': 'application/json',
                'Authorization': payload.authorization ?  `Bearer ${token}` : ''
            },
            body: payload.body
        });

        if (response.status === 401) {
          logout()
        } else if (!response.ok) {
          throw new Error(`Error fetching data: ${response.statusText}`);
        }

        const result = await response.json();
        setData(result.data || Math.random());
        setIsLoading(false);
      } catch (error) {
        setError(error);
        setIsLoading(false);
      }
    };


  return [data, error, isLoading, fetchData];
};

export default useFetch;