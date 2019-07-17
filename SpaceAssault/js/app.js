
// A cross-browser requestAnimationFrame
// See https://hacks.mozilla.org/2011/08/animating-with-javascript-from-setinterval-to-requestanimationframe/
var requestAnimFrame = (function(){
    return window.requestAnimationFrame       ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame    ||
        window.oRequestAnimationFrame      ||
        window.msRequestAnimationFrame     ||
        function(callback){
            window.setTimeout(callback, 1000 / 60);
        };
})();

// Create the canvas
var canvas = document.createElement("canvas");
var ctx = canvas.getContext("2d");
canvas.width = 512;
canvas.height = 480;
document.body.appendChild(canvas);

// The main game loop
var lastTime;
function main() {
    var now = Date.now();
    var dt = (now - lastTime) / 1000.0;

    update(dt);
    render();

    lastTime = now;
    requestAnimFrame(main);
};

function init() {
    terrainPattern = ctx.createPattern(resources.get('img/terrain.png'), 'repeat');

    initMegaliths();

    document.getElementById('play-again').addEventListener('click', function() {
        reset();
    });

    reset();
    lastTime = Date.now();
    main();
}

resources.load([
    'img/sprites.png',
    'img/terrain.png'
]);

function initMegaliths() {
    var widthmegalith = 180;
    var heightmegalith = 100;
    var countmegaliths =  Math.round(Math.random()*5 + 4);
 // init random position
    for( var i = 0; i < countmegaliths; i++ )
    {
 
     var megalithPosx = Math.round(Math.random()*(canvas.width*0.68 + 1) + canvas.width*0.2 - 0.5);
     var megalithPosy = Math.round(Math.random()*(canvas.height*0.67 + 1) + canvas.height*0.1 - 0.5);
    
         for( var j = 0; j < i; j++ )
         {
             if (boxCollides([megalithPosx,megalithPosy],[widthmegalith,heightmegalith],megaliths[j].pos,[widthmegalith,heightmegalith]))
                 {
                     var megalithPosx = Math.round(Math.random()*(canvas.width*0.68 + 1) + canvas.width*0.2 - 0.5);
                     var megalithPosy = Math.round(Math.random()*(canvas.height*0.67 + 1) + canvas.height*0.1 - 0.5);
                     j = -1;
                 }
          }
          
     if (Math.round(Math.random()) == 0 )
         var type = 210;
     else
         var type = 270;
 
     megaliths[i] = { 
                         pos: [megalithPosx,megalithPosy],
                         sprite: new Sprite('img/sprites.png', [0,type], [60,60], 0, [0])
                     }
     }
 }

 function initManna() {

    var check = false;

    if(manna.length < 12)
    {
       if(manna.length < 4 || Math.random() < 1 - Math.pow(.999, 1.2) ) {
         var mannaPosx = Math.round(Math.random()*(canvas.width - 80));
         var mannaPosy = Math.round(Math.random()*(canvas.height - 80));
         
         for(var j=0; j<megaliths.length; j++) {
            var megalithPos = megaliths[j].pos;
            var megalithSize = megaliths[j].sprite.size;

            if(boxCollides([mannaPosx,mannaPosy], [60, 60], megalithPos, megalithSize))
            {
                check = true;
            }
        }
        if(!check)
        manna.push(
            {
                pos: [mannaPosx,mannaPosy],
                sprite: new Sprite('img/sprites.png', [0, 153], [60, 60],
                0, [0, 1, 2, 3], null, true)
            });
        }
    }
}

resources.onReady(init);

// Game state
var player = {
    pos: [0, 0],
    sprite: new Sprite('img/sprites.png', [0, 0], [39, 39], 16, [0, 1])
};

var bullets = [];
var enemies = [];
var explosions = [];
var megaliths = [];

var lastFire = Date.now();
var gameTime = 0;
var isGameOver;
var terrainPattern;

var score = 0;
var scoreEl = document.getElementById('score');
var manna = [];
var mannascore = 0;
var mannascoreEl = document.getElementById('mannascore');

// Speed in pixels per second
var playerSpeed = 200;
var bulletSpeed = 500;
var enemySpeed = 100;

