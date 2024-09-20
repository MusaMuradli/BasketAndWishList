//console.log("salam")
//const addToBasketButtons = document.querySelectorAll(".addToBasket");
//const basketArea = document.querySelector(".basketPartial")

//addToBasketButtons.forEach(btn => {
//    btn.addEventListener('click', async (e) => {
//        e.preventDefault()
//        const response = await fetch(btn.href);
//        const partial = await response.text();
//        basketArea.innerHTML = _basketPartial;
//    })

//})

fetch('https://api.example.com/data')
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.json(); // Parse JSON data
    })
    .then(data => {
        console.log(data); // Handle the received data
    })
    .catch(error => {
        console.error('There was a problem with the fetch operation:', error);
    });