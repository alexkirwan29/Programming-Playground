# Error
An error contains a few details about why a API call failed.

## Examples
```json
{
	"code": 22,
	"message": "the authentication token was invalid",
}
```

## Variables
### `code` *16bit int*
The error code of this error. If presented to the end user format as hexadecimal. Example: `0x0fc4`.
Refer to the [errors documentation](./errors.md).

### `message` *string*
The string of the message.

	TODO: For Multilingual support this could possibly return something like [api-errors:auth-token-bad] which the game client can use to lookup in a language file. 
	
	Alternatively, have a plaintext error along side with this multilingual field if one it exists.

# Player
The player object stores data about the uses of the game. Every player who ever existed MUST have an entry in the player table.

Accessible at [/v1/players/](endpoints.md#players)

## Example
```json
{
	"player_id": 251,
	"username": "Tom Tom Tommy Thomas The Tom Engine",
	"last_login": "2020-06-13T22:52:43.511",
}
```

## Variables
### `player_id` *uint*
The unique id of the player.


### `username` *string*
The username of the player.


### `last_login` *timestamp*
The last time this user logged in at GMT+0. **Formatting might change!**





# Item
The Item object stores the data for all game items.

Accessible at [/v1/items/](endpoints.md#items)

## Example
```json
{
	"item_id": 3961,
	"name": "Rotten Apple",
	"description": "An Apple that sat in your school bag over the school holidays.",
	"image_location": "content.pp.nrms.xyz/items/3961.bmp"
}
```

## Variables
### `item_id` *uint*
The unique id of the item.


### `name` *string*
The name of the item.


### `description` *string*
The description of the item.

### `image_location` *string*
The url to the image/thumbnail of the item.


# Inventory
The inventory object stores the items and quantities for an inventory.

Accessible at [/v1/inventories/](endpoints.md#inventories)

## Example
```json
{
	"inv_id": 42,
	"name": "Player Inventory",
	"items": [2,5,9,4,33,281,12,3],
	"quantities": [1,4,22935,125,103,99,1,1]
}
```

## Variables
### `inv_id` *uint*
The unique id of the item.


### `name` *string*
The name of the inventory.


### `items` *uint[]*
An array of item id


### `quantities` *int[]*
An array of integers that represents the quantities of each item in the items array.