class PlayersList {
    constructor() {
        this.players = []
    }

    add(player) {
        this.players.push(player)
    }

    remove(player) {
        let index = this.getArrayIndex(player)
        this.players.splice(index, 1)
    }

    change(player) {
        let index = this.getArrayIndex(player)
        this.players[index].position = player.position
    }

    resize(player) {
        let index = this.getArrayIndex(person)
        this.players[index].size = player.size
    }
}