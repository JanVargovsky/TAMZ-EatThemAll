class EatThemAll {

    constructor($canvas, server) {
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

    set id(value) { this._id = value; this.checkInitialize(); }
    get id() { return this._id; }

    start() { this.running = true; }
    stop() { this.running = false; }

    checkInitialize() {
        this.initialized = (typeof this.ctx !== "undefined" && (typeof this.id !== "undefined" || this.id != ""));
    }

    render(players) {
        if (!this.running)
            return;

        if (!this.initialized) {
            console.log("Object is not correctly initialized");
            return;
        }
        this.ctx.globalAlpha = 1;
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

        console.log(`MY ID:${this.id}`);
        players.forEach((player) => console.log(player.id));
        let player = players.find(p => p.id === this.id);
        debugger;
        let offset = player.location;

        this.ctx.shadowBlur = 10;
        players.forEach((player) => {
            player.location.x += offset.x;
            player.location.y += offset.y;


            this.ctx.globalAlpha = player.alpha;
            this.ctx.beginPath();
            this.ctx.fillStyle = player.color;
            this.ctx.arc(player.location.x, player.location.y, player.radius, 0, 2 * Math.PI);
            this.ctx.fill();

            //this.ctx.fillStyle = this.color;
            //this.ctx.fillRect(this.location.x, this.location.y, 20, 20);

            this.ctx.globalAlpha = 1;
            this.ctx.fillStyle = "black";
            this.ctx.font = "12px Arial";
            this.ctx.fillText(player.name, player.location.x - 17, player.location.y - 10);
        });
        this.ctx.shadowBlur = 0;
    }
}