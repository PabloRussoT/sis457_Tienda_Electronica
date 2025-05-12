# SIS457 Tienda Electrónica

## Descripción del Negocio

**Tienda Electrónica** es un sistema de gestión diseñado para una tienda de productos electrónicos que busca optimizar la administración de inventarios, ventas y clientes. Este sistema permite registrar, consultar, actualizar y eliminar información clave para el negocio, proporcionando una experiencia fluida tanto para los administradores como para los clientes.

La tienda se enfoca en la venta de productos electrónicos como teléfonos móviles, laptops, tablets, accesorios tecnológicos, y mucho más. El objetivo principal es brindar a los clientes una experiencia eficiente y organizada al momento de realizar sus compras, y facilitar la gestión interna de los procesos para los encargados.

## Estructura de la Base de Datos

El sistema está respaldado por una base de datos SQL Server optimizada con las siguientes tablas principales:

### 1. Categoría
Clasificación de los productos electrónicos disponibles.
- `id`: Identificador único (PK)
- `nombre`: Nombre de la categoría
- `descripcion`: Descripción detallada

### 2. Marca
Registro de fabricantes de productos electrónicos.
- `id`: Identificador único (PK)
- `nombre`: Nombre de la marca
- `pais`: País de origen de la marca

### 3. Producto
Artículos disponibles para la venta.
- `id`: Identificador único (PK)
- `codigo`: Código único del producto
- `nombre`: Nombre del producto
- `descripcion`: Descripción detallada
- `idCategoria`: Categoría del producto (FK)
- `idMarca`: Marca del producto (FK)
- `modelo`: Modelo específico
- `stock`: Cantidad disponible
- `precioCompra`: Precio de adquisición
- `precioVenta`: Precio de venta al público
- `garantiaMeses`: Duración de la garantía en meses

### 4. Proveedor
Entidades que suministran productos a la tienda.
- `id`: Identificador único (PK)
- `nit`: Número de Identificación Tributaria
- `razonSocial`: Nombre legal
- `direccion`: Ubicación física
- `telefono`: Número de contacto
- `email`: Correo electrónico
- `sitioWeb`: Página web
- `representante`: Persona de contacto

### 5. Empleado
Personal que trabaja en la tienda.
- `id`: Identificador único (PK)
- `cedulaIdentidad`: Documento de identidad
- `nombres`: Nombres del empleado
- `primerApellido`: Primer apellido
- `segundoApellido`: Segundo apellido
- `direccion`: Domicilio del empleado
- `celular`: Número de contacto
- `email`: Correo electrónico
- `cargo`: Puesto que desempeña
- `fechaContratacion`: Fecha de inicio laboral

### 6. Cliente
Compradores de los productos electrónicos.
- `id`: Identificador único (PK)
- `tipoDocumento`: Tipo de identificación (CI, NIT, Pasaporte)
- `numeroDocumento`: Número de identificación
- `nombres`: Nombres del cliente
- `apellidos`: Apellidos del cliente
- `direccion`: Domicilio del cliente
- `telefono`: Número de contacto
- `email`: Correo electrónico

### 7. Usuario
Credenciales para acceso al sistema.
- `id`: Identificador único (PK)
- `idEmpleado`: Empleado asociado (FK)
- `usuario`: Nombre de usuario
- `clave`: Contraseña encriptada
- `nivelAcceso`: Nivel de privilegios (Admin, Gerente, Vendedor, Técnico)

### 8. Compra
Adquisiciones de productos a proveedores.
- `id`: Identificador único (PK)
- `idProveedor`: Proveedor asociado (FK)
- `numeroFactura`: Número de factura de compra
- `fecha`: Fecha de la transacción
- `total`: Monto total
- `observaciones`: Notas adicionales

### 9. CompraDetalle
Detalle de productos adquiridos en cada compra.
- `id`: Identificador único (PK)
- `idCompra`: Compra asociada (FK)
- `idProducto`: Producto adquirido (FK)
- `cantidad`: Unidades compradas
- `precioUnitario`: Precio por unidad
- `subtotal`: Monto parcial

