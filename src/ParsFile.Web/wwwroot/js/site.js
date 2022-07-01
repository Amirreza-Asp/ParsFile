const btnHamburgerMenu = document.querySelector(".hamburger-menu");
const overlay = document.querySelector(".overlay");
const nav = document.querySelector("nav");

btnHamburgerMenu.addEventListener("click", () => {
    btnHamburgerMenu.classList.toggle("clicked");
    overlay.classList.toggle("open");
    nav.classList.toggle("hidden");
})