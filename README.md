# Examen de Tesis

Aplicación local para gestionar tutores y tesis. La solución contiene una API ASP.NET Core y un frontend Blazor WebAssembly.

## Requisitos

- .NET SDK 11
- SQL Server Express (u otra instancia SQL Server)

## Configuración local

La cadena de conexión y la clave de firma JWT se guardan mediante .NET User Secrets y no se incluyen en el repositorio. Desde la raíz de la solución, configura tus valores:

```powershell
dotnet user-secrets set "ConnectionStrings:ConexionDB" "Data Source=TU_SERVIDOR\SQLEXPRESS;Initial Catalog=tesisexamenfinal;Integrated Security=True;TrustServerCertificate=True" --project .\WebApplication-Ejemplo-1\WebApplication-Ejemplo-1.csproj
dotnet user-secrets set "Jwt:Key" "UNA_CLAVE_ALEATORIA_LOCAL_DE_AL_MENOS_32_CARACTERES" --project .\WebApplication-Ejemplo-1\WebApplication-Ejemplo-1.csproj
```

Usa una clave aleatoria propia; no publiques los valores de User Secrets.

## Ejecutar

Inicia ambos proyectos desde Visual Studio:

- API: `WebApplication-Ejemplo-1`
- Interfaz: `FrontendTesis`

Direcciones locales predeterminadas:

- Interfaz: https://localhost:7269/login
- Swagger: https://localhost:7227/swagger

Las migraciones y las semillas de tutores y tesis se aplican al iniciar la API.

## Imágenes

Selecciona las imágenes desde el formulario de tesis. La API las guarda localmente en `WebApplication-Ejemplo-1/wwwroot/imagenes`; la base de datos conserva solo la ruta. Los archivos añadidos a esa carpeta se ignoran en Git para evitar publicar imágenes personales o con derechos de terceros. La carpeta incluye `INSTRUCCIONES.txt`.
