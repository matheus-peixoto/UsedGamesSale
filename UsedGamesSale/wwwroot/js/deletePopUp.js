document.addEventListener('DOMContentLoaded', () => {
    let overlay = document.getElementById('overlay')
    let overlayCard = document.getElementById('overlay-card')
    let deleteBtn = document.getElementById('deleteBtn')
    let cancelBtn = document.getElementById('cancelBtn')
    let deleteLinks = document.querySelectorAll('.deleteLink')
    let info = document.getElementById('info')
    let redirectLocation = ''

    deleteLinks.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault()
            redirectLocation = e.target.href
            appearPopUp()
            let gameNameEl = e.target.parentElement.parentElement.querySelector('.game-name')
            info.innerText = gameNameEl.innerText
        })
    });

    cancelBtn.addEventListener('click', deseappearPopUp)

    deleteBtn.addEventListener('click', () => {
        deseappearPopUp()
        window.location.href = redirectLocation
    })

    overlay.addEventListener('click', deseappearOverlay)

    function appearPopUp() {
        overlayCard.classList.remove('display-none')
        overlay.classList.remove('display-none')
    }

    function deseappearPopUp() {
        overlayCard.classList.add('display-none')
        overlay.classList.add('display-none')
    }

    function deseappearOverlay() {
        overlayCard.classList.add('display-none')
        overlay.classList.add('display-none')
    }
})

