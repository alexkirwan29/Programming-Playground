from api_v1 import app, connection
# from pp_api_v1 import app, foo

# app.register_blueprint(foo.docs)
app.run(host='0.0.0.0', port=80, debug=True)

connection.close()