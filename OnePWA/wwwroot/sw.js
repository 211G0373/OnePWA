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
    console.log("sync")
    if (event.tag == "one") {
        event.waitUntil(enviarAlReconectar());
    }
    console.log("sync2")
});



self.addEventListener('fetch', (event) => {
    console.log("problems");
    if (event.request.url.includes('/api/Users')) {
        console.log("xddddd");
    }
    if (event.request.url.includes('/api/')) {
        console.log("jaja");
    }

  

    if (event.request.method === "GET") {
        // Network First para APIs

        if (event.request.url.includes('/api/')) {
            event.respondWith(manejarSessiones(event.request));
        }
        // Cache First para archivos estáticos
        else {
            event.respondWith(cacheFirst(event.request));
        }
    }
    else { //POST, PUT o DELETE
        if (event.request.url.includes('/api/Users') && event.request.method === "PUT") {
            console.log("xddddd");

            event.respondWith(manejarModificaciones(event.request));

        } else {
            //Manejo de sesiones
            event.respondWith(manejarSessiones(event.request));
        }
    }
});


const networkFirst = async (request) => {
    try {

        let clone = request.clone();

        let networkResponse = await fetch(request);
        console.log("mas problems1");


         //Detectar 401: Unauthorized
        if (networkResponse.status === 401) {
            //Si detecto 401, es porque el token ya caduco
            console.log("mas problems2");

            let response = await fetch("/api/users/renew");
            if (response.ok) {
                const token = await response.text();

                const clients = await self.clients.matchAll();
                for (const client of clients) {
                    client.postMessage({
                        type: 'TOKEN_EXPIRADO',
                        jwt: token
                    });
                }
                console.log("mas problems");

                clone.headers.set("Authorization", "Bearer " + token)
                networkResponse = await fetch(clone);
                console.log("mas problems");

            }
            console.log("mas problems3");


        }

        if (networkResponse.ok) {
            const cache = await caches.open(cacheName);
            await cache.put(request, networkResponse.clone());
        }

        return networkResponse;
    } catch (error) {
        console.log(error);
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
async function manejarSessiones(request) {

    const requestClone = request.clone();
    let body = null;

    // Leer body SOLO si existe
    if (request.method !== "GET" && request.method !== "HEAD") {
        body = await requestClone.text();
    }

    let networkResponse;

    try {
        networkResponse = await fetch(request);
    } catch {
        return new Response(
            JSON.stringify({ offline: true }),
            { status: 202, headers: { "Content-Type": "application/json" } }
        );
    }

    if (networkResponse.status !== 401) {
        return networkResponse;
    }

    // 🔁 Renovar token
    const renewResponse = await fetch("/api/users/renew");

    if (!renewResponse.ok) {
        return networkResponse;
    }

    const token = await renewResponse.text();

    // Avisar a las pestañas
    const clients = await self.clients.matchAll();
    for (const client of clients) {
        client.postMessage({
            type: "TOKEN_EXPIRADO",
            jwt: token
        });
    }

    // 🔥 Reconstruir request ORIGINAL
    const headers = Object.fromEntries(request.headers.entries());

    // 🔥 eliminar cualquier authorization previo (case-insensitive)
    for (const key in headers) {
        if (key.toLowerCase() === "authorization") {
            delete headers[key];
        }
    }

    // agregar el nuevo token
    headers.Authorization = `Bearer ${token}`;

    const newRequest = new Request(request.url, {
        method: request.method,
        headers,
        body
    });

    return await fetch(newRequest);
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


async function eliminarObjeto(storeName, id) {
    const db = await openDatabase(); // Función que abre la DB 
    const transaction = db.transaction(storeName, 'readwrite');
    const store = transaction.objectStore(storeName);
    const request = store.delete(id);
    return new Promise((resolve, reject) => {
        request.onsuccess = () => resolve(); // Confirmación de eliminación 
        request.onerror = () => reject(request.error);
    });

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


//
//const requestClone = request.clone();
//let body = null;

//// Leer body SOLO si existe
//if (request.method !== "GET" && request.method !== "HEAD") {
//    body = await requestClone.text();
//}

//let networkResponse;

//try {
//    networkResponse = await fetch(request);
//} catch {
//    return new Response(
//        JSON.stringify({ offline: true }),
//        { status: 202, headers: { "Content-Type": "application/json" } }
//    );
//}

//if (networkResponse.status !== 401) {
//    return networkResponse;
//}

//// 🔁 Renovar token
//const renewResponse = await fetch("/api/users/renew");

//if (!renewResponse.ok) {
//    return networkResponse;
//}

//const token = await renewResponse.text();

//// Avisar a las pestañas
//const clients = await self.clients.matchAll();
//for (const client of clients) {
//    client.postMessage({
//        type: "TOKEN_EXPIRADO",
//        jwt: token
//    });
//}

//// 🔥 Reconstruir request ORIGINAL
//const headers = Object.fromEntries(request.headers.entries());

//// 🔥 eliminar cualquier authorization previo (case-insensitive)
//for (const key in headers) {
//    if (key.toLowerCase() === "authorization") {
//        delete headers[key];
//    }
//}

//// agregar el nuevo token
//headers.Authorization = `Bearer ${token}`;

//const newRequest = new Request(request.url, {
//    method: request.method,
//    headers,
//    body
//});

//return await fetch(newRequest);
////


async function enviarAlReconectar() {

    let one = await obtenerTodos("one");
    for (let p of one) {
        //Enviar de uno por uno a internet

        try {
            let response = await fetch(p.url, {
                method: p.method,
                headers: new Headers(p.headers),
                body: p.method == "DELETE" ? null : p.body
            });

            if (response.ok) {
                console.log("hasta aqui bien");
                await eliminarObjeto("one", p.id);
                console.log("ID INDEXED DELETE" + p.id);

            } else if (response.status === 401) {
                //Si recibo 401, lo reintento actualizando el token
                let renewResponse = await fetch("/api/users/renew");
                if (renewResponse.ok) {
                    const token = await renewResponse.text();
                    const clients = await self.clients.matchAll();
                    for (const client of clients) {
                        client.postMessage({
                            type: "TOKEN_EXPIRADO",
                            jwt: token
                        });
                    }

                    //Reintentar la petición original con el nuevo token
                    p.headers = p.headers.filter(h => h[0].toLowerCase() !== "authorization");
                    p.headers.push(["Authorization", "Bearer " + token]);
                    let retryResponse = await fetch(p.url, {
                        method: p.method,
                        headers: new Headers(p.headers),
                        body: p.method == "DELETE" ? null : p.body
                    });
                    if (retryResponse.ok) {
                        await eliminarObjeto("one", p.id);
                        console.log("ID INDEXED DELETE AFTER RENEW" + p.id);
                    }
                }
            }






        } catch (error) {
            break;
        }
    }
}

self.addEventListener("push", function (event) {
    event.waitUntil(mostrarNotificacion(event));
});

async function mostrarNotificacion(event) {
    //en data viene el json que mandamos desde el service
    if (event.data) {
        let data = event.data.json();

        if (data) {
            //buscar las ventanas (o pestañas) que registraron el service worker
            const windows = await clients.matchAll({ type: "window" });

            //verificar si alguna esta actualmente visible
            const appVisible = windows.some(w => w.visibilityState == "visible");

            if (appVisible) {
                // Enviar mensaje a las ventanas visibles, no saldra la ventanita
                //de notificaciones
                for (let w of windows) {
                    if (w.visibilityState == "visible") {
                        w.postMessage({
                            tipo: "RECIBIDA",
                            titulo: data.titulo,
                            mensaje: data.mensaje
                        });
                    }
                }

                // Mostrar notificación aunque la app esté visible
                //descomenten esto para se vea la notificación aun si esta abierta
                await self.registration.showNotification(data.titulo,
                    {
                        body: data.mensaje
                    }
                );
            }
            else {
                // Mostrar ventana notificación del sistema si no hay cerradas
                await self.registration.showNotification(data.titulo,
                    {
                        body: data.mensaje,
                        data: {} //por si queiren mandar mas datos
                    }
                );
            }

        }
    }
}

