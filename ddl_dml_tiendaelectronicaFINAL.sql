-- Use the master database to manage other databases
USE [master];
GO

-- Check if the database exists and drop it if it does
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'TiendaElectronica')
BEGIN
    ALTER DATABASE TiendaElectronica SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE TiendaElectronica;
END
GO

-- Create the TiendaElectronica database
CREATE DATABASE TiendaElectronica;
GO

-- Create a login for the user
CREATE LOGIN [usrelectronica] WITH PASSWORD = N'123456',
    DEFAULT_DATABASE = [TiendaElectronica],
    CHECK_EXPIRATION = OFF,
    CHECK_POLICY = ON;
GO

-- Switch to the new database
USE [TiendaElectronica];
GO

-- Create a user for the login within this database
CREATE USER [usrelectronica] FOR LOGIN [usrelectronica];
GO

-- Grant db_owner role to the user
ALTER ROLE [db_owner] ADD MEMBER [usrelectronica];
GO

-- --- Table Dropping (Order matters due to foreign keys) ---
DROP TABLE IF EXISTS VentaDetalle;
DROP TABLE IF EXISTS Venta;
DROP TABLE IF EXISTS CompraDetalle;
DROP TABLE IF EXISTS Compra;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS Cliente; -- Drop Cliente before Empleado if Venta is gone
DROP TABLE IF EXISTS Empleado;
DROP TABLE IF EXISTS Proveedor;
DROP TABLE IF EXISTS Producto;
DROP TABLE IF EXISTS Categoria;
DROP TABLE IF EXISTS Marca;
GO

-- --- Table Creation ---

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

