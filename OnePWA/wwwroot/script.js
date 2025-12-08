
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


/* ----------------------- Actualiza función copyCode en Vista de Iniciar partida --------------------------------------------------- */

function copyCode() {
    const code = document.getElementById('gameCode').textContent;
    const copyMessage = document.getElementById('copyMessage');
    const copyButton = document.querySelector('.copy-button');

    navigator.clipboard.writeText(code).then(() => {
        copyMessage.classList.add('show');
        copyButton.classList.add('copied');

        setTimeout(() => {
            copyMessage.classList.remove('show');
            copyButton.classList.remove('copied');
        }, 2000);
    }).catch(err => {
        console.error('Error al copiar:', err);
    });
}


/* --------------------------------------------------- FUNCIONALIDAD DEL MODAL DE LOGOUT --------------------------------------------------- */

function openModal() {
    const modal = document.getElementById('logoutModal');
    if (modal) {
        modal.classList.add('active');
        document.body.style.overflow = 'hidden';
    }
}

function closeModal() {
    const modal = document.getElementById('logoutModal');
    if (modal) {
        modal.classList.remove('active');
        document.body.style.overflow = '';
    }
}

function closeModalOnOverlay(event) {
    if (event.target.id === 'logoutModal') {
        closeModal();
    }
}

function confirmLogout() {
    console.log('Usuario confirmó logout');
    alert('Saliendo del juego...');
    closeModal();
}


/* --------------------------------------------------- INICIALIZACIÓN DEL DOM --------------------------------------------------- */

document.addEventListener('DOMContentLoaded', () => {
    // Toggle remember me
    const rememberToggle = document.getElementById('rememberToggle');
    if (rememberToggle) {
        rememberToggle.addEventListener('click', () => {
            rememberToggle.classList.toggle('active');
        });
    }

    // Funcionalidad de perfil de usuario
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

    // Modal de avatares - click en overlay
    const avatarModal = document.getElementById('avatarModal');
    window.addEventListener('click', (event) => {
        if (avatarModal && event.target === avatarModal) {
            avatarModal.style.display = 'none';
        }
    });

    // Funcionalidad de selección de avatares
    const avatarOptions = document.querySelectorAll('.avatar-option');
    const profileImage = document.querySelector('.profile-avatar-img');

    avatarOptions.forEach(option => {
        option.addEventListener('click', function () {
            avatarOptions.forEach(opt => opt.classList.remove('selected'));

            this.classList.add('selected');

            const newAvatarFileName = this.getAttribute('data-avatar');
            const newAvatarSrc = `Assets/avatars/${newAvatarFileName}`;

            if (profileImage) {
                profileImage.src = newAvatarSrc;
            }
        });
    });
});