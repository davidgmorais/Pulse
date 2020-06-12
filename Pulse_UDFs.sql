--udf para ver se a consulta pode ser marcada
GO
CREATE FUNCTION Pulse.ConsultaDisponivel(@CodigoMedico VARCHAR(8), @DataConsulta Date, @HoraConsulta Time)
returns bit
as
begin
	declare @ret bit;
	declare @hora time;
	set @ret = 0;


	select @hora = Hora
	from Pulse.Consulta join Pulse.Utilizador on (Consulta.CodigoMedico = Utilizador.Codigo)
	where Codigo = @CodigoMedico and Data = @DataConsulta

	if (@hora is null)
		return 1;
	if (@HoraConsulta >= DATEADD(MINUTE, 30, @hora) OR  DATEADD(MINUTE, 30, @HoraConsulta) <= @hora)
		SET @ret = 1;
	return @ret;
end





--udf para retornar as receitas de um Paciente
create function Pulse.LoadReceitas( @Codigo CHAR(8) )
returns @receitas table (
	ID CHAR(5) NOT NULL PRIMARY KEY,
	Data Date not null,
	Codigo char(8) not null,
	Nome Varchar(30) not null
)
as
begin

	insert into @receitas
	select 
		Receita.ID, 
		Receita.Data, 
		Codigo, Nome
	from 
		Pulse.Receita join Pulse.TemReceita on (ID = IDReceita) join Pulse.Consulta on (TemReceita.IDConsulta = Consulta.ID) join Pulse.Utilizador on (CodigoMedico = Codigo)
	where 
		CodigoPaciente = @Codigo
	;
return
end


--udf para carregar as Analises de um Paciente
create function Pulse.LoadAnalises(@Codigo Char(8)) 
returns @analises table (
	ID char(5) not null primary key,
	Data date not null,
	Codigo char(8) not null,
	Nome varchar(40) not null,
	Descricao varchar(40) not null
)
as
begin
	insert @analises
	select 
		Analise.ID, 
		Analise.Data, 
		Codigo, 
		Nome, 
		Descricao
	 from 
		Pulse.Analise join Pulse.TemAnalise on (Analise.ID = IDAnalise) join Pulse.Consulta on (TemAnalise.IDConsulta = Consulta.ID) join Pulse.Utilizador on (CodigoMedico = Codigo)
	 where 
		CodigoPaciente = @Codigo
	;
return
end


--udf para obter pacientes associados com um dotor
create function Pulse.getPacientes(@CodigoMedico Char(8), @CodigoPaciente Char(8)) 
returns @pacientes table (
	Nome VARCHAR(100) not null,
	Estado VARCHAR(60) not null,
	Localizacao varchar(40) not null
)
as
begin
	INSERT @pacientes
	SELECT 
		Nome,
		Estado, 
		Localizacao 
	FROM 
		Pulse.Atende JOIN Pulse.Paciente ON (CodigoPacienteAtende = Codigo) JOIN Pulse.Utilizador ON (Paciente.Codigo = Utilizador.Codigo)
	WHERE 
		CodigoMedicoAtende = @CodigoMedico AND CodigoPacienteAtende = @CodigoPaciente and Estado IS NOT NULL AND Localizacao IS NOT NULL
	;
return
end


--udf para obeter a consulta mais proxima de um médico
CREATE FUNCTION Pulse.getNearestAppointment(@Codigo CHAR(8) )
RETURNS DATE
AS
BEGIN

	DECLARE @dataConsulta DATE;
	
	SELECT 
		@dataConsulta = min(Data)
	FROM
		Pulse.Utilizador join Pulse.Consulta on (Utilizador.Codigo = Consulta.CodigoMedico)
	WHERE
		Data > GETDATE() and CodigoMedico = @Codigo
	;
	
	RETURN @dataConsulta
END


--udf para obter a data do proximo turno de um médico
CREATE FUNCTION Pulse.getNearestShift(@Codigo CHAR(8) )
RETURNS @proximoTurno TABLE (
	dataTurno Date,
	hora Time
)
AS
BEGIN

	INSERT INTO
		@proximoTurno
	SELECT 
		min(Data) as dataTurno,
		min(HoraInicio) as hora
	FROM
		Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) join Pulse.Utilizador on (Realiza.IDMedico = Utilizador.Codigo)
	WHERE
		Data > GETDATE() and HoraInicio > CONVERT(TIME, GETDATE())  and IDMedico = @Codigo
	;
	
	RETURN
END


--udf para obter a data da proxima consulta de um paciente
CREATE FUNCTION Pulse.getNearestPacientAppointment( @Codigo CHAR(8) )
RETURNS DATE
AS
BEGIN

	DECLARE @dataConsulta DATE;
	
	SELECT 
		@dataConsulta = min(Data)
	FROM
		Pulse.Consulta 
	WHERE
		Data > GETDATE() and CodigoPaciente = @Codigo
	;
	
	RETURN @dataConsulta
END


--udf para obter a lista de turnos de um médico
CREATE FUNCTION Pulse.getShifts(@Codigo CHAR(8) )
RETURNS @turnos TABLE (
	Data Date,
	HoraInicio Time,
	HoraFim Time,
	Descricao VARCHAR(30)
)
AS
BEGIN

	INSERT INTO
		@turnos
	SELECT 
		Data, 
		HoraInicio, 
		HoraFim, 
		Descricao 
	FROM
		Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) 
	WHERE
		IDMedico = @Codigo and Data > GETDATE()
	;
	
	RETURN
