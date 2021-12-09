class Player {
    constructor(player, socket) {
        this.id = player.id
        this.name = player.username
        this.position = player.position
        this.color = player.color
        this.weight = player.weight
        this.speed = player.speed
        this.direction = UP
    }
}