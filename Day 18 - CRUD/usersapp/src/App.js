import { useEffect, useState } from 'react';
import './App.css';
import Form from './components/MainForm.js';
import axios from 'axios';

function App() {
  const [bookstore, setBookstores] = useState([]);

  useEffect(() => {
    const fetchValuesFromAPI = async () => {
      try{
        const response = await axios.get('https://localhost:44349/api/Bookstore');
        setBookstores(response.data);
      } catch (error)
      {
        console.error(error);
      }
    };

    fetchValuesFromAPI();
  }, []);

  return (
    <div>
      <header>
        <Form />
      </header>
    </div>
  );
}

export default App;
