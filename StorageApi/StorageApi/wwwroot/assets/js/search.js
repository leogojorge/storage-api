const searchResult = document.getElementById('searchResult');
const searchForm = document.getElementById('searchForm');
const searchInput = document.getElementById('searchInput');
const currentPageSpan = document.getElementById('currentPage');
const prevPageBtn = document.getElementById('prevPage');
const nextPageBtn = document.getElementById('nextPage');

let pageNumber = 1;
let pageSize = 100;
let currentSearch = "";

async function fetchItems(nameAndDescription = "", page = 1) {
    try {
        const response = await fetch(`/items?NameAndDescription=${encodeURIComponent(nameAndDescription)}&PageNumber=${page}&PageSize=${pageSize}`,
        {
            credentials: 'include'
        });
        const data = await response.json();
        renderItems(data.items);
        updatePagination(data.pageNumber);
    } catch (error) {
        console.error("Erro ao buscar itens:", error);
    }
}

function renderItems(items) {
    searchResult.innerHTML = ""; // limpa antes de renderizar
    if (!items.length) {
        searchResult.innerHTML = "<p style='text-align:center;'>Nenhum item encontrado.</p>";
        return;
    }

    items.forEach(item => {
        //cria link para editar item. por em css?
        const link = document.createElement('a');
        link.href = `edit.html?itemId=${item.id}`;
        link.className = 'itemLink';
        link.style.textDecoration = 'none';
        link.style.color = 'inherit';
        //
        const itemDiv = document.createElement('div');
        itemDiv.className = 'itemContent';

        const img = document.createElement('img');
        img.src = `data:image/png;base64,${item.picture || ''}`;

        const infoDiv = document.createElement('div');
        infoDiv.className = 'infoItem';

        const textDiv = document.createElement('div');
        textDiv.className = 'textInfo';
        textDiv.innerHTML = `
            <p>${item.name}</p>
            <p>${item.place}</p>
        `;

        const deleteBtn = document.createElement('button');
        deleteBtn.className = 'btnDelete';
        deleteBtn.innerHTML = `<i class="fa fa-trash"></i>`;

        // evita navegação para pagina de edição ao clicar no botão de deleção
        deleteBtn.addEventListener('click', (e) => {
            e.stopPropagation();
            e.preventDefault(); 
           
            const confirmDelete = confirm(`Deseja realmente excluir o item "${item.name}"?`);
            if (!confirmDelete) return;

            deleteBtn.disabled = true;

            deleteItem(item.id);
        });

        infoDiv.appendChild(textDiv);
        infoDiv.appendChild(deleteBtn);

        itemDiv.appendChild(img);
        itemDiv.appendChild(infoDiv);

        link.appendChild(itemDiv);
        searchResult.appendChild(link);

    });
}

async function deleteItem(itemId) {
    try {
        const response = await fetch(`/items/${itemId}`, {
            method: 'DELETE',
            credentials: 'include'
        });

        if (!response.ok) {
            throw new Error('Erro ao deletar item');
        }

        alert('Item deletado com sucesso!');
        fetchItems(currentSearch, pageNumber);
    } catch (error) {
        console.error('Erro ao deletar item:', error);
        alert('Erro ao deletar item. Tente novamente.');
    }
}

function updatePagination(page) {
    currentPageSpan.textContent = page;
}

searchForm.addEventListener('submit', (e) => {
    e.preventDefault();
    currentSearch = searchInput.value.trim();
    pageNumber = 1;
    fetchItems(currentSearch, pageNumber);
});

prevPageBtn.addEventListener('click', () => {
    if (pageNumber > 1) {
        pageNumber--;
        fetchItems(currentSearch, pageNumber);
    }
});

nextPageBtn.addEventListener('click', () => {
    pageNumber++;
    fetchItems(currentSearch, pageNumber);
});

// Carregar itens na primeira vez que abrir a página
window.addEventListener('DOMContentLoaded', () => {
    fetchItems();
});