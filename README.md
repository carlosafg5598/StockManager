# StockManager

## Orden de subidas

### 1) 

1) Lista de las tareas pendientes de la primera semana 
2) Esquema de la base de datos
3) Proyecto asp.Net creado

### 2) 
1) Modificación del proyecto para usar Sqlite y evitarme problemas a la hora de usar diferentes ordenadores

### 3) 
1) Conexión y migraciones
2) Seed
3) Prueba página inicio de registro/inicio sesión

### 4) 
1) CRUD de productos
2) Gestión de movimientos

### 5) (13/10/25)
1) Segundo intento identiity
Después de probar con tutoriales decidí descargar la rama principal de github y probar con chatgpt, a ver si lo podía solucionar. Me mandaba hacer practicamente lo mismo y después de un rato y varios consejos seguí con los mismos errores
2) Arreglo identity
Después de dar una pausa probé a abrir esta rama del proyecto desde el ordenador de casa, y al leer el código que escribí a lo largo de la mañana y a hacer un par de pruebas vi que tenía dos lineas de código diferentes pero que hacían lo mismo , lo que provocaba el error a la hora de abrir el proyecto, una vez corregido el proyecto, por lo que en principio el identity está ya configurado

### 6) (14/10/25)
1) Creación de vistas básicas para los usuarios
Creación de los controladores oportunos así como de las vistas dependiendo de que tipo de usuario sea el que inicia sesión (roles)
Corregir inicio de sesión (explicado en sección errores)

### 7) (16/10/25)
1) Creación del CRUD de proveedores

### 8) (30/10/25)
1) Creación de la vista para editar perfiles
2) Modificación del Layout para poder modificar cada una de las vistas al editar el perfil
3) **Dudas en el proyecto:**  Después de comprobar todo no tengo del todo claro la función de la vista de informes que metí en el anteproyecto.

### 9) (03/11/25)
1) Corrección de las fuentes a la hora de editar un perfil
2) Corrección del controlador MovimientosController para gestionar el editar y el eliminar y así poder modificar la cantidad de stock de los productos
3) Creación del controlador de informes
4) Vista de informes y uso de QuestPDF para generar informes

### 10) (05/11/25)
1) Administración de las contraseñas de los usuarios
2) Mejora de la vista para editar perfiles de forma que puedan editar el número

### 11) (10/11/25)
1) Publicación del proyecto en un directorio
2) Configureción del proyecto en una red local
3) Pruebas de errores

***

## Errores a lo largo del proyecto
1) 10/10/25
- ERROR: A la hora de hacer la gestión de los moviminetos, conecta bien con la base de datos, pero a la hora de registrar el movimiento falla debido a la clave foraneo del producto
- CAUSA:El formulario manda un id de Producto nulo
- SOLUCIÓN: Arreglar el controlador para que envie correctamente el id
  
2) 13/10/25
- ERROR: Para poder realizar el control de los movimientos, decidí implementar ya el identity de los usuarios, pero eso me dió errores a la hora de iniciar sesión
- CAUSA: Tenía dos lineas de código que hacían lo mismo repetidas
- SOLUCIÓN: Borré la primera linea ya que con la segunda es con la que usaba identity
  
3) 14/10/25
- ERROR: Después de gestionar las vistas me vi con la problemática de que puedo acceder a la vista de cada uno de los diferentes roles sin iniciar sesión
- CAUSA: Las vistas no tenían aplicada ninguna política de autorización, por lo que eran accesibles de forma anónima.
- SOLUCIÓN: Añadí los atributos [Authorize] y [Authorize(Roles="...")] en los controladores y vistas correspondientes. De esta forma, cada vista solo puede ser accedida por los usuarios con los roles adecuados, garantizando la seguridad y el control de acceso dentro de la aplicación.
  
4) 30/10/25
- ERROR: Después de crear la vista para editar perfiles y probar los diferentes apartados que se pueden modificar veo que las fuentes no se modifican, sin embargo si me registra el cambio
- CAUSA: El CSS aplicaba la fuente desde una variable personalizada que no estaba actualizando el valor dinámicamente en el Layout. Aunque la nueva fuente se guardaba, el archivo de estilos no se estaba refrescando con el cambio, y además algunas fuentes que probé requerían carga desde Google Fonts.
- SOLUCIÓN: Simplifiqué la lógica usando fuentes básicas del sistema (Arial, Verdana, Times New Roman, etc.) para evitar dependencias externas, y modifiqué el CSS base del Layout para que la fuente se aplicara a todo el contenido mediante una variable global (--font-family).
  
5) 31/10/25
- ERROR: Creando la vista de informes (en la cual he decidido que se trate sobre poder filtrar los movimientos y generar pdf) me encontré con el error de que si elimino movimientos creados las cantidades de esos movimintos siguen afectando a los productos
- CAUSA: No tenía acabados los métodos de editar y de eliminar en MovimientosController
- SOLUCIÓN: Completé los elementos Edit y Delete.
  
