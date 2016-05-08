// For an introduction to the Blank template, see the following documentation:
// http://go.microsoft.com/fwlink/?LinkID=397704
// To debug code on page load in Ripple or on Android devices/emulators: launch your app, set breakpoints, 
// and then run "window.location.reload()" in the JavaScript Console.

/// <reference path="jquery-1.6.4.js" />
/// <reference path="jquery.signalR-2.2.0.js" />

(function () {
    if (navigator.userAgent.match(/(iPhone|iPod|iPad|Android|BlackBerry|IEMobile)/)) {
        document.addEventListener('deviceready', onDeviceReady.bind(this), false);
    } else {
        onDeviceReady();
    }

    var game;
    var forceStop = false;

    function onDeviceReady() {
        $.connection.hub.url = "http://vsb-tamz-var0065.azurewebsites.net/signalr";
        //$.connection.hub.url = "http://localhost:23135/signalr";
        //$.connection.hub.logging = true;
        var reconnecting = false,
            tries = 1;

        var eatThemAllHub = $.connection.eatThemAllHub;
        if (typeof eatThemAllHub === "undefined") {
            if (typeof navigator.notification !== "undefined")
                navigator.notification.alert("We're sorry, but application is not corectly installed.", function () { }, "Missing files", "OK");
            return;
        }

        game = new EatThemAll($("#canvas"), eatThemAllHub.server);
        game.renderInfo("Trying to connect ...");
        //console.log(eatThemAllHub);

        eatThemAllHub.client.update = (players, foods) => game.render(players, foods);
        eatThemAllHub.client.setConnectionId = (id) => game.id = id;
        eatThemAllHub.client.notifyDead = (score) => game.notifyDead(score);

        $.connection.hub.start(function () {
            console.log("start");
            game.start();
            tries = 1;
            reconnecting = false;
            game.forceStop = false;
        });

        $.connection.hub.connectionSlow(function () {
        });

        $.connection.hub.disconnected(function () {
            debugger;
            if (forceStop || game.forceStop)
                return;

            if ($.connection.hub.lastError) {
                console.log("Disconnected. Reason: " + $.connection.hub.lastError.message);
                if (typeof navigator.notification !== "undefined")
                    navigator.notification.alert($.connection.hub.lastError.message, function () { }, "Disconnect", "OK");
            }

            // try it again
            if (!reconnecting)
                setTimeout(() => {
                    game.renderInfo(`Trying to reconnect #${tries}`);
                    $.connection.hub.start();
                    tries++;
                }, 1000);
        });4

        $.connection.hub.reconnecting(function () {
            reconnecting = true;
            console.log("reconnecting");
        });

        $.connection.hub.reconnected(function () {
            reconnecting = false;
        });

        document.addEventListener("pause", onPause, false);
        document.addEventListener("resume", onResume, false);
    };

    function onPause() {
        forceStop = true;
        $.connection.hub.stop();
    };

    function onResume() {
        forceStop = false;
        $.connection.hub.start();
    };

    if (!Array.prototype.find) {
        Array.prototype.find = function (predicate) {
            if (this === null) {
                throw new TypeError('Array.prototype.find called on null or undefined');
            }
            if (typeof predicate !== 'function') {
                throw new TypeError('predicate must be a function');
            }
            var list = Object(this);
            var length = list.length >>> 0;
            var thisArg = arguments[1];
            var value;

            for (var i = 0; i < length; i++) {
                value = list[i];
                if (predicate.call(thisArg, value, i, list)) {
                    return value;
                }
            }
            return undefined;
        };
    }
})();