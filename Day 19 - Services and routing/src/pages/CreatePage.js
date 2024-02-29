import React from "react";
import Textbox from "../components/Textbox";
import { Bookstore } from "../models/bookstore";
import { BookstoreService } from "../services/BookstoreService";
import Button from "../components/Button";

// Routing

export default function CreatePage(){
    const bookstoreService = new BookstoreService();

    async function addBookstore(){
        const nameInput = document.forms.createBookstoreForm.Name.value;
        const addressInput = document.forms.createBookstoreForm.Address.value;
        const ownerInput = document.forms.createBookstoreForm.Owner.value;

        await bookstoreService.addAsync(nameInput, addressInput, ownerInput);
    }

    return(
        <div className ="container">
            <h1>Add a new bookstore</h1>
                <form id="createBookstoreForm">
                    <Textbox label={"Name:"} excludeOnChange={["Name:"]}/>
                    <Textbox label={"Address:"} excludeOnChange={["Address:"]}/>
                    <Textbox label={"Owner:"} excludeOnChange={["Owner:"]}/>
                    <Button text = "Create" onClick={addBookstore}/>
                </form>
        </div>
    );
}