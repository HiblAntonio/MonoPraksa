class User{
    constructor(id, firstName, lastName, mailAddress)
    {
        this.id = id;
        this.firstName = firstName;
        this.lastName = lastName;
        this.mailAddress = mailAddress;
    }
}

function saveUserToLocalStorage(user){
    localStorage.setItem('users', JSON.stringify(user))
}

function getUsersFromLocalStorage() {
    return localStorage.getItem('users') ? JSON.parse(users) : [];
}

function checkIfUserExists(users){
    if (users.some(user => user.id === document.getElementById("id").value)) {
        console.log("ID already exists");
        return true;
    }
}

function createUser()
{
    const idInput = document.getElementById("id").value;
    const fnameInput = document.getElementById("fname").value;
    const lnameInput = document.getElementById("lname").value;
    const emailInput = document.getElementById("email").value;

    if (isNaN(parseInt(idInput))) {
        alert("Please enter a valid integer ID.");
        return;
    }

    const user = new User(idInput, fnameInput, lnameInput, emailInput);

    if (localStorage.getItem('users') === null) {
        const users = [];
        if(checkIfUserExists(users)) return;
        users.push(user);
        localStorage.setItem('users', JSON.stringify(users));
    } else {
        const users = JSON.parse(localStorage.getItem('users'));
        if(checkIfUserExists(users)) return;
        users.push(user);
        localStorage.setItem('users', JSON.stringify(users));
    }

    console.log("New user:", user);
}

function loadUserTable(){
    const usersList = JSON.parse(localStorage.getItem('users'));

    if(!usersList) return;

    let tableRows = `<tr>
                <th>First name</th>
                <th>Last name</th>
                <th>Email</th>
                <th>Actions</th>
            </tr>`;

    usersList.forEach(user => {
        tableRows += `
            <tr>
                <td>${user.firstName}</td>
                <td>${user.lastName}</td>
                <td>${user.mailAddress}</td>
                <td class="td_odd">
                    <a href="edit.html?id=${user.id}"><button class="actionButton editActionButton"">Edit</button></a>
                    <button class="actionButton deleteActionButton" onclick="deleteUserById(${user.id})">Delete</button>
                </td>
            </tr>`;
    });

    document.getElementById('usersList').innerHTML = tableRows;
}

function deleteUserById(userId) {
    if (confirm("Are you sure you want to delete this user?")) {
        let usersList = JSON.parse(localStorage.getItem('users'));

        usersList = usersList.filter(user => parseInt(user.id) !== parseInt(userId));

        console.log("New user list after deletion:", usersList);

        localStorage.setItem('users', JSON.stringify(usersList));

        loadUserTable();
    } else {
        return;
    }
}

function editUser(userId) {
    const users = JSON.parse(localStorage.getItem('users'));
    const user = users.find(user => user.id === userId);

    if (!user) {
        console.error("User not found");
        return;
    }

    // Construct the URL for edit.html with the user ID as a query parameter
    const editUrl = `edit.html?id=${user.id}`;

    // Redirect to the edit.html page
    window.location.href = editUrl;
}

function saveUserChanges() {
    const idInput = document.getElementById("id").value;
    const fnameInput = document.getElementById("fname").value;
    const lnameInput = document.getElementById("lname").value;
    const emailInput = document.getElementById("email").value;

    const users = JSON.parse(localStorage.getItem('users'));

    const index = users.findIndex(user => user.id === idInput);

    if (index === -1) {
        console.error("User not found");
        return;
    }

    users[index].firstName = fnameInput;
    users[index].lastName = lnameInput;
    users[index].email = emailInput;

    localStorage.setItem('users', JSON.stringify(users));

    console.log("User updated successfully");
}

function insertValuesIntoTextboxes(){
    const params = new URLSearchParams(window.location.search);
            const userId = params.get('id');

            const users = JSON.parse(localStorage.getItem('users'));
            const user = users.find(user => user.id === userId);

            if (!user) {
                console.error("User not found");
                return;
            }

            document.getElementById("id").value = user.id;
            document.getElementById("fname").value = user.firstName;
            document.getElementById("lname").value = user.lastName;
            document.getElementById("email").value = user.mailAddress;
}