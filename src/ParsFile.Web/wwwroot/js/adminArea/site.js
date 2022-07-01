const searchForm = document.querySelector(".search-form");
const searchInput = document.querySelector(".search-input");
const searchSubmit = document.querySelector(".search-submit");

searchInput.addEventListener("keyup", (event) => {

    if (event.key === "Enter") {
        searchSubmit.click();
    }

    const response = sendRequest(searchInput.value)
    response.then(value => {

        removeOldChildren();

        value.data.forEach((file , index) => {
            addOption(file , index);
        })
    })
})

const sendRequest = async (filter) => {
    const response = await fetch(`/Admin/File/SearchFilter?filter=${filter}`);
    return response.json()
}

const removeOldChildren = () => {
    Array.from(searchForm.children).forEach(child => {
        if (child.classList.contains("search-option")) {
            child.remove();
        }
    })
}

const addOption = (file , index) => {
    const container = document.createElement("a");
    container.setAttribute("class", "search-option");
    container.setAttribute("href", `/Admin/File/Show/${file.id}`);
    container.setAttribute("style" , `top : calc(150% + ${index * 40}px);`)

    const image = document.createElement("img");
    image.setAttribute("src", `/file/user files/images/${file.image}`);

    const title = document.createElement("p");
    title.innerHTML = file.name;

    container.appendChild(image)
    container.appendChild(title)

    searchForm.appendChild(container);
}