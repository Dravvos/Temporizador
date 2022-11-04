create database Temporizador
go
use Temporizador

create table tarefas
(
id int primary key identity(1,1),
nome varchar(250)
)

create table tbLog
(
id int primary key identity(1,1),
cod_tarefa int foreign key references tarefas,
dia datetime, 
tempo time
)
