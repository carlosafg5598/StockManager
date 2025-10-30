# StockManager

## Orden de subidas

### 1) 

1) Lista de las tareas pendientes de la primera semana (voy tarde)
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

**Dudas en el proyecto:**  Después de comprobar todo no tengo del todo claro la función de la vista de informes que metí en el anteproyecto.


***

## Errores a lo largo del proyecto
1) 10/10/25
A la hora de hacer la gestión de los moviminetos, conecta bien con la base de datos, pero a la hora de registrar el movimiento falla debido a la clave foraneo del producto
2) 13/10/25
Para poder realizar el control de los movimientos, decidí implementar ya el identity de los usuarios, pero eso me dió errores a lo largo de la mañana (conseguí solventarlo a la noche)
3) 14/10/25
Después de gestionar las vistas me vi con la problemática de que puedo acceder a la vista de cada uno de los diferentes roles sin iniciar sesión
4) 30/10/25
Después de crear la vista para editar perfiles y probar los diferentes apartados que se pueden modificar veo que las fuentes no se modifican, sin embargo si me registra el cambio

## Cursos Referencia

1) Curso sobre asp.Net con MVC: https://www.youtube.com/watch?v=28LjewDjaz4&t=3458s  (curso visto)
2) Segundo curso sobre Asp.Net con MVC: https://www.youtube.com/watch?v=16N5OhcrLws&list=PLx2nia7-PgoDptcrh4k4ZStjpVLZbS7rU&index=1  (curso visto)


