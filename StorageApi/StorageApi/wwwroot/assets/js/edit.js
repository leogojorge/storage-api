const form = document.getElementById('itemForm');

const params = new URLSearchParams(window.location.search);
const itemId = params.get('id');

if (!itemId) {
    alert('ID do item não fornecido.');
    window.location.href = 'search.html';
}

async function loadItem() {
    try {
        const response = await fetch(`https://localhost:7082/items/${itemId}`, {
            credentials: 'include'
        });

        if (!response.ok) throw new Error('Erro ao buscar item');

        const item = await response.json();

        form.name.value = item.name || '';
        form.partNumber.value = item.partNumber || '';
        form.category.value = item.category || '';
        form.place.value = item.place || '';
        form.description.value = item.description || '';
        form.supplier.value = item.supplier || '';
        form.quantity.value = item.quantity || 0;

        if (item.picture) {
            const img = document.getElementById('preview');
            img.src = `data:image/png;base64,${item.picture || ''}`; // fallback para vazio

            preview.style.display = 'block';
        }
    } catch (err) {
        alert('Erro: ' + err.message);
    }
}

form.addEventListener('submit', async (e) => {
    e.preventDefault();
    const formData = new FormData();

    formData.append('name', form.name.value);
    formData.append('partNumber', form.partNumber.value);
    formData.append('category', form.category.value);
    formData.append('place', form.place.value);
    formData.append('description', form.description.value);
    formData.append('supplier', form.supplier.value);
    formData.append('quantity', form.quantity.value);

    if (imageBlob) {
        formData.append('picture', imageBlob, 'foto.jpg');
    }
    if (!imageBlob && fileInput.files.length > 0) {
        formData.append('picture', fileInput.files[0]); // mesmo nome do backend
    }

    try {
        const response = await fetch(`https://localhost:7082/items/${itemId}`, {
            method: 'PUT',
            body: formData,
            credentials: 'include'
        });

        if (!response.ok) throw new Error('Erro ao salvar alterações');

        alert('Item atualizado com sucesso!');
        //window.location.href = 'busca.html'; // voltar para tela de busca
    } catch (err) {
        alert('Erro: ' + err.message);
    }
});

loadItem();
