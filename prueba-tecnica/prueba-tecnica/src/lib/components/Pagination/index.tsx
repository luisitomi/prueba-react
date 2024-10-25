import { useState } from 'react';
import './styles.css';

export default function Pagination({
  totalItems,
  itemsPerPage,
  onPageChange,
}: {
  totalItems: number;
  itemsPerPage: number;
  onPageChange?: (pageNumber: number) => void;
}) {
  const [currentPage, setCurrentPage] = useState(1);

  // Get current items
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentItems = totalItems > 0 && itemsPerPage > 0 ? Array.from({ length: itemsPerPage }, (_, i) => i + indexOfFirstItem) : [];

  // Change page
  const paginate = (pageNumber: number) => {
    setCurrentPage(pageNumber);
    onPageChange?.(pageNumber); // Call the provided method if available
  };

  // Generate page numbers
  const pageNumbers = [];
  for (let i = 1; i <= Math.ceil(totalItems / itemsPerPage); i++) {
    pageNumbers.push(i);
  }

  return (
    <div className="pagination-container">
      <nav>
        <ul className="pagination">
          <li className={`page-item ${currentPage === 1 ? 'disabled' : ''}`}>
            <button onClick={() => paginate(currentPage - 1)} disabled={currentPage === 1}>
              Anterior
            </button>
          </li>
          {pageNumbers.map((number) => (
            <li key={number} className={`page-item ${currentPage === number ? 'active' : ''}`}>
              <button onClick={() => paginate(number)}>{number}</button>
            </li>
          ))}
          <li className={`page-item ${currentPage === pageNumbers.length ? 'disabled' : ''}`}>
            <button onClick={() => paginate(currentPage + 1)} disabled={currentPage === pageNumbers.length}>
              Siguiente
            </button>
          </li>
        </ul>
      </nav>
    </div>
  );
}