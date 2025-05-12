-- Script para crear la base de datos TiendaElectronica
CREATE DATABASE TiendaElectronica;
GO
USE [master]
GO
-- Crear el login para la base de datos
CREATE LOGIN [usrelectronica] WITH PASSWORD = N'123456',
	DEFAULT_DATABASE = [TiendaElectronica],
	CHECK_EXPIRATION = OFF,
	CHECK_POLICY = ON
GO
USE [TiendaElectronica]
GO
-- Crear el usuario para la base de datos
CREATE USER [usrelectronica] FOR LOGIN [usrelectronica]
GO
ALTER ROLE [db_owner] ADD MEMBER [usrelectronica]
GO

-- Eliminación de tablas en caso de que existan (para reejecutar el script)
IF OBJECT_ID('VentaDetalle', 'U') IS NOT NULL DROP TABLE VentaDetalle;
IF OBJECT_ID('Venta', 'U') IS NOT NULL DROP TABLE Venta;
IF OBJECT_ID('CompraDetalle', 'U') IS NOT NULL DROP TABLE CompraDetalle;
IF OBJECT_ID('Compra', 'U') IS NOT NULL DROP TABLE Compra;
IF OBJECT_ID('Usuario', 'U') IS NOT NULL DROP TABLE Usuario;
IF OBJECT_ID('Cliente', 'U') IS NOT NULL DROP TABLE Cliente;
IF OBJECT_ID('Empleado', 'U') IS NOT NULL DROP TABLE Empleado;
IF OBJECT_ID('Producto', 'U') IS NOT NULL DROP TABLE Producto;
IF OBJECT_ID('Categoria', 'U') IS NOT NULL DROP TABLE Categoria;
IF OBJECT_ID('Marca', 'U') IS NOT NULL DROP TABLE Marca;
IF OBJECT_ID('Proveedor', 'U') IS NOT NULL DROP TABLE Proveedor;

-- Creación de tablas

-- Tabla Categoría para clasificar productos electrónicos
CREATE TABLE Categoria (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombre VARCHAR(50) NOT NULL,
  descripcion VARCHAR(250) NULL
);

-- Tabla Marca para identificar fabricantes
CREATE TABLE Marca (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nombre VARCHAR(50) NOT NULL,
  pais VARCHAR(50) NULL
);

-- Tabla Producto con relaciones a Categoría y Marca
CREATE TABLE Producto (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  codigo VARCHAR(20) NOT NULL,
  nombre VARCHAR(100) NOT NULL,
  descripcion VARCHAR(250) NOT NULL,
  idCategoria INT NOT NULL,
  idMarca INT NOT NULL,
  modelo VARCHAR(50) NULL,
  stock DECIMAL NOT NULL DEFAULT 0,
  precioCompra DECIMAL NOT NULL CHECK (precioCompra > 0),
  precioVenta DECIMAL NOT NULL CHECK (precioVenta > 0),
  garantiaMeses INT NULL DEFAULT 0,
  CONSTRAINT fk_Producto_Categoria FOREIGN KEY(idCategoria) REFERENCES Categoria(id),
  CONSTRAINT fk_Producto_Marca FOREIGN KEY(idMarca) REFERENCES Marca(id)
);

-- Tabla Proveedor
CREATE TABLE Proveedor (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  nit BIGINT NOT NULL,
  razonSocial VARCHAR(100) NOT NULL,
  direccion VARCHAR(250) NULL,
  telefono VARCHAR(30) NOT NULL,
  email VARCHAR(100) NULL,
  sitioWeb VARCHAR(100) NULL,
  representante VARCHAR(100) NOT NULL
);

-- Tabla Empleado
CREATE TABLE Empleado (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  cedulaIdentidad VARCHAR(12) NOT NULL,
  nombres VARCHAR(30) NOT NULL,
  primerApellido VARCHAR(30) NULL,
  segundoApellido VARCHAR(30) NULL,
  direccion VARCHAR(250) NOT NULL,
  celular BIGINT NOT NULL,
  email VARCHAR(100) NULL,
  cargo VARCHAR(50) NOT NULL,
  fechaContratacion DATE NOT NULL DEFAULT GETDATE()
);

