import './App.css';
import { Route, Routes, Link } from 'react-router-dom';
import CreatePage from './pages/CreatePage.js';
import TableViewPage from './pages/TableViewPage.js';
import EditPage from './pages/EditPage.js';

function App() {
  return (
    <div>
  <nav className="navbar navbar-expand navbar-dark bg-dark fixed-top">
    <div className="navbar-nav mr-auto">
      <ul>
        <li className="nav-item">
          <Link to={"/create"} className="nav-link">
            Add a bookstore
          </Link>
        </li>
        <li className="nav-item">
          <Link to={"/view"} className="nav-link">
            Table of bookstores
          </Link>
        </li>
      </ul>
    </div>
  </nav>
  <div className="container mt-5"> {/* Added margin top to push content down below the navigation */}
    <Routes>
      <Route path="/create" element={<CreatePage />} />
      <Route path="/view" element={<TableViewPage />} />
      <Route path="/edit/:id" element={<EditPage />} />
    </Routes>
  </div>
</div>
  );
}

export default App;
