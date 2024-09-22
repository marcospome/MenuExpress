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


-------------------- PRC CreateOrder --------------------

CREATE PROCEDURE [dbo].[CreateOrder]
    @ClientName NVARCHAR(100),
    @ClientLastName NVARCHAR(100),
    @ClientDNI NVARCHAR(20),
    @ClientEmail NVARCHAR(255)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insertar en la tabla Order
        INSERT INTO [dbo].[Order] (IdStatus, ClientName, ClientLastName, ClientDNI, ClientEmail, AddDate)
        VALUES (1, @ClientName, @ClientLastName, @ClientDNI, @ClientEmail, GETDATE()); -- Usa GETDATE() para establecer la fecha actual

        -- Obtener el ID de la orden recién insertada
        DECLARE @IdOrder INT = SCOPE_IDENTITY();
        SELECT @IdOrder AS IdOrder; -- Devuelve el IdOrder

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO



--------------------------------------------------------------------------------------------------------------------

-------------------- PRC CreateOrderDetail --------------------

CREATE PROCEDURE [dbo].[CreateOrderDetail]
    @IdOrder INT,
    @Qty INT,
    @Note NVARCHAR(255) NULL,
    @IdProduct INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Insertar en la tabla OrderDetail
        INSERT INTO [dbo].[OrderDetail] (Qty, IdStatus, Note, IdOrder, IdProduct, AddDate)
        VALUES (@Qty, 1, @Note, @IdOrder, @IdProduct, getdate()); -- Incluir AddDate

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

--------------------------------------------------------------------------------------------------------------------

-------------------- PRC DeleteOrderDetail] --------------------

Create PROCEDURE [dbo].[DeleteOrderDetail]
@IdOrder INT,
@IdOderDetail NVARCHAR(100)
AS
BEGIN
-- Actualizar el producto en la tabla Product
UPDATE OrderDetail
SET 
	Deleted = 1
WHERE 
    IdOrderDetail = @IdOderDetail and IdOrder = @IdOrder;
END;
GO

--------------------------------------------------------------------------------------------------------------------

-------------------- PRC UpdateOrderDetail --------------------

CREATE PROCEDURE [dbo].[UpdateOrderDetail]
    @IdOrder INT,
    @IdOderDetail NVARCHAR(100),
    @Note NVARCHAR(500),
	@Qty INT
AS
BEGIN
    -- Actualizar el producto en la tabla Product
    UPDATE OrderDetail
    SET 
        Note = @Note,
        Qty = @qty,
		AddDate = GETDATE(),
		IdStatus=2
    WHERE 
        IdOrderDetail = @IdOderDetail and IdOrder = @IdOrder;
END;



select * from OrderDetail
GO


-------------------- PRC AddItemOrderDetail --------------------

-- Procedimiento almacenado para agregar una nueva línea en OrderDetail con validación
CREATE PROCEDURE [dbo].[AddItemOrderDetail]
    @IdOrder INT,          -- ID de la orden
    @Qty INT,              -- Cantidad del producto
    @Note NVARCHAR(255) = NULL,  -- Nota opcional
    @IdProduct INT        -- ID del producto
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        -- Validar si el estado de la orden en la tabla Order es 1
        IF EXISTS (SELECT 1 FROM [dbo].[Order] WHERE IdOrder = @IdOrder AND IdStatus = 1)
        BEGIN
            -- Si el estado es 1, insertar el nuevo registro en OrderDetail
            INSERT INTO [dbo].[OrderDetail] (Qty, IdStatus, AddDate, Note, IdOrder, IdProduct)
            VALUES (@Qty, 1, GETDATE(), @Note, @IdOrder, @IdProduct);

            COMMIT TRANSACTION;
        END
        ELSE
        BEGIN
            -- Si la orden no tiene el estado 1, lanzar un error
            RAISERROR('La orden debe tener el estado 1(Ingresado) para agregar nuevos items.', 16, 1);
            ROLLBACK TRANSACTION;
        END
    END TRY
    BEGIN CATCH
        -- Si ocurre un error, revertir la transacción
        ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END;
GO