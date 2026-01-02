/* ----------------------------- ANIMACION PUNTOS SUSPENSIVOS LOADING PAGE --------------------------------------------------- */

// Solo ejecutar en la página de loading
const dotsElement = document.getElementById('dots');
if (dotsElement) {
    let dotCount = 0;

    setInterval(() => {
        dotCount = (dotCount + 1) % 4;
        dotsElement.textContent = '.'.repeat(dotCount);
    }, 500);

    // Simular conexión y redireccionar al juego
    // En producción, esto se activaría cuando la conexión WebSocket esté lista
    setTimeout(() => {
        document.body.style.transition = 'opacity 0.5s';
        document.body.style.opacity = '0';

        setTimeout(() => {
            // Cambiar a la URL de tu juego
          //  window.location.href = 'game.html';
            // O si usas el mismo HTML con diferentes vistas:
            // window.location.href = '#game';
        }, 500);
    }, 3000); // 3 segundos de ejemplo
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

/* --------------------------------------------------- MODAL DE RESULTADO (WIN/LOSE) --------------------------------------------------- */
function openEndgameModal(state = 'win', hero = {}, standings = []) {
    const overlay = document.getElementById('endgameModal');
    if (!overlay) return;
    console.log("xdd");
    // Datos por defecto
    //const defaultHero = { name: 'Pato', avatar: 'Assets/avatars/4.jpg', place: state === 'win' ? 1 : 3 };
    //const defaultStandings = state === 'win' 
    //    ? [
    //        { position: 1, name: 'Pato', avatar: 'Assets/avatars/4.jpg', cardsText: 'Sin cartas' },
    //        { position: 2, name: 'Teo', avatar: 'Assets/avatars/1.jpg', cardsText: 'Con 3 cartas' },
    //        { position: 3, name: 'Lali', avatar: 'Assets/avatars/5.jpg', cardsText: 'Con 4 cartas' }
    //    ]
    //    : [
    //        { position: 1, name: 'Pedro', avatar: 'Assets/avatars/4.jpg', cardsText: 'Sin cartas' },
    //        { position: 2, name: 'Pato', avatar: 'Assets/avatars/4.jpg', cardsText: 'Con 1 carta' },
    //        { position: 3, name: 'Lali', avatar: 'Assets/avatars/5.jpg', cardsText: 'Con 2 cartas' }
    //    ];

    //const finalHero = { ...defaultHero, ...hero };
    //const finalStandings = standings.length > 0 ? standings : defaultStandings;

    // Actualizar título
    //const titleEl = document.getElementById('endgameTitle');
    //if (titleEl) titleEl.textContent = state === 'lose' ? 'Perdiste' : '¡Ganaste!';

    //// Actualizar hero
    //const heroNameEl = document.getElementById('endgameHeroName');
    //const heroPlaceEl = document.getElementById('endgameHeroPlace');
    //const heroAvatarEl = document.getElementById('endgameHeroAvatar');
    
    //if (heroNameEl) heroNameEl.textContent = finalHero.name;
    //if (heroPlaceEl) heroPlaceEl.textContent = `${finalHero.place}º lugar`;
    //if (heroAvatarEl) {
    //    heroAvatarEl.src = "Assets/avatars/"+finalHero.avatar;
    //    heroAvatarEl.alt = `Avatar de ${finalHero.name}`;
    //}

    // Actualizar standings
    //const standingsContainer = document.getElementById('endgameStandings');
    //if (standingsContainer) {
    //    standingsContainer.innerHTML = finalStandings.map(player => `
    //        <div class="endgame-row">
    //            <div class="endgame-row-left">
    //                <span class="endgame-position">${player.position}</span>
    //                <img class="endgame-row-avatar" src="Assets/avatars/${player.avatar}" alt="${player.name}" />
    //                <span class="endgame-row-name">${player.name}</span>
    //            </div>
    //            <span class="endgame-row-cards">${player.cardsText || player.cards}</span>
    //        </div>
    //    `).join('');
    //}

    // Aplicar clase de estado
    overlay.classList.toggle('is-lose', state === 'lose');
    overlay.classList.add('active');
    overlay.setAttribute('aria-hidden', 'false');
    document.body.style.overflow = 'hidden';
}

function closeEndgameModal() {
    const overlay = document.getElementById('endgameModal');
    if (!overlay) return;
    overlay.classList.remove('active', 'is-lose');
    overlay.setAttribute('aria-hidden', 'true');
    document.body.style.overflow = '';
}

function playAgainFromModal() {
    closeEndgameModal();
}

// Función temporal para probar el modal
function testEndgameModal(state) {
    if (state === 'win') {
        openEndgameModal('win', { name: 'Pato', avatar: 'Assets/avatars/4.jpg', place: 1 });
    } else {
        openEndgameModal('lose', { name: 'Lali', avatar: 'Assets/avatars/5.jpg', place: 3 });
    }
}

/* --------------------------------------------------- MODAL DE COMODÍN (ELECCIÓN DE COLOR) --------------------------------------------------- */
function openColorPickerModal() {
    const overlay = document.getElementById('colorPickerModal');
    if (!overlay) return;
    overlay.classList.add('active');
    overlay.setAttribute('aria-hidden', 'false');
    document.body.style.overflow = 'hidden';
}

function closeColorPickerModal() {
    const overlay = document.getElementById('colorPickerModal');
    if (!overlay) return;
    overlay.classList.remove('active');
    overlay.setAttribute('aria-hidden', 'true');
    document.body.style.overflow = '';
}


/* --------------------------------------------------- FUNCIONALIDAD DEL CONTADOR DE JUGADORES --------------------------------------------------- */

function updatePlayerCount(change) {
    const counterDisplay = document.querySelector('.counter-display');
    if (!counterDisplay) return;

    let currentCount = parseInt(counterDisplay.textContent);
    let newCount = currentCount + change;

    // Limitar entre 1 y 8
    if (newCount < 2) newCount = 2;
    if (newCount > 8) newCount = 8;

    counterDisplay.textContent = newCount;
}


/* --------------------------------------------------- TIMER DE JUGADORES (GAME PAGE) --------------------------------------------------- */

// Configuración del timer
const TURN_DURATION = 30; // 30 segundos por turno
let currentTimerInterval = null;
let timeRemaining = TURN_DURATION;

// Función para iniciar el timer de un jugador
function startPlayerTimer(playerSlot) {
    // Limpiar timer anterior si existe
    stopPlayerTimer();
    
    // Resetear tiempo
    timeRemaining = TURN_DURATION;
    
    // Obtener elementos del jugador actual
    const timerBar = playerSlot.querySelector('.timer-bar');
    if (!timerBar) return;
    
    // Marcar como jugador activo
    document.querySelectorAll('.player-slot').forEach(slot => {
        slot.classList.remove('active');
    });
    playerSlot.classList.add('active');
    
    // Iniciar countdown
    currentTimerInterval = setInterval(() => {
        timeRemaining -= 0.1;
        
        // Calcular porcentaje restante
        const percentage = (timeRemaining / TURN_DURATION) * 100;
        timerBar.style.width = percentage + '%';
        
        // Advertencia cuando quedan 10 segundos
        if (timeRemaining <= 10) {
            timerBar.classList.add('warning');
        } else {
            timerBar.classList.remove('warning');
        }
        
        // Tiempo agotado
        if (timeRemaining <= 0) {
            stopPlayerTimer();
            onTimerExpired(playerSlot);
        }
    }, 100);
}

// Función para detener el timer
function stopPlayerTimer() {
    if (currentTimerInterval) {
        clearInterval(currentTimerInterval);
        currentTimerInterval = null;
    }
    
    // Resetear todas las barras
    document.querySelectorAll('.timer-bar').forEach(bar => {
        bar.style.width = '100%';
        bar.classList.remove('warning');
    });
}

// Función cuando el tiempo expira
function onTimerExpired(playerSlot) {
    const playerName = playerSlot.querySelector('.player-name').textContent;
    console.log('¡Tiempo agotado para:', playerName);
    
    // Aquí puedes agregar lógica como:
    // - Robar carta automáticamente
    // - Pasar turno al siguiente jugador
    // - Mostrar notificación
    
    // Ejemplo: pasar al siguiente jugador
    // nextPlayerTurn();
}

// Función para pasar al siguiente jugador (ejemplo)
function nextPlayerTurn() {
    const slots = document.querySelectorAll('.player-slot');
    const currentActive = document.querySelector('.player-slot.active');
    
    if (!currentActive || slots.length === 0) return;
    
    // Encontrar índice del jugador actual
    let currentIndex = Array.from(slots).indexOf(currentActive);
    
    // Calcular siguiente jugador (circular)
    let nextIndex = (currentIndex + 1) % slots.length;
    
    // Iniciar timer del siguiente jugador
    startPlayerTimer(slots[nextIndex]);
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

    // Cerrar modal de resultado tocando el fondo
    const endgameOverlay = document.getElementById('endgameModal');
    if (endgameOverlay) {
        endgameOverlay.addEventListener('click', (event) => {
            if (event.target === endgameOverlay) {
                closeEndgameModal();
            }
        });
    }

    // Cerrar modal de color tocando el fondo
    const colorPickerOverlay = document.getElementById('colorPickerModal');
    if (colorPickerOverlay) {
        colorPickerOverlay.addEventListener('click', (event) => {
            if (event.target === colorPickerOverlay) {
                closeColorPickerModal();
            }
        });
    }

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

    // Contador de jugadores en crear partida
    const minusBtn = document.querySelector('.minus-btn');
    const plusBtn = document.querySelector('.plus-btn');

    if (minusBtn) {
        minusBtn.addEventListener('click', () => {
            updatePlayerCount(-1);
        });
    }

    if (plusBtn) {
        plusBtn.addEventListener('click', () => {
            updatePlayerCount(1);
        });
    }

    // INICIAR TIMER DE JUEGO (solo en game.html)
    // Ejemplo: iniciar timer para el jugador con clase 'active'
    const gameTable = document.querySelector('.game-table');
    if (gameTable) {
        // Buscar el jugador que tiene turno actual
        const activePlayer = document.querySelector('.player-slot.active');
        if (activePlayer) {
            startPlayerTimer(activePlayer);
        }
    }
}); 