const token = localStorage.getItem('jwtToken');
if (token) {
    window.location.href = 'index.html';
}
else {
    document.getElementById('newForm').addEventListener('submit', async (e) => {
        e.preventDefault();

        const submitButton = e.target.querySelector('button[type="submit"]');
        const originalText = submitButton.textContent;

        try {
            submitButton.textContent = 'Iniciando sesión...';
            submitButton.disabled = true;

            const formData = {
                Name: document.getElementById('name').value,
                Priveate: document.getElementById('priveate').value,
                NewRules: document.getElementById('newrules').value
            };

            const response = await fetch('/api/Sessions/New', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(formData)
            });

            if (response.ok) {
                const token = await response.text();
                localStorage.setItem('jwtToken', token);
                window.location.href = 'index.html';
            } else {
                const emailInput = document.getElementById('email');
                emailInput.setCustomValidity('Credenciales incorrectas');
                emailInput.reportValidity();
            }
        } catch (error) {
            const emailInput = document.getElementById('email');
            emailInput.setCustomValidity('Error de conexión');
            emailInput.reportValidity();
        } finally {
            submitButton.textContent = originalText;
            submitButton.disabled = false;
        }
    });

    // Quitar mensaje validación al escribir
    document.getElementById('email').addEventListener('input', function () {
        this.setCustomValidity('');
    });
    document.getElementById('password').addEventListener('input', function () {
        document.getElementById('email').setCustomValidity('');
    });
}