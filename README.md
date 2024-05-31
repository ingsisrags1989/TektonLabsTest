# Product API - Microservice

Este proyecto es una API  en .NET Core 8 diseñada para crear, actualizar y buscar un producto por su Id. 

Sigue los principios de Arquitectura Limpia y emplea el patrón CQRS (Command Query Responsibility Segregation) usando Mediator.

### Estructura de Directorios

```bash 
ProductService/
├── ProductApi/
├── Domain/
│ ├── Entities
│ ├── Repositories
├── Application/
│ ├── Commands
│ ├── Queries
│ ├── DTOs
│ ├── Mappers
│ └── Handlers
├── Infrastructure/
│ ├── Context
│ ├── Repositories
│ ├── Injection
│ ├── Migrations
├── Test
│ ├── DomainTests
```

## Características Clave

- **Patrón CQRS**: Separa los comandos y las consultas para simplificar las operaciones y mejorar la escalabilidad.
- **Patrón Mediator**: Desacopla el manejo de solicitudes de la solicitud en sí.
- **Inyección de Dependencias**: Promueve el acoplamiento débil y mejora la capacidad de prueba.
- **Patrón de Repositorio**: Abstrae la lógica de acceso a datos.
- **Código Limpio y Principios SOLID**: Asegura un código mantenible y escalable.

# Repositorio Genérico
Este repositorio proporciona una estructura genérica que puede ser utilizada como base para diferentes proyectos. Su objetivo es facilitar la reutilización de código y la eficiencia en el desarrollo al ofrecer componentes y funcionalidades listas para integrarse en diversos contextos.

**Integración del Middleware:**
   - El middleware realiza el manejo tanto de las peticiones http como el archivo de logs de tiempo de respuestas de la API.

# Lanzar la Api 

## Requisitos

- Docker

### Linux y Windows
    
```bash 
docker-compose up --build
```

## Puertos API
La api corre por los puertos \
\
http --> 5100 \
https --> 5150

## Swagger

https://localhost:5150/swagger/index.html
