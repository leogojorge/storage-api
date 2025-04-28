const startCameraBtn = document.getElementById('startCamera');
const video = document.getElementById('video');
const canvas = document.getElementById('canvas');
const preview = document.getElementById('preview');
const takePhotoBtn = document.getElementById('takePhoto');
const cameraContainer = document.getElementById('cameraContainer');

let stream;
let imageBlob;
const fieInput = document.getElementById('fileInput');

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