"use strict";

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var EatThemAll = (function () {
    function EatThemAll($canvas, server) {
        _classCallCheck(this, EatThemAll);

        this.canvas = $canvas[0];
        this.server = server;
        this.canvas.width = window.innerWidth;
        this.canvas.height = window.innerHeight;
        this.ctx = canvas.getContext("2d");
        this.running = false;
        this.width = this.height = 5000;
        this.visibleWidth = this.canvas.width;
        this.visibleHeight = this.canvas.height;
        this.initialized = false;
        this.id = "";
        $canvas.bind("contextmenu", function (e) {
            e.preventDefault();
            var point = { x: e.offsetX, y: e.offsetY };
            server.updateDestination(point);
        });
    }

    _createClass(EatThemAll, [{
        key: "start",
        value: function start() {
            this.running = true;
        }
    }, {
        key: "stop",
        value: function stop() {
            this.running = false;
        }
    }, {
        key: "checkInitialize",
        value: function checkInitialize() {
            this.initialized = typeof this.ctx !== "undefined" && (typeof this.id !== "undefined" || this.id != "");
        }
    }, {
        key: "render",
        value: function render(players) {
            var _this = this;

            if (!this.running) return;

            if (!this.initialized) {
                console.log("Object is not correctly initialized");
                return;
            }
            this.ctx.globalAlpha = 1;
            this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

            console.log("MY ID:" + this.id);
            players.forEach(function (player) {
                return console.log(player.id);
            });
            var player = players.find(function (p) {
                return p.id === _this.id;
            });
            debugger;
            var offset = player.location;

            this.ctx.shadowBlur = 10;
            players.forEach(function (player) {
                player.location.x += offset.x;
                player.location.y += offset.y;

                _this.ctx.globalAlpha = player.alpha;
                _this.ctx.beginPath();
                _this.ctx.fillStyle = player.color;
                _this.ctx.arc(player.location.x, player.location.y, player.radius, 0, 2 * Math.PI);
                _this.ctx.fill();

                //this.ctx.fillStyle = this.color;
                //this.ctx.fillRect(this.location.x, this.location.y, 20, 20);

                _this.ctx.globalAlpha = 1;
                _this.ctx.fillStyle = "black";
                _this.ctx.font = "12px Arial";
                _this.ctx.fillText(player.name, player.location.x - 17, player.location.y - 10);
            });
            this.ctx.shadowBlur = 0;
        }
    }, {
        key: "id",
        set: function set(value) {
            this._id = value;this.checkInitialize();
        },
        get: function get() {
            return this._id;
        }
    }]);

    return EatThemAll;
})();

