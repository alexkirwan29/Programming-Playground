from flask import Flask, request
from flask_restful import Resource, Api

# Import the resources for players, items and inventories.
from .players import Player, PlayerList
from .items import Item, ItemList
from .inventories import Inventory

# Create our flask App and Api.
app = Flask(__name__)
api = Api(app)

# Add the player resources.
api.add_resource(Player, '/players/<int:player_id>')
api.add_resource(PlayerList, '/players/')

# Add the items resources.
api.add_resource(Item, '/items/<int:item_id>')
api.add_resource(ItemList, '/items/')

# Add the inventory resource.
api.add_resource(Inventory, '/inventories/<int:inv_id>')