-- Tabla Cliente
CREATE TABLE Cliente (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  tipoDocumento VARCHAR(20) NOT NULL DEFAULT 'CI',  -- CI, NIT, Pasaporte
  numeroDocumento VARCHAR(20) NOT NULL,
  nombres VARCHAR(30) NOT NULL,
  apellidos VARCHAR(60) NULL,
  direccion VARCHAR(250) NULL,
  telefono VARCHAR(30) NULL,
  email VARCHAR(100) NULL
);

-- Tabla Usuario
CREATE TABLE Usuario (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idEmpleado INT NOT NULL,
  usuario VARCHAR(20) NOT NULL,
  clave VARCHAR(250) NOT NULL,
  nivelAcceso VARCHAR(20) NOT NULL DEFAULT 'Vendedor', -- Admin, Gerente, Vendedor, Técnico
  CONSTRAINT fk_Usuario_Empleado FOREIGN KEY(idEmpleado) REFERENCES Empleado(id)
);

-- Tabla Compra
CREATE TABLE Compra (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idProveedor INT NOT NULL,
  numeroFactura VARCHAR(20) NOT NULL,
  fecha DATE NOT NULL DEFAULT GETDATE(),
  total DECIMAL NOT NULL DEFAULT 0,
  observaciones VARCHAR(500) NULL,
  CONSTRAINT fk_Compra_Proveedor FOREIGN KEY(idProveedor) REFERENCES Proveedor(id)
);

-- Tabla Detalle de Compra
CREATE TABLE CompraDetalle (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idCompra INT NOT NULL,
  idProducto INT NOT NULL,
  cantidad DECIMAL NOT NULL CHECK (cantidad > 0),
  precioUnitario DECIMAL NOT NULL,
  subtotal DECIMAL NOT NULL,
  CONSTRAINT fk_CompraDetalle_Compra FOREIGN KEY(idCompra) REFERENCES Compra(id),
  CONSTRAINT fk_CompraDetalle_Producto FOREIGN KEY(idProducto) REFERENCES Producto(id)
);

-- Tabla Venta
CREATE TABLE Venta (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idCliente INT NOT NULL,
  idUsuario INT NOT NULL,
  numeroFactura VARCHAR(20) NOT NULL,
  fecha DATETIME NOT NULL DEFAULT GETDATE(),
  subtotal DECIMAL NOT NULL,
  descuento DECIMAL NOT NULL DEFAULT 0,
  iva DECIMAL NOT NULL DEFAULT 0,
  total DECIMAL NOT NULL,
  tipoPago VARCHAR(20) NOT NULL DEFAULT 'Efectivo', -- Efectivo, Tarjeta, Transferencia
  CONSTRAINT fk_Venta_Cliente FOREIGN KEY(idCliente) REFERENCES Cliente(id),
  CONSTRAINT fk_Venta_Usuario FOREIGN KEY(idUsuario) REFERENCES Usuario(id)
);

-- Tabla Detalle de Venta
CREATE TABLE VentaDetalle (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  idVenta INT NOT NULL,
  idProducto INT NOT NULL,
  cantidad DECIMAL NOT NULL CHECK (cantidad > 0),
  precioUnitario DECIMAL NOT NULL,
  descuento DECIMAL NOT NULL DEFAULT 0,
  subtotal DECIMAL NOT NULL,
  CONSTRAINT fk_VentaDetalle_Venta FOREIGN KEY(idVenta) REFERENCES Venta(id),
  CONSTRAINT fk_VentaDetalle_Producto FOREIGN KEY(idProducto) REFERENCES Producto(id)
);

-- Añadir campos de auditoría a todas las tablas
ALTER TABLE Categoria ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Categoria ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Categoria ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

ALTER TABLE Marca ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Marca ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Marca ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

ALTER TABLE Producto ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Producto ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Producto ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

ALTER TABLE Proveedor ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Proveedor ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Proveedor ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

ALTER TABLE Empleado ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Empleado ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Empleado ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

ALTER TABLE Cliente ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Cliente ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Cliente ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

ALTER TABLE Usuario ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Usuario ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Usuario ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

ALTER TABLE Compra ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Compra ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Compra ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

ALTER TABLE CompraDetalle ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE CompraDetalle ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE CompraDetalle ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

ALTER TABLE Venta ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE Venta ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE Venta ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

