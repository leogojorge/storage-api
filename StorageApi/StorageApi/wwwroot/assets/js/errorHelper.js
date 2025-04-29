function showErrors(errors) {
    const errorsContainer = document.getElementById('errorContainer');
    errorsContainer.innerHTML = '';

    const ul = document.createElement('ul');
    errors.forEach(error => {
        const li = document.createElement('li');
        li.textContent = error;
        ul.appendChild(li);
    });

    errorsContainer.appendChild(ul);
    errorsContainer.style.display = 'block';
}

function clearErros() {
    const errorsContainer = document.getElementById('errorContainer');

    errorsContainer.innerHTML = '';
    errorsContainer.style.display = 'nome';
}