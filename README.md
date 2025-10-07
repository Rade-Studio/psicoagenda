# PsicoAgenda 🧠

Aplicativo web para psicólogos independientes: **gestionar pacientes, citas, notas de sesión (plantilla SOAP) y cuestionarios simples**, con foco en privacidad y facilidad de uso.

> Estado: MVP en definición y arranque (oct–nov).

---

## ✨ Objetivos del MVP
- **Pacientes**: registro básico, búsqueda y etiquetas.
- **Agenda**: crear/editar/cancelar citas; vista por semana/mes.
- **Sesiones**: notas (libre o SOAP), adjuntos.
- **Cuestionarios**: formularios sencillos (Likert) y evolución por paciente.
- **Dashboard**: próximas citas y accesos rápidos.

> En V1 se asume un solo profesional (sin multi-clínica).

---

## 🏗️ Arquitectura (resumen)
- **Monorepo**:
  - `backend/` → API en C# (ASP.NET Core) + EF Core + PostgreSQL.
  - `frontend/` → (se agregará después; probablemente Next.js + TS).
- **Capas backend**:
  - Controllers → Servicios de aplicación → Infraestructura (EF Core) → Base de datos.
- **Almacenamiento**: PostgreSQL; adjuntos en storage (a definir).

**Más detalles**: ver [`docs/arquitectura.md`](docs/arquitectura.md).

---

## 🧰 Tecnologías (planeadas)
**Backend**
- .NET 8, ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL (Docker)
- Swagger (documentación de API)

**Frontend (posterior)**
- Next.js + TypeScript + Tailwind + shadcn/ui
- React Hook Form + Zod
- Recharts (gráficas)

---

## 📂 Estructura del repo (propuesta)
