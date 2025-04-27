fetch('assets/components/header.html')
    .then(response => response.text())
    .then(data => {
        document.getElementById('headerContainer').innerHTML = data;

        // Ativar o menu hamburguer
        const menuIcon = document.getElementById('menuIcon');
        const nav = document.getElementById('nav');

        if (menuIcon && nav) {
            menuIcon.addEventListener('click', () => {
                nav.classList.toggle('active');
            });
        }

        // Atualizar a logo com o título da página
        const logo = document.getElementById('logo');
        if (logo) {
            logo.textContent = document.title;
        }
    })
    .catch(error => console.error('Erro ao carregar o header:', error));

