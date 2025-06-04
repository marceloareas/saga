import os
from flask import Flask, render_template, request, abort, jsonify, send_from_directory
from core.main import main
from core.scriptsdb import create_tables

create_tables()
app = Flask(__name__)

@app.route("/")
def index():
    return render_template('index.html')

@app.route("/createReport", methods=['POST'])
def createReport():
    body = request.get_json()
    if not body.get('beginYear') or not body.get('endYear'):
        abort(400, "Especifique uma data.")
    filename = main(body.get('beginYear'), body.get('endYear'))
    return jsonify({"filename": filename})

@app.route('/download/<path:filename>')
def download_file(filename):
    return send_from_directory('/app', filename, as_attachment=False)

if __name__ == "__main__":
    app.run(debug=True)