6) 02/11/25
- ERROR: Al intentar implementar la exportación del informe de movimientos a PDF utilizando la librería QuestPDF, el programa lanzó una excepción. El mensaje de error indicaba que era necesario seleccionar un tipo de licencia para continuar usando la librería.
- CAUSA:QuestPDF tiene dos tipos de licencias, Community License (gratuita para proyectos personales o con ingresos menores a 1 millón USD) y Commercial License (para empresas grandes). Debido a no tener una licencia configurada saltó la excepción.
- SOLUCIÓN: Añadí la siguiente linea antes de generar el documento:
  ~~~~~
  QuestPDF.Settings.License = LicenseType.Community;
  ~~~~~
  
7) 03/11/25
  - ERROR: Al hacer otra vez las pruebas de las autorizaciones, veo que los mensjaes de error que quiero que se muestren en las propias vistas cuando un tipo de usuario no puede acceder a algo en concreto (que un empleado edite un producto, lo cual no puede) se muestran en otras vistas
  - CAUSA: Mal uso de la propiedad TempData. En las clases MovimientosController y ProductosController uso esta propiedad dándole el mismo valor, "Error", y marco para que se redireccione a la vista Index, lo que hace que al provocar varios errores seguidos se muestren los errores en vistas diferentes. A mayores en la clase ProductosController no había añadido el if para el uso del TempData.
  - SOLUCIÓN: Completé la clae ProductosController para el uso del TempData y le cambié los valores a los TempData de ProductosController y de MovimientosController.

8) 05/11/25
- ERROR:Durante la fase final del proyecto, al revisar detalles antes de grabar los vídeos, detecté que no había implementado la posibilidad de gestionar o cambiar contraseñas desde el perfil del usuario. Al intentar añadir esta funcionalidad, la aplicación comenzó a fallar.
- CAUSA: El problema se debía a la existencia de dos controladores que gestionaban la autenticación (LoginController y AccountController), lo que generaba conflictos al manejar el inicio de sesión y las operaciones relacionadas con Identity.
- SOLUCIÓN: Eliminé el controlador AccountController, ya que este se genera automáticamente al usar Identity, y mantuve únicamente el LoginController, adaptando este último para que gestione el inicio de sesión y complementándolo con el PerfilController para el cambio de contraseña y la edición de datos del usuario.

9) 10/11/25
 - ERROR: En una de las pruebas de hoy estaba usando dos usuarios a la vez en dos ordenadores diferentes, y al momento de hacer pruebas de editar y eliminar perfiles ocurrió que uno de los perfiles no me dejó eliminarlo 
 - CAUSA: Esataba borrando un usuario con su id asociada a la tabla de movimientos
 - SOLUCIÓN: Controlé ese error y mando un mensaje conforme no se puede borrar por tener un movimiento asociado
 - SOLUCIÓN 2: Decicí añadir un segundo control. A  parte de poder eliminar usuarios, el usuario "jefe" va a poder activar o desactivar usuarios, de forma que puedo desactivar un usuario para que no pueda entrar y que eso no afecte a sus movimientos registrados. De todas formas se van a poder seguir eliminando usuarios 

***

## Cursos y Documentación de Referencia

### Cursos

1) Curso sobre asp.Net con MVC: https://www.youtube.com/watch?v=28LjewDjaz4&t=3458s  
2) Segundo curso sobre Asp.Net con MVC: https://www.youtube.com/watch?v=16N5OhcrLws&list=PLx2nia7-PgoDptcrh4k4ZStjpVLZbS7rU&index=1  
3) Identity: https://www.youtube.com/watch?v=0PZpjDYT0ZQ&t=1129s
4) Autenticación: https://www.youtube.com/watch?v=IvoDzgrjMOY
5) Bootstrap 5: https://www.youtube.com/watch?v=QCw0L6FupQ0

### Documentación

1) Información general ASP: https://learn.microsoft.com/es-es/aspnet/core/mvc/overview?view=aspnetcore-8.0
2) Introducción ASP: https://learn.microsoft.com/es-es/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-8.0&tabs=visual-studio
3) Sintaxis Razor: https://learn.microsoft.com/es-es/aspnet/core/mvc/views/razor?view=aspnetcore-8.0
4) Entity Framework: https://learn.microsoft.com/es-es/ef/core
5) Introducción EF Core: https://learn.microsoft.com/es-es/ef/core/get-started/overview/first-app?tabs=netcore-cli
6) Migraciones: https://learn.microsoft.com/es-es/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli
7) Identity: https://learn.microsoft.com/es-es/aspnet/core/security/authentication/identity?view=aspnetcore-9.0&tabs=visual-studio
8) Roles en ASP: https://learn.microsoft.com/es-es/aspnet/core/security/authorization/roles?view=aspnetcore-8.0
9) Controladores: https://learn.microsoft.com/es-es/aspnet/core/mvc/controllers/actions?view=aspnetcore-9.0
10) Sintaxis Razor: https://learn.microsoft.com/es-es/aspnet/core/mvc/views/razor?view=aspnetcore-9.0


