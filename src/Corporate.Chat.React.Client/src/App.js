import React from 'react';
import logo from './logo.svg';
import './App.css';
import Chat from './components/Chat'
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter as Router, Route, Link } from "react-router-dom";

function App() {
  return (
    <div className="App">
      <Router>
        <Route exact path="/" component={Chat} className="App-chat" />
      </Router>
    </div>
  );
}

export default App;