### 10. Venta
Transacciones con clientes.
- `id`: Identificador único (PK)
- `idCliente`: Cliente asociado (FK)
- `idUsuario`: Usuario que realizó la venta (FK)
- `numeroFactura`: Número de factura de venta
- `fecha`: Fecha y hora de la transacción
- `subtotal`: Monto sin impuestos
- `descuento`: Reducción aplicada
- `iva`: Impuesto al valor agregado
- `total`: Monto final
- `tipoPago`: Método de pago (Efectivo, Tarjeta, Transferencia)

### 11. VentaDetalle
Detalle de productos vendidos en cada transacción.
- `id`: Identificador único (PK)
- `idVenta`: Venta asociada (FK)
- `idProducto`: Producto vendido (FK)
- `cantidad`: Unidades vendidas
- `precioUnitario`: Precio por unidad
- `descuento`: Descuento por producto
- `subtotal`: Monto parcial

## Entidades Conceptuales del Sistema

El sistema gestiona las siguientes entidades principales:

### 1. **Producto**
- **ID Producto** (entero, clave primaria)
- **Nombre** (cadena, requerido)
- **Descripción** (cadena, opcional)
- **Precio** (decimal, requerido)
- **Cantidad en Inventario** (entero, requerido)
- **Categoría** (cadena, requerido)

### 2. **Cliente**
- **ID Cliente** (entero, clave primaria)
- **Nombre Completo** (cadena, requerido)
- **Correo Electrónico** (cadena, requerido, único)
- **Teléfono** (cadena, opcional)
- **Dirección** (cadena, opcional)

### 3. **Pedido**
- **ID Pedido** (entero, clave primaria)
- **Fecha** (fecha, requerido)
- **ID Cliente** (entero, clave foránea)
- **Estado** (cadena, requerido, valores posibles: "Pendiente", "Enviado", "Completado")

### 4. **Detalle de Pedido**
- **ID Detalle** (entero, clave primaria)
- **ID Pedido** (entero, clave foránea)
- **ID Producto** (entero, clave foránea)
- **Cantidad** (entero, requerido)
- **Precio Unitario** (decimal, requerido)
- **Subtotal** (decimal, calculado)

### 5. **Categoría**
- **ID Categoría** (entero, clave primaria)
- **Nombre** (cadena, requerido, único)
- **Descripción** (cadena, opcional)

## Funcionalidades Principales

- **Gestión de productos**: Creación, edición, eliminación y consulta de productos con su información detallada.
- **Gestión de clientes**: Registro de clientes nuevos, actualización de información personal y consulta de historial de compras.
- **Procesamiento de pedidos**: Creación de nuevos pedidos, actualización de estado y seguimiento desde la compra hasta la entrega.
- **Gestión de inventario**: Control automático de stock con alertas de reposición y registro de movimientos.
- **Gestión de proveedores**: Registro y seguimiento de proveedores con historial de compras y evaluación.
- **Control de empleados**: Administración del personal con roles y permisos específicos.
- **Reportes y análisis**: Generación de informes de ventas, inventario, rendimiento de productos y tendencias de mercado.
- **Facturación electrónica**: Emisión automática de facturas con cumplimiento de normativas tributarias.

## Relaciones del Sistema

- Un **Producto** pertenece a una **Categoría** y una **Marca**
- Una **Compra** se realiza a un **Proveedor** y contiene múltiples **CompraDetalle**
- Una **Venta** está asociada a un **Cliente** y un **Usuario** (Empleado) y contiene múltiples **VentaDetalle**
- Un **Usuario** está vinculado a un **Empleado**

Este sistema integral permite una gestión eficiente de todos los aspectos del negocio de electrónica, optimizando procesos y mejorando la experiencia tanto para clientes como para el personal administrativo.
