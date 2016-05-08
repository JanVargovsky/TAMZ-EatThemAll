class EatThemAll {

    constructor($canvas, server) {
        this.canvas = $canvas[0];
        this.server = server;
        this.canvas.width = window.innerWidth;
        this.canvas.height = window.innerHeight;
        this.ctx = canvas.getContext("2d");
        this.running = false;
        this.width = this.height = 5000;
        this.offset = { x: this.canvas.width / 2, y: this.canvas.height / 2 };
        this.initialized = false;
        this.id = "";
        this.gridSize = 30;
        this.player;

        $canvas.bind("contextmenu", (e) => {
            e.preventDefault();

            if (!this.initialized || !this.running)
                return;

            var vector = {
                x: (e.offsetX - this.offset.x) / (this.offset.x / 5),
                y: (e.offsetY - this.offset.y) / (this.offset.y / 5)
            };
            this.server.updateDirection(vector);
        });
    }

    set id(value) { this._id = value; this.checkInitialize(); }
    get id() { return this._id; }

    start() { this.running = true; }
    stop() { this.running = false; }

    checkInitialize() {
        this.initialized = (typeof this.ctx !== "undefined" && (typeof this.id !== "undefined" || this.id != "") && typeof this.server !== "undefined");
    }

    render(players) {
        if (!this.running)
            return;

        if (!this.initialized)
            return;

        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

        // Get current player to determine middle of the screen
        this.player = players.find(p => p.id === this.id);
        if (this.player == null && server != null) {
            server.GetConnectionId();
            // wait with render till user has correct id
            return;
        }

        // Render grid
        // NOTE: Dont know why its negative, but it fixed that grid fits with static object while moving...
        this.renderGrid(
            {
                x: (-this.player.location.x) % this.gridSize,
                y: (-this.player.location.y) % this.gridSize
            });

        // Render players
        this.ctx.shadowBlur = 10;
        players.forEach((p) => this.renderPlayer(p));
        this.ctx.shadowBlur = 0;

        this.renderLabels();
    }

    renderLabels() {
        this.ctx.globalAlpha = 1;
        this.ctx.fillStyle = "black";
        this.ctx.font = "20px Arial";
        this.ctx.textAlign = "center";
        this.ctx.textBaseline = "top";

        this.ctx.fillText(`Score: ${this.player.score}`, this.canvas.width / 2, 0);
    }

    renderPlayer(player) {

        var location = {
            x: player.location.x - this.player.location.x + this.offset.x,
            y: player.location.y - this.player.location.y + this.offset.y,
        }

        // its outside of canvas
        if (location.x < -10 ||
            location.y < -10 ||
            location.x > this.canvas.width + 10 ||
            location.y > this.canvas.height + 10)
            return;

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
        this.ctx.fillText(player.name + `[${player.location.x},${player.location.y}]`, location.x, location.y - player.radius);
    }

    renderGrid(offset) {
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
}