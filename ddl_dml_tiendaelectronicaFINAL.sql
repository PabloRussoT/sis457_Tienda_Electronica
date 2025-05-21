﻿-- Crear la base de datos y el usuario
CREATE DATABASE TiendaElectrica;
GO
USE [master]
GO
CREATE LOGIN [usrelectronica] WITH PASSWORD = N'123456',
    DEFAULT_DATABASE = [TiendaElectrica],
    CHECK_EXPIRATION = OFF,
    CHECK_POLICY = ON;
GO
USE [TiendaElectrica]
GO
CREATE USER [usrelectronica] FOR LOGIN [usrelectronica];
GO
ALTER ROLE [db_owner] ADD MEMBER [usrelectronica];
GO

-- Crear las tablas
DROP TABLE IF EXISTS CompraDetalle;
DROP TABLE IF EXISTS Compra;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS Empleado;
DROP TABLE IF EXISTS Proveedor;
DROP TABLE IF EXISTS Producto;
DROP TABLE IF EXISTS Categoria;
DROP TABLE IF EXISTS Marca;

CREATE TABLE Categoria (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(250) NULL,
    estado SMALLINT NOT NULL DEFAULT 1 -- -1: Eliminado, 0: Inactivo, 1: Activo
);

CREATE TABLE Marca (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    nombre VARCHAR(100) NOT NULL,
    descripcion VARCHAR(250) NULL,
    estado SMALLINT NOT NULL DEFAULT 1 -- -1: Eliminado, 0: Inactivo, 1: Activo
);

CREATE TABLE Producto (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idCategoria INT NOT NULL,
    idMarca INT NOT NULL,
    codigo VARCHAR(20) NOT NULL,
    descripcion VARCHAR(250) NOT NULL,
    unidadMedida VARCHAR(20) NOT NULL,
    saldo DECIMAL NOT NULL DEFAULT 0,
    precioVenta DECIMAL NOT NULL CHECK (precioVenta > 0),
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1, -- -1: Eliminado, 0: Inactivo, 1: Activo
    CONSTRAINT fk_Producto_Categoria FOREIGN KEY(idCategoria) REFERENCES Categoria(id),
    CONSTRAINT fk_Producto_Marca FOREIGN KEY(idMarca) REFERENCES Marca(id)
);

CREATE TABLE Proveedor (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    nit BIGINT NOT NULL,
    razonSocial VARCHAR(100) NOT NULL,
    direccion VARCHAR(250) NULL,
    telefono VARCHAR(30) NOT NULL,
    representante VARCHAR(100) NOT NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1 -- -1: Eliminado, 0: Inactivo, 1: Activo
);

CREATE TABLE Empleado (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    cedulaIdentidad VARCHAR(12) NOT NULL,
    nombres VARCHAR(30) NOT NULL,
    primerApellido VARCHAR(30) NULL,
    segundoApellido VARCHAR(30) NULL,
    direccion VARCHAR(250) NOT NULL,
    celular BIGINT NOT NULL,
    cargo VARCHAR(50) NOT NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1 -- -1: Eliminado, 0: Inactivo, 1: Activo
);

CREATE TABLE Usuario (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idEmpleado INT NOT NULL,
    usuario VARCHAR(20) NOT NULL,
    clave VARCHAR(250) NOT NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1, -- -1: Eliminado, 0: Inactivo, 1: Activo
    CONSTRAINT fk_Usuario_Empleado FOREIGN KEY(idEmpleado) REFERENCES Empleado(id)
);

CREATE TABLE Compra (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idProveedor INT NOT NULL,
    transaccion INT NOT NULL,
    fecha DATE NOT NULL DEFAULT GETDATE(),
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1, -- -1: Eliminado, 0: Inactivo, 1: Activo
    CONSTRAINT fk_Compra_Proveedor FOREIGN KEY(idProveedor) REFERENCES Proveedor(id)
);