ALTER TABLE VentaDetalle ADD usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME();
ALTER TABLE VentaDetalle ADD fechaRegistro DATETIME NOT NULL DEFAULT GETDATE();
ALTER TABLE VentaDetalle ADD estado SMALLINT NOT NULL DEFAULT 1; -- -1:Eliminado, 0: Inactivo, 1: Activo

-- Procedimientos almacenados útiles

GO
-- Procedimiento para listar productos
CREATE PROC paProductoListar @parametro VARCHAR(100)
AS
  SELECT p.*, c.nombre as categoria, m.nombre as marca 
  FROM Producto p
  INNER JOIN Categoria c ON p.idCategoria = c.id
  INNER JOIN Marca m ON p.idMarca = m.id
  WHERE p.estado<>-1 AND p.codigo+p.nombre+p.descripcion+c.nombre+m.nombre LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY p.estado DESC, p.nombre ASC;
GO

-- Procedimiento para listar empleados
CREATE PROC paEmpleadoListar @parametro VARCHAR(100)
AS
  SELECT ISNULL(u.usuario,'--') AS usuario, e.* 
  FROM Empleado e
  LEFT JOIN Usuario u ON e.id = u.idEmpleado
  WHERE e.estado<>-1 
	AND e.cedulaIdentidad+e.nombres+ISNULL(e.primerApellido,'')+ISNULL(e.segundoApellido,'') LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY e.estado DESC, e.nombres ASC, e.primerApellido ASC;
GO

-- Procedimiento para listar categorías
CREATE PROC paCategoriaListar @parametro VARCHAR(100)
AS
  SELECT * FROM Categoria
  WHERE estado<>-1 AND nombre+ISNULL(descripcion,'') LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY estado DESC, nombre ASC;
GO

-- Procedimiento para listar marcas
CREATE PROC paMarcaListar @parametro VARCHAR(100)
AS
  SELECT * FROM Marca
  WHERE estado<>-1 AND nombre+ISNULL(pais,'') LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY estado DESC, nombre ASC;
GO

-- Procedimiento para generar reporte de ventas por período
CREATE PROC paReporteVentas @fechaInicio DATE, @fechaFin DATE
AS
  SELECT v.id, v.numeroFactura, v.fecha, 
         c.nombres + ' ' + ISNULL(c.apellidos,'') as cliente,
         u.usuario as vendedor,
         v.total, v.tipoPago
  FROM Venta v
  INNER JOIN Cliente c ON v.idCliente = c.id
  INNER JOIN Usuario u ON v.idUsuario = u.id
  WHERE v.estado = 1 AND v.fecha BETWEEN @fechaInicio AND @fechaFin
  ORDER BY v.fecha DESC;
GO

-- Datos de ejemplo

-- Insertar Categorías
INSERT INTO Categoria(nombre, descripcion) VALUES 
('Smartphones', 'Teléfonos inteligentes de diferentes marcas'),
('Laptops', 'Computadoras portátiles para diferentes usos'),
('Tablets', 'Dispositivos tipo tableta'),
('Accesorios', 'Accesorios para dispositivos electrónicos'),
('Smart TV', 'Televisores inteligentes');

-- Insertar Marcas
INSERT INTO Marca(nombre, pais) VALUES 
('Samsung', 'Corea del Sur'),
('Apple', 'Estados Unidos'),
('Xiaomi', 'China'),
('HP', 'Estados Unidos'),
('LG', 'Corea del Sur');

-- Insertar Productos
INSERT INTO Producto(codigo, nombre, descripcion, idCategoria, idMarca, modelo, stock, precioCompra, precioVenta, garantiaMeses)
VALUES 
('SM001', 'Galaxy S21', 'Smartphone Samsung Galaxy S21 128GB', 1, 1, 'SM-G991B', 10, 4500, 5500, 12),
('LP001', 'MacBook Pro', 'Laptop Apple MacBook Pro M1 256GB', 2, 2, 'MYDA2LL/A', 5, 8000, 9800, 12),
('TB001', 'iPad Pro', 'Tablet Apple iPad Pro 11" 128GB', 3, 2, 'MHQT3LL/A', 8, 5000, 6200, 12),
('AC001', 'AirPods Pro', 'Audífonos inalámbricos Apple', 4, 2, 'MWP22AM/A', 15, 1200, 1500, 6),
('TV001', 'Smart TV 55"', 'Televisor Samsung Smart TV 4K 55"', 5, 1, 'UN55TU8000', 6, 3500, 4200, 12);

