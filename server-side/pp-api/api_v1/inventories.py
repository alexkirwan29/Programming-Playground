from flask import Flask, request
from flask_restful import Resource, Api

class Inventory(Resource):
	def get(self, inv_id):
		return inv_id