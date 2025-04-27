const startCameraBtn = document.getElementById('startCamera');
const video = document.getElementById('video');
const canvas = document.getElementById('canvas');
const preview = document.getElementById('preview');
const takePhotoBtn = document.getElementById('takePhoto');
const cameraContainer = document.getElementById('cameraContainer');
const form = document.getElementById('itemForm');

let stream;
let imageBlob;
const fileInput = document.getElementById('fileInput');

startCameraBtn.addEventListener('click', async () => {
    try {
        stream = await navigator.mediaDevices.getUserMedia({ video: true });
        video.srcObject = stream;
        cameraContainer.style.display = 'block';
    } catch (err) {
        alert('Não foi possível acessar a câmera: ' + err.message);
    }
});

takePhotoBtn.addEventListener('click', () => {
    const context = canvas.getContext('2d');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    context.drawImage(video, 0, 0, canvas.width, canvas.height);
    canvas.toBlob((blob) => {
        imageBlob = blob;
        const url = URL.createObjectURL(blob);
        preview.src = url;
        preview.style.display = 'block';
    }, 'image/jpeg');
});

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
            body: formData
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