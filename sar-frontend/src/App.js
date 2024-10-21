import React, { Component } from "react";
import {Route, NavLink, HashRouter, Routes} from "react-router-dom";

import Home from "./content_pages/Home"
import About from "./content_pages/About"
import Contact from "./content_pages/Contact"

class App extends Component {
  render() {
    return (
      <HashRouter>
        <div className="App">
          <ul className="header"> {/* Header component */}
            <li><NavLink to="/">Home</NavLink></li>
            <li><NavLink to="/about">About</NavLink></li>
            <li><NavLink to="/contact">Contact</NavLink></li>
          </ul>
          <div className="content"> {/* Content component */}
            <Routes>
              <Route exact path="/" element={<Home />}></Route>
              <Route exact path="/about" element={<About />}></Route>
              <Route exact path="/contact" element={<Contact />}></Route>
            </Routes>
          </div>
        </div>
      </HashRouter>
    );
  }
}
export default App;
