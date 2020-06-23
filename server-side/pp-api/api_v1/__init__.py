from flask import Flask, request
from flask_restful import Resource, Api
import pymysql.cursors
import os

# Create our flask App and Api.
app = Flask(__name__)
api = Api(app)

connection = pymysql.connect(
  host='database',
  user='pp-user',
  db='pp',
  charset='utf8',
  cursorclass=pymysql.cursors.DictCursor
)

# Import the resources for players, items and inventories.
from .players import Players, PlayerList
from .items import Item, ItemList
from .inventories import Inventory

# Add the player resources.
api.add_resource(Players, '/players/<int:player_id>')
api.add_resource(PlayerList, '/players/')

# Add the items resources.
api.add_resource(Item, '/items/<int:item_id>')
api.add_resource(ItemList, '/items/')

# Add the inventory resource.
api.add_resource(Inventory, '/inventories/<int:inv_id>')