USE[ReqGraduaciones]
-- Crear antes del trigger para cualquier ambiente
CREATE TABLE AlumnosPeriodoEstimado(
	Id INT IDENTITY(1,1) NOT NULL,
	Matricula VARCHAR(9),
	Estatus BIT,
	Error VARCHAR(MAX),
	FechaRegistro DATETIME,
	Accion VARCHAR(50),
    CONSTRAINT PK_AlumnosPeriodoEstimado PRIMARY KEY (Id)
);
