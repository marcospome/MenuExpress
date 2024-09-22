const modeToggle = document.querySelector('.mode-toggle');
const modeIcon = document.getElementById('mode-icon');
const html = document.documentElement;

// Función para cambiar entre modo claro y oscuro
function toggleMode() {
    html.classList.toggle('dark-mode');

    // Actualizar el color del botón según el modo actual
    const currentMode = html.classList.contains('dark-mode') ? 'dark' : 'light';
    modeToggle.style.backgroundColor = `var(--bg-color-${currentMode})`;
    modeToggle.style.color = `var(--text-color-${currentMode})`;

    // Cambiar el ícono del modo
    modeIcon.src = html.classList.contains('dark-mode') 
        ? '/images/mode.png' 
        : '/images/modedarkorwhite.png';
}

modeToggle.addEventListener('click', toggleMode);

// Función asíncrona para obtener los productos
async function fetchProducts() {
    const errorMessage = document.getElementById('error-message');
    try {
        const response = await fetch('https://localhost:7080/api/Product/getAllProducts');
        if (!response.ok) {
            throw new Error('Error en la respuesta de la API');
        }
        const products = await response.json();
        displayProducts(products);
        errorMessage.style.display = 'none'; // Ocultar mensaje de error si la conexión es exitosa
    } catch (error) {
        console.error('Error:', error);
        errorMessage.style.display = 'block'; // Mostrar mensaje de error
    }
}

// Función para mostrar los productos en la página
function displayProducts(products) {
    const container = document.getElementById('products-container');
    container.innerHTML = '';

    products.forEach(product => {
        const productDiv = document.createElement('div');
        productDiv.className = 'product';
        productDiv.innerHTML = `
            <img src="${product.image}" alt="${product.name}" />
            <h2>${product.name || 'Nombre no disponible'}</h2>
            <p>${product.description || 'Descripción no disponible'}</p>
            <p>Precio: $${product.price !== undefined ? product.price : 'No disponible'}</p>
        `;
        container.appendChild(productDiv);
    });
}

// Llamar a la función para obtener los productos al cargar la página
fetchProducts();
