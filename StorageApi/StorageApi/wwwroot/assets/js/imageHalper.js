const cameraSelect = document.getElementById('cameraSelect');
const video = document.getElementById('video');
const canvas = document.getElementById('canvas');
const preview = document.getElementById('preview');
const takePhotoBtn = document.getElementById('takePhoto');
const cameraContainer = document.getElementById('cameraContainer');
const activateCameraBtn = document.getElementById('activateCameraBtn');

let stream;
let imageBlob;
const fileInput = document.getElementById('fileInput');

async function loadCameras() {
    const devices = await navigator.mediaDevices.enumerateDevices();
    const videoDevices = devices.filter(device => device.kind === 'videoinput');

    cameraSelect.innerHTML = '';

    videoDevices.forEach((device, index) => {
        const option = document.createElement('option');
        option.value = device.deviceId;
        option.text = device.label || `Câmera ${index + 1}`;
        cameraSelect.appendChild(option);
    });
}

async function startSelectedCamera() {
    const deviceId = cameraSelect.value;

    try {
        if (stream) {
            stream.getTracks().forEach(track => track.stop());
        }

        stream = await navigator.mediaDevices.getUserMedia({
            video: { deviceId: { exact: deviceId } }
        });
        video.srcObject = stream;
        cameraContainer.style.display = 'block';
    } catch (err) {
        alert('Não foi possível iniciar a câmera: ' + err.message);
    }
}

async function base64ToBlob(base64, contentType = 'image/png') {
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
        byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    return new Blob([byteArray], { type: contentType });
}

activateCameraBtn.addEventListener('click', startSelectedCamera);

cameraSelect.addEventListener('change', () => {
    const deviceId = cameraSelect.value;
    startCamera(deviceId);
});

// Tirar foto
takePhotoBtn.addEventListener('click', () => {
    const context = canvas.getContext('2d');
    canvas.width = video.videoWidth;
    canvas.height = video.videoHeight;
    context.drawImage(video, 0, 0, canvas.width, canvas.height);
    canvas.toBlob((blob) => {
        imageBlob = new File([blob], 'foto.jpg', { type: 'image/jpeg' });
        const url = URL.createObjectURL(blob);
        preview.src = url;
        preview.style.display = 'block';
    }, 'image/jpeg');
});

loadCameras();