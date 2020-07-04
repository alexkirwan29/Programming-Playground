from flask import Flask, request
from flask_restful import Resource, Api

from api_v1 import connection

class Inventory(Resource):
  def get(self, inv_id):
    with connection.cursor() as cursor:

      # Get the information of an inventory.
      query = "SELECT name, max_slots FROM inventories WHERE inv_id = %s"
      cursor.execute(query, inv_id)
      invMetaData = cursor.fetchone()

      # Return an error 404 if there is no inventory.
      if invMetaData is None:
        return {"error":{
          "message":"The inventory ID of {} does not exist".format(inv_id),
          "code": 0xF404
          }},404


      # Get the items in the inventory. Rename item_id as i and quantity as q
      query = "SELECT item_id as i, quantity as q FROM inventory_map WHERE inv_id = %s"
      cursor.execute(query, inv_id)
      invItems = cursor.fetchall()


      # Add a field called slots containing the items of the inventory to the invMetaData.
      invMetaData.update({"slots":invItems})
      return invMetaData