CREATE TABLE CompraDetalle (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idCompra INT NOT NULL,
    idProducto INT NOT NULL,
    cantidad DECIMAL NOT NULL CHECK (cantidad > 0),
    precioUnitario DECIMAL NOT NULL,
    total DECIMAL NOT NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1, -- -1: Eliminado, 0: Inactivo, 1: Activo
    CONSTRAINT fk_CompraDetalle_Compra FOREIGN KEY(idCompra) REFERENCES Compra(id),
    CONSTRAINT fk_CompraDetalle_Producto FOREIGN KEY(idProducto) REFERENCES Producto(id)
);

-- Procedimientos almacenados para listar
GO
CREATE PROC paProductoListar @parametro VARCHAR(100)
AS
  SELECT * FROM Producto
  WHERE estado<>-1 AND codigo+descripcion+unidadMedida LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY estado DESC, descripcion ASC;
GO
CREATE PROC paEmpleadoListar @parametro VARCHAR(100)
AS
  SELECT ISNULL(u.usuario,'--') AS usuario,e.* 
  FROM Empleado e
  LEFT JOIN Usuario u ON e.id = u.idEmpleado
  WHERE e.estado<>-1 
	AND e.cedulaIdentidad+e.nombres+ISNULL(e.primerApellido,'')+ISNULL(e.segundoApellido,'') LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY e.estado DESC, e.nombres ASC, e.primerApellido ASC;



CREATE OR ALTER PROC paCategoriaListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT * FROM Categoria
    WHERE estado <> -1 AND (nombre + descripcion LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY estado DESC, nombre ASC;
END;

CREATE OR ALTER PROC paMarcaListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT * FROM Marca
    WHERE estado <> -1 AND (nombre + descripcion LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY estado DESC, nombre ASC;
END;

CREATE OR ALTER PROC paProveedorListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT * FROM Proveedor
    WHERE estado <> -1 AND (CAST(nit AS VARCHAR) + razonSocial + ISNULL(direccion, '') + telefono + representante LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY estado DESC, razonSocial ASC;
END;

CREATE OR ALTER PROC paCompraListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT c.*, p.razonSocial AS proveedor
    FROM Compra c
    INNER JOIN Proveedor p ON c.idProveedor = p.id
    WHERE c.estado <> -1 AND (CAST(c.transaccion AS VARCHAR) + p.razonSocial LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY c.estado DESC, c.fecha DESC;
END;

CREATE OR ALTER PROC paCompraDetalleListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT cd.*, pr.descripcion AS producto
    FROM CompraDetalle cd
    INNER JOIN Producto pr ON cd.idProducto = pr.id
    WHERE cd.estado <> -1 AND (pr.codigo + pr.descripcion LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY cd.estado DESC, pr.descripcion ASC;
END;
-- Insertar datos iniciales
INSERT INTO Categoria (nombre, descripcion) VALUES ('Herramientas', 'Herramientas manuales y eléctricas');
INSERT INTO Categoria (nombre, descripcion) VALUES ('Iluminación', 'Bombillas, lámparas y accesorios de iluminación');

INSERT INTO Marca (nombre, descripcion) VALUES ('Bosch', 'Herramientas eléctricas de alta calidad');
INSERT INTO Marca (nombre, descripcion) VALUES ('Philips', 'Productos de iluminación y tecnología avanzada');

INSERT INTO Producto (idCategoria, idMarca, codigo, descripcion, unidadMedida, saldo, precioVenta)
VALUES (1, 1, 'H001', 'Taladro Bosch 500W', 'Unidad', 10, 150);

INSERT INTO Producto (idCategoria, idMarca, codigo, descripcion, unidadMedida, saldo, precioVenta)
VALUES (2, 2, 'L001', 'Bombilla LED Philips 10W', 'Caja', 50, 12);

INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, segundoApellido, direccion, celular, cargo)
VALUES ('8765432', 'Maria', 'Gomez', 'Fernandez', 'Av. Las Americas 123', 76543210, 'Vendedora');

INSERT INTO Usuario (idEmpleado, usuario, clave)
VALUES (1, 'jperez', 'i0hcoO/nssY6WOs9pOp5Xw==' );


SELECT * FROM Producto;
SELECT * FROM Usuario;
SELECT * FROM MARCA;