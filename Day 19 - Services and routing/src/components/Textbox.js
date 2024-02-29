import React, { useState } from 'react';

export default function Textbox({label, value, onChange, excludeOnChange = []})
{
    const initialValue = value || '';
    const [inputValue, setInputValue] = useState(initialValue);
    const handleChange = (event) => {
        const newValue = event.target.value;
        setInputValue(newValue);
        // Call the onChange function with the new value
        console.log(excludeOnChange);
        console.log(excludeOnChange.includes(label) );
        if (!excludeOnChange.includes(label)) {
            console.log("RADI")
            onChange(newValue);
        }
    };
    
    return (
        <>
            <label for="id">{label}</label>
            <input 
                type="text" 
                id={label.split(':')[0]} 
                name={label.split(':')[0]} 
                value={inputValue}
                onChange={handleChange}>
            </input> 
            <br></br>
        </>
    );
    }