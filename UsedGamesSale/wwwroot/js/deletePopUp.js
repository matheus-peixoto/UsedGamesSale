document.addEventListener('DOMContentLoaded', () => {
    let overlay = document.getElementById('overlay')
    let deleteBtn = document.getElementById('deleteBtn')
    let cancelBtn = document.getElementById('cancelBtn')
    let deleteLinks = document.querySelectorAll('.deleteLink')
    let redirectLocation = ''

    deleteLinks.forEach(link => {
        link.addEventListener('click', (e) => {
            e.preventDefault()
            redirectLocation = e.target.href
            console.log(redirectLocation)
            toggleOverlay()
        })
    });

    cancelBtn.addEventListener('click', toggleOverlay)

    deleteBtn.addEventListener('click', (e) => {
        toggleOverlay()
        window.location.href = redirectLocation

    })

    function toggleOverlay() {
        overlay.classList.toggle('display-none')
    }
})

