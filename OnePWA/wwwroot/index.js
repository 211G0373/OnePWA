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
        const token = localStorage.getItem('jwtToken');
        const payload = token.split('.')[1];
        const decoded = JSON.parse(atob(payload));
        return decoded;
    }

    function cargarDatosUsuario() {
        const datos = obtenerDatosDelToken();
        document.getElementById('name').textContent = datos.Nombre;
        document.getElementById('email').textContent = datos.Email;
        document.getElementById('id').textContent = datos.Id;
    }



    botonCerrarSesion.addEventListener("click", function () {
        cerrarSesion();
    })
    function cerrarSesion() {
        localStorage.removeItem('jwtToken');
        window.location.href = 'login.html';
    }







}