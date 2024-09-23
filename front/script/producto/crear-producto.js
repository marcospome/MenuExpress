document.addEventListener('DOMContentLoaded', async () => {
    await fetchCategories();
});

async function fetchCategories() {
    const token = localStorage.getItem('token'); // Obtener el token almacenado
    try {
        const response = await fetch('https://localhost:7080/api/Category/getAllCategories', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`, // Agregar el token al header de la solicitud
                'Content-Type': 'application/json'
            }
        });
        
        if (!response.ok) {
            throw new Error('Error al obtener categorías');
        }
        
        const categories = await response.json();
        populateCategorySelect(categories);
    } catch (error) {
        console.error('Error:', error);
        document.getElementById('error-message').style.display = 'block';
    }
}

function populateCategorySelect(categories) {
    const categorySelect = document.getElementById('idCategory');
    categories.forEach(category => {
        const option = document.createElement('option');
        option.value = category.idCategory;
        option.textContent = category.name;
        categorySelect.appendChild(option);
    });
}

// Código para manejar el envío del formulario
document.getElementById('product-form').addEventListener('submit', async function (event) {
    event.preventDefault();
    
    const token = localStorage.getItem('token'); // Obtener el token almacenado
    const product = {
        Name: document.getElementById('name').value,
        Deleted: 0,
        Description: document.getElementById('description').value,
        Price: parseFloat(document.getElementById('price').value),
        AddDate: new Date().toISOString(),
        Image: document.getElementById('image').value,
        IdCategory: parseInt(document.getElementById('idCategory').value)
    };

    try {
        const response = await fetch('https://localhost:7080/api/Product/CreateProduct', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`, // Agregar el token al header de la solicitud
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(product)
        });

        if (!response.ok) {
            throw new Error('Error en la creación del producto');
        }

        document.getElementById('success-message').style.display = 'block';
        document.getElementById('error-message').style.display = 'none';
        document.getElementById('product-form').reset();

    } catch (error) {
        console.error('Error:', error);
        document.getElementById('error-message').style.display = 'block';
        document.getElementById('success-message').style.display = 'none';
    }
    console.log(product);

});