// Update game objects
function update(dt) {
    gameTime += dt;

    handleInput(dt);
    updateEntities(dt);

    // It gets harder over time by adding enemies using this
    // equation: 1-.993^gameTime
    if(Math.random() < 1 - Math.pow(.993, gameTime)) {
        enemies.push({
            pos: [canvas.width,
                  Math.random() * (canvas.height - 39)],
            sprite: new Sprite('img/sprites.png', [0, 78], [80, 39],
                               6, [0, 1, 2, 3, 2, 1])
        });
    }

    initManna();

    checkCollisions(dt);

    scoreEl.innerHTML = score;
    mannascoreEl.innerHTML = mannascore;
};

function checkPlayerCollisions(dx, dy = 0) {

    
    for(var i=0; i<megaliths.length; i++) {
        var pos = megaliths[i].pos;
        var size = megaliths[i].sprite.size;
        
        if(boxCollides(pos, size, [player.pos[0] + dx, player.pos[1] + dy], player.sprite.size)) {
            return true;
        }
    }
    return false;
}

function handleInput(dt) {
    if(input.isDown('DOWN') || input.isDown('s')) {
        if(!(checkPlayerCollisions(0, playerSpeed * dt)))
        {
            player.pos[1] += playerSpeed * dt;
        }
    }

    if(input.isDown('UP') || input.isDown('w')) {
        if(!(checkPlayerCollisions(0, -playerSpeed * dt)))
        {
            player.pos[1] -= playerSpeed * dt;
        }
    }

    if(input.isDown('LEFT') || input.isDown('a')) {
        if(!(checkPlayerCollisions(dx = -playerSpeed * dt)))
        {
            player.pos[0] -= playerSpeed * dt;
        }
    }

    if(input.isDown('RIGHT') || input.isDown('d')) {
        if(!(checkPlayerCollisions(dx = playerSpeed * dt)))
        {
            player.pos[0] += playerSpeed * dt;
        }
    }

    if(input.isDown('SPACE') &&
       !isGameOver &&
       Date.now() - lastFire > 100) {
        var x = player.pos[0] + player.sprite.size[0] / 2;
        var y = player.pos[1] + player.sprite.size[1] / 2;

        bullets.push({ pos: [x, y],
                       dir: 'forward',
                       sprite: new Sprite('img/sprites.png', [0, 39], [18, 8]) });
        bullets.push({ pos: [x, y],
                       dir: 'up',
                       sprite: new Sprite('img/sprites.png', [0, 50], [9, 5]) });
        bullets.push({ pos: [x, y],
                       dir: 'down',
                       sprite: new Sprite('img/sprites.png', [0, 60], [9, 5]) });

        lastFire = Date.now();
    }
}

function updateEntities(dt) {
    // Update the player sprite animation
    player.sprite.update(dt);

    // Update all the bullets
    for(var i=0; i<bullets.length; i++) {
        var bullet = bullets[i];

        switch(bullet.dir) {
        case 'up': bullet.pos[1] -= bulletSpeed * dt; break;
        case 'down': bullet.pos[1] += bulletSpeed * dt; break;
        default:
            bullet.pos[0] += bulletSpeed * dt;
        }

        // Remove the bullet if it goes offscreen
        if(bullet.pos[1] < 0 || bullet.pos[1] > canvas.height ||
           bullet.pos[0] > canvas.width) {
            bullets.splice(i, 1);
            i--;
        }
    }

    // Update all the enemies
    for(var i=0; i<enemies.length; i++) {
        enemies[i].pos[0] -= enemySpeed * dt;
        enemies[i].sprite.update(dt);

        // Remove if offscreen
        if(enemies[i].pos[0] + enemies[i].sprite.size[0] < 0) {
            enemies.splice(i, 1);
            i--;
        }
    }

    // Update all the explosions
    for(var i=0; i<explosions.length; i++) {
        explosions[i].sprite.update(dt);

        // Remove if animation is done
        if(explosions[i].sprite.done) {
            explosions.splice(i, 1);
            i--;
        }
    }
    for(var i=0; i<manna.length; i++) {
        manna[i].sprite.update(dt);
    
        if(manna[i].sprite.done) {
            mannascore++;
            manna.splice(i, 1);
            i--;
        }
    }
}

// Collisions

