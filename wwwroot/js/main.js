
const swiper = new Swiper('.swiper', {
    slidesPerView: 'auto',
    spaceBetween: 30,
});

const productContainers = [...document.querySelectorAll('.product-content')];
const nxtBtn = [...document.querySelectorAll('.right-btn')];
const preBtn = [...document.querySelectorAll('.left-btn')];
productContainers.forEach((item, i) => {
    let containerDimensions = item.getBoundingClientRect();
    let containerWidth = containerDimensions.width;

    nxtBtn[i].addEventListener('click', () => {
        item.scrollLeft += containerWidth;
    })

    preBtn[i].addEventListener('click', () => {
        item.scrollLeft -= containerWidth;
    })
})

document.querySelectorAll('.name').forEach((element) => {
    const textLength = element.textContent.trim().length;

    if (textLength <= 25) {
        element.style.fontSize = '1rem';
        element.style.fontWeight = '400';
    } else {
        element.style.fontSize = '0.8rem';
    }
});

let darkModeEnabled = false;
document.getElementById('darkModeToggle').addEventListener('click', () => {
    if (darkModeEnabled) {
        DarkReader.disable();
    } else {
        DarkReader.enable({
            brightness: 100,
            contrast: 100,
            sepia: 10
        });
    }
    darkModeEnabled = !darkModeEnabled;
});

function replaceClass() {
    const icon = document.getElementById("myIcon");

    if (icon.classList.contains("fa-sun")) {
        icon.classList.remove("fa-sun");
        icon.classList.add("fa-moon");
    } else {
        icon.classList.remove("fa-moon");
        icon.classList.add("fa-sun");
    }
}

const searchIcon = document.querySelector('.search-icon'); 
const searchModal = document.querySelector('#searchModal');

searchIcon.addEventListener('click', () => {
    searchModal.style.display = 'block';
});

searchModal.addEventListener('click', (e) => {
    if (e.target === searchModal) {
        searchModal.style.display = 'none'; 
    }
});