-- Insertar Proveedores
INSERT INTO Proveedor(nit, razonSocial, direccion, telefono, email, sitioWeb, representante)
VALUES 
(123456789, 'Electro Importaciones S.A.', 'Av. Principal #123', '591-2-1234567', 'contacto@electroimport.com', 'www.electroimport.com', 'Carlos Pérez'),
(987654321, 'Tech Supplies', 'Calle Comercio #456', '591-2-7654321', 'ventas@techsupplies.com', 'www.techsupplies.com', 'Ana López');

-- Insertar Empleados
INSERT INTO Empleado(cedulaIdentidad, nombres, primerApellido, segundoApellido, direccion, celular, email, cargo, fechaContratacion)
VALUES 
('1234567', 'Juan', 'Pérez', 'López', 'Av. Las Américas #789', 71717171, 'jperez@tiendaelectronicos.com', 'Gerente', '2020-01-15'),
('7654321', 'María', 'Gómez', 'Torres', 'Calle 23 #456', 72727272, 'mgomez@tiendaelectronicos.com', 'Vendedor', '2021-03-10');

-- Insertar Usuarios
INSERT INTO Usuario(idEmpleado, usuario, clave, nivelAcceso)
VALUES 
(1, 'jperez', 'i0hcoO/nssY6WOs9pOp5Xw==', 'Admin'), -- Contraseña: 123456 (Encriptada)
(2, 'mgomez', 'i0hcoO/nssY6WOs9pOp5Xw==', 'Vendedor');

-- Insertar Clientes
INSERT INTO Cliente(tipoDocumento, numeroDocumento, nombres, apellidos, direccion, telefono, email)
VALUES 
('CI', '9876543', 'Pedro', 'Martínez Suárez', 'Av. Banzer #1000', '70707070', 'pmartinez@gmail.com'),
('NIT', '12345678901', 'Empresa XYZ', NULL, 'Zona Industrial #500', '33445566', 'compras@xyz.com');

-- Insertar una Compra con su Detalle
INSERT INTO Compra(idProveedor, numeroFactura, fecha, total, observaciones)
VALUES (1, 'F00123', '2023-05-15', 67000, 'Compra mensual de inventario');

INSERT INTO CompraDetalle(idCompra, idProducto, cantidad, precioUnitario, subtotal)
VALUES 
(1, 1, 5, 4500, 22500), -- 5 Samsung Galaxy S21
(1, 2, 3, 8000, 24000), -- 3 MacBook Pro
(1, 4, 10, 1200, 12000), -- 10 AirPods Pro
(1, 5, 2, 3500, 7000); -- 2 Smart TV Samsung

-- Actualizar el stock de productos
UPDATE Producto SET stock = stock + 5 WHERE id = 1;
UPDATE Producto SET stock = stock + 3 WHERE id = 2;
UPDATE Producto SET stock = stock + 10 WHERE id = 4;
UPDATE Producto SET stock = stock + 2 WHERE id = 5;

-- Insertar una Venta con su Detalle
INSERT INTO Venta(idCliente, idUsuario, numeroFactura, fecha, subtotal, descuento, iva, total, tipoPago)
VALUES (1, 2, 'V00001', '2023-05-20', 7000, 200, 910, 7710, 'Tarjeta');

INSERT INTO VentaDetalle(idVenta, idProducto, cantidad, precioUnitario, descuento, subtotal)
VALUES 
(1, 1, 1, 5500, 0, 5500), -- 1 Samsung Galaxy S21
(1, 4, 1, 1500, 200, 1300); -- 1 AirPods Pro con descuento

-- Actualizar el stock de productos vendidos
UPDATE Producto SET stock = stock - 1 WHERE id = 1;
UPDATE Producto SET stock = stock - 1 WHERE id = 4;

-- Probar procedimientos almacenados
EXEC paProductoListar 'samsung';
EXEC paEmpleadoListar '';
EXEC paCategoriaListar '';
EXEC paMarcaListar '';
EXEC paReporteVentas '2023-01-01', '2023-12-31';