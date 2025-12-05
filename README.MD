# NCQ-ADT Administrador de Tareas  
Aplicación de escritorio desarrollada en **C# / WinForms**, diseñada para gestionar tareas simples con una interfaz amigable, validación de datos y una arquitectura preparada para futuras extensiones.

---

## 📌 Objetivo del Proyecto
El propósito de esta aplicación es permitir a los usuarios **crear, visualizar, editar y eliminar tareas**, aplicando reglas de negocio claras y ofreciendo una experiencia de usuario fluida.  
La solución está diseñada para ser **mantenible**, **escalable** y **fácil de migrar** a Entity Framework en el futuro.

---

## ✅ Funcionalidades Principales

### 📝 Gestión de Tareas
Cada tarea contiene:
- **Descripción**
- **Usuario asignado**
- **Estado** (Pendiente, En Proceso, Terminada)
- **Prioridad** (Baja, Media, Alta)
- **Fecha de compromiso**
- **Notas**

### 🔄 Flujo de Estados
Las tareas solo pueden avanzar en este orden:


### ✏️ Reglas de Edición y Eliminación
- **Editar:** Solo permitido si la tarea está en estado **Pendiente**.
- **Eliminar:** Permitido si la tarea **no está en estado En Proceso**.

### 📋 Visualización
- Lista tipo tabla ordenada por **Fecha de Compromiso**.
- Filtros por:
  - Estado
  - Usuario
  - Prioridad
  - Rango de fechas

---

## 🧱 Arquitectura del Proyecto

La solución sigue una arquitectura por capas para facilitar la mantenibilidad y la futura migración a Entity Framework.

### ✅ Beneficios de esta arquitectura
- La UI no depende de la base de datos.
- La lógica de negocio está centralizada.
- La migración a EF Core solo requiere reemplazar la capa `Infrastructure`.

---

## 🗄️ Capa de Datos

### 🔌 Tecnologías actuales
- **SQLite** como base de datos local.
- **Dapper** como micro ORM.

### 🔄 Migración futura
La capa de datos está diseñada para que la implementación Dapper pueda reemplazarse por una implementación EF Core sin modificar la UI ni la lógica de negocio.

---

## 🖥️ Interfaz de Usuario

La aplicación utiliza WinForms con un diseño limpio y organizado.  
Opcionalmente puede integrarse **DevExpress** para mejorar la experiencia visual (grillas, validaciones, filtros).

Pantallas principales:
- **Lista de tareas**
- **Formulario de creación/edición**
- **Confirmación de eliminación**
- **Cambio de estado**

---

## 🚀 Cómo Ejecutar el Proyecto

### ✅ Requisitos previos
- .NET 6 o superior
- Visual Studio 2022
- SQLite (incluido automáticamente mediante paquetes NuGet)

### ✅ Pasos
1. Clonar el repositorio:
   ```bash
   git clone <url-del-repositorio>


- Abrir la solución en Visual Studio.
- Restaurar paquetes NuGet.
- Ejecutar el proyecto desde el proyecto WinForms.
- La base de datos SQLite se creará automáticamente si no existe.



