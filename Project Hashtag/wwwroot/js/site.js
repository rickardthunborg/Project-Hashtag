function autoExpand(textarea) {
        textarea.style.height = "auto";
        textarea.style.height = textarea.scrollHeight + "px";
}

const fileInput = document.getElementById('photo-input');
const fileLabel = document.getElementById('file-label');


var notificationButton = document.querySelector('#notificationButton')
var notificationList = document.querySelector('#notificationList')

notificationButton.addEventListener('click', function () {
    notificationList.classList.toggle('hidden')
});



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



//Javascript to stick stick the banderolls to the top
window.onscroll = function () { stickBanderoll() };
var header = document.getElementById("banderoll1");
var sticky;

if (header) {
  sticky = header.offsetTop;
}

var banderoll2 = document.getElementById("banderoll2")
var sticky2;

if (banderoll2) {
  sticky2 = banderoll2.offsetTop;
}

function stickBanderoll() {
    if (header) {

      if (window.pageYOffset > (sticky - (window.innerHeight * 0.1 ))) {
          header.classList.add("stuck");
      } else {
          header.classList.remove("stuck");
      }
    }
    if (banderoll2){
      if (window.pageYOffset >= sticky2 - (window.innerHeight * 0.1) ) {
          banderoll2.classList.add("stuck");
          header.classList.add("hidden");
        } else {
          banderoll2.classList.remove("stuck");
          header.classList.remove("hidden")
        }
    }
}




//Scroll funtion for the timeline
var scrollButton = document.getElementById("scrollButton");

if (scrollButton){
  
  scrollButton.addEventListener("click", function() {
    scrollToBottom();});
  
    function scrollToBottom() {
      var scrollTo = document.documentElement.scrollHeight - window.innerHeight;
      scrollToPosition(scrollTo, document.documentElement.scrollHeight * 3.5);
    }
    
    function scrollToPosition(to, duration) {
      if (duration <= 0) return;
    
      var difference = to - window.scrollY;
      var perTick = (difference / duration) * 10;

      setTimeout(function() {
        window.scrollTo(0, window.scrollY + perTick);
        if (window.scrollY === to) return;
        scrollToPosition(to, duration - 10);
      }, 10);
  }

}



//Code for showing the posts in the timeline
const contentDivs = document.querySelectorAll('.animatePost');

window.addEventListener('scroll', showBoxesInView);

function elementInView(element) {
  const rect = element.getBoundingClientRect();
  const windowHeight = window.innerHeight || document.documentElement.clientHeight;

  return (rect.top <= windowHeight) && ((rect.top + rect.height) >= 0);
}
 

function showBoxesInView() {
  for (let div of contentDivs) {
    if (elementInView(div)) {
      div.classList.add('showing');
    }
  }
}


document.addEventListener('DOMContentLoaded', function() {
  var liElements = document.querySelectorAll('.flow li');
  liElements.forEach(function(li) {
    var tag = li.getAttribute('data-tag');
    LoadAd(tag, li, true);
  });
});

async function LoadAd(tag, li, fakeApi) {

    let url = `https://laboutique.azurewebsites.net/api/Product/GetByName?name=${tag}`

    let response;
    let json;

    let description;
    let productID; 
    let price;
    let stock; 
    let link;
    let imageLink; 
    
    if (!fakeApi)
    {
      try {
          response = await fetch(url);
          json = await response.json();
      }
      catch (error) {
          console.log(error)
          return;
      }
    }
      else {
        json = jsonData.find(j => j.productName === tag);

        if (!json){
          return;
        }

        description = json.description;
        productID = json.productID;
        price = json.price;
        stock = json.stock;
        link = `https://www.example.com`;
        imageLink = json.imageLink;
    }

    

    let adSpace = li.querySelector('#adSpace');
    adSpace.classList.add('adVisible')
    adSpace.setAttribute("href", imageLink)

    let adPrompt = document.createElement('div')
    let text = document.createElement('p')
    text.textContent = "Ad:"
    adPrompt.appendChild(text)
    adPrompt.classList.add("adPromptBox")
    adSpace.appendChild(adPrompt)

    let image = document.createElement("img")
    image.src = imageLink;
    image.classList.add("adImage")
    adSpace.appendChild(image);


    let adSpaceContainer = document.createElement('div')
    adSpaceContainer.classList.add('adText')
    let adSpaceText = document.createElement('p')
    adSpaceText.classList.add('adTextMain')
    adSpaceText.textContent = description;
    let priceText = document.createElement('p');
    priceText.textContent = "Price: " + price + " SEK";

    let stockText = document.createElement('p');
    stockText.textContent = "Stock: " + stock;

    adSpaceContainer.appendChild(adSpaceText);
    adSpaceContainer.appendChild(priceText);
    adSpaceContainer.appendChild(stockText);

    adSpace.appendChild(adSpaceContainer);
}


const jsonData = [
  {
    "productName": "apple",
    "description": "Delicious Green Apple",
    "productID": 111111,
    "imageLink": "https://healthiersteps.com/wp-content/uploads/2021/12/green-apple-benefits.jpeg",
    "price": 20,
    "stock": 2031
  },
  {
    "productName": "banana",
    "description": "Sweet Yellow Banana",
    "productID": 222222,
    "imageLink": "https://upload.wikimedia.org/wikipedia/commons/thumb/8/8a/Banana-Single.jpg/2324px-Banana-Single.jpg",
    "price": 15,
    "stock": 415
  },
  {
    "productName": "orange",
    "description": "Juicy Orange",
    "imageLink": "https://upload.wikimedia.org/wikipedia/commons/thumb/c/c4/Orange-Fruit-Pieces.jpg/1200px-Orange-Fruit-Pieces.jpg",
    "productID": 333333,
    "price": 10,
    "stock": 32
  }
];