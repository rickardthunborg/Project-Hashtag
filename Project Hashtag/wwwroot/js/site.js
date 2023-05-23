function autoExpand(textarea) {
        textarea.style.height = "auto";
        textarea.style.height = textarea.scrollHeight + "px";
}

const fileInput = document.getElementById('photo-input');
const fileLabel = document.getElementById('file-label');
let liCounter = 0;


//function GetAd(tag)
//{
//    let post = document.querySelector("ul.flow > li:nth-child(" + liCounter + ")");

//    let url = `https://laboutique.azurewebsites.net/api/Product/GetByName?name=${tag}`

//    let response = await fetch(url);
//    let json = await response.json();   

//    let URL = json.URL;

//    if (json == null)
//    {
//        liCounter++;
//        return;
//    }

//    let adSpace = post.createElement('div');
//    adSpace.setAttribute("id", "adDiv");

//    let adSpaceText = post.createElement('p')
//    adSpace.appendChild(adSpaceText)

//    liCounter++;

//}

var notificationButton = document.querySelector('#notificationButton')
var notificationList = document.querySelector('#notificationList')

notificationButton.addEventListener('click', function () {
    notificationList.classList.toggle('hidden')
});

//document.addEventListener('click', function (event) {
//     var targetElement = event.target;
//     if (targetElement !== notificationButton && !notificationList.contains(targetElement)) {
//     notificationList.classList.add('hidden');
//        }
//    });


if (fileInput != null) {
    fileInput.addEventListener('change', function () {
        if (fileInput.files.length > 0) {
            const checkmark = document.createElement('img');
            checkmark.src = '../done.png';
            checkmark.alt = 'Done';
    
            // Append the checkmark image
            fileLabel.appendChild(checkmark);
        }
    });
}



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

var banderoll2 = document.getElementById("banderoll2")
var sticky2 = banderoll2.offsetTop

function stickBanderoll() {
    if (window.pageYOffset > (sticky - (window.innerHeight * 0.1 ))) {
        header.classList.add("stuck");
    } else {
        header.classList.remove("stuck");
    }

    if (window.pageYOffset >= sticky2 - (window.innerHeight * 0.1) ) {
        banderoll2.classList.add("stuck");
        header.classList.add("hidden");
      } else {
        banderoll2.classList.remove("stuck");
        header.classList.remove("hidden")
      }
}


