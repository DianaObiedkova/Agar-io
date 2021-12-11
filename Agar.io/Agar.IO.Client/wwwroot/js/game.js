const UP = 0
const RIGHT = 1
const DOWN = 2
const LEFT = 3

const fieldWidth = 1000
const fieldHeight = 1000

const visibleWidth = document.client.clientWidth
const visibleHeight = document.client.clientHeight

var currentPlayer = null;
var playersList = new PlayersList();

var socket = new WebSocket("ws://localhost:8080/" + username);
socket.onmessage = onMessage;
socket.onclose = onClose;

var field = document.getElementById("game-field")

field.width = visibleWidth
field.height = visibleHeight

var ctx = field.getContext("2d");
console.log(ctx);

document.onkeydown = function (e) {
    switch (e.key) {
        case "ArrowUp":
            currentPlayer.dirrection = UP
            break
        case "ArrowDown":
            currentPlayer.dirrection = DOWN
            break
        case "ArrowLeft":
            currentPlayer.dirrection = LEFT
            break
        case "ArrowRight":
            currentPlayer.dirrection = RIGHT
            break
    }
}

function move() {
    switch (currentPlayer.direction) {
        case LEFT:
            if (currentPlayer.position.x <= 0) break;
            currentPlayer.position.x -= currentPlayer.speed
            break;
        case UP: 
            if (currentPlayer.position.y <= 0) break;
            currentPlayer.position.y -= currentPlayer.speed
            break;
        case RIGHT:
            if (currentPlayer.position.x >= fieldWidth) break;
            currentPlayer.position.x += currentPlayer.speed
            break;
        case DOWN:
            if (currentPlayer.position.y >= fieldHeight) break;
            currentPlayer.position.y += currentPlayer.speed
            break;
    }

    update();
    let message = { "eventType": "CoordsChange", player: currentPlayer };
    socket.send(JSON.stringify(message))
}

function onMessage(e) {
    let message = JSON.parse(e.data);
    let player = new Player(message.person, socket);

    switch (e.eventType) {
        case "SpawnMyself":
            currentPlayer = player;
            playersList.add(player);
            break;
        case "Spawn":
            playersList.add(player);
            break;
        case "Die":
            playersList.remove(person);
            break;
        case "CoordsChange":
            playersList.change(person);
            break;
        case "SizeChange":
            playersList.resize(person);
            break;
        default:
            console.log(message);
            break;
        
    }

    update();
}

function update() {
    ctx.clearRect(0, 0, field.width, field.height);
    let x0 = 0, y0 = 0;

    //x0 and y0 changes

    playerList.toArray().forEach(function (item) {
        if (item.equals(currentPlayer)) {
            item.currentDraw(ctx, x0, y0);
        }
        else {
            item.draw(ctx, x0, y0);
        }
    });

}

function onClose() {
    alert("Connection with server is closed...");
}