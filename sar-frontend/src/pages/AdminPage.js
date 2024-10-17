import React from 'react';
import './styles/Admin.css';

const AdminPage = () => {
    // Define a sample user list for display
    const users = [
        { id: 1, email: 'user1@example.com', accountType: 'Player' },
        { id: 2, email: 'user2@example.com', accountType: 'Game Master' },
        { id: 3, email: 'user3@example.com', accountType: 'Administrator' },
    ];

    // Define available roles
    const roles = ['Player', 'Game Master', 'Administrator'];

    return (
        <div className="admin-page">
            <h2>User Management</h2>
            <table>
                <thead>
                    <tr>
                        <th>Email</th>
                        <th>Current Role</th>
                        <th>Change Role</th>
                    </tr>
                </thead>
                <tbody>
                    {users.map(user => (
                        <tr key={user.id}>
                            <td>{user.email}</td>
                            <td>{user.accountType}</td>
                            <td>
                                <select value={user.accountType}>
                                    {roles.map((role) => (
                                        <option key={role} value={role}>{role}</option>
                                    ))}
                                </select>
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
};

export default AdminPage;
