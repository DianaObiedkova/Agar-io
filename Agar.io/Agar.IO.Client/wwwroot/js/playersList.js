class PlayersList {
    constructor() {
        this.persons = []
    }

    add(person) {
        this.persons.push(person)
    }

    remove(person) {
        let index = this.getArrayIndex(person)
        this.persons.splice(index, 1)
    }

    change(person) {
        let index = this.getArrayIndex(person)
        this.persons[index].position = person.position
    }

    resize(person) {
        let index = this.getArrayIndex(person)
        this.persons[index].size = person.size
    }
}