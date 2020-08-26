import React from 'react';
import logo from './logo.svg';
import './App.css';
//import ChatComponent from './components/Chat';
import ChatHook from './components/ChatHook';
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter as Router, Route, Link } from "react-router-dom";

function App() {
  return (
    <div className="App">
      <Router>
        <Route exact path="/" component={ChatHook} className="App-chat" />
      </Router>
    </div>
  );
}

export default App;
