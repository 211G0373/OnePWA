  if ('serviceWorker' in navigator) {
            try {
        navigator.serviceWorker.register('/sw.js?v=1');

                navigator.serviceWorker.ready.then(reg =>
                    //Avisame cuando se vaya y regrese el internet
                    reg.sync.register("one")
                );

                navigator.serviceWorker.addEventListener('message', (event) => {
                    if (event.data.type === 'TOKEN_EXPIRADO') {
        console.log('Token expirado detectado por SW');
    let token = event.data.jwt;
    localStorage.setItem('jwtToken', token);
    headers = {
        'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
                        };
                        
                    }
                });
            } catch (error) {
        console.log('Error registrando Service Worker:', error);
            }
        }