CREATE TABLE Cliente (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    nit BIGINT NULL,
    nombres VARCHAR(50) NOT NULL, -- Added directly to CREATE TABLE
    apellidos VARCHAR(50) NULL,   -- Added directly to CREATE TABLE
    nombreCompleto VARCHAR(150) NOT NULL,
    direccion VARCHAR(250) NULL,
    telefono VARCHAR(15) NULL,
    email VARCHAR(100) NULL,
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1 -- -1: Eliminado, 0: Inactivo, 1: Activo
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

CREATE TABLE Venta (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idEmpleado INT NOT NULL,
    idCliente INT NULL, -- Added here as NULL initially
    fecha DATE NOT NULL DEFAULT GETDATE(),
    total DECIMAL NOT NULL CHECK (total >= 0),
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1, -- -1: Eliminado, 0: Inactivo, 1: Activo
    CONSTRAINT fk_Venta_Empleado FOREIGN KEY(idEmpleado) REFERENCES Empleado(id)
);

-- Add foreign key for idCliente after Cliente table is created
ALTER TABLE Venta ADD CONSTRAINT fk_Venta_Cliente FOREIGN KEY (idCliente) REFERENCES Cliente(id);


CREATE TABLE VentaDetalle (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idVenta INT NOT NULL,
    idProducto INT NOT NULL,
    cantidad DECIMAL NOT NULL CHECK (cantidad > 0),
    precioUnitario DECIMAL NOT NULL CHECK (precioUnitario >= 0),
    total DECIMAL NOT NULL CHECK (total >= 0),
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1, -- -1: Eliminado, 0: Inactivo, 1: Activo
    CONSTRAINT fk_VentaDetalle_Venta FOREIGN KEY(idVenta) REFERENCES Venta(id),
    CONSTRAINT fk_VentaDetalle_Producto FOREIGN KEY(idProducto) REFERENCES Producto(id)
);
GO

-- --- Stored Procedures ---

CREATE OR ALTER PROC paProductoListar @parametro VARCHAR(100)
AS
    SELECT * FROM Producto
    WHERE estado<>-1 AND codigo+descripcion+unidadMedida LIKE '%'+REPLACE(@parametro,' ','%')+'%'
    ORDER BY estado DESC, descripcion ASC;
GO

CREATE OR ALTER PROC paEmpleadoListar @parametro VARCHAR(100)
AS
    SELECT ISNULL(u.usuario,'--') AS usuario,e.*
    FROM Empleado e
    LEFT JOIN Usuario u ON e.id = u.idEmpleado
    WHERE e.estado<>-1
        AND e.cedulaIdentidad+e.nombres+ISNULL(e.primerApellido,'')+ISNULL(e.segundoApellido,'') LIKE '%'+REPLACE(@parametro,' ','%')+'%'
    ORDER BY e.estado DESC, e.nombres ASC, e.primerApellido ASC;
GO

CREATE OR ALTER PROC paCategoriaListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT * FROM Categoria
    WHERE estado <> -1 AND (nombre + descripcion LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY estado DESC, nombre ASC;
END;
GO

CREATE OR ALTER PROC paMarcaListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT * FROM Marca
    WHERE estado <> -1 AND (nombre + descripcion LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY estado DESC, nombre ASC;
END;
GO

CREATE OR ALTER PROC paProveedorListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT * FROM Proveedor
    WHERE estado <> -1 AND (CAST(nit AS VARCHAR) + razonSocial + ISNULL(direccion, '') + telefono + representante LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY estado DESC, razonSocial ASC;
END;
GO

CREATE OR ALTER PROC paCompraListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT c.*, p.razonSocial AS proveedor
    FROM Compra c
    INNER JOIN Proveedor p ON c.idProveedor = p.id
    WHERE c.estado <> -1 AND (CAST(c.transaccion AS VARCHAR) + p.razonSocial LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY c.estado DESC, c.fecha DESC;
END;
GO

CREATE OR ALTER PROC paCompraDetalleListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT cd.*, pr.descripcion AS producto
    FROM CompraDetalle cd
    INNER JOIN Producto pr ON cd.idProducto = pr.id
    WHERE cd.estado <> -1 AND (pr.codigo + pr.descripcion LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY cd.estado DESC, pr.descripcion ASC;
END;
GO

CREATE OR ALTER PROC paVentaListar
    @parametro VARCHAR(100)
AS
BEGIN
    SELECT
        v.id AS idVenta,
        v.fecha AS fechaVenta,
        v.total AS totalVenta,
        v.estado AS estadoVenta,
        v.usuarioRegistro AS usuarioVenta,
        v.fechaRegistro AS fechaRegistroVenta,
        -- Concatenation of the full name of the employee
        e.nombres + ' ' + ISNULL(e.primerApellido, '') + ' ' + ISNULL(e.segundoApellido, '') AS empleado,
        -- Full name of the client
        c.nombreCompleto AS cliente
    FROM Venta v
    INNER JOIN Empleado e ON v.idEmpleado = e.id
    LEFT JOIN Cliente c ON v.idCliente = c.id
    WHERE v.estado <> -1
      AND (
            CAST(v.id AS VARCHAR) +
            e.nombres +
            ISNULL(e.primerApellido, '') +
            ISNULL(e.segundoApellido, '') +
            ISNULL(c.nombreCompleto, '') LIKE '%' + REPLACE(@parametro, ' ', '%') + '%'
        )
    ORDER BY v.estado DESC, v.fecha DESC;
END;
GO

CREATE OR ALTER PROC paVentaDetalleListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT vd.*, p.descripcion AS producto
    FROM VentaDetalle vd
    INNER JOIN Producto p ON vd.idProducto = p.id
    WHERE vd.estado <> -1 AND (p.codigo + p.descripcion LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
    ORDER BY vd.estado DESC, p.descripcion ASC;
END;
GO

-- --- Initial Data Insertion ---

INSERT INTO Categoria (nombre, descripcion, estado) VALUES
('Herramientas', 'Herramientas manuales y el�ctricas', 1),
('Iluminaci�n', 'Bombillas, l�mparas y accesorios de iluminaci�n', 1),
('Electr�nica', 'Dispositivos y componentes electr�nicos', 1),
('Inform�tica', 'Hardware y software para computadoras', 1),
('Smartphones', 'Tel�fonos inteligentes y accesorios', 1),
('Accesorios', 'Diversos accesorios para dispositivos electr�nicos', 1),
('Hogar Inteligente', 'Dispositivos para automatizaci�n del hogar', 1);

INSERT INTO Marca (nombre, descripcion, estado) VALUES
('Bosch', 'Herramientas el�ctricas de alta calidad', 1),
('Philips', 'Productos de iluminaci�n y tecnolog�a avanzada', 1),
('Samsung', 'Marca surcoreana l�der en electr�nica', 1),
('Apple', 'Empresa estadounidense de tecnolog�a', 1),
('Sony', 'Conglomerado multinacional japon�s de electr�nica', 1),
('Xiaomi', 'Empresa china de electr�nica y software', 1),
('Logitech', 'Fabricante suizo de perif�ricos de ordenador', 1);

INSERT INTO Producto (idCategoria, idMarca, codigo, descripcion, unidadMedida, saldo, precioVenta, usuarioRegistro, estado) VALUES
(1, 1, 'H001', 'Taladro Bosch 500W', 'Unidad', 10, 150, SUSER_NAME(), 1),
(2, 2, 'L001', 'Bombilla LED Philips 10W', 'Caja', 50, 12, SUSER_NAME(), 1),
(3, 3, 'TVLED4K-SAM001', 'Televisor Samsung 55" 4K Smart TV', 'Unidad', 15, 799.99, 'AdminUser', 1),
(4, 4, 'MACB-AIR-M2-002', 'MacBook Air M2 13" 256GB SSD', 'Unidad', 10, 1199.00, 'AdminUser', 1),
(5, 6, 'REDMI-NOTE12-003', 'Xiaomi Redmi Note 12 Pro 5G', 'Unidad', 50, 299.50, 'AdminUser', 1),
(6, 7, 'MOUSE-MXM-004', 'Mouse Inal�mbrico Logitech MX Master 3S', 'Unidad', 100, 99.99, 'AdminUser', 1),
(3, 5, 'AUD-WH1000XM5', 'Auriculares Sony WH-1000XM5 Cancelaci�n de Ruido', 'Unidad', 25, 349.00, 'AdminUser', 1);


INSERT INTO Empleado (cedulaIdentidad, nombres, primerApellido, segundoApellido, direccion, celular, cargo, usuarioRegistro, estado)
VALUES ('8765432', 'Maria', 'Gomez', 'Fernandez', 'Av. Las Americas 123', 76543210, 'Vendedora', SUSER_NAME(), 1);

INSERT INTO Usuario (idEmpleado, usuario, clave, usuarioRegistro, estado)
VALUES (1, 'jperez', 'i0hcoO/nssY6WOs9pOp5Xw==', SUSER_NAME(), 1);

INSERT INTO Usuario (idEmpleado, usuario, clave, usuarioRegistro, estado)
VALUES (1, 'pablo', 'i0hcoO/nssY6WOs9pOp5Xw==', SUSER_NAME(), 1);

INSERT INTO Cliente (nit, nombreCompleto, direccion, telefono, email, nombres, apellidos, usuarioRegistro, estado)
VALUES
    (123456789, 'Juan Perez', 'Calle Falsa 123', '78945612', 'juan.perez@email.com', 'Juan', 'Perez', SUSER_NAME(), 1),
    (987654321, 'Maria Gomez', 'Av. Siempre Viva 742', '78945613', 'maria.gomez@email.com', 'Maria', 'Gomez', SUSER_NAME(), 1);

-- Insert initial sales data
INSERT INTO Venta (idEmpleado, idCliente, total, usuarioRegistro, estado)
VALUES (1, 1, 450, SUSER_NAME(), 1);

INSERT INTO VentaDetalle (idVenta, idProducto, cantidad, precioUnitario, total, usuarioRegistro, estado)
VALUES (1, 1, 2, 150, 300, SUSER_NAME(), 1);

INSERT INTO VentaDetalle (idVenta, idProducto, cantidad, precioUnitario, total, usuarioRegistro, estado)
VALUES (1, 2, 5, 12, 60, SUSER_NAME(), 1);

-- --- Verification Queries (Optional, for quick check) ---
SELECT * FROM Categoria;
SELECT * FROM Marca;
SELECT * FROM Producto;
SELECT * FROM Proveedor;
SELECT * FROM Empleado;
SELECT * FROM Usuario;
SELECT * FROM Cliente;
SELECT * FROM Compra;
SELECT * FROM CompraDetalle;
SELECT * FROM Venta;
SELECT * FROM VentaDetalle;

-- Execute stored procedures for verification
EXEC paProductoListar '';
EXEC paEmpleadoListar '';
EXEC paCategoriaListar '';
EXEC paMarcaListar '';
EXEC paProveedorListar '';
EXEC paCompraListar '';
EXEC paCompraDetalleListar '';
EXEC paVentaListar '';
EXEC paVentaDetalleListar '';

