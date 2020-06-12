CREATE UNIQUE INDEX ixCodigo
ON Pulse.Utilizador (Codigo);

create nonclustered index ix_Medico
on Pulse.Consulta (CodigoMedico);

create nonclustered index ix_Paciente
on Pulse.Consulta (CodigoPaciente);