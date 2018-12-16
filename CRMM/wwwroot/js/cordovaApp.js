var App = App || {};
$(function () {
    $('[data-toggle="tooltip"]').tooltip();

    App.UpdateTimer = false;

    App.Redraw = function () {
        $('#loginPage').addClass('pagehide');
        $('#mapPage').removeClass('pagehide');
        App.CreateMap();
        App.Update();
        App.UpdateTimerInstance = setTimeout(App.Redraw, 10000);
    };

    if (currentUser != null && currentUser.id > 0) {
        App.User = currentUser;
        App.UpdateTimer = true;
        App.Redraw();
    }

    $('#loginPage form').submit(function(event) {
        event.preventDefault();
        App.LoginForm($(this).serialize(), function (data) {
            App.UpdateTimer = true;
            App.Redraw();
        });
    });

    $('#logout').click(function() {
        App.Logout(function () {
            App.UpdateTimer = false;
            if(App.UpdateTimerInstance != null)
                clearTimeout(App.UpdateTimerInstance);
            $('#loginPage').removeClass('pagehide');
            $('#mapPage').addClass('pagehide');
        });
    });

    /*App.CheckLogin(function (result) {
        if (result) {
            App.Update();
        } else {
            App.Login('naruto', 'ramen',
                function (data) {
                    console.log(data);
                    App.Update();
                    //App.Logout(function() {
                    //    console.log("Logout success");
                    //}, function () {
                    //    console.log("Logout failed");
                    //});
                },
                function () {
                    console.log("Login Error");
                }
            );
        }
    });*/
});

function placeDetail(id) {
    console.log('place', id);
    App.Place(id, function (data) {
        console.log(data);
        $('#placeName').text(data.place.name);
        $('#placeType').text(data.place.type);
        $('#placeAddress').text(data.place.address);

        $('#customerName').text(data.user.name);

        $('#placePage').removeClass('pagehide');
        $('#mapPage').addClass('pagehide');

        $('#placeOrders').empty();
        data.orders.forEach(function(order) {
            var el = $($('#placeOrderTemplate').html());

            el.find('.name').text(order.name);
            el.find('.description').text(order.description);
            el.find('.state').text(order.state);
            el.find('.statedate').text(order.stateCreatedOnUtc);
            el.find('.date').text(order.createdOnUtc);

            if (order.nextState == null || order.nextState.length <= 0) {
                el.find('.nextstate-btn').remove();
            }
            else {
                el.find('.nextstate-btn')
                    .data("order", { id: order.id, placeId: id, state: order.nextState })
                    .click(function() {
                        var data = $(this).data("order");
                        App.SetOrder(data,
                            function (state) {
                                el.find('.state').text(data.state);
                                el.find('.statedate').text(new Date().toUTCString());

                                if (state == null || state.length <= 0) {
                                    el.find('.nextstate-btn').remove();
                                }
                                else {
                                    el.find('.nextstate-btn')
                                        .data("order", { id: order.id, placeId: id, state: state })
                                        .find('.nextstate').text(state);
                                }
                            });
                    })
                    .find('.nextstate').text(order.nextState);
            }

            $('#placeOrders').append(el);
        });

        if (data.workers == null || data.workers.length <= 0)
            $('#placeWorkersWrapper').addClass('pagehide');
        else
            $('#placeWorkersWrapper').removeClass('pagehide');

        $('#placeWorkers').empty();
        data.workers.forEach(function(worker) {
            var el = $($('#placeWorkerTemplate').html());

            el.find('.name').text(worker.name);

            $('#placeWorkers').append(el);
        });
    });
}

function placeClose() {
    $('#placePage').addClass('pagehide');
    $('#mapPage').removeClass('pagehide');
}