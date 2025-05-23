-- Crear tablas Venta y VentaDetalle
USE TiendaElectronica;

GO
DROP TABLE IF EXISTS VentaDetalle;
DROP TABLE IF EXISTS Venta;

CREATE TABLE Venta (
    id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    idEmpleado INT NOT NULL,
    fecha DATE NOT NULL DEFAULT GETDATE(),
    total DECIMAL NOT NULL CHECK (total >= 0),
    usuarioRegistro VARCHAR(50) NOT NULL DEFAULT SUSER_NAME(),
    fechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    estado SMALLINT NOT NULL DEFAULT 1, -- -1: Eliminado, 0: Inactivo, 1: Activo
    CONSTRAINT fk_Venta_Empleado FOREIGN KEY(idEmpleado) REFERENCES Empleado(id)
);

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
-- Procedimientos almacenados para listar las ventas
CREATE OR ALTER PROC paVentaListar @parametro VARCHAR(100)
AS
BEGIN
    SELECT v.*, e.nombres + ' ' + ISNULL(e.primerApellido, '') AS empleado
    FROM Venta v
    INNER JOIN Empleado e ON v.idEmpleado = e.id
    WHERE v.estado <> -1 AND (CAST(v.id AS VARCHAR) + e.nombres + ISNULL(e.primerApellido, '') LIKE '%' + REPLACE(@parametro, ' ', '%') + '%')
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

-- Insertar datos iniciales en Venta y VentaDetalle
INSERT INTO Venta (idEmpleado, total)
VALUES (1, 450);

INSERT INTO VentaDetalle (idVenta, idProducto, cantidad, precioUnitario, total)
VALUES (1, 1, 2, 150, 300);

INSERT INTO VentaDetalle (idVenta, idProducto, cantidad, precioUnitario, total)
VALUES (1, 2, 5, 12, 60);

-- Consultar datos para verificar
SELECT * FROM Venta;
SELECT * FROM VentaDetalle;
