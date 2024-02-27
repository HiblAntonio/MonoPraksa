import React from "react";
import { useState, useEffect } from "react";

// Get, update, insert

export default function TableForm({setPage, setbookstoreToEdit}){
    const [bookstoreList, setBookstoresList] = useState([]);

    useEffect(() => {
        // Load usersList from localStorage on component mount
        const storedBookstores = localStorage.getItem('bookstores');
        if (storedBookstores) {
            setBookstoresList(JSON.parse(storedBookstores));
        }
    }, []);

    // Function to handle click on Edit button
    const handleEditClick = (id) => {
        setPage("edit");
        console.log(bookstoreList.find(bookstore => bookstore.id === id));
        setbookstoreToEdit(bookstoreList.find(bookstore => bookstore.id === id));
    };

    function loadBookstoreTable() {
        if (!bookstoreList) return;

        return (
            <table id="bookstoresList">
                <thead>
                    <tr>
                        <th>Name</th>
                        <th>Address</th>
                        <th>Owner</th>
                    </tr>
                </thead>
                <tbody>
                    {bookstoreList.map(bookstore => (
                        <tr key={bookstore.id}>
                            <td>{bookstore.Name}</td>
                            <td>{bookstore.Address}</td>
                            <td>{bookstore.Owner}</td>
                            <td>
                                <button className="actionButton editActionButton" onClick={() => handleEditClick(bookstore.id)}>Edit</button>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        );
    }

    return (
        <div>
            {loadBookstoreTable()}
        </div>
    );
}