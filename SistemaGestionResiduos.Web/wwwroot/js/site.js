// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//Obteniendo html 
let rutas_dashboard = document.querySelector("#rutas")
let colecciones_dashboard = document.querySelector("#contenedores")
let recolecciones_dashboard = document.querySelector("#recolecciones")
let total_recolectado_dashboard = document.querySelector("#total_recolectado")

async function obtener_datos_get(url) {
    try {
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error(`Response status: ${response.status}`);
      }
  
      const json = await response.json();
      console.log(json);
    } catch (error) {
      console.error(error.message);
    }
  }

  async function obtener_datos_get(url) {
    try {
      const response = await fetch(url);
      if (!response.ok) {
        throw new Error(`Response status: ${response.status}`);
      }
      const json = await response.json();
      return json; // Se retorna el JSON obtenido
    } catch (error) {
      console.error(error.message);
      return null;
    }
  }
  
 
  (async () => {
    let rutas = await obtener_datos_get("http://localhost:5011/api/rutas");
    let recolecciones = await obtener_datos_get("http://localhost:5011/api/recolecciones");
    let contenedores = await obtener_datos_get("http://localhost:5011/api/contenedores");
    let recolectado;
    

    rutas_dashboard.textContent = rutas.length;
    colecciones_dashboard.textContent = contenedores.length;
    recolecciones_dashboard.textContent = recolecciones.length;
     
    let total_recolecciones = recolecciones.reduce((acumulador, recoleccion) => {
        return acumulador + recoleccion.pesoRecolectado;
    }, 0);
    
    total_recolectado_dashboard.textContent = `${total_recolecciones} kg`
    
   
  })();
  