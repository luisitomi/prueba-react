import { useState } from 'react';

export const useLocalStorage = <T>(key: string, initialValue: T): [T, (newValue: T) => void, () => void] => {
    const [storedValue, setStoredValue] = useState<T>(() => {
        try {
            const item = localStorage.getItem(key);
            if (item) {
                return typeof initialValue === 'string' ? item as T : JSON.parse(item) as T;
            } else {
                localStorage.setItem(key, JSON.stringify(initialValue));
                return initialValue;
            }
        } catch (error) {
            console.error('Error retrieving value from localStorage:', error);
            return initialValue;
        }
    });

    const setValue = (newValue: T) => {
        try {
            localStorage.setItem(key, typeof newValue === 'string' ? newValue : JSON.stringify(newValue));
            setStoredValue(newValue);
        } catch (error) {
            console.error('Error setting value in localStorage:', error);
        }
    };

    const remove = () => {
        try {
            localStorage.removeItem(key);
            setStoredValue(initialValue);
        } catch (error) {
            console.error('Error removing value from localStorage:', error);
        }
    };

    return [storedValue, setValue, remove];
};
