import React from 'react';
import logo from './logo.svg';
import './App.css';
//import ChatComponent from './components/Chat'
import ChatHook from './components/ChatHook'
import 'bootstrap/dist/css/bootstrap.min.css';
import { BrowserRouter as Router, Route, Link } from "react-router-dom";

const App1 = ({ children }) => (
  <>

    {children}
  </>
);


const HasRouter = () => (

  <Router>
    <Route exact path="/" component={ChatHook} className="App-chat" />
  </Router>
)

function App() {
  return (
    <div className="App">
      <HasRouter />
    </div>
  );
}

export default App;
