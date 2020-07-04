from flask import Flask, request
from flask_restful import Resource, Api

from api_v1 import connection

class Inventory(Resource):
	def get(self, inv_id):
		with connection.cursor() as cursor:
			query = "SELECT * FROM inventories WHERE inv_id = %s"
			# query = "SELECT item_id, quantity FROM inventory_map WHERE inv_id = %s"
			cursor.execute(query, inv_id)

			result = cursor.fetchone()

			result = (result, ("items":2))

			return list(result)