BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Candidatos" (
	"candidatoID"	INTEGER,
	"cedula"	TEXT NOT NULL UNIQUE,
	"nombre"	TEXT NOT NULL,
	"apellido"	TEXT NOT NULL,
	"fechaNacimiento"	TEXT NOT NULL,
	"trabajoActual"	TEXT,
	"expectativaSalarial"	INT,
	"observaciones"	TEXT,
	PRIMARY KEY("candidatoID" AUTOINCREMENT)
);
COMMIT;
