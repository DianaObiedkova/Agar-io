const UP = 0
const RIGHT = 1
const DOWN = 2
const LEFT = 3

var currentPlayer = null;
var playersList = new PlayersList();

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

}