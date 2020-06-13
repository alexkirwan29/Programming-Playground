# API V1 Objects




## Player



```json
{
	"player_id": 251,
	"username": "Tom Tom Tommy Thomas The Tom Engine",
	"last_login": "2020-06-13T22:52:43.511",
}
```

### `player_id` *uint*
The unique id of the player.


### `username` *string*
The username of the player.


### `last_login` *timestamp*
The last time this user logged in at GMT+0. **Formatting might change!**





## Items



```json
{
	"item_id": 3961,
	"name": "Rotten Apple",
	"description": "An Apple that sat in your school bag over the school holidays."
}
```

### `item_id` *uint*
The unique id of the item.


### `name` *string*
The name of the item.


### `description` *string*
The description of the item.




## Inventory



```json
{
	"inv_id": 42,
	"name": "Player Inventory",
	"items": [2,5,9,4,33,281,12,3],
	"quantities": [1,4,22935,125,103,99,1,1]
}
```

### `inv_id` *uint*
The unique id of the item.


### `name` *string*
The name of the inventory.


### `items` *uint[]*
An array of item id


### `quantities` *int[]*
An array of integers that represents the quantities of each item in the items array.