END


--udf para obter a lista de consultas de um médico numa data especifica
CREATE FUNCTION Pulse.getConsultas(@Codigo CHAR(8), @DataConsulta DATE )
RETURNS @consultas TABLE (
	Data Date,
	Hora Time,
	NrConsultorio INT,
	Nome VARCHAR(100)
)
AS
BEGIN

	INSERT INTO
		@consultas
	SELECT 
		Data, 
		Hora, 
		NrConsultorio, 
		Nome
	FROM
		Pulse.Consulta join Pulse.Utilizador on (CodigoPaciente = Codigo)	
	WHERE
		CodigoMedico = @Codigo and Data = @DataConsulta
	;
	
	RETURN
END


--udf para obter a lista de consultas que um paciente numa data especifica
CREATE FUNCTION Pulse.getPacienteConsultas(@Codigo CHAR(8), @DataConsulta DATE )
RETURNS @consultas TABLE (
	Data Date,
	Hora Time,
	NrConsultorio INT,
	Nome VARCHAR(100)
)
AS
BEGIN

	INSERT INTO
		@consultas
	SELECT 
		Data, 
		Hora, 
		NrConsultorio, 
		Nome
	FROM
		Pulse.Consulta join Pulse.Utilizador on (CodigoMedico= Codigo)	
	WHERE
		CodigoPaciente = @Codigo and Data = @DataConsulta
	;
	
	RETURN
END


--udf para obter a lista de médicos a dar consultas num dia especifico
CREATE FUNCTION Pulse.loadDoctorsList( @DataConsulta DATE )
RETURNS @medicos TABLE (
	Codigo CHAR(8),
	Nome VARCHAR(100)
)
AS
BEGIN

	INSERT INTO
		@medicos
	SELECT 
		Codigo, 
		Nome
	FROM
		Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) join Pulse.Utilizador on (Realiza.IDMedico = Codigo) 	
	WHERE
		Data = @DataConsulta and Descricao = 'Consulta'
	;
	
	RETURN
END


--udf para obter a lista de horas que um médico tem disponiveis para consulta num certo dia
CREATE FUNCTION Pulse.loadListHoras(@DataConsulta DATE, @Nome VARCHAR(100))
RETURNS @horas TABLE (
	HoraInicio TIME,
	HoraFim TIME
)
AS
BEGIN

	INSERT INTO
		@horas
	SELECT 
		HoraInicio,
		HoraFim
	FROM
		Pulse.Turno join Pulse.Realiza on (Turno.IDTurno = Realiza.IDTurno) join Pulse.Utilizador on (Realiza.IDMedico = Codigo)
	WHERE
		Data = @DataConsulta and Nome = @Nome
	;
	
	RETURN
END


--udf para obter uma lista de pacientes que um utilizador acompanha
CREATE FUNCTION Pulse.getAcompanhantes(@Codigo CHAR(8) )
RETURNS @acompanhantes TABLE (
	Codigo CHAR(8),
	Nome VARCHAR(100),
	Estado VARCHAR(60),
	Localizacao VARCHAR(40)
)
AS
BEGIN

	INSERT INTO
		@acompanhantes
	SELECT 
		Pulse.Paciente.Codigo,
		Nome, 
		Estado,
		Localizacao
	FROM
		Pulse.Utilizador join Pulse.Paciente on (Utilizador.Codigo = Paciente.Codigo) join Pulse.Acompanha on (Paciente.Codigo = CodigoPacienteAcompanha)
	WHERE
		Pulse.Acompanha.CodigoAcompanhanteAcompanha = @Codigo;
	
	RETURN
END


CREATE FUNCTION Pulse.verifyAcompanhantes(@Codigo CHAR(8) )
RETURNS @acompanhante TABLE (
	Codigo CHAR(8),
	Nome VARCHAR(100),
	Estado VARCHAR(60),
	Localizacao VARCHAR(40)
)
AS
BEGIN

	INSERT INTO
		@acompanhante
	SELECT 
		Paciente.Codigo,
		Nome, 
		Estado,
		Localizacao
	FROM
		Pulse.Utilizador join Pulse.Paciente on (Utilizador.Codigo = Paciente.Codigo) 
	WHERE
		Paciente.Codigo = @Codigo
	;
	
	RETURN
END


--udf para fazer verificar se um user é válido e, se for, obter os dados do mesmo
create function Pulse.login(@Email VARCHAR(30), @Password VARCHAR(30))
RETURNS @user TABLE (
	Codigo			CHAR(8),
	Nome			NVARCHAR(100),
	DataNascimento	DATE,
	Morada		VARCHAR(80),
	Email			VARCHAR(30),
	Telefone		CHAR(9),
	Telemovel		CHAR(9),
	NIF			CHAR(9)
)
AS
BEGIN

	DECLARE @pwd VARCHAR(30);
	DECLARE @PassPhrase VARCHAR(30);
	SET @PassPhrase = 'Cataplana de Bacalhau';

	SELECT @pwd = CONVERT(VARCHAR(30), DecryptByPassPhrase(@PassPhrase, PalavraPasse)) FROM Pulse.Utilizador WHERE Email = @Email;
	
	IF (@pwd = @Password)				
	BEGIN
		INSERT INTO 
			@user
		SELECT 
			Codigo, Nome, DataNascimento, Morada, Email, Telefone, Telemovel, NIF
		FROM
			Pulse.Utilizador where Email = @Email

	END
	
	RETURN
END