from flask import Flask, request
from flask_restful import Resource, Api

from .docs import docs

# Create our flask App and Api.
app = Flask(__name__, template_folder='../docs/templates')
app.register_blueprint(docs)
api = Api(app)
