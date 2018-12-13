function cordovaInit(mapSelector, urls) {
    var map = new mapboxgl.Map({
        container: mapSelector,
        style: 'mapbox://styles/mapbox/streets-v10',
        center: [0, 0],
        zoom: 1
    });

    // Add zoom and rotation controls to the map.
    map.addControl(new mapboxgl.NavigationControl());
    // Add geolocate control to the map.
    map.addControl(new mapboxgl.GeolocateControl({
        positionOptions: {
            enableHighAccuracy: true
        },
        trackUserLocation: true
    }));

    var app = {
        urls: urls,
        locations: [],
        user: {},
        login: function () {
            $.post(this.urls.login, )
            
        }
    };

    console.log(urls);

    $.getJSON(urls.map, {}, function(data) {
        console.log(data);
    });
    // Add markers
    new mapboxgl.Marker()
        .setLngLat([0, 0])
        .setPopup(
            new mapboxgl.Popup({ offset: 25 })
            .setHTML('<h6 class="m-0">Name</h6><small>Type</small><br/>Address')
        )
        .addTo(map);
}
