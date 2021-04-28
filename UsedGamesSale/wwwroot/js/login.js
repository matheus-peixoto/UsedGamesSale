document.addEventListener("DOMContentLoaded", () => {
    let showPasswordBtn = document.getElementById("showpasswordBtn");
    let passwordInput = document.getElementById("password");

    showPasswordBtn.addEventListener('click', () => {
        if (!showPasswordBtn.check) {
            passwordInput.type = "text"
            showPasswordBtn.check = true
            showPasswordBtn.checked = true
        }
        else {
            passwordInput.type = "password"
            showPasswordBtn.checked = false
            showPasswordBtn.check = false
        }
    })
})