Create database prueba_react
go

use prueba_react
go

create table usuario
(
	id int identity(1,1) primary key,
	[user] varchar(50),
	nombres varchar(50),
	apellidos varchar(50),
	password varchar(50)
)
go

create procedure consultar_usuarios (@search varchar(30))
as
begin
	select * from usuario
	where nombres like '%'+@search+'%'
	and apellidos like '%'+@search+'%';
end
go

create procedure validar_usuarios (@user varchar(30), @password varchar(30))
as
begin
	select * from usuario
	where [user] = @user
	and [password] = @password;
end
go

create procedure procesar_usuario_add (
	@Nombres varchar(50),
	@Apellidos varchar(50),
	@User varchar(50),
	@Password varchar(50),
	@Success int output
)
as
BEGIN TRY
	begin
		insert into usuario
		select @User,@Nombres,@Apellidos,@Password;
		set @Success = 1
	end
end try
BEGIN catch
	set @Success = 0
end catch

create procedure procesar_usuario_edit (
	@Id int,
	@Nombres varchar(50),
	@Apellidos varchar(50),
	@User varchar(50),
	@Password varchar(50),
	@Success int output
)
as
BEGIN TRY
	begin
		update usuario
		set [user] = @User,
		password = @Password,
		apellidos = @Apellidos,
		nombres = @Nombres
		where id = @Id;

		set @Success = 1
	end
end try
BEGIN catch
	set @Success = 0
end catch

create procedure procesar_usuario_delete (
	@Id int,
	@Success int output
)
as
BEGIN TRY
	begin
		delete from usuario
		where id = @Id;

		set @Success = 1
	end
end try
BEGIN catch
	set @Success = 0
end catch