"use strict";

var _createClass = (function () { function defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } } return function (Constructor, protoProps, staticProps) { if (protoProps) defineProperties(Constructor.prototype, protoProps); if (staticProps) defineProperties(Constructor, staticProps); return Constructor; }; })();

function _classCallCheck(instance, Constructor) { if (!(instance instanceof Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

var EatThemAll = (function () {
    function EatThemAll($canvas, server) {
        var _this = this;

        _classCallCheck(this, EatThemAll);

        this.canvas = $canvas[0];
        this.server = server;
        this.ctx = canvas.getContext("2d");
        this.running = false;
        this.width = this.height = 5000;
        this.resize();

        this.initialized = false;
        this.id = "";
        this.gridSize = 30;
        this.player;
        this.isDead = false;
        this.forceStop = false;

        if (typeof navigator.accelerometer !== "undefined") this.accelerationWatchId = navigator.accelerometer.watchAcceleration(function (acceleration) {
            if (!_this.initialized || !_this.running) return;

            var vector = {
                x: acceleration.y.toFixed(1),
                y: acceleration.x.toFixed(1)
            };
            _this.server.updateDirection(vector);
        }, function () {
            navigator.notification.alert("Acceleration error.", function () {}, "Acceleration", "OK");
        }, { frequency: 300 });else if (typeof navigator.notification !== "undefined") navigator.notification.alert("accelerometer is not available.", function () {}, "Accelerometer", "OK");

        $(window).bind("resize", function () {
            return _this.resize();
        });

        $canvas.bind("click contextmenu", function (e) {
            if (_this.isDead) {
                _this.isDead = false;
                $.connection.hub.start();
            }

            e.preventDefault();

            if (!_this.initialized || !_this.running) return;

            var vector = {
                x: (e.offsetX - _this.offset.x) / (_this.offset.x / 5),
                y: (e.offsetY - _this.offset.y) / (_this.offset.y / 5)
            };
            _this.server.updateDirection(vector);
        });
    }

    _createClass(EatThemAll, [{
        key: "start",
        value: function start() {
            this.running = true;this.isDead = false;
        }
    }, {
        key: "stop",
        value: function stop() {
            this.running = false;
        }
    }, {
        key: "resize",
        value: function resize() {
            this.canvas.width = window.innerWidth;
            this.canvas.height = window.innerHeight;
            this.offset = { x: this.canvas.width / 2, y: this.canvas.height / 2 };
        }
    }, {
        key: "checkInitialize",
        value: function checkInitialize() {
            this.initialized = typeof this.ctx !== "undefined" && (typeof this.id !== "undefined" || this.id != "") && typeof this.server !== "undefined";
        }
    }, {
        key: "clearCanvas",
        value: function clearCanvas() {
            this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        }
    }, {
        key: "render",
        value: function render(players, foods) {
            var _this2 = this;

            if (!this.running || !this.initialized || this.isDead) return;

            this.clearCanvas();

            // Get current player to determine middle of the screen
            //this.player = players.filter(p => p.id === this.id)[0];
            this.player = players.find(function (p) {
                return p.id === _this2.id;
            });
            if (this.player == null) {
                this.server.getConnectionId();
                // wait with render till user has correct id
                return;
            }

            // Render grid
            // NOTE: Dont know why its negative, but it fixed that grid fits with static object while moving...
            this.renderGrid({
                x: -this.player.location.x % this.gridSize,
                y: -this.player.location.y % this.gridSize
            });

            // Render players
            this.ctx.shadowBlur = 10;
            players.forEach(function (p) {
                return _this2.renderPlayer(p);
            });
            // Render foods
            foods.forEach(function (f) {
                return _this2.renderFood(f);
            });

            this.ctx.shadowBlur = 0;

            this.renderLabels();
        }
    }, {
        key: "renderLabels",
        value: function renderLabels() {
            this.ctx.globalAlpha = 1;
            this.ctx.fillStyle = "black";
            this.ctx.font = "20px Arial";
            this.ctx.textAlign = "center";
            this.ctx.textBaseline = "top";

            this.ctx.fillText("Score: " + this.player.score, this.canvas.width / 2, 0);
        }
    }, {
        key: "renderPlayer",
        value: function renderPlayer(player) {
            var location = {
                x: player.location.x - this.player.location.x + this.offset.x,
                y: player.location.y - this.player.location.y + this.offset.y
            };

            // its outside of canvas
            if (location.x < -player.radius || location.y < -player.radius || location.x > this.canvas.width + player.radius || location.y > this.canvas.height + player.radius) return;

            this.ctx.globalAlpha = player.alpha;
            this.ctx.beginPath();
            this.ctx.fillStyle = player.color;
            this.ctx.arc(location.x, location.y, player.radius, 0, 2 * Math.PI);
            this.ctx.fill();

            this.ctx.globalAlpha = 1;
            this.ctx.textAlign = "center";
            this.ctx.textBaseline = "bottom";
            this.ctx.fillStyle = "black";
            this.ctx.font = "12px Arial";
            this.ctx.fillText(player.name + ("[" + player.location.x + "," + player.location.y + "]"), location.x, location.y - player.radius);
        }
    }, {
        key: "renderFood",
        value: function renderFood(food) {
            var location = {
                x: food.location.x - this.player.location.x + this.offset.x,
                y: food.location.y - this.player.location.y + this.offset.y
            };

            // its outside of canvas
            if (location.x < -food.radius || location.y < -food.radius || location.x > this.canvas.width + food.radius || location.y > this.canvas.height + food.radius) return;

            this.ctx.globalAlpha = food.alpha;
            this.ctx.beginPath();
            this.ctx.fillStyle = food.color;
            this.ctx.arc(location.x, location.y, food.radius, 0, 2 * Math.PI);
            this.ctx.fill();

            this.ctx.globalAlpha = 1;
            this.ctx.textAlign = "center";
            this.ctx.textBaseline = "middle";
            this.ctx.fillStyle = "black";
            this.ctx.font = "12px Arial";
            this.ctx.fillText(food.score.toString(), location.x, location.y);
        }
    }, {
        key: "renderGrid",
        value: function renderGrid(offset) {
            for (var x = offset.x; x < this.canvas.width; x += this.gridSize) {
                this.ctx.moveTo(x, 0);
                this.ctx.lineTo(x, this.canvas.height);
            }

            for (var y = offset.y; y < this.canvas.height; y += this.gridSize) {
                this.ctx.moveTo(0, y);
                this.ctx.lineTo(this.canvas.width, y);
            }

            this.ctx.globalAlpha = 0.3;
            this.ctx.strokeWidth = 0.1;
            this.ctx.strokeStyle = "Black";
            this.ctx.stroke();
        }
    }, {
        key: "notifyDead",
        value: function notifyDead(finalScore) {
            if (!this.running || !this.initialized) return;

            this.isDead = true;
            this.forceStop = true;
            this.stop();
            $.connection.hub.stop();

            if (typeof navigator.vibrate !== "undefined") navigator.vibrate(2000);

            this.ctx.globalAlpha = 1;
            this.ctx.textAlign = "center";
            this.ctx.fillStyle = "black";

            this.ctx.textBaseline = "bottom";
            this.ctx.font = "40px Arial";
            this.ctx.fillText("You diedededed, final score " + finalScore, this.offset.x, this.offset.y);

            this.ctx.textBaseline = "top";
            this.ctx.font = "25px Arial";
            this.ctx.fillText("Tap to start again", this.offset.x, this.offset.y);
        }
    }, {
        key: "renderInfo",
        value: function renderInfo(message) {
            this.clearCanvas();

            this.ctx.globalAlpha = 1;
            this.ctx.fillStyle = "black";
            this.ctx.font = "40px Arial";
            this.ctx.textAlign = "center";
            this.ctx.textBaseline = "middle";
            this.ctx.fillText(message, this.offset.x, this.offset.y);
        }
    }, {
        key: "id",
        set: function set(value) {
            this._id = value;this.checkInitialize();
        },
        get: function get() {
            return this._id;
        }
    }, {
        key: "forceStop",
        set: function set(value) {
            this._forceStop = value;if (value) this.stop();
        },
        get: function get() {
            return this._forceStop;
        }
    }]);

    return EatThemAll;
})();

