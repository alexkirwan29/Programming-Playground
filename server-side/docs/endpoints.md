# <a name="players"></a> `GET` /v1/players/
Gets All Players.

## Response

- `200 OK` on success, returns a list of [player objects](objects.md#Player).
```json
GET /v1/players/
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


# `GET` /v1/players/`{id}`

Get the data of a single player.

## Arguments
- `id` is the player_id of the requested player.

## Response
- `200 OK` on success, returns a single [player object](objects.md#Player).

```json
GET /v1/players/2
{
	"player_id": 2,
	"username": "Geoff",
	"last_login": null,
	"inv_id": 8,
}
```

- `404 Not Found` when the id does not exist.

# <a name="items"></a> `GET` /v1/items/

Gets all the items.

## Response
- `200 OK` on success, returns a list of [Item objects](objects.md#Item).

```json
GET /v1/items/
[
	{
		"item_id": 3960,
		"name": "Slightly Bruised Apple",
		"description": "An Apple you didn't eat because it was lightly bruised.",
		"item_location": "content.pp.nrms.xyz/items/3960.bmp"
	},
	{
		"item_id": 3961,
		"name": "Rotten Apple",
		"description": "An Apple that sat in your school bag over the school holidays.",
		"image_location": "content.pp.nrms.xyz/items/3961.bmp"
	}
]
```

# `GET` /v1/items/`{id}`

Gets a single item.

## Arguments

- `id` is the item_id of an item.

## Response
- `200 OK` on success, returns a single [Item objects](objects.md#Item).

```json
GET /v1/items/{3960}
{
	"item_id": 3960,
	"name": "Slightly Bruised Apple",
	"description": "An Apple you didn't eat because it was lightly bruised.",
	"item_location": "content.pp.nrms.xyz/items/3960.bmp"
}
```

- `404 Not Found` when the item id does not exist.

# <a name="inventories"></a> `GET` /v1/inventories/`{id}`

Gets the contents of an inventory.

## Arguments

- `id` is the inv_id of a inventory.

## Response

- `200 OK` on success, returns a single [Inventory object](objects.md#Inventory).

```json
GET /v1/inventories/8
{
  "name": "Player Inventory",
  "max_slots": 10,
  "slots": [
    {
      "i": 1,
      "q": 4
    },
    {
      "i": 3,
      "q": 2
    },
    {
      "i": 6,
      "q": 2
    }
  ]
}
```

- `404 Not Found` when the inv_id does not exist.
- `401 Unauthorized` when the user does not have permission to view the inventory.

# `PUT` /v1/inventories/`{id}`
Updates the items of an [Inventory Object](objects.md#Inventory).

## Arguments

- `id` is the inv_id of the inventory.

## JSON Body Content

- `name` the new name of the inventory. *leave out for no change*
- `slots` the array of items in this inventory.


```json
PUT /v1/inventories/8
{
  "name": "Player Inventory",
  "max_slots": 10,
  "slots": [
    {
      "i": 1,
      "q": 4
    },
    {
      "i": 3,
      "q": 2
    },
    {
      "i": 6,
      "q": 2
    }
  ]
}
```

## Response
- `204 No Content` on successful update.
- `404 Not Found` when the inv_id does not exist.
- `401 Unauthorized` when the user does not have permission to modify the inventory.