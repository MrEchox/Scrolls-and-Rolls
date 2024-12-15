import React from "react";

const Footer = () => {
    return (
        <footer className="footer">
            <div className="footer-container">
                <p>A project done by: Alanas Švažas IFF-1/7</p>
                <p>&copy; 2024 Scrolls and Rolls. All Rights Reserved.</p>
                <br />
                <ul className="footer-links">
                    <li>
                        <a className="footer-link">
                            Privacy Policy
                        </a>
                    </li>
                    <li>
                        <a className="footer-link">
                            Terms of Service
                        </a>
                    </li>
                    <li>
                        <a className="footer-link">
                            Contact Us
                        </a>
                    </li>
                </ul>
            </div>
        </footer>
    );
};

export default Footer;
