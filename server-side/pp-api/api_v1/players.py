from flask import Flask, request
from flask_restful import Resource, Api

from  api_v1 import connection

class Players(Resource):
	# Get a single player with the player_id
	def get(self, player_id):

		# Start a cursor and close it once done.
		with connection.cursor() as cursor:

			# Run the SQl command to get all the details of the player that
			# has the provided player_id
			cursor.execute("SELECT * FROM `players` WHERE player_id = %s", player_id)
			result = cursor.fetchone()
			return result

class PlayerList(Resource):

	# Get all the players.
	def get(self):

		# Start a cursor and close it once done.
		with connection.cursor() as cursor:

			# Run the SQl command to get all the details of all the
			# players from the players table.
			cursor.execute("SELECT * FROM `players`")
			result = cursor.fetchall()

			# Convert the results from tuples to a list to be encoded
			# into json.
			return list(result)