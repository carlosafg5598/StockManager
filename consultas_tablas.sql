--create database StockManager

--use StockManager

--create table Proveedor(
--IdProveedor int identity(1,1) primary key,
--Nombre NVarchar(100) NOT NULL,
--Telefono NVarchar(50) NULL,
--Email NVarchar(100) NULL,
--Direccion NVarchar(255) NULL
--);

--select * from proveedor

--CREATE TABLE Producto (
--    IdProducto INT IDENTITY(1,1) PRIMARY KEY,
--    Nombre NVARCHAR(100) NOT NULL,
--    Descripcion NVARCHAR(255) NULL,
--    PrecioUnitario DECIMAL(18,2) NOT NULL,
--    IdProveedor INT NULL,
--    StockActual INT NOT NULL DEFAULT 0,
--    Activo BIT NOT NULL DEFAULT 1,
--    CONSTRAINT FK_Producto_Proveedor FOREIGN KEY (IdProveedor) REFERENCES Proveedor(IdProveedor)
--);

--select * from producto

--CREATE TABLE Usuario (
--    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
--    Nombre NVARCHAR(100) NOT NULL,
--    Email NVARCHAR(100) NOT NULL UNIQUE,
--    PasswordHash NVARCHAR(255) NOT NULL,
--    Rol NVARCHAR(50) NOT NULL -- Ej: 'Admin', 'Empleado'
--);

CREATE TABLE MovimientoStock (
    IdMovimiento INT IDENTITY(1,1) PRIMARY KEY,
    IdProducto INT NOT NULL,
    FechaMovimiento DATETIME NOT NULL DEFAULT GETDATE(),
    TipoMovimiento CHAR(1) NOT NULL CHECK (TipoMovimiento IN ('E','S')), -- E = Entrada, S = Salida
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    Descripcion NVARCHAR(255) NULL,
    UsuarioId INT NULL,
    CONSTRAINT FK_Movimiento_Producto FOREIGN KEY (IdProducto) REFERENCES Producto(IdProducto),
    CONSTRAINT FK_Movimiento_Usuario FOREIGN KEY (UsuarioId) REFERENCES Usuario(IdUsuario)
);
