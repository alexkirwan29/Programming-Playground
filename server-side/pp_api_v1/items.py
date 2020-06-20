from flask import Flask, request
from flask_restful import Resource, Api

class Item(Resource):
	def get(self, item_id):
		return {item_id}

class ItemList(Resource):
	def get(self):
		return "Item List"