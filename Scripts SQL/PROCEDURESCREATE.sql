USE [MENUEXPRESSDB]
GO

/****** Object:  StoredProcedure [dbo].[AddUser]    Script Date: 12/9/2024 11:31:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AddUser] 
    @Name NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Password NVARCHAR(255),
    @Email NVARCHAR(100),
    @Active INT
AS
BEGIN
    INSERT INTO [User] (Name, LastName, Password, Email, Active, AddDate)
    VALUES (
        @Name, 
        @LastName, 
        ENCRYPTBYPASSPHRASE('sssemgdtvecldhstdp', @Password), 
        @Email, 
        @Active, 
        getdate()
    );
END;
GO


--------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[CreateCategory]    Script Date: 12/9/2024 11:31:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------

-------------------- PRC CreateCategory --------------------

CREATE PROCEDURE [dbo].[CreateCategory]
    @Name NVARCHAR(100),
    @Deleted INT
AS
BEGIN
    -- Insertar una nueva categoría en la tabla Category
    INSERT INTO Category (Name, Deleted)
    VALUES (@Name, @Deleted);
END;
GO

--------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[CreateProduct]    Script Date: 12/9/2024 11:32:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------

-------------------- PRC CreateProduct --------------------


CREATE PROCEDURE [dbo].[CreateProduct]
    @Name NVARCHAR(100),
    @Deleted INT,
    @Description NVARCHAR(500),
    @Price DECIMAL(10, 2),
    @AddDate DATE,
    @Image NVARCHAR(500),
    @IdCategory INT
AS
BEGIN
    -- Insertar el nuevo producto en la tabla Product
    INSERT INTO Product (Name, Deleted, Description, Price, AddDate, Image, IdCategory)
    VALUES (@Name, @Deleted, @Description, @Price, @AddDate, @Image, @IdCategory)
	END;
GO

--------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[DeleteCategory]    Script Date: 12/9/2024 11:32:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------

-------------------- PRC DeleteCategory --------------------

CREATE PROCEDURE [dbo].[DeleteCategory]
    @IdCategory INT
AS
BEGIN
    -- Eliminar la categoría de la tabla Category
    DELETE FROM Category
    WHERE IdCategory = @IdCategory;
END;
GO

--------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[DeleteProduct]    Script Date: 12/9/2024 11:32:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------


-------------------- PRC DeleteProduct --------------------


CREATE PROCEDURE [dbo].[DeleteProduct]
    @IdProduct INT
AS
BEGIN
    -- Eliminar el producto de la tabla Product
    DELETE FROM Product
    WHERE IdProduct = @IdProduct;
END;
GO

--------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[GetCategory]    Script Date: 12/9/2024 11:33:26 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-------------------- PRC GetCategory --------------------

CREATE PROCEDURE [dbo].[GetCategory]
AS
BEGIN
    -- Obtiene todos los datos de la tabla Category
    SELECT * FROM Category;
END;
GO

--------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[GetProduct]    Script Date: 12/9/2024 11:33:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-------------------- PRC GetProduct --------------------

CREATE PROCEDURE [dbo].[GetProduct]
AS
BEGIN
    -- Obtiene todos los datos de la tabla Product
    select * FROM Product
END;
GO

--------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[UpdateCategory]    Script Date: 12/9/2024 11:34:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------

-------------------- PRC UpdateCategory --------------------

CREATE PROCEDURE [dbo].[UpdateCategory]
    @IdCategory INT,
    @Name NVARCHAR(100),
    @Deleted INT
AS
BEGIN
    -- Actualizar la categoría en la tabla Category
    UPDATE Category
    SET 
        Name = @Name,
        Deleted = @Deleted
    WHERE 
        IdCategory = @IdCategory;
END;
GO

--------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[UpdateProduct]    Script Date: 12/9/2024 11:34:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


------------------------------------------------------------

 
-------------------- PRC UpdateProduct --------------------

CREATE PROCEDURE [dbo].[UpdateProduct]
    @IdProduct INT,
    @Name NVARCHAR(100),
    @Deleted INT,
    @Description NVARCHAR(500),
    @Price DECIMAL(10, 2),
    @AddDate DATE,
    @Image NVARCHAR(500),
    @IdCategory INT
AS
BEGIN
    -- Actualizar el producto en la tabla Product
    UPDATE Product
    SET 
        Name = @Name,
        Deleted = @Deleted,
        Description = @Description,
        Price = @Price,
        AddDate = @AddDate,
        Image = @Image,
        IdCategory = @IdCategory
    WHERE 
        IdProduct = @IdProduct;
END;
GO

--------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[UserValidate]    Script Date: 12/9/2024 11:34:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

	create procedure [dbo].[UserValidate]
	@Email NVARCHAR(100),
	@Password NVARCHAR(255)
	as
	begin
	select * from [user] where Email=@Email and convert(NVARCHAR(255),DECRYPTBYPASSPHRASE('sssemgdtvecldhstdp',Password)) = @Password
	end
GO


