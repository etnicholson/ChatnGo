var urlParams = new URLSearchParams(window.location.search);
console.log(urlParams.get('UserName')); 



var chatterName = urlParams.get('UserName'); 

var roomNumber = urlParams.get('RoomName'); 

// Initialize the SignalR client
var connection = new signalR.HubConnectionBuilder()
    .withUrl('/chatHub?roomNumber=' + roomNumber + '&chatterName=' + chatterName)
    .build();



connection.on('ReceiveMessage', renderMessage);

connection.on('Send', renderJoin);
connection.on('NewName', setName);

connection.on('Left', renderLeft);
connection.on('UserJoin', renderUsers);
connection.start();



connection.onclose(function () {
    connection.invoke('RemoveUser', chatterName)
    //console.log('Reconnecting in 5 seconds...');
    //setTimeout(startConnection, 5000);
})










//function showChatDialog() {
//    var dialogEl = document.getElementById('chatDialog');
//    dialogEl.style.display = 'block';
//}

function setRoomNumber() {
    var roomSpan = document.createElement('span');
    roomSpan.textContent = 'Room: ' + roomNumber;
    var headerText = document.getElementById('heading');
    headerText.appendChild(roomSpan);


}





function setName(name) {
    chatterName = name;


}

function sendMessage(text) {
    if (text && text.length) {
        connection.invoke('SendMessage', chatterName, text, roomNumber);
    }
}

function ready() {


    setRoomNumber()
    var chatFormEl = document.getElementById('chatForm');
    chatFormEl.addEventListener('submit', function (e) {
        e.preventDefault();

        var text = e.target[0].value;
        e.target[0].value = '';
        sendMessage(text);
    })
    connection.on('ReceiveMessages', renderMessages);
    connection.invoke('AddToGroup', roomNumber, chatterName);

}


function renderMessage(name, time, message) {
    var nameSpan = document.createElement('span');
    nameSpan.className = 'name';
    nameSpan.textContent = name;

    var timeSpan = document.createElement('span');
    timeSpan.className = 'time';
    var friendlyTime = moment(time).format('H:mm');
    timeSpan.textContent = friendlyTime;

    var headerDiv = document.createElement('div');
    headerDiv.appendChild(nameSpan);
    headerDiv.appendChild(timeSpan);


    if (name === chatterName) {
        var messageDiv = document.createElement('div');
        messageDiv.className = 'MyMessage';
        messageDiv.textContent = message;

    }
    else {
        var messageDiv = document.createElement('div');
        messageDiv.className = 'message';
        messageDiv.textContent = message;

    }


    var newItem = document.createElement('li');
    newItem.appendChild(headerDiv);
    newItem.appendChild(messageDiv);

    var chatHistoryEl = document.getElementById('chatHistory');
    chatHistoryEl.appendChild(newItem);
    chatHistoryEl.scrollTop = chatHistoryEl.scrollHeight - chatHistoryEl.clientHeight;
}







function renderJoin(name, time, message) {
    var nameSpan = document.createElement('span');
    nameSpan.className = 'nameExtra';
    nameSpan.textContent = name;

    var timeSpan = document.createElement('span');
    timeSpan.className = 'time';
    var friendlyTime = moment(time).format('H:mm');
    timeSpan.textContent = friendlyTime;

    var headerDiv = document.createElement('div');
    headerDiv.appendChild(nameSpan);
    headerDiv.appendChild(timeSpan);

    var messageDiv = document.createElement('div');
    messageDiv.className = 'messageJoin';
    messageDiv.textContent = message;

    var newItem = document.createElement('li');
    newItem.appendChild(headerDiv);
    newItem.appendChild(messageDiv);

    var chatHistoryEl = document.getElementById('chatHistory');
    chatHistoryEl.appendChild(newItem);
    chatHistoryEl.scrollTop = chatHistoryEl.scrollHeight - chatHistoryEl.clientHeight;
}



function renderLeft(name, time, message) {
    var nameSpan = document.createElement('span');
    nameSpan.className = 'nameExtraLeft';
    nameSpan.textContent = name;

    var timeSpan = document.createElement('span');
    timeSpan.className = 'time';
    var friendlyTime = moment(time).format('H:mm');
    timeSpan.textContent = friendlyTime;

    var headerDiv = document.createElement('div');
    headerDiv.appendChild(nameSpan);
    headerDiv.appendChild(timeSpan);

    var messageDiv = document.createElement('div');
    messageDiv.className = 'messageJoin';
    messageDiv.textContent = message;

    var newItem = document.createElement('li');
    newItem.appendChild(headerDiv);
    newItem.appendChild(messageDiv);

    var chatHistoryEl = document.getElementById('chatHistory');
    chatHistoryEl.appendChild(newItem);
    chatHistoryEl.scrollTop = chatHistoryEl.scrollHeight - chatHistoryEl.clientHeight;
}


function renderMessages(messages) {
    if (!messages) return;

    messages.forEach(function (m) {
        renderMessage(m.senderName, m.sentAt, m.text);
    });
}




function renderUser(name) {
    var nameSpan = document.createElement('span');
    nameSpan.className = 'name';
    nameSpan.textContent = name;
    var newItem = document.createElement('li');
    newItem.appendChild(nameSpan);


    var userHistory = document.getElementById('userList');
    userHistory.appendChild(newItem);


}


function renderUsers(users) {

    var userHistory = document.getElementById('userList');
    userHistory.innerHTML = "";
    users.forEach(function (m) {
        renderUser(m);
    });

}



document.addEventListener('DOMContentLoaded', ready);


window.addEventListener('unload', function (event) {

    connection.invoke('RemoveUser', roomNumber,chatterName)
})