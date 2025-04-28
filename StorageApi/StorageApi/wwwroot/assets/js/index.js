const form = document.getElementById('itemForm');

form.addEventListener('submit', async (e) => {
    e.preventDefault();
    const formData = new FormData(form);

    if (imageBlob) {
        formData.append('picture', imageBlob, 'foto.jpg');
    }
    if (!imageBlob && fileInput.files.length > 0) {
        formData.append('picture', fileInput.files[0]); // mesmo nome do backend
    }

    try {

        const response = await fetch('/items', {
            method: 'POST',
            body: formData,
            credentials: 'include'
        });

        if (!response.ok) throw new Error('Erro ao cadastrar item');

        alert('Item cadastrado com sucesso!');
        form.reset();
        preview.style.display = 'none';
        if (stream) stream.getTracks().forEach(track => track.stop());
    } catch (err) {
        alert('Erro: ' + err.message);
    }
});