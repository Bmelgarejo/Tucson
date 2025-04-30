# 🍽️ Tucson

**Tucson** es una aplicación de gestión de reservas para restaurantes, diseñada para manejar mesas, clientes y listas de espera.

---

## ✨ Características Principales

- 🔘 **Gestión de Mesas**: Control de disponibilidad y capacidad.
- 📆 **Gestión de Reservas**: Crear, consultar y cancelar reservas.
- ⏳ **Lista de Espera**: Administración automática de clientes en espera.
- 👥 **Clientes**: Soporte para categorías de membresía: Classic, Gold, Platinum y Diamond.
- 🌐 **API REST**: Servicios expuestos mediante ASP.NET Core.
- 🧪 **Pruebas Unitarias**: Testeado con **xUnit** y **FluentAssertions**.

---

## 🧱 Estructura del Proyecto

El proyecto está organizado en las siguientes capas:

| Capa                | Descripción                                                                 |
|---------------------|-----------------------------------------------------------------------------|
| `Tucson.Domain`     | Contiene las entidades centrales e interfaces del dominio.                  |
| `Tucson.Application`| Implementa la lógica de negocio y las estrategias de asignación.            |
| `Tucson.Infrastructure` | Repositorios y datos simulados para pruebas e implementación.           |
| `Tucson.API`        | Exposición de la API REST para interacción externa.                         |
| `Tucson.Test`       | Pruebas unitarias que aseguran el comportamiento del sistema.              |

---

## ⚙️ Requisitos Previos

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 o cualquier editor compatible con .NET
- (Opcional) [Postman](https://www.postman.com/) o Swagger UI para probar la API

---

## 🚀 Configuración y Uso

### 1. Clonar el Repositorio

```bash
git clone https://github.com/Bmelgarejo/Tucson.git
```

### 2. Compilar y Ejecutar

- Abrí la solución en Visual Studio
- Ejecutá el proyecto `Tucson.API`
- Accedé a la documentación Swagger en:

```
http://localhost:5140/swagger/index.html
```

---

## 📡 Endpoints de la API

Todos los endpoints están disponibles bajo la ruta base: `http://localhost:5140/api/reservation`

### 📁 Gestión de Reservas

| Acción              | Método | Endpoint                                      | Descripción                           |
|---------------------|--------|-----------------------------------------------|---------------------------------------|
| Obtener Reservas    | GET    | `/reservations`                               | Devuelve la lista de reservas activas |
| Crear Reserva       | POST   | `/create-reservation`                         | Registra una nueva reserva            |
| Eliminar Reserva    | DELETE | `/reservations/{reservationId}`              | Cancela una reserva por ID            |

### ⏱️ Lista de Espera

| Acción                  | Método | Endpoint            | Descripción                                                |
|--------------------------|--------|---------------------|------------------------------------------------------------|
| Obtener Lista de Espera | GET    | `/waiting-list`     | Devuelve la lista de clientes en espera                   |

---
## Diagrama de secuencia

![image](https://github.com/user-attachments/assets/d7bd7208-0576-4bcb-b9c1-639dc4be09f7)

---

## ✅ Pruebas

El proyecto incluye pruebas unitarias que podés ejecutar desde Visual Studio o desde la terminal:

```bash
dotnet test Tucson.Test
```

---

## 📝 Notas

- El puerto por defecto es `5140`. Podés modificarlo desde `launchSettings.json`.
- El sistema considera la **categoría de membresía del cliente** al asignar mesas.
- El almacenamiento de datos está simulado para fines de prueba, sin persistencia real.

---

## 🔗 Enlaces Útiles

- 🧠 [Repositorio en GitHub](https://github.com/Bmelgarejo/Tucson.git)
- 📖 [Documentación Swagger](http://localhost:5140/swagger/index.html)

---
