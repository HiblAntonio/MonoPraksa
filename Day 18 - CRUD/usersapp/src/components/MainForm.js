import React from "react";
import Textbox from "./Textbox";
import { useState } from "react";
import Button from "./Button";
import TableForm from "./TableForm";
import axios from "axios";

export default function Form(){
    
    const [bookstores, setBookstores] = useState([]);
    const [bookstoreToEdit, setbookstoreToEdit] = useState({});
    const [page, setPage] = useState("view");
    const [textValue, setTextValue] = useState('');

    const handleTextChange = (newValue) => {
        // Update the state with the new value
        setTextValue(newValue);
    };

    class Bookstore{
        constructor(id, Name, Address, Owner, Books)
        {
            this.id = id;
            this.Name = Name;
            this.Address = Address;
            this.Owner = Owner;
            this.Books = Books;
        }
    }

    async function addBookstore() {
        try {
            const idInput = crypto.randomUUID();
            const nameInput = document.forms.createBookstoreForm.Name.value;
            const addressInput = document.forms.createBookstoreForm.Address.value;
            const ownerInput = document.forms.createBookstoreForm.Owner.value;
            const books = [];

            const bookstore = new Bookstore(idInput, nameInput, addressInput, ownerInput, books);

            // Send a POST request to the API endpoint to add a new bookstore
            const response = await axios.post('https://localhost:44349/api/bookstore', bookstore);
    
            // Assuming the response contains the added bookstore data, you can handle it here if needed
            console.log("Bookstore added successfully:", response.data);
            return response.data; // Return the added bookstore data
        } catch (error) {
            console.error("Error adding bookstore:", error);
            throw error; // Rethrow the error for the caller to handle
        }
    }

    async function saveBookstoreChanges(bookstoreToEdit) {
        const id = bookstoreToEdit.id;
        const name = document.forms.editBookstoreForm.Name.value;
        const address = document.forms.editBookstoreForm.Address.value;
        const owner = document.forms.editBookstoreForm.Owner.value;

        try {
            // Send a PUT request to update the bookstore details
            await axios.put(`https://localhost:44349/api/bookstore/${id}`, {
                'Name': name,
                'Address': address,
                'Owner': owner
            });
    
            console.log("Bookstore updated successfully");
        } catch (error) {
            console.error("Error updating bookstore:", error);
            // Handle error
        }
    }

    const renderContent = () => {
        switch (page){
            case "index":
                return(
                    <div className ="container">
                        <h1>Add a new bookstore</h1>
                    <form id="createBookstoreForm">
                        <Textbox label={"Name:"} excludeOnChange={["Name:"]}/>
                        <Textbox label={"Address:"} excludeOnChange={["Address:"]}/>
                        <Textbox label={"Owner:"} excludeOnChange={["Owner:"]}/>
                    <Button text = "Create" onClick={addBookstore}/>
                    <Button text ="View all" onClick={() => setPage("view")}/>
                    </form>
                    </div>
                );
            case "view":
                return(
                    <>
                        <TableForm setPage={setPage} setbookstoreToEdit={setbookstoreToEdit}/>
                        <Button text = "Add a new bookstore" onClick={() => setPage("index")}/>
                    </>
                );
            case "edit":
                return (
                    <div className ="container">
                        <h1>Edit user</h1>
                    <form id="editBookstoreForm">
                        <Textbox label={"Name:"} value={bookstoreToEdit.name} onChange={handleTextChange} />
                        <Textbox label={"Address:"} value={bookstoreToEdit.address} onChange={handleTextChange}/>
                        <Textbox label={"Owner:"} value={bookstoreToEdit.owner} onChange={handleTextChange}/>
                        <Button text = "Save changes" onClick={() =>(saveBookstoreChanges(bookstoreToEdit))}/>
                        <Button text ="View all" onClick={() => setPage("view")}/>
                    </form>
                    </div>
                );
        }
    }

    return (
        renderContent());
}