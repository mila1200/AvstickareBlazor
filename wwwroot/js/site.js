let mapInstance;

window.initMap = async function () {
    const { Map } = await google.maps.importLibrary("maps");

    mapInstance = new Map(document.getElementById("map"), {
        center: { lat: 59.3293, lng: 18.0686 },
        zoom: 7,
        mapId: "DEMO_MAP_ID"
    });

    window.mapInstance = mapInstance;
};

window.drawTrip = async function (polyline, places) {
    if (!window.mapInstance || !polyline || !places) return;

    const { encoding } = await google.maps.importLibrary("geometry");

    const path = encoding.decodePath(polyline);

    const route = new google.maps.Polyline({
        path: path,
        geodesic: true,
        strokeColor: "#FF0000",
        strokeOpacity: 1.0,
        strokeWeight: 4
    });
    route.setMap(window.mapInstance);

    for (const place of places) {
        new google.maps.Marker({
            position: { lat: place.lat, lng: place.lng },
            map: window.mapInstance,
            title: place.name
        });
    }
};