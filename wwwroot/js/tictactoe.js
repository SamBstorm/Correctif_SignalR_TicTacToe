"use strict";

let connection = new signalR.HubConnectionBuilder().withUrl('/ttt_hub').build();
let user = { userId : '', username : '' };

connection.start()
    .then(
        () => {
            do {
                user.username = prompt('Quel est votre nom?');
            } while (user.username == '' || user.username == null);
            console.log(user);
            connection.invoke('NewConnection', user);
        })
    .catch(
        (err) => console.error(err)
    );

connection.on('SetAsPlayer', (number) => {
    alert('Vous êtes joueur ' + number + '!');
    connection.invoke('CheckPlayers');
});
connection.on('NoMorePlayer', (username) => alert('Vous êtes spectateur :' + username));
connection.on('Registered', (userCS) => {
    user = userCS;
});

connection.on('StartGame', (playerId) => {
    alert('Let\'s Go!');
    if (playerId == user.userId) {
        document.querySelectorAll('input[type=button]')
            .forEach(
                (btn) => {
                    btn.disabled = false;
                });
    }
});

connection.on('BadMove', () => { alert('Mauvais choix...'); });
connection.on('EndGame', (userId, row, col) => {
    if (userId == user.userId) {
        document.querySelectorAll('input[type=button]')
            .forEach(
                (btn) => {
                    if (btn.getAttribute('row') == row && btn.getAttribute('col') == col) btn.value = 'O';
                    btn.disabled = true;
                });
    }
    else {
        document.querySelectorAll('input[type=button]')
            .forEach(
                (btn) => {
                    btn.disabled = true;
                });
    }
    console.invoke("Reset");
    alert('Fin de partie');
});

connection.on('NewTurn', (userId, row, col) => {
    if (userId == user.userId) {
        document.querySelectorAll('input[type=button]')
            .forEach(
                (btn) => {
                    if (btn.value != 'O' && btn.value != 'X') btn.disabled = false;
                    if (btn.getAttribute('row') == row && btn.getAttribute('col') == col) {
                        btn.value = 'O';
                        btn.disabled = true;
                    }
                });
    }
    else {
        document.querySelectorAll('input[type=button]')
            .forEach(
                (btn) => {
                    btn.disabled = true;
                });
    }
});

document.querySelectorAll('input[type=button]')
    .forEach(
        (btn) => {
            btn.disabled = true;
            btn.addEventListener('click', (event) => {
                let row = btn.getAttribute('row');
                let col = btn.getAttribute('col');
                btn.value = "X";
                connection.invoke('PlayerPlayed', user.userId, row, col);
                event.preventDefault();
            })
        });