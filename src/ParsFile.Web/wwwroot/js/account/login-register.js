const swtichToLoginBtn = document.querySelector("#switch-to-login");
const swtichToSigninBtn = document.querySelector("#switch-to-register");

const loginForm = document.querySelector(".container--login");
const singinForm = document.querySelector(".container--signin");

swtichToLoginBtn.addEventListener("click", () => {
    loginForm.classList.remove("login-none-active");
    singinForm.classList.remove("signin-active");
    swtichToLoginBtn.classList.add("active");
    swtichToSigninBtn.classList.remove("active");
});

swtichToSigninBtn.addEventListener("click", () => {
    loginForm.classList.add("login-none-active");
    singinForm.classList.add("signin-active");
    swtichToSigninBtn.classList.add("active");
    swtichToLoginBtn.classList.remove("active");
});
