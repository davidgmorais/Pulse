--CREATE SCHEMA Pulse;
CREATE TABLE Pulse.Utilizador (
	Codigo			CHAR(8)			NOT NULL,
	Nome			NVARCHAR(100)	NOT NULL,
	DataNascimento	DATE			NOT NULL,
	Morada			VARCHAR(80),
	Email			VARCHAR(30)		NOT NULL,
	Telefone		CHAR(9),
	Telemovel		CHAR(9),
	NIF				CHAR(9)			NOT NULL,
	PalavraPasse	VARBINARY(128)	NOT NULL,

	PRIMARY KEY(Codigo)
);

CREATE TABLE Pulse.Paciente (
	Codigo			CHAR(8),
	Estado			VARCHAR(60),
	Localizacao		VARCHAR(40)

	PRIMARY KEY(Codigo),
	FOREIGN KEY(Codigo) REFERENCES Pulse.Utilizador(Codigo),

);

CREATE TABLE Pulse.Atende (
	CodigoPacienteAtende	CHAR(8),
	CodigoMedicoAtende		CHAR(8),

	PRIMARY KEY(CodigoPacienteAtende, CodigoMedicoAtende),
	FOREIGN KEY(CodigoPacienteAtende) REFERENCES Pulse.Paciente(Codigo),
	FOREIGN KEY(CodigoMedicoAtende) REFERENCES Pulse.Utilizador(Codigo)
	
);


CREATE TABLE Pulse.Acompanha (
	CodigoPacienteAcompanha		CHAR(8),
	CodigoAcompanhanteAcompanha CHAR(8),	

	PRIMARY KEY(CodigoPacienteAcompanha, CodigoAcompanhanteAcompanha),
	FOREIGN KEY(CodigoPacienteAcompanha) REFERENCES Pulse.Paciente(Codigo),
	FOREIGN KEY(CodigoAcompanhanteAcompanha ) REFERENCES Pulse.Utilizador(Codigo)

); 

CREATE TABLE Pulse.Turno (
	IDTurno			CHAR(5),
	Data			DATE,
	HoraInicio		TIME,
	HoraFim 		TIME,
	Descricao		VARCHAR(30),

	CHECK(HoraFim >= HoraInicio),
	PRIMARY KEY(IDTurno)
); 

CREATE TABLE Pulse.Realiza (
	IDMedico	CHAR(8),
	IDTurno		CHAR(5),

	PRIMARY KEY(IDMedico, IDTurno),
	FOREIGN KEY(IDMedico) REFERENCES Pulse.Utilizador(Codigo),
	FOREIGN KEY(IDTurno) REFERENCES Pulse.Turno(IDTurno)
); 

CREATE TABLE Pulse.Analise (
	ID			CHAR(5),
	Data		Date,
	Descricao	VARCHAR(40),	
	
	PRIMARY KEY(ID)
); 

CREATE TABLE Pulse.Consulta (
	ID				int			identity(10000,1),
	Hora			Time,
	Data			Date,
	NrConsultorio	INT CHECK	(NrConsultorio < 100),
	CodigoMedico	CHAR(8)		NOT NULL,
	CodigoPaciente	CHAR(8)		NOT NULL,

	PRIMARY KEY(ID),
	FOREIGN KEY(CodigoMedico) REFERENCES Pulse.Utilizador(Codigo),
	FOREIGN KEY(CodigoPaciente) REFERENCES Pulse.Paciente(Codigo)

); 

CREATE TABLE Pulse.TemAnalise (
	IDConsulta		INT,
	IDAnalise		CHAR(5),

	PRIMARY KEY(IDConsulta, IDAnalise),
	FOREIGN KEY(IDConsulta) REFERENCES Pulse.Consulta(ID),
	FOREIGN KEY(IDAnalise) REFERENCES Pulse.Analise(ID)
);

CREATE TABLE Pulse.Receita (
	ID		CHAR(5),
	Data	Date,
	
	PRIMARY KEY(ID)
); 

CREATE TABLE Pulse.Medicamento (
	ID			CHAR(6),
	Designacao	VARCHAR(30),
	Dosagem		VARCHAR(20),
	IDReceita	CHAR(5)			NOT NULL,

	PRIMARY KEY(ID),
	FOREIGN KEY(IDReceita) REFERENCES Pulse.Receita(ID)

);

CREATE TABLE Pulse.TemReceita (
	IDConsulta		INT,
	IDReceita		CHAR(5),

	PRIMARY KEY(IDConsulta, IDReceita),
	FOREIGN KEY(IDConsulta) REFERENCES Pulse.Consulta(ID),
	FOREIGN KEY(IDReceita) REFERENCES Pulse.Receita(ID)
); 

CREATE TABLE Pulse.Fatura (
	NrFatura		CHAR(5),
	Preco			DECIMAL(10,2)	NOT NULL,
	Data			Date,
	CodigoPaciente	CHAR(8)			NOT NULL,
	
	PRIMARY KEY(NrFatura),
	FOREIGN KEY(CodigoPaciente) REFERENCES Pulse.Paciente(Codigo)
);