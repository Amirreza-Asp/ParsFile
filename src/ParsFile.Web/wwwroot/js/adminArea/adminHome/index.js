/************** cards functionality ****************/
const detailsCard = document.querySelectorAll(".card-update");

Array.from(detailsCard).forEach((item) => {
    item.addEventListener("click", () => {
        item.classList.add("clicked")
        UpdateCard(item);
    })
})

async function UpdateCard(item) {
    let id = item.getAttribute("id");
    let valueEl = document.querySelector(`.${id}`)

    GetData(`/Admin/AdminHome/${id}`)
        .then(data => valueEl.children[0].innerText = data.toLocaleString());

    item.classList.remove("clicked")
}

async function GetData(href) {
    return await fetch(href)
        .then(data => data.json())
        .then(data => data)
}

/************** chart attribute ****************/
let xValues = []
let yValues = []

async function fetchData() {
    let url = "https://localhost:7146/Admin/AdminHome/Categories";
    const response = await fetch(url);
    let data = await response.json()

    Array.from(data).forEach(item => {
        xValues.push(item.categoryName);
        yValues.push(item.totalSale);
    })
}

(async function () {
    await fetchData();

    let colors = []
    for (let i = 0; i < xValues.length; i++) {
        colors.push(RandomColor());
    }

    let backgroundColors = []
    let borderColors = []

    colors.forEach((color) => {
        backgroundColors.push(color + "4D");
        borderColors.push(color);
    })

    const data = {
        labels: xValues,
        datasets: [{
            label: 'میزان فروش هرنوع محصول',
            data: yValues,
            backgroundColor: backgroundColors,
            borderColor: borderColors,
            borderWidth: 1
        }]
    };
    const config = {
        type: 'bar',
        data: data,
        options: {
            scales: {
                x: {
                    ticks: {
                        color: "white"
                    },
                    grid: {
                        color: "rgba(170,170,170,.2)"
                    }
                },
                y: {
                    beginAtZero: true,
                    ticks: {
                        color: "white"
                    },
                    grid: {
                        color: "rgba(170,170,170,.2)"
                    }
                }
            },
        },
    };

    const myChart = new Chart(
        document.getElementById('grouping-sales'),
        config
    ).de
})();

function RandomColor() {
    return "#" + Math.random().toString(16).substring(2, 8);
}

window.addEventListener("DOMContentLoaded", () => {
    document.querySelectorAll(".card-footer").forEach(item => {
        item.click();
    })
})