# Pony Challenge API
https://ponychallenge.trustpilot.com/index.html

### Help the pony escape the Domokun!
1. Create your maze API call (dimensions 15 to 25) + valid pony name
2. Get the maze with the ID from 1: you will get pony (player) location, Domokun (monster) location and maze walls
3. Move your pony (until you are dead or you reach the end-point)
(you can also print the maze with the API)

```
create new maze game
POST /pony-challenge/maze
```

```
get maze current state
GET /pony-challenge/maze/{maze-id}
```

```
make next move in the maze
POST /pony-challenge/maze/{maze-id}
```

```
get visual of the current state of the maze
GET /pony-challenge/maze/{maze-id}/print
```