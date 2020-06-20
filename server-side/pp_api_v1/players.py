from flask import Flask, request
from flask_restful import Resource, Api

class Player(Resource):
	def get(self, player_id):
		return {player_id: 2}

class PlayerList(Resource):
	def get(self):
		return "Player List"