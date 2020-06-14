import markdown

from flask import Flask, request, send_from_directory, render_template
from flask_restful import Resource, Api
import os

app = Flask(__name__, template_folder='../docs/templates')
api = Api(app)


# When the user sends a request to /, return the rendered README file
# @app.route('/')
# def index():

# 	# Open the readme file.
# 	with open(os.path.dirname(app.root_path) + '/docs/README.md', 'r') as markdown_file:
# 		# Generate the html from the contents of the markdown file.
# 		return markdown.markdown(markdown_file.read(), extensions=['toc', 'extra', 'codehilite'])

@app.route('/')
def index():
	return markdownDocs("README")

@app.route('/<path>.md')
def markdownDocs(path):
	with open(os.path.dirname(app.root_path) + '/docs/' + path + '.md', 'r') as markdown_file:
		markdown_parsed = markdown.markdown(markdown_file.read(), extensions=['toc', 'extra', 'codehilite'])
		return render_template("doc-page.html", title="readme.md", markdown = markdown_parsed)

@app.route('/<path>')
def sendDocs(path):
	return send_from_directory(os.path.dirname(app.root_path) + '/docs/', path)
