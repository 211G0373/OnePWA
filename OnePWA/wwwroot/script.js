//// toggle contra
//const passwordToggle = document.getElementById('passwordToggle');
//const passwordInput = document.getElementById('password');

////passwordToggle.addEventListener('click', () => {
////    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
////    passwordInput.setAttribute('type', type);
////    passwordToggle.textContent = type === 'password' ? '👁️' : '🙈';
////});

//// Toggle remember me
//const rememberToggle = document.getElementById('rememberToggle');
//rememberToggle.addEventListener('click', () => {
//    rememberToggle.classList.toggle('active');
//});



/* --------------------------------------------------- FUNCIONALIDAD DE PERFIL DE USUARIO --------------------------------------------------- */

const passwordToggle = document.querySelector('.password-toggle');
const passwordInput = document.getElementById('password');

if (passwordToggle && passwordInput) {
    passwordToggle.addEventListener('click', () => {
        const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
        passwordInput.setAttribute('type', type);

        const icon = passwordToggle.querySelector('ion-icon');
        if (icon) {
            if (type === 'password') {
                icon.setAttribute('name', 'eye-off-outline'); 
            } else {
                icon.setAttribute('name', 'eye-outline'); 
            }
        }
    });
}




/* --------------------------------------------------- FUNCIONALIDAD DEL MODAL DE AVATARES --------------------------------------------------- */


function openAvatarModal() {
    const modal = document.getElementById('avatarModal');
    if (modal) {
        modal.style.display = 'block';
    }
}


function closeAvatarModal() {
    const modal = document.getElementById('avatarModal');
    if (modal) {
        modal.style.display = 'none';
    }
}


window.addEventListener('click', (event) => {
    const modal = document.getElementById('avatarModal');
    if (modal && event.target === modal) {
        modal.style.display = 'none';
    }
});



document.addEventListener('DOMContentLoaded', () => {
    const avatarOptions = document.querySelectorAll('.avatar-option');
    const profileImage = document.querySelector('.profile-avatar-img');

    avatarOptions.forEach(option => {
        option.addEventListener('click', function () {
            avatarOptions.forEach(opt => opt.classList.remove('selected'));

            this.classList.add('selected');

            const newAvatarFileName = this.getAttribute('data-avatar');
            const newAvatarSrc = `Assets/Avatares/${newAvatarFileName}`;

            if (profileImage) {
                profileImage.src = newAvatarSrc;
            }
        });
    });
});
//Login
