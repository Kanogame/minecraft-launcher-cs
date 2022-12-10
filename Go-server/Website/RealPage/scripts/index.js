var canvas = document.getElementById("canvas"),
    ctx = canvas.getContext('2d');

canvas.width = 1920;
canvas.height = 800;

var stars = [], 
    FPS = 120,
    x = 5,
    mouse = {
      x: 0,
      y: 0
    };

for (var i = 0; i < x; i++) {
  stars.push({
    x: Math.random() * canvas.width,
    y: Math.random() * canvas.height,
    radius: Math.random() * 1 + 1,
    vx: Math.floor(Math.random() * 50) - 25,
    vy: Math.floor(Math.random() * 50) - 25
  });
}

function draw() {
  ctx.clearRect(0,0,canvas.width,canvas.height);
  
  for (var i = 0, x = stars.length; i < x; i++) {
    var s = stars[i];
  
    ctx.fillStyle = "#5755b3";
    ctx.beginPath();
    ctx.arc(s.x, s.y, 100, 0, 2 * Math.PI);
    ctx.fill();
    ctx.fillStyle = '#5755b3';
    ctx.stroke();
  }
  
  ctx.beginPath();
  for (var i = 0, x = stars.length; i < x; i++) {
    var starI = stars[i];
    ctx.moveTo(starI.x,starI.y); 
    if(distance(mouse, starI) < 150) ctx.lineTo(mouse.x, mouse.y);
    for (var j = 0, x = stars.length; j < x; j++) {
      var starII = stars[j];
      if(distance(starI, starII) < 150) {
        ctx.lineTo(starII.x,starII.y); 
      }
    }
  }
  ctx.lineWidth = 0.05;
  ctx.strokeStyle = '#5755b3';
  ctx.stroke();
  ctx.filter = 'blur(40px)';
}

function distance( point1, point2 ){
  var xs = 0;
  var ys = 0;
 
  xs = point2.x - point1.x;
  xs = xs * xs;
 
  ys = point2.y - point1.y;
  ys = ys * ys;
 
  return Math.sqrt( xs + ys );
}

function update() {
  for (var i = 0, x = stars.length; i < x; i++) {
    var s = stars[i];
  
    s.x += s.vx / FPS;
    s.y += s.vy / FPS;
    
    if (s.x < 0 || s.x > canvas.width) s.vx = -s.vx;
    if (s.y < 0 || s.y > canvas.height) s.vy = -s.vy;
  }
}

function tick() {
  draw();
  update();
  requestAnimationFrame(tick);
}


tick();