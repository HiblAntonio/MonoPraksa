import axios from "axios";
import React from "react";
import { useState, useEffect } from "react";
import Textbox from "./Textbox";
import { BookstoreService } from "../services/BookstoreService";

export default function TableForm({setPage, setbookstoreToEdit}){
    const bookstoreService = new BookstoreService();

    const [bookstoreList, setBookstoresList] = useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize] = useState(5); // Number of bookstores per page
    const [searchQuery, setSearchQuery] = useState('');

    useEffect(() => {
        const searchDelay = setTimeout(() => {
            fetchBookstores();
        }, 100);
        return () => clearTimeout(searchDelay);
    }, [currentPage, searchQuery]);

    // Function to fetch bookstores data
    const fetchBookstores = async () => {
        const response = await bookstoreService.getAsync(pageSize, currentPage, searchQuery);
        setBookstoresList(response);
    };

    function handleSearchInputChange (tet) {
        console.log(tet);
        setSearchQuery(tet);
        setCurrentPage(1); // Reset current page when searching
    };

    const handleEditClick = (id) => {
        setPage("edit");
        setbookstoreToEdit(bookstoreList.find(bookstore => bookstore.id === id));
    };

    const handlePageChange = (direction) => {
        if (direction === 'prev' && currentPage > 1) {
            setCurrentPage(currentPage - 1);
        } else if (direction === 'next') {
            setCurrentPage(currentPage + 1);
        }
    };

    async function deleteBookstore(bookstoreId) {
        await bookstoreService.deleteAsync(bookstoreId);
        fetchBookstores();
    }

    function loadBookstoreTable() {
        if (!bookstoreList) return;
        return (
            <div>
                <form id="search">
                    <Textbox label={"Search:"} value={searchQuery} excludeOnChange={["Search"]} onChange={handleSearchInputChange}/>
                </form>
                <br></br>
                <table id="bookstoresList">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Address</th>
                            <th>Owner</th>
                        </tr>
                    </thead>
                    <tbody>
                    {bookstoreList.map(bookstore => {
                        return (
                            <tr key={bookstore.id}>
                                <td>{bookstore.name}</td>
                                <td>{bookstore.address}</td>
                                <td>{bookstore.owner}</td>
                                <td>
                                    <button className="actionButton editActionButton" onClick={() => handleEditClick(bookstore.id)}>Edit</button>
                                    <button className="actionButton editActionButton" onClick={() => deleteBookstore(bookstore.id)}>Delete</button>
                                </td>
                            </tr>
                        );
                    })}
                    </tbody>
                </table>
                <br></br>
                <div className="pagination">
                    <button onClick={() => handlePageChange('prev')}>Previous</button>
                    <span>{currentPage}</span>
                    <button onClick={() => handlePageChange('next')}>Next</button>
                </div>
                <br></br>
            </div>
        );
    }

    return (
        <div>
            {loadBookstoreTable()}
        </div>
    );
}