function collides(x, y, r, b, x2, y2, r2, b2) {
    return !(r <= x2 || x > r2 ||
             b <= y2 || y > b2);
}

function boxCollides(pos, size, pos2, size2) {
    return collides(pos[0], pos[1],
                    pos[0] + size[0], pos[1] + size[1],
                    pos2[0], pos2[1],
                    pos2[0] + size2[0], pos2[1] + size2[1]);
}

function checkCollisions(dt) {
    checkPlayerBounds();
    
    for(var i=0; i<enemies.length; i++) {
        var pos = enemies[i].pos;
        var size = enemies[i].sprite.size;

        for(var j=0; j<bullets.length; j++) {
            var pos2 = bullets[j].pos;
            var size2 = bullets[j].sprite.size;

            if(boxCollides(pos, size, pos2, size2)) {
                enemies.splice(i, 1);
                i--;

                score += 100;

                explosions.push({
                    pos: pos,
                    sprite: new Sprite('img/sprites.png',
                                       [0, 117],
                                       [39, 39],
                                       16,
                                       [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                                       null,
                                       true)
                });

                bullets.splice(j, 1);
                break;
            }
        }


        if(boxCollides(pos, size, player.pos, player.sprite.size)) {
            gameOver();
        }
    }

    for(var i=0; i<manna.length; i++)
    {
        var pos = manna[i].pos;
        var size = manna[i].sprite.size;

        if (boxCollides(pos, size, player.pos, player.sprite.size))
        {
            manna[i].sprite.speed = 9;
        }
    }
    
    for(var i=0; i<megaliths.length; i++) {
        var megalithPos = megaliths[i].pos;
        var megalithSize = megaliths[i].sprite.size;

        for(var j=0; j<enemies.length; j++) {
            var enemiesPos = enemies[j].pos;
            var enemiesSize = enemies[j].sprite.size;
            if(boxCollides(megalithPos, megalithSize, enemiesPos, enemiesSize)) {
                enemies[j].pos[1]-=enemySpeed * dt;
                enemies[j].pos[0]+=enemySpeed * dt / 1.3;
            }
        }
    }

    for(var i=0; i<bullets.length; i++) {
        var pos = bullets[i].pos;
        var size = bullets[i].sprite.size;

        for(var j=0; j<megaliths.length; j++) {
            var pos2 = megaliths[j].pos;
            var size2 = megaliths[j].sprite.size;

            if(boxCollides(pos, size, pos2, size2)) {
                bullets.splice(i, 1);
                break;
            }
        }
    }
}

function checkPlayerBounds() {
    // Check bounds
    if(player.pos[0] < 0) {
        player.pos[0] = 0;
    }
    else if(player.pos[0] > canvas.width - player.sprite.size[0]) {
        player.pos[0] = canvas.width - player.sprite.size[0];
    }

    if(player.pos[1] < 0) {
        player.pos[1] = 0;
    }
    else if(player.pos[1] > canvas.height - player.sprite.size[1]) {
        player.pos[1] = canvas.height - player.sprite.size[1];
    }
}

// Draw everything
function render() {
    ctx.fillStyle = terrainPattern;
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    // Render the player if the game isn't over
    if(!isGameOver) {
        renderEntity(player);
    }

    renderEntities(bullets);
    renderEntities(megaliths);
    renderEntities(manna);
    renderEntities(enemies);
    renderEntities(explosions);
};

function renderEntities(list) {
    for(var i=0; i<list.length; i++) {
        renderEntity(list[i]);
    }    
}

function renderEntity(entity) {
    ctx.save();
    ctx.translate(entity.pos[0], entity.pos[1]);
    entity.sprite.render(ctx);
    ctx.restore();
}

// Game over
function gameOver() {
    document.getElementById('game-over').style.display = 'block';
    document.getElementById('game-over-overlay').style.display = 'block';
    isGameOver = true;
}

// Reset game to original state
function reset() {
    document.getElementById('game-over').style.display = 'none';
    document.getElementById('game-over-overlay').style.display = 'none';
    isGameOver = false;
    gameTime = 0;
    score = 0;
    mannascore = 0;

    enemies = [];
    bullets = [];

    player.pos = [50, canvas.height / 2];
};
