# Vehicle Appointments API

API para el agendamiento de citas de revisión vehicular, construida con **.NET 10** siguiendo los principios de la **Arquitectura Hexagonal**.

## 🛠 Tecnologías Necesarias

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (opcional, para despliegue en contenedores)
- Un editor de código como **VS Code** o **Visual Studio 2022**.

## 🏗 Arquitectura

El proyecto está organizado en capas para mantener la lógica de negocio aislada de la infraestructura:
- **Domain**: Entidades y reglas de negocio.
- **Application**: Servicios y casos de uso.
- **Infrastructure**: Implementaciones de persistencia (en memoria) y servicios externos.
- **Web**: Controladores API y configuración del servidor.

## 🚀 Cómo Correr Localmente

1. **Restaurar dependencias:**
   ```bash
   dotnet restore
   ```
2. **Ejecutar la API:**
   Navega a la carpeta del proyecto web y corre:
   ```bash
   cd HexagonalApi.Web
   dotnet run
   ```
   La API estará disponible en `http://localhost:5000` (o el puerto configurado en tus `launchSettings.json`).

## 🐳 Cómo Correr con Docker

Si prefieres usar Docker, sigue estos pasos desde la raíz del proyecto:

1. **Construir la imagen:**
   ```bash
   docker build -t vehicle-appointments-api .
   ```
2. **Correr el contenedor:**
   ```bash
   docker run -d -p 8080:8080 --name vehicle-api-container vehicle-appointments-api
   ```
   Accede a la API en `http://localhost:8080`.

## 🛑 Cómo Detener Docker

Para detener y limpiar el contenedor, usa:

1. **Detener:** `docker stop vehicle-api-container`
2. **Eliminar:** `docker rm vehicle-api-container`
3. **Todo en uno:** `docker rm -f vehicle-api-container`

## ⏱ Reglas de Negocio (Validaciones)

La API aplica las siguientes reglas para agendar una cita:
- **Horario:** Lunes a Viernes de **08:00 AM a 02:00 PM**.
- **Intervalos:** Las citas deben ser exactas cada **30 minutos** (ej: 08:00, 08:30, 09:00).
- **Zona Horaria:** El backend convierte automáticamente cualquier fecha recibida a la **Hora de Ecuador (UTC-5)** antes de validar.

## 📡 Endpoints Principales

### Crear Cita
`POST /api/appointments`

**Cuerpo (JSON):**
```json
{
  "licensePlate": "ABC-1234",
  "scheduledAt": "2026-02-25T10:30:00"
}
```

---
> [!NOTE]
> Este proyecto utiliza un repositorio **en memoria**, por lo que los datos se perderán al reiniciar el servidor.
