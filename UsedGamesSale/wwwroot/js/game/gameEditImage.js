document.addEventListener('DOMContentLoaded', () => {
    let imgContainers = document.querySelectorAll('.img-container')

    addUploadTempImg()

    function addUploadTempImg() {
        let imgs = []
        let inputs = []
        imgContainers.forEach(i => imgs.push(i.querySelector('img')))
        imgContainers.forEach(i => inputs.push(i.parentElement.querySelector('input')))

        imgs.forEach(img => {
            img.addEventListener('click', () => img.parentElement.parentElement.querySelector('input').click())
        })

        inputs.forEach(input => {
            input.addEventListener('change', () => {
                let url = new URL(window.location.href)
                url.pathname = 'Seller/Game/ChangeImage'
                let request = makeRequest('POST', url)
                let img = input.parentElement.querySelector('img')
                let gameId = document.getElementById('gameId').value
                let form = makeForm(input, img, gameId)
                sendChangeRequest(request, form, input)
            })
        })
    }

    function getImgPath(img) {
        return new URL(img.src).pathname.replaceAll('%20', ' ')
    }

    function getImgId(img) {
        let id = 0
        let strId = img.getAttribute('data-img-id')
        if (!isNaN(strId)) id = Number.parseInt(strId)
        return id
    }

    function makeRequest(method, url) {
        let request = new XMLHttpRequest()
        request.responseType = 'json'
        request.open(method, url, true)
        return request
    }

    function makeForm(input, oldImg, gameId) {
        let form = new FormData()
        form.append('ImgFile', input.files[0])
        form.append('OldImgRelativePath', getImgPath(oldImg))
        form.append('ImgId', getImgId(oldImg))
        form.append('GameId', gameId)
        return form
    }

    function sendChangeRequest(request, form, input) {
        request.send(form)

        request.onload = () => {
            changeResponseHandler(request, input)
        }
    }

    function changeResponseHandler(request, input) {
        if (request.status == 200) {
            let imgContainer = input.parentElement.querySelector('.img-container');
            let img = imgContainer.querySelector('img')
            img.src = request.response.imgPath
        }

        console.log(request.response)
    }
})