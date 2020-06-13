# `GET` /players/
Gets All Players.

## Response

- `200 OK` on success

```json
GET /players/
{
	"total":2,
	"players":
	[
		{
			"player_id": 1,
			"username": "Geoff",
			"last_login": null
		},
		{
			"player_id": 2,
			"username": "Jane",
			"last_login": "2020-06-13T22:52:43.511",
		}
	]
}
```


# `GET` /players/`{id}`

Get the data of a single player.

## Arguments
- `id` is the player_id of the requested player.

## Response
- `200 OK` on success

```json
GET /players/2
{
	"player_id": 2,
	"username": "Geoff",
	"last_login": null,
	"inv_id": 8,
}
```

- `404 Not Found` when the id does not exist.


# `GET` /inventories/`{id}`

Gets the contents of an inventory.

## Arguments

- `id` is the inv_id of the inventory.

## Response

- `200 OK` on success

```json
GET /inventories/8
{
	"inv_id":8,
	"name": "Player Inventory",
	"items": [2,5,9,4,33,281,12,3],
	"quantities": [1,4,22935,125,103,99,1,1]
}
```

- `404 Not Found` when the id does not exist.

- `401 Unauthorized` when the user does not have permission to view the inventory.

# `PUT` /inventories/`{id}`
Updates the items.

## Arguments

- `id` is the inv_id of the inventory.

## JSON Body Content

- `name` the new name of the inventory. *leave out for no change*
- `items` the array of item_id's that this inventory stores.
- `quantities` the array of the quantity of each item in the items array.


```json
PUT /inventories/8
{
	"name": "My Cool Inventory",
	"items": [2, 2, 2, 2, 2, 2, 2, 2, 2],
	"quantities": [1, 1, 1, 1, 1, 1, 1, 1, 1]
}
```

## Response
- `204 No Content` on successful update.
- `404 Not Found` when the id does not exist.
- `401 Unauthorized` when the user does not have permission to modify the inventory.