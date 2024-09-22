CREATE DATABASE [MENUEXPRESSDB]
GO

USE [MENUEXPRESSDB]
GO


/****** Object:  Table [dbo].[Permission]    Script Date: 12/9/2024 11:25:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Permission](
	[IdPermission] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdPermission] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[Rol]    Script Date: 12/9/2024 11:26:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Rol](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[RolePermission]    Script Date: 12/9/2024 11:26:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RolePermission](
	[IdRolePermission] [int] IDENTITY(1,1) NOT NULL,
	[IdRol] [int] NOT NULL,
	[IdPermission] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdRolePermission] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[RolePermission]  WITH CHECK ADD FOREIGN KEY([IdPermission])
REFERENCES [dbo].[Permission] ([IdPermission])
GO

ALTER TABLE [dbo].[RolePermission]  WITH CHECK ADD FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO


--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/****** Object:  Table [dbo].[User]    Script Date: 12/9/2024 11:27:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[IdUser] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[LastName] [nvarchar](100) NOT NULL,
	[Password] [varbinary](255) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Active] [int] NOT NULL,
	[AddDate] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[UserRole]    Script Date: 12/9/2024 11:27:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserRole](
	[IdUserRole] [int] IDENTITY(1,1) NOT NULL,
	[IdUser] [int] NOT NULL,
	[IdRol] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdUserRole] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD FOREIGN KEY([IdRol])
REFERENCES [dbo].[Rol] ([IdRol])
GO

ALTER TABLE [dbo].[UserRole]  WITH CHECK ADD FOREIGN KEY([IdUser])
REFERENCES [dbo].[User] ([IdUser])
GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


/****** Object:  Table [dbo].[Category]    Script Date: 12/9/2024 11:23:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Category](
	[IdCategory] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Deleted] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdCategory] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[OrderStatus]    Script Date: 12/9/2024 11:25:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OrderStatus](
	[IdStatus] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[IdStatus] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[Product]    Script Date: 12/9/2024 11:25:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Product](
	[IdProduct] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Deleted] [int] NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Price] [decimal](10, 2) NOT NULL,
	[AddDate] [date] NOT NULL,
	[Image] [nvarchar](500) NULL,
	[IdCategory] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[IdProduct] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([IdCategory])
REFERENCES [dbo].[Category] ([IdCategory])
GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  Table [dbo].[Order]    Script Date: 12/9/2024 11:24:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Order](
	[IdOrder] [int] IDENTITY(1,1) NOT NULL,
	[AddDate] [date] NOT NULL,
	[idUser] [int] NULL,
	[IdStatus] [int] NULL,
	[ClientName] [nvarchar](100) NOT NULL,
	[ClientLastName] [nvarchar](100) NOT NULL,
	[ClientDNI] [nvarchar](20) NOT NULL,
	[ClientEmail] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdOrder] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([IdStatus])
REFERENCES [dbo].[OrderStatus] ([IdStatus])
GO

ALTER TABLE [dbo].[Order]  WITH CHECK ADD FOREIGN KEY([idUser])
REFERENCES [dbo].[User] ([IdUser])
GO

--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE [dbo].[OrderDetail](
	[IdOrderDetail] [int] IDENTITY(1,1) NOT NULL,
	[Qty] [int] NOT NULL,
	[IdStatus] [int] NULL,
	[AddDate] [date] NOT NULL,
	[Note] [nvarchar](255) NULL,
	[IdOrder] [int] NULL,
	[IdProduct] [int] NULL,
	[Deleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[IdOrderDetail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[OrderDetail] ADD  DEFAULT ((0)) FOR [Deleted]
GO

ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([IdOrder])
REFERENCES [dbo].[Order] ([IdOrder])
GO

ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([IdProduct])
REFERENCES [dbo].[Product] ([IdProduct])
GO

ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD FOREIGN KEY([IdStatus])
REFERENCES [dbo].[OrderStatus] ([IdStatus])
GO


--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


------------------------- INSERTS DE CATEGORÍAS Y PRODUCTOS -------------------------

INSERT INTO Category (Name, Deleted) 
VALUES 
('Ensaladas', 0),
('Sopas', 0),
('Pizzas', 0),
('Snacks', 0),
('Cafetería', 0);

INSERT INTO Product (Name, Deleted, Description, Price, AddDate, Image, IdCategory) 
VALUES 
('Ensalada César', 0, 'Ensalada con lechuga, crutones, queso parmesano y aderezo César', 6000, GETDATE(), 'https://www.cocinacaserayfacil.net/wp-content/uploads/2018/06/Ensalada-cesar.jpg', 1),
('Sopa de Pollo', 0, 'Sopa de pollo con verduras', 3500, GETDATE(), 'https://especiasmontero.com/wp-content/uploads/2018/02/CaldoDePollo-1.jpg', 2),
('Pizza Margherita', 0, 'Pizza clásica con tomate, mozzarella y albahaca', 12000, GETDATE(), 'https://imag.bonviveur.com/pizza-margarita.jpg', 3),
('Papas Fritas', 0, 'Papas fritas crujientes con sal', 4500, GETDATE(), 'https://phantom-marca.unidadeditorial.es/813d16708dc72860fd3cf319c9a245b5/resize/828/f/jpg/assets/multimedia/imagenes/2023/08/04/16911461030527.jpg', 4),
('Café Americano', 0, 'Café negro clásico', 1500, GETDATE(), 'https://www.somoselcafe.com.ar/img/novedades/47.jpg', 5),
('Té Verde', 0, 'Té verde con un toque de menta', 1200, GETDATE(), 'https://image.tuasaude.com/media/article/yp/dt/beneficios-del-te-verde_17350_l.jpg', 5),
('Pizza Pepperoni', 0, 'Pizza con pepperoni y queso mozzarella', 15000, GETDATE(), 'https://www.sortirambnens.com/wp-content/uploads/2019/02/pizza-de-peperoni.jpg', 3),
('Galletas de Chocolate', 0, 'Galletas con chispas de chocolate', 1000, GETDATE(), 'https://s1.elespanol.com/2023/04/09/cocinillas/recetas/postres/754934523_232288662_1706x960.jpg', 4);


