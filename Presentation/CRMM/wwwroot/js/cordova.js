class CRMM {
    constructor(mapSelector, urls) {
        this.self = this;

        self.MapSelector = mapSelector;
        self.Urls = urls;
        self.Created = false;
    }

    CreateMap() {
        if (self.Created)
            return;

        self.Markers = [];

        self.Map = new mapboxgl.Map({
            container: self.MapSelector,
            style: 'mapbox://styles/mapbox/streets-v10',
            center: [0, 0],
            zoom: 1
        });

        // Add zoom and rotation controls to the map.
        self.Map.addControl(new mapboxgl.NavigationControl());
        // Add geolocate control to the map.
        self.Map.addControl(new mapboxgl.GeolocateControl({
            positionOptions: {
                enableHighAccuracy: true
            },
            trackUserLocation: true
        }));

        self.Created = true;
    }

    Update() {
        $.post(self.Urls.map, function (data) {
            console.log(data);

            var validMarkers = [];

            // Add markers
            data.forEach(function (place) {
                var popup = '<div onclick="placeDetail('+place.place.id+')"><h6 class="m-0">'+place.place.name+'</h6><small>'+place.place.type+'</small><br/>'+place.place.address+'</div>';

                var found = false;

                // Update existing
                self.Markers.forEach(function (marker) {
                    var position = marker.getLngLat();
                    if (position.lng === place.place.x &&
                        position.lat === place.place.y) {
                        validMarkers.push(marker);
                        marker.getPopup().setHTML(popup);
                        marker.place = place.place;
                        found = true;
                    }
                });

                // Add new
                if (!found) {
                    var marker = new mapboxgl.Marker()
                        .setLngLat([place.place.x, place.place.y])
                        .setPopup(
                            new mapboxgl.Popup({ offset: 25 })
                            .setHTML(popup)
                        )
                        .addTo(self.Map);

                    marker.place = place.place;

                    self.Markers.push(marker);
                    validMarkers.push(marker);
                }
            });

            // Remove invalid markers
            var dropMarkers = self.Markers.filter(function (i) { return validMarkers.indexOf(i) < 0; });
            dropMarkers.forEach(function (drop) {
                self.Markers.splice(self.Markers.indexOf(drop), 1);
                drop.remove();
            });
        }, 'json');
        
    }

    UpdateUser(result) {
        $.post(self.Urls.user, function (data) {
            if (data != null && data.id > 0) {
                self.User = data;
                if (result != null)
                    return result(true);
            }
            result(false);
        }).fail(function () {
            if (result != null)
                result(false);
        });
    }

    CheckLogin(result) {
        return this.UpdateUser(result);
    }

    Login(email, password, success, error) {
        $.post(self.Urls.login, { email: email, password: password }, function (data) {
            if (data != null && data.id > 0) {
                self.User = data;
                if (success != null)
                    success(data);
            } else if (error != null)
                error();
        }).fail(error);
    }

    LoginForm(form, success, error) {
        $.post(self.Urls.login, form, function (data) {
            if (data != null && data.id > 0) {
                self.User = data;
                if (success != null)
                    success(data);
            } else if (error != null)
                error();
        }).fail(error);
    }

    Logout(success, error) {
        $.post(self.Urls.logout, function (data) {
            self.User = null;
            if (success != null)
                success();
        }).fail(error);
    }

    Place(id, success, error) {
        $.post(self.Urls.location, {id: id}, function (data) {
            if (data != null && data.place.id === id) {
                if (success != null)
                    success(data);
            } else if (error != null)
                error();
        }).fail(error);
    }

    SetOrder(order, success, error) {
        $.get(self.Urls.order.replace('_id_', order.id).replace('_placeid_', order.placeId).replace('_state_', order.state), function (data) {
                if (success != null)
                    success(data);
                else if (error != null)
                    error();
        }).fail(error);
    }
}