import os
import logging

import openpyxl
from core.main import main
from core.scriptsdb import create_tables
from flask import Flask, render_template, request, abort, send_file, jsonify
from flask_cors import CORS 

create_tables()
app = Flask(__name__)
CORS(app)

@app.route("/")
def index():
    return render_template('createReport.html') # No need for api_url here

@app.route("/createReport", methods=['POST'])
def createReport():
    body: dict = request.get_json()
    if not body.get('beginYear') or not body.get('endYear'):
        abort(400, "Especifique uma data.")
    filename = main(body.get('beginYear'), body.get('endYear'))
    # Invés de enviar o arquivo direto, vamos retornar um nome do arquivo
    return jsonify({"filename": filename})


if __name__ == "__main__":
    app.run()