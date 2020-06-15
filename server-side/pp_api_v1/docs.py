import markdown

from flask import Flask, request, send_from_directory, render_template, Blueprint
import os

docs = Blueprint('docs', __name__)

# When the user sends a request to /, return the rendered README file.
@docs.route('/')
def index():
	return markdownDocs("README")

# when the user is requesting a md file at the root of the server, return the rendered markdown file.
@docs.route('/<path>.md')
def markdownDocs(path):
	with open(os.path.dirname(docs.root_path) + '/docs/' + path + '.md', 'r') as markdown_file:
		markdown_parsed = markdown.markdown(markdown_file.read(), extensions=['toc', 'extra', 'codehilite'])
		return render_template("doc-page.html", title="readme.md", markdown = markdown_parsed)

# For the few images in the markdown, return anything the user requests at the root from the docs folder.
@docs.route('/<path>')
def sendDocs(path):
	return send_from_directory(os.path.dirname(docs.root_path) + '/docs/', path)