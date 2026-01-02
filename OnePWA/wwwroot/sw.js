const cacheName = 'one-v1';
const assets = [
    '/',
    '/index.html',
    '/login.html',
    '/crearCuenta.html',
    '/PerfilUsuario.html',
    '/styles.css',
    '/script.js',

];

const cacheAssets = async () => {
    const cache = await caches.open(cacheName);
    await cache.addAll(assets);
};

self.addEventListener('install', (event) => {
    event.waitUntil(cacheAssets());
});
self.addEventListener('fetch', (event) => {
    if (event.request.method === "GET") {
        // Network First para APIs
        if (event.request.url.includes('/api/')) {
            event.respondWith(networkFirst(event.request));
        }
        // Cache First para archivos estáticos
        else {
            event.respondWith(cacheFirst(event.request));
        }
    } console.log("FETCH APIS WORKING");
});


const networkFirst = async (request) => {
    try {

        let clone = request.clone();

        let networkResponse = await fetch(request);

         //Detectar 401: Unauthorized
        //if (networkResponse.status === 401) {
        //    //Si detecto 401, es porque el token ya caduco
        //    let response = await fetch("api/usuarios/renew");
        //    if (response.ok) {
        //        const token = await response.text();

        //        const clients = await self.clients.matchAll();
        //        for (const client of clients) {
        //            client.postMessage({
        //                type: 'TOKEN_EXPIRADO',
        //                jwt: token
        //            });
        //        }
        //        clone.headers.set("Authorization", "Bearer " + token)
        //        networkResponse = await fetch(clone);

        //    }

        //}

        if (networkResponse.ok) {
            const cache = await caches.open(cacheName);
            await cache.put(request, networkResponse.clone());
        }

        return networkResponse;
    } catch (error) {
        const cachedResponse = await caches.match(request);
        return cachedResponse || new Response('Offline', { status: 503 });
    }
};

const cacheFirst = async (request) => {
    const cachedResponse = await caches.match(request);
    if (cachedResponse) {
        return cachedResponse;
    }
    return await fetch(request);
};

