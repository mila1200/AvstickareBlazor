let mapInstance;

window.initMap = async function () {
    try {
        const { Map } = await google.maps.importLibrary("maps");

        mapInstance = new Map(document.getElementById("map"), {
            //fallback centrering om rutten inte fungerar
            center: { lat: 59.3293, lng: 18.0686 },
            zoom: 7,
            mapId: "DEMO_MAP_ID"
        });

        window.mapInstance = mapInstance;
    } catch (error) {
        console.error("Kunde inte ladda Google Maps:", error);
    }

};

window.drawTrip = async function (polyline, places) {
    if (!window.mapInstance || !polyline || !places) return;

    const { encoding } = await google.maps.importLibrary("geometry");
    const { AdvancedMarkerElement, PinElement } = await google.maps.importLibrary("marker");

    const path = encoding.decodePath(polyline);

    //centrera kartan på rutten
    const bounds = new google.maps.LatLngBounds();
    path.forEach(coord => bounds.extend(coord));
    window.mapInstance.fitBounds(bounds);

    const route = new google.maps.Polyline({
        path: path,
        geodesic: true,
        strokeColor: "#FFDF78",
        strokeOpacity: 1.0,
        strokeWeight: 2
    });
    route.setMap(window.mapInstance);

    for (const place of places) {
        const pin = new PinElement({
            background: "#009688",
            borderColor: "#223843",
            glyphColor: "#FFD54F",
        });

        const marker = new AdvancedMarkerElement({
            map: window.mapInstance,
            position: { lat: place.lat, lng: place.lng },
            content: pin.element,
            title: place.name
        })

        //öppnar upp detaljsidan när användaren klickar på markören
        if (place.mapServicePlaceId) {
            marker.addListener("gmp-click", () => {
                window.location.href = `/plats/${place.mapServicePlaceId}`;
            });
        }
    }
};

window.highlightPlace = async function (lat, lng, name) {
    if (!window.mapInstance || !lat || !lng) return;

    const { AdvancedMarkerElement, PinElement } = await google.maps.importLibrary("marker");

    const pin = new PinElement({
        background: "#f44336", // röd för att sticka ut
        borderColor: "#b71c1c",
        glyphColor: "#fff"
    });

    const marker = new AdvancedMarkerElement({
        map: mapInstance,
        position: { lat: lat, lng: lng },
        content: pin.element,
        title: name
    });

    const info = new google.maps.InfoWindow({
        content: `<strong>${name}</strong>`
    });

    info.open(mapInstance, marker);

    marker.addListener("gmp-click", () => {
        info.open(mapInstance, marker);
    });

    mapInstance.setZoom(14);
    mapInstance.panTo({ lat: lat, lng: lng });
};