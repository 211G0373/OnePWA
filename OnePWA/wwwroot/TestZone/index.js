// Verificar autenticación
const token = localStorage.getItem('jwtToken');
if (!token) {
    window.location.href = 'login.html';
}
else {
    // Configurar headers para las requests
    const headers = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
    };



    const botonCerrarSesion = document.getElementById('logoutBtn');

    // Cargar tareas al iniciar
    document.addEventListener('DOMContentLoaded', () => {
     
        cargarDatosUsuario();
    });


    function obtenerDatosDelToken() {
        console.log("d");

        const token = localStorage.getItem('jwtToken');
        console.log(token);
        const payload = token.split('.')[1];
        console.log(payload);


        const decoded = JSON.parse(atob(payload));

        console.log(decoded);
        return decoded;
    }

    function cargarDatosUsuario() {
        const datos = obtenerDatosDelToken();
        document.getElementById('name').textContent = datos.unique_name;
        document.getElementById('email').textContent = datos.email;
        document.getElementById('id').textContent = datos.nameid;
    }



    botonCerrarSesion.addEventListener("click", function () {
        cerrarSesion();
    })
    function cerrarSesion() {
        localStorage.removeItem('jwtToken');
        window.location.href = 'login.html';
    }







}