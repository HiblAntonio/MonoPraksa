import axios from "axios";
import { v4 as uuidv4 } from 'uuid';

export class BookstoreService{
    async getAsync(pageSize, currentPage, searchQuery){
        try {
            const response = await axios.get(`https://localhost:44349/api/bookstore?pageSize=${pageSize}&pageNum=${currentPage}&searchQuery=${searchQuery}`);
            return response.data;

            //headers
        } catch (error) {
            console.error('Error fetching bookstores:', error);
        }
    }

    async getBookstoreById(bookstoreId){
        try {
            const response = await axios.get(`https://localhost:44349/api/bookstore/${bookstoreId}`);
            return response.data;
        } catch (error) {
            console.error(`Error fetching bookstore with ID ${bookstoreId}:`, error);
            throw error;
        }
    }

    async addAsync(name, address, owner){
        try {
            const idInput = uuidv4();
            const books = [];

            const bookstore = {
                id: idInput,
                name: name,
                owner: owner,
                address: address,
                books: books
            };

            // Send a POST request to the API endpoint to add a new bookstore
            const response = await axios.post('https://localhost:44349/api/bookstore', bookstore);
    
            console.log("Bookstore added successfully:", response.data);
            return response.data; // Return the added bookstore data
        } catch (error) {
            console.error("Error adding bookstore:", error);
            throw error; // Rethrow the error for the caller to handle
        }
    }

    async updateAsync(id, name, address, owner){
        try {
            const updatedBookstore = {
                id: id,
                name: name,
                owner: owner,
                address: address
            };

            // Send a PUT request to the API endpoint to update the specified bookstore
            await axios.put(`https://localhost:44349/api/bookstore/${id}`, updatedBookstore);

            console.log("Bookstore updated successfully");
        } catch (error) {
            console.error("Error updating bookstore:", error);
            throw error; // Rethrow the error for the caller to handle
        }
    }

    async deleteAsync(bookstoreId){
        try {
            // Send a DELETE request to the API endpoint to delete the specified bookstore
            await axios.delete(`https://localhost:44349/api/bookstore/${bookstoreId}`);

            console.log("Bookstore deleted successfully");
        } catch (error) {
            console.error("Error deleting bookstore:", error);
            throw error; // Rethrow the error for the caller to handle
        }
    }
}