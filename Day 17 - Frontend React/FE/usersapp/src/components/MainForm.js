import React from "react";
import Textbox from "./Textbox";
import { useState } from "react";
import Button from "./Button";
import TableForm from "./TableForm";

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
        constructor(id, Name, Address, Owner)
        {
            this.id = id;
            this.Name = Name;
            this.Address = Address;
            this.Owner = Owner;
        }
    }

    function createBookstore()
    {
        const idInput = crypto.randomUUID();
        const nameInput = document.forms.createBookstoreForm.Name.value;
        const addressInput = document.forms.createBookstoreForm.Address.value;
        const ownerInput = document.forms.createBookstoreForm.Owner.value;

        const bookstore = new Bookstore(idInput, nameInput, addressInput, ownerInput);

        if (localStorage.getItem('bookstores') === null) {
            const bookstores = [];
            bookstores.push(bookstore);
            localStorage.setItem('bookstores', JSON.stringify(bookstores));
        } else {
            const bookstores = JSON.parse(localStorage.getItem('bookstores'));
            bookstores.push(bookstore);
            localStorage.setItem('bookstores', JSON.stringify(bookstores));
        }

        setBookstores(localStorage.getItem('bookstores'));

        console.log("New bookstore:", bookstore);
    }

    function saveBookstoreChanges() {
        const idInput = bookstoreToEdit.id;
        const nameInput = bookstoreToEdit.Name;
        const addressInput = bookstoreToEdit.Address;
        const ownerInput = bookstoreToEdit.Owner;
    
        const updatedBookstores =JSON.parse(localStorage.getItem('bookstores'));
        const index = updatedBookstores.findIndex(bookstore => bookstore.id === idInput);
    
        // Update the user details in the copied array
        updatedBookstores[index] = {
            ...updatedBookstores[index],
            Name: nameInput,
            Address: addressInput,
            Owner: ownerInput
        };

        // Update state with the modified copy
        setBookstores(updatedBookstores);

        // Update localStorage
        localStorage.setItem('bookstores', JSON.stringify(updatedBookstores));

        console.log("Bookstores updated successfully");
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
                    <Button text = "Create" onClick={createBookstore}/>
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
                    <form id="createBookstoreForm">
                    <Textbox label={"Name:"} value={bookstoreToEdit.Name} onChange={handleTextChange} />
                    <Textbox label={"Address:"} value={bookstoreToEdit.Owner} onChange={handleTextChange}/>
                    <Textbox label={"Owner:"} value={bookstoreToEdit.Address} onChange={handleTextChange}/>
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