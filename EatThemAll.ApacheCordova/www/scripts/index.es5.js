// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints,
// and then run "window.location.reload()" in the JavaScript Console.

/// <reference path="jquery-1.6.4.js" />
/// <reference path="jquery.signalR-2.2.0.js" />

"use strict";

(function () {
    if (navigator.userAgent.match(/(iPhone|iPod|iPad|Android|BlackBerry|IEMobile)/)) {
        document.addEventListener('deviceready', onDeviceReady.bind(this), false);
    } else {
        onDeviceReady();
    }

    var game;

    function onDeviceReady() {
        //$.connection.hub.url = "http://vsb-tamz-var0065.azurewebsites.net/signalr";
        $.connection.hub.url = "http://localhost:23135/signalr";
        //$.connection.hub.logging = true;
        var reconnecting = false;

        var eatThemAllHub = $.connection.eatThemAllHub;
        if (typeof eatThemAllHub === "undefined") {
            if (typeof navigator.notification.alert !== "undefined") navigator.notification.alert("We're sorry, but application is not corectly installed.", function () {}, "Missing files", "OK");
            return;
        }

        game = new EatThemAll($("#canvas"), eatThemAllHub.server);
        //console.log(eatThemAllHub);

        eatThemAllHub.client.updateDestination = function (players) {
            return game.render(players);
        };

        $.connection.hub.start(function () {
            console.log("start");
            debugger;
            game.id = $.connection.hub.id;
            game.checkInitialize();
            game.start();
        });

        $.connection.hub.connectionSlow(function () {});

        $.connection.hub.disconnected(function () {
            if ($.connection.hub.lastError) console.log("Disconnected. Reason: " + $.connection.hub.lastError.message);

            //debugger;

            // try it again
            if (!reconnecting) $.connection.hub.start();
            //setTimeout(() =>$.connection.hub.start(), 1000);
        });

        $.connection.hub.reconnecting(function () {
            reconnecting = true;
            console.log("reconnecting");
        });

        $.connection.hub.reconnected(function () {});
    };

    function onPause() {
        // TODO: This application has been suspended. Save application state here.
        game.stop();
    };

    function onResume() {
        game.start();
    };
})();

