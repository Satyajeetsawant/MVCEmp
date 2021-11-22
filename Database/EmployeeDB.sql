create database EmployeeDB

use EmployeeDB
go

create table Employees
(
	Id int primary key identity(1,1) not null,
	Name nvarchar(50) not null,
	City nvarchar(50) not null,
	Address nvarchar(50) not null, 
)
go
/*  Employee create procedure  */
create proc [dbo].[Employees_Create]
	@Id int,
	@Name nvarchar(50),
	@City nvarchar(50),
	@Address varchar(50) 
as begin
	insert into Employees
	(
		Name,
		City,
		Address
	)
    values
	(
		@Name,
		@City,
		@Address
		 
	)

	select SCOPE_IDENTITY() InsertedId
end
go
/*employee getall procedure */

create proc [dbo].[Employees_GetAll]
	@Id int = null,
	@Search nvarchar(50) = null,
	@OrderBy varchar(100) = 'name',
	@IsDescending bit = 0
as begin
	select 
		Id,
		Name,
		City,
		Address
	from 
		Employees
	where
		Id = coalesce(@Id, Id)
		and
		(
			(@Search is null or @Search = '')
			or
			(
				@Search is not null
				and
				(
					Name like '%' + @Search + '%'
					or
					City like '%' + @Search + '%'
					or
					Address like '%' + @Search + '%'
					
				)
			)
		)
	order by
		case when @OrderBy = 'name' and @IsDescending = 0 then Name end asc
		, case when @OrderBy = 'name' and @IsDescending = 1 then Name end desc
		, case when @OrderBy = 'city' and @IsDescending = 0 then city end asc
		, case when @OrderBy = 'city' and @IsDescending = 1 then city end desc
		, case when @OrderBy = 'address' and @IsDescending = 0 then address end asc
		, case when @OrderBy = 'address' and @IsDescending = 1 then address end desc
		
end
go
/* insert record */

SET IDENTITY_INSERT [dbo].[Employees] ON 
GO
INSERT [dbo].[Employees] ([Id], [Name], [City], [Address]) VALUES (1, N'Rahul', N'Thane', N'Yashodhan nagar')
GO
INSERT [dbo].[Employees] ([Id], [Name], [City], [Address]) VALUES (2, N'Sonali', N'Mulund', N'Shivai nagar')
GO
INSERT [dbo].[Employees] ([Id], [Name], [City], [Address]) VALUES (3, N'Sanket', N'Nahur', N'Gokul nagar')
GO
INSERT [dbo].[Employees] ([Id], [Name], [City], [Address]) VALUES (4, N'Deepak', N'Thane', N'vasant vihar')
GO
SET IDENTITY_INSERT [dbo].[Employees] OFF
GO
/* get employees */
create proc [dbo].[Employees_Get]
	@Id int
as begin
	select 
		Id,
		Name,
		City,
		Address
		
	from 
		Employees
	where 
		Id = @Id
end
go


/*   */


create proc [dbo].[Employees_Update]
	@Id int,
	@Name nvarchar(50),
	@City nvarchar(50),
	@Address varchar(50)
	
as begin
	update 
		Employees
	set
		Name = @Name,
		City = @City,
		Address = @Address
	where 
		Id=@Id
end
go

/* */

create proc [dbo].[Employees_Delete]
	@Id int
as begin
	delete 
	from 
		Employees
	where 
		Id = @Id
end
go