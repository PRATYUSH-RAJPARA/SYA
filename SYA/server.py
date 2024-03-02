from flask import Flask, jsonify
import sqlite3

app = Flask(__name__)

# API endpoint to get all items from the ITEM_MASTER table
@app.route('/get_items', methods=['GET'])
def get_items():
    connection = sqlite3.connect('path_to_your_database.db')  # Replace with the actual path to your SQLite database
    cursor = connection.cursor()

    cursor.execute('SELECT * FROM ITEM_MASTER')
    items = cursor.fetchall()

    connection.close()

    # Convert the result to a list of dictionaries
    items_list = [{'id': item[0], 'itemName': item[1], 'itemType': item[2], 'prCode': item[3]} for item in items]

    return jsonify(items_list)

if __name__ == '__main__':
    app.run(debug=True)
