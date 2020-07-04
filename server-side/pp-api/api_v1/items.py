from flask import Flask, request
from flask_restful import Resource, Api

from api_v1 import connection

class Item(Resource):
  def get(self, item_id):
    with connection.cursor() as cursor:

      # Get the first item that has the item_id from the items table.
      cursor.execute("SELECT * FROM `items` WHERE item_id = %s", item_id)
      result = cursor.fetchone()
      
      # Return an error 404 if there is no items.
      if result is None:
        return {"error":{
          "message":"The item ID of {} does not exist".format(item_id),
          "code": 0xF404
          }},404


      return result

class ItemList(Resource):
  def get(self):

    with connection.cursor() as cursor:

      # Get all items from the items table and return the results as a list.
      cursor.execute("SELECT * FROM items")
      result = cursor.fetchall()
      
      return list(result)