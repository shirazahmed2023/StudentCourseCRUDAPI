const proxyUrl = "https://my-proxy-server.com/proxy.php";
const apiUrl = "https://localhost:7183";

fetch(proxyUrl + "?url=" + encodeURIComponent(apiUrl))
    .then(response => response.json())
    .then(data => {
        // Process the response data
    })
    .catch(error => {
        console.error(error);
    });

