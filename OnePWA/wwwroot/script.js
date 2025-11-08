// toggle contra
const passwordToggle = document.getElementById('passwordToggle');
const passwordInput = document.getElementById('password');

//passwordToggle.addEventListener('click', () => {
//    const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
//    passwordInput.setAttribute('type', type);
//    passwordToggle.textContent = type === 'password' ? '👁️' : '🙈';
//});

// Toggle remember me
const rememberToggle = document.getElementById('rememberToggle');
rememberToggle.addEventListener('click', () => {
    rememberToggle.classList.toggle('active');
});



//Login