function autoExpand(textarea) {
        textarea.style.height = "auto";
        textarea.style.height = textarea.scrollHeight + "px";
}

const fileInput = document.getElementById('photo-input');
const fileLabel = document.getElementById('file-label');

fileInput.addEventListener('change', function () {
    if (fileInput.files.length > 0) {
        const checkmark = document.createElement('img');
        checkmark.src = '../done.png';
        checkmark.alt = 'Done';

        // Append the checkmark image
        fileLabel.appendChild(checkmark);
    }
});