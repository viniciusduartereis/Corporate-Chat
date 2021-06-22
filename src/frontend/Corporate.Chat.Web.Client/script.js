(function () {

    var name = '';
    var isConnected = false;

    moment.locale('pt-br');

    name = localStorage.getItem('chatusername');
    if (!name) {
        name = prompt('Type your name...');
        while (!name) {
            name = prompt('Please type your name...');
        }
        document.getElementById('username').innerHTML = name;
    }

    document.getElementById('username').innerHTML = name;
    localStorage.setItem('chatusername', name);

    changeUsername = () => {
        var name = prompt('Type your name...');
        while (!name) {
            name = prompt('Please type your name...');
        }
        localStorage.setItem('chatusername', name);
        document.getElementById('username').innerHTML = name;
    }

    let messageInput = document.getElementById('message');
    messageInput.focus();


    // Start the connection.
    let hubConnection = new signalR.HubConnectionBuilder()
        .withUrl('http://localhost:5000/chat')
        .build();

    hubConnection.connection.onclose = (error) => {
        isConnected = false;
        console.log(`disconnect: ${error.message}`);
        setTimeout(() => {
            if (!isConnected) {
                startConnection();
            }
        }, 3000);
    };

    startConnection = () => {
        // Transport fallback functionality is now built into start.
        hubConnection.start()
            .then(() => {
                console.log('connection started');
                isConnected = true;
                setStatusConnection();
                clearTimeout();
                sendUserName();
            })
            .catch(error => {
                console.error(`error on start: ${error.message}`);
                isConnected = false;
                setStatusConnection();
                setTimeout(() => {
                    if (!isConnected) {
                        startConnection();
                    }
                }, 3000);
            });
    }

    function setStatusConnection() {
        var status = document.getElementById('status');
        if (isConnected) {
            status.className = 'badge badge-success';
        } else {
            status.className = 'badge badge-danger';
        }
    }

    startConnection();

    sendUserName = () => {
        if (isConnected) {
            hubConnection.invoke('onUserConnected', {
                name: name,
                text: ''
            });
        }
    }

    sendMessage = () => {

        if (!messageInput.value) {
            messageInput.focus();
            return;
        }
        var message = {
            text: messageInput.value,
            name: name
        };

        hubConnection.invoke('send', message);

        messageInput.value = '';
        messageInput.focus();
    }

    document.addEventListener('submit', (event) => {
        sendMessage();
        event.preventDefault();
        return false;
    });

    document.getElementById('send').addEventListener('click', (event) => {
        sendMessage();
        event.preventDefault();
    });

    bindMessage = (message) => {
        var encodedDate = moment(new Date(Date.parse(message.createdDate))).format('L LTS');
        var liElement = document.createElement('li');
        liElement.classList.add('list-group-item')
        liElement.innerHTML = `<strong> ${message.name} </strong>:&nbsp;&nbsp;${message.text}<br/> <i> ${encodedDate} <i/>`;
        document.getElementById('messages').appendChild(liElement);
    }

    hubConnection.on('receiveMessage', (message) => {
        console.log(`receive message: ${JSON.stringify(message)}`);
        bindMessage(message);
    });

    hubConnection.on('userDisconnected', (message) => {
        console.log(`user disconnected': ${JSON.stringify(message)}`);
        bindMessage(message);
    });

    hubConnection.on('userConnected', (message) => {
        console.log(`user connected': ${JSON.stringify(message)}`);
        bindMessage(message);
    });



})();