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


//Below is code for popup on report button

var popupButtons = document.querySelectorAll('.popupButton');
var popupForms = document.querySelectorAll('.popupForm');
var closeButtons = document.querySelectorAll('#closeButton');


// Attach event listeners to each popup button
popupButtons.forEach(function (button, index) {
    button.addEventListener('click', function () {
        popupForms[index].style.display = 'grid'; // Show the corresponding popup form
    });
});

closeButtons.forEach(function (button) {
    button.addEventListener('click', function () {
        var popupForm = button.closest('.popupForm');
        popupForm.style.display = 'none'; // Hide the parent popup form
    });
});


window.onscroll = function () { stickBanderoll() };
var header = document.getElementById("banderoll1");
var sticky = header.offsetTop;

function stickBanderoll() {
    if (window.pageYOffset > (sticky - (window.innerHeight * 0.1 - 4))) {
        header.classList.add("stuck");
    } else {
        header.classList.remove("stuck");
    }
}
