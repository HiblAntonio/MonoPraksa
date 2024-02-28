import React from "react";
import Button from "./Button";
import Textbox from "./Textbox";
import { useState } from "react";

export default function EditForm()
{
    const [bookstores, setBookstores] = useState([]);

    function saveBookstoresChanges() {
        const idInput = document.forms.createUserForm.id.value;
        const nameInput = document.forms.createUserForm.Name.value;
        const addressInput = document.forms.createUserForm.Address.value;
        const ownerInput = document.forms.createUserForm.Owner.value;
    
        const index = bookstores.findIndex(user => user.id === idInput);
    
        const updatedBookstores = [...bookstores];
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

        console.log("Bookstore updated successfully");
    }

    return (
        <>
            <Textbox label={"Id:"}/>
            <Textbox label={"Name:"}/>
            <Textbox label={"Lastname:"}/>
            <Textbox label={"Email:"}/>
            <Button text = "Save changes" onClick={saveUserChanges}/>
        </>
    );
}