document.getElementById('loginForm').addEventListener('submit', async function(event) {
    event.preventDefault(); // Evitar que el formulario se envíe de forma predeterminada

    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    const response = await fetch('https://localhost:7080/api/User/ValideUserLogin', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ Email: email, Password: password }),
    });

    if (response.ok) {
        const data = await response.json(); // Convertir a JSON

        // Verificar si el token existe en el JSON
        if (data.token) {
            // Almacenar el token en localStorage
            localStorage.setItem('token', data.token);
            console.log("Token almacenado: ", localStorage.getItem('token'));

            // Redirigir después de almacenar el token
            window.location.href = '/';  // Redirige a la página de inicio
        } else {
            document.getElementById('message').textContent = "Error al obtener el token.";
        }
    } else {
        const errorMessage = await response.text(); // Si hay error, obtener el texto de la respuesta
        document.getElementById('message').textContent = errorMessage || "Error en el inicio de sesión.";
    }
});
