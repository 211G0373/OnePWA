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
self.addEventListener("sync", function (event) {
    if (event.tag == "one") {
        event.waitUntil(enviarAlReconectar());
    }
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
    } else { //POST, PUT o DELETE
        event.respondWith(manejarModificaciones(event.request));
    }
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



async function openDatabase() {
    return new Promise((resolve, reject) => {
        const request = indexedDB.open('oneIDB', 1);
        request.onerror = () => reject(request.error);
        request.onsuccess = () => resolve(request.result);

        request.onupgradeneeded = (event) => {
            const db = event.target.result;
            if (!db.objectStoreNames.contains('one')) {
                const store = db.createObjectStore('one', {
                    keyPath: 'id',
                    autoIncrement: true
                });
                //store.createIndex('indexName', 'objectProperty', { unique: false });
            }
        };
    });
}

async function manejarModificaciones(request) {
    let clon = request.clone();

    try {
        return await fetch(request);
    }
    catch (error) {
        //No pudo enviarlo


        let objeto = {
            method: request.method,
            url: request.url,
            headers: Array.from(request.headers.entries())
        };

        if (request.method == "POST" || request.method == "PUT") {
            let datos = await clon.text();
            objeto.body = datos;
        }


        await actualizarObjeto('one', objeto); 
        console.log("INDEXED WORKS");

        return new Response(null, { status: 200 }); //ok

    }
}



async function actualizarObjeto(storeName, objetoActualizado) {

    const db = await openDatabase(); // Función que abre la DB 

    const transaction = db.transaction([storeName], 'readwrite');

    const store = transaction.objectStore(storeName);

    const request = store.put(objetoActualizado); // Reemplaza completamente el objeto 

    return new Promise((resolve, reject) => {

        request.onsuccess = () => resolve(request.result); // Retorna el ID 

        request.onerror = () => reject(request.error);

    });

} 

async function obtenerTodos(storeName) {
    const db = await openDatabase(); // Función que abre la DB 
    const transaction = db.transaction(storeName, 'readonly');
    const store = transaction.objectStore(storeName);
    const request = store.getAll();
    return new Promise((resolve, reject) => {
        request.onsuccess = () => resolve(request.result);
        request.onerror = () => reject(request.error);
    });
}


async function enviarAlReconectar() {

    let one = await obtenerTodos("one");
    for (let p of one) {
        //Enviar de uno por uno a internet
        try {
            let response = await fetch(p.url, {
                method: p.method,
                headers: Array.from(p.headers.entries()),
                body: p.method == "DELETE" ? null : p.body
            });

            if (response.ok) {
                await eliminarObjeto("one", p.id);
                console.log("ID INDEXED DELETE"+p.id);

            }
        } catch (error) {
            break;
        }
    }
}

