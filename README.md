# sis457_Tienda_Electronica

## Descripción del Negocio
**Tienda Electrónica** es un sistema de gestión diseñado para una tienda de productos electrónicos que busca optimizar la administración de inventarios, ventas y clientes. Este sistema permite registrar, consultar, actualizar y eliminar información clave para el negocio, proporcionando una experiencia fluida tanto para los administradores como para los clientes.

La tienda se enfoca en la venta de productos electrónicos como teléfonos móviles, laptops, tablets, accesorios tecnológicos, y mucho más. El objetivo principal es brindar a los clientes una experiencia eficiente y organizada al momento de realizar sus compras, y facilitar la gestión interna de los procesos para los encargados.

---

## Entidades del Sistema

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

---

## Funcionalidades Principales
- Gestión de productos: creación, edición, eliminación y consulta.
- Gestión de clientes: registro, actualización de información y consulta.
- Procesamiento de pedidos: creación, actualización de estado y consulta.
- Reportes de ventas e inventarios.

---
