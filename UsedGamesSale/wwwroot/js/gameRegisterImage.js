document.addEventListener('DOMContentLoaded', () => {
    const defaultImagePath = '/img/default-img.png'
    let imgContainers = document.querySelectorAll('.img-container')

    addUploadTempImg()
    addDeleteTempImg()

    function addUploadTempImg() {
        let imgs = []
        let inputs = []
        imgContainers.forEach(i => imgs.push(i.querySelector('img')))
        imgContainers.forEach(i => inputs.push(i.parentElement.querySelector('input')))

        imgs.forEach(img => {
            img.addEventListener('click', () => {
                if (!img.src || img.src.endsWith(defaultImagePath))
                    img.parentElement.parentElement.querySelector('input').click()
            })
        })

        inputs.forEach(input => {
            input.addEventListener('change', () => {
                let url = new URL(window.location.href)
                url.pathname = 'Seller/Game/UploadTempImage'
                let request = makeRequest('POST', url)
                let form = makeForm(input)
                sendUploadRequest(request, form, input)
            })
        })
    }

    function addDeleteTempImg() {
        let btns = []
        imgContainers.forEach(i => btns.push(i.querySelector('button')))

        btns.forEach(btn => {
            btn.addEventListener('click', () => {
                let img = btn.parentElement.querySelector('img')

                let url = new URL(window.location.href)
                url.pathname = 'Seller/Game/DeleteTempImage'
                let imgPath = getImgPath(img)
                console.log('Img path = ', imgPath)
                url.searchParams.set('imgPath', imgPath)

                if (confirm("Make the request?")) {
                    let request = makeRequest('GET', url)
                    sendDeleteTempImgRequest(request)

                    request.onload = () => {
                        deleteTempImgResponseHandler(request, img)
                    }
                }
            })
        })
    }

    function getImgPath(img) {
        return new URL(img.src).pathname.replaceAll('%20', ' ')
    }

    function makeRequest(method, url) {
        let request = new XMLHttpRequest()
        request.responseType = 'json'
        request.open(method, url, true)
        return request
    }

    function makeForm(input) {
        let form = new FormData()
        form.append('img', input.files[0])
        return form
    }

    function sendUploadRequest(request, form, input) {
        request.send(form)

        request.onload = () => {
            uploadResponseHandler(request, input)
        }
    }

    function sendDeleteTempImgRequest(request, input) {
        request.send()

        request.onload = () => {
            deleteTempImgResponseHandler(request, input)
        }
    }

    function uploadResponseHandler(request, input) {
        if (request.status == 200) {
            let imgContainer = input.parentElement.querySelector('.img-container');

            let label = imgContainer.parentElement.querySelector('label')
            label.classList.add('display-none')

            let img = imgContainer.querySelector('img')
            img.src = request.response.imgPath
            img.classList.add('cursor-default')

            let btn = imgContainer.querySelector('button')
            btn.classList.remove('display-none')
        }

        console.log(request.response)
    }

    function deleteTempImgResponseHandler(request, img) {
        if (request.status == 200) {
            img.src = defaultImagePath
            img.classList.remove('cursor-default')

            let input = img.parentElement.parentElement.querySelector('input')
            input.value = ''

            let label = img.parentElement.parentElement.querySelector('label')
            label.classList.remove('display-none')

            let btn = img.parentElement.querySelector('button')
            btn.classList.add('display-none')
        }

        console.log(request.response)
    }
})