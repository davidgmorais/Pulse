--trigger para registar um user
GO
CREATE TRIGGER Pulse.Register ON Pulse.Utilizador
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @Codigo CHAR(8);
	DECLARE @Nome VARCHAR(100); 
	DECLARE @DataNascimento DATE;
	DECLARE @Email VARCHAR(30);
	DECLARE @NIF CHAR(9);
	DECLARE	@PalavraPasse VARCHAR(30);

	SELECT @Codigo= Codigo,  @Nome= Nome, @DataNascimento= DataNascimento, @Email= Email, @NIF= NIF, @PalavraPasse = CONVERT(VARCHAR(30), PalavraPasse)
	FROM inserted;
	 
	IF NOT EXISTS (
		SELECT Codigo, Email 
		FROM Pulse.Utilizador 
		WHERE Codigo = @Codigo and Email = @Email ) 
	BEGIN
		INSERT INTO Pulse.Utilizador(Codigo, Nome, DataNascimento, Email, NIF, PalavraPasse) VALUES (@Codigo, @Nome, @DataNascimento, @Email, @NIF, ENCRYPTBYPASSPHRASE('Cataplana de Bacalhau' ,@PalavraPasse) );
	END ELSE BEGIN
		 RAISERROR ('Utilizador já existe.', 16, 1 );  
	END

END



CREATE TRIGGER Pulse.CriarConsulta ON Pulse.Consulta
INSTEAD OF INSERT
AS
BEGIN
	declare @id char(5);
	declare @hora time;
	declare @date date;
	declare @consultorio int;
	declare @medico char(8);
	declare @paciente char(8);

	declare @ret bit

	select @hora = Hora,  @date = Data, @consultorio = NrConsultorio, @medico = CodigoMedico, @paciente = CodigoPaciente
	from inserted;

	set @ret = (select Pulse.ConsultaDisponivel(@medico, @date, @hora));

	if ( @ret = 1 ) begin
		if not exists (select * from Pulse.Paciente where Paciente.Codigo = @paciente) begin
			INSERT INTO Pulse.Paciente VALUES (@paciente, 'Consulta Marcada', 'Fora do Hospital');
		end

		INSERT INTO Pulse.Consulta(Hora, Data, NrConsultorio, CodigoMedico, CodigoPaciente) VALUES ( @hora, @date, @consultorio, @medico, @paciente);
	End
END



GO
create trigger Pulse.Alta ON Pulse.Atende
INSTEAD OF delete
AS
BEGIN
	declare @CodigoPaciente char(8);
	declare @CodigoMedico char(8);


	select @CodigoPaciente = CodigoPacienteAtende,  @CodigoMedico = CodigoMedicoAtende
	from deleted;

	delete from Pulse.Atende 
	WHERE CodigoPacienteAtende = @CodigoPaciente and CodigoMedicoAtende = @CodigoMedico
	;

	update Pulse.Paciente
	set Estado = 'Alta', Localizacao = 'Fora do Hospital'
	where Codigo = @CodigoPaciente
	;

END

--trigger usado para  inserir os dados completos de todos os utilizadores e, caso os users já existam, atualiza-os
--drop Trigger Pulse.UpAndReg
/*
GO
CREATE TRIGGER Pulse.UpAndReg ON Pulse.Utilizador
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @Codigo CHAR(8);
	DECLARE @Nome VARCHAR(100); 
	DECLARE @DataNascimento DATE;
	DECLARE @Morada VARCHAR(80);
	DECLARE @Email VARCHAR(30);
	DECLARE @Telefone CHAR(9);
	DECLARE @Telemovel CHAR(9);
	DECLARE @NIF CHAR(9);
	DECLARE	@PalavraPasse VARCHAR(30);

	SELECT 
		@Codigo= Codigo,  
		@Nome= Nome, 
		@DataNascimento= DataNascimento, 
		@Morada = Morada, 
		@Email= Email, 
		@Telefone = Telefone,
		@Telemovel = Telemovel,
		@NIF= NIF, 
		@PalavraPasse = CONVERT(VARCHAR(30), PalavraPasse)
	FROM inserted;

	IF NOT EXISTS (
		SELECT Codigo
		FROM Pulse.Utilizador 
		WHERE Codigo = @Codigo)
	BEGIN
		INSERT INTO Pulse.Utilizador(Codigo, Nome, DataNascimento, Morada, Email, Telefone, Telemovel, NIF, PalavraPasse) VALUES (@Codigo, @Nome, @DataNascimento, @Morada, @Email, @Telefone, @Telemovel, @NIF, ENCRYPTBYPASSPHRASE('Cataplana de Bacalhau' ,@PalavraPasse) );
	END ELSE BEGIN
		UPDATE Pulse.Utilizador
		SET
			Nome= @Nome, 
			DataNascimento= @DataNascimento, 
			Morada = @Morada, 
			Email= @Email, 
			Telefone = @Telefone,
			Telemovel = @Telemovel,
			NIF= @NIF,
			PalavraPasse = ENCRYPTBYPASSPHRASE('Cataplana de Bacalhau' ,@PalavraPasse)
		WHERE
			Codigo = @Codigo 			
	END

END
*/