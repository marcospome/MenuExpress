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
        const response = await fetch('https://localhost:7080/api/Product/getAllProducts'); // Conexión a la API.
        if (!response.ok) {
            throw new Error('Error en la respuesta de la API');
        }
        const products = await response.json();
        displayProducts(products);
        errorMessage.style.display = 'none'; // Ocultar mensaje de error si la conexión es exitosa
    } catch (error) {
        console.error('Error:', error);
        errorMessage.textContent = error.message; // Muestra el mensaje de error
        errorMessage.style.display = 'block'; // Mostrar mensaje de error
    }
}

// Función para eliminar un artículo del carrito
function removeFromCart(button) {
    const index = button.getAttribute('data-index'); // Obtener el índice del artículo
    cart.splice(index, 1); // Eliminar el artículo del array
    updateCartDisplay(); // Renderizar el carrito de nuevo
}


// Función para mostrar los productos en la página
function displayProducts(products) {
    const container = document.getElementById('products-container');
    container.innerHTML = '';

    // Filtrar productos que no estén eliminados
    const filteredProducts = products.filter(product => product.deleted !== 1);

    filteredProducts.forEach(product => {
        const productDiv = document.createElement('div');
        productDiv.className = 'product';
        productDiv.innerHTML = `
            <img src="${product.image}" alt="${product.name}" />
            <h2>${product.name || 'Nombre no disponible'}</h2>
            <p>${product.description || 'Descripción no disponible'}</p>
            <p>Precio: $${product.price !== undefined ? product.price : 'No disponible'}</p>
            <button onclick='addToCart(${product.idProduct}, "${product.name}", ${product.price})'>Agregar al carrito</button>
        `;
        container.appendChild(productDiv);
    });
}

// Función para agregar productos al carrito
function addToCart(idProduct, name, price) {
    // Verifica que idProduct, name y price estén definidos
    if (idProduct === undefined || name === undefined || price === undefined) {
        console.error("Error: Uno o más parámetros son indefinidos", { idProduct, name, price });
        return; // Salir de la función si hay parámetros indefinidos
    }

    // Verifica que los valores no sean nulos o no válidos
    if (!idProduct || !name || !price) {
        console.error("Error: Uno o más parámetros del producto son inválidos", { idProduct, name, price });
        return; // Salir si hay valores inválidos
    }

    // Busca el producto en el carrito
    const existingProduct = cart.find(item => item.idProduct === idProduct);
    
    if (existingProduct) {
        existingProduct.qty += 1; // Incrementa la cantidad si ya existe
    } else {
        // Agrega nuevo producto al carrito
        cart.push({ idProduct, name, price, qty: 1 });
    }

    updateCartDisplay(); // Actualiza la visualización del carrito
}

// Función para actualizar la visualización del carrito
function updateCartDisplay() {
    const cartContainer = document.getElementById('cart-container');
    cartContainer.innerHTML = ''; // Limpia el contenido previo

    if (cart.length === 0) {
        cartContainer.innerHTML = '<p>El carrito está vacío.</p>';
        return;
    }

    cart.forEach((item, index) => {
        const itemDiv = document.createElement('div');
        itemDiv.className = 'cart-item';
        itemDiv.innerHTML = `
            <h3>${item.name}</h3>
            <p>Precio: $${item.price}</p>
            <p>Cantidad: ${item.qty}</p>
            <button onclick="removeFromCart(this)" data-index="${index}">Eliminar</button>
        `;
        cartContainer.appendChild(itemDiv);
    });
}


// Función para actualizar la cantidad en el carrito
function updateCartQuantity(idProduct, qty) {
    const item = cart.find(item => item.idProduct === idProduct);
    if (item) {
        item.qty = Math.max(1, qty); // Asegura que la cantidad sea al menos 1
        updateCartDisplay(); // Actualiza la visualización del carrito
    }
}

// Función para enviar la orden al backend
function sendOrder() {
    if (cart.length === 0) {
        alert('El carrito está vacío. No se puede enviar la orden.');
        return;
    }

    const orderDetails = cart.map(item => ({
        idProduct: item.idProduct,
        qty: item.qty,
        note: '' // Puedes agregar la lógica para notas aquí si es necesario
    }));

    const order = {
        clientName: 'NombreCliente',
        clientLastName: 'ApellidoCliente',
        clientDNI: '12345678',
        clientEmail: 'cliente@correo.com',
        orderDetails: orderDetails
    };

    fetch('https://localhost:7080/api/Order/CreateOrder', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(order)
    })
    .then(response => {
        if (response.ok) {
            alert('Orden enviada con éxito');
            cart = []; // Limpia el carrito después de enviar la orden
            updateCartDisplay();
        } else {
            alert('Hubo un problema al enviar la orden');
        }
    })
    .catch(error => console.error('Error:', error));
}

let cart = [];
// Llamar a la función para obtener los productos al cargar la página

function sendOrder() {
    // Abre el modal para ingresar información del cliente
    document.getElementById('orderModal').style.display = 'block';
}

function closeModal() {
    document.getElementById('orderModal').style.display = 'none';
}

function submitOrder() {
    const clientName = document.getElementById('clientName').value;
    const clientLastName = document.getElementById('clientLastName').value;
    const clientEmail = document.getElementById('clientEmail').value;
    const clientDNI = document.getElementById('clientDNI').value;

    // Valida que los campos no estén vacíos
    if (!clientName || !clientLastName || !clientEmail || !clientDNI) {
        alert('Por favor, complete todos los campos.');
        return;
    }

    if (cart.length === 0) {
        alert('El carrito está vacío. No se puede enviar la orden.');
        return;
    }

    const orderDetails = cart.map(item => ({
        idProduct: item.idProduct,
        qty: item.qty,
        note: '' // Puedes agregar lógica para notas aquí si es necesario
    }));

    const order = {
        clientName: clientName,
        clientLastName: clientLastName,
        clientDNI: clientDNI,
        clientEmail: clientEmail,
        orderDetails: orderDetails
    };

    fetch('https://localhost:7080/api/Order/CreateOrder', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(order)
    })
    .then(response => {
        if (response.ok) {
            alert('Orden enviada con éxito');
            cart = []; // Limpia el carrito después de enviar la orden
            updateCartDisplay();
            closeModal(); // Cierra el modal
        } else {
            alert('Hubo un problema al enviar la orden');
        }
    })
    .catch(error => console.error('Error:', error));
}

// Agregar el evento para cerrar el modal al hacer clic fuera de él
window.onclick = function(event) {
    const modal = document.getElementById('orderModal');
    if (event.target === modal) {
        modal.style.display = 'none';
    }
}



fetchProducts();
