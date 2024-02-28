import axios from "axios";
import React from "react";
import { useState, useEffect } from "react";
import Textbox from "./Textbox";

export default function TableForm({setPage, setbookstoreToEdit}){
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
        try {
            const response = await axios.get(`https://localhost:44349/api/bookstore?pageSize=${pageSize}&pageNum=${currentPage}&searchQuery=${searchQuery}`);
            setBookstoresList(response.data);
        } catch (error) {
            console.error('Error fetching bookstores:', error);
        }
    };

    function handleSearchInputChange (tet) {
        console.log(tet);
        setSearchQuery(tet);
        setCurrentPage(1); // Reset current page when searching
    };

    // Function to handle click on Edit button
    const handleEditClick = (id) => {
        setPage("edit");
        setbookstoreToEdit(bookstoreList.find(bookstore => bookstore.id === id));
    };

    const handlePreviousPage = () => {
        if (currentPage > 1) {
            setCurrentPage(currentPage - 1);
        }
    };

    // Function to handle click on Next button
    const handleNextPage = () => {
        setCurrentPage(currentPage + 1);
    };

    async function deleteBookstore(bookstoreId) {
        try {
            // Send a DELETE request to the API endpoint to delete the specified bookstore
            await axios.delete(`https://localhost:44349/api/bookstore/${bookstoreId}`);

            fetchBookstores();
            console.log("Bookstore deleted successfully");
        } catch (error) {
            console.error("Error deleting bookstore:", error);
            // Handle error
            throw error; // Rethrow the error for the caller to handle
        }
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
                {/* Pagination - PageNum + 1*/}
                <br></br>
                <div className="pagination">
                    <button onClick={handlePreviousPage}>Previous</button>
                    <span>{currentPage}</span>
                    <button onClick={handleNextPage}>Next</button>
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