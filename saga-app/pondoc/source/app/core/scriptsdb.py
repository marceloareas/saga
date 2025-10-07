import json
import os
from .database import db
import csv
from unidecode import unidecode

def create_tables():
    tables = ['researchers', 'students', 'qualis']

    # Drop tables if they exist
    for table in tables:
        db.create_drop_db(f'DROP TABLE IF EXISTS {table}')

    # Create tables
    db.create_drop_db('''CREATE TABLE researchers(
        nome        VARCHAR(255),
        referencia  VARCHAR(50),
        PRIMARY KEY (nome, referencia)
    )''')

    db.create_drop_db('''CREATE TABLE students(
        nome        VARCHAR(255),
        referencia  VARCHAR(50),
        PRIMARY KEY (nome, referencia)
    )''')

    db.create_drop_db('''CREATE TABLE qualis(
        issn    VARCHAR(9),
        nome    VARCHAR(255),
        qualis  VARCHAR(2),
        PRIMARY KEY (issn, nome)
    )''')

    basePath = os.path.dirname(os.path.abspath(__file__))

    # Insert researchers (idempotent)
    with open(f'{basePath}/PPCICresearchers.json', encoding='utf-8') as arq:
        researchers = json.load(arq)
        for researcher, citations in researchers.items():
            rname = unidecode(researcher).upper().replace("'", "''")
            for citation in citations:
                ref = unidecode(citation).upper().replace("'", "''")
                db.insert_delete_db(
                    f"""INSERT INTO researchers (nome, referencia)
                        VALUES ('{rname}', '{ref}')
                        ON CONFLICT (nome, referencia) DO NOTHING"""
                )

    # Insert students (idempotent)
    with open(f'{basePath}/discentes.csv', encoding='utf-8') as arq:
        reader = csv.reader(arq)
        for line in reader:
            # Skip header-ish rows
            if not line or 'nome' in [c.lower() for c in line if isinstance(c, str)]:
                continue
            n = unidecode(line[0]).upper().replace("'", "''")
            r1 = unidecode(line[1]).upper().replace("'", "''")
            r2 = unidecode(line[2]).upper().replace("'", "''") if len(line) > 2 else None

            db.insert_delete_db(
                f"""INSERT INTO students (nome, referencia)
                    VALUES ('{n}', '{r1}')
                    ON CONFLICT (nome, referencia) DO NOTHING"""
            )
            if r2:
                db.insert_delete_db(
                    f"""INSERT INTO students (nome, referencia)
                        VALUES ('{n}', '{r2}')
                        ON CONFLICT (nome, referencia) DO NOTHING"""
                )

    # Insert qualis in batches (idempotent)
    with open(f'{basePath}/qualis.csv', encoding='utf-8') as arq:
        reader = csv.reader(arq)
        LIMIT = 1000
        rows = []
        for line in reader:
            if not line or 'ISSN' in line[0]:
                continue
            issn = unidecode(line[0]).upper()
            # Skip invalid ISSN
            if len(issn) > 8:
                continue
            nome = unidecode(line[1]).replace("'", "")
            qual = unidecode(line[2])

            rows.append(f"('{issn}', '{nome}', '{qual}')")

            if len(rows) == LIMIT:
                values = ",".join(rows)
                db.insert_delete_db(
                    f"""INSERT INTO qualis (issn, nome, qualis)
                        VALUES {values}
                        ON CONFLICT (issn, nome) DO NOTHING"""
                )
                rows = []

        if rows:
            values = ",".join(rows)
            db.insert_delete_db(
                f"""INSERT INTO qualis (issn, nome, qualis)
                    VALUES {values}
                    ON CONFLICT (issn, nome) DO NOTHING"""
            )
