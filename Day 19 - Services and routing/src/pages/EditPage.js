import React, { useEffect } from "react";
import { BookstoreService } from "../services/BookstoreService";
import Textbox from "../components/Textbox";
import { useState } from "react";
import Button from "../components/Button";
import { Link, useParams } from "react-router-dom";

export default function EditPage(){

    const bookstoreService = new BookstoreService();
    const [bookstoreToEdit, setBookstoreToEdit] = useState({});
    const [textValue, setTextValue] = useState('');
    const params = useParams();
    useEffect(() => {
        getBookstore();
    }, [])

    const handleTextChange = (newValue) => {
        // Update the state with the new value
        setTextValue(newValue);
    };
 
    async function getBookstore(){
        await bookstoreService.getBookstoreById(params.id).then((bookstore) => {
            setBookstoreToEdit(bookstore);
        });
    }

    async function updateBookstore(){
        await bookstoreService.updateAsync(params.id);
    }

    return(
        bookstoreToEdit.name &&
        <div>
            <h1>Edit user</h1>
            <div className ="container">
                <form id="editBookstoreForm">
                    <Textbox label={"Name:"} value={bookstoreToEdit.name} onChange={handleTextChange} />
                    <Textbox label={"Address:"} value={bookstoreToEdit.address} onChange={handleTextChange}/>
                    <Textbox label={"Owner:"} value={bookstoreToEdit.owner} onChange={handleTextChange}/>
                    <Link to={{pathname:"/view"}}onClick={() => updateBookstore()}>Save changes</Link>
                    {/* <Button text = "Save changes" onClick={() =>(updateBookstore())}/> */}
                </form>
            </div>
        </div>

    );
}