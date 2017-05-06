"use strict";

Array.prototype.flatten = function() {
  var temp = [];
  function recursiveFlatten(arr) {
    for(var i = 0; i < arr.length; i++) {
      if(Array.isArray(arr[i])) {
        recursiveFlatten(arr[i]);
      } else {
        temp.push(arr[i]);
      }
    }
  }
  recursiveFlatten(this);
  return temp;
}

var GameOfLife = {
  Constants: {
    DEAD: 0,
    ALIVE: 1,
    SETUP: 4,
    SIMULATING: 5,
    PAUSE: 6,
  },
  Speed: 1000,
  Steps: 0,
  Width: 20,
  Height: 20,
  Data: [],
  FirstGeneration: [],
  Phase: 4, /* SETUP */
  Init: function() {
    for (var w = 0; w < GameOfLife.Width; w++) {
      GameOfLife.Data[w] = [];
      for (var h = 0; h < GameOfLife.Height; h++) {
        GameOfLife.Data[w][h] = GameOfLife.Constants.DEAD;
      }
    }
    var world = GameOfLife.GetWorld();
    var html = "";
    for (var h = 0; h < GameOfLife.Height; h++) {
      html += "<tr>"
      for (var w = 0; w < GameOfLife.Width; w++) {
        html += "<td data-live='" + GameOfLife.Data[w][h] + "' data-x='"+w+"' data-y='"+h+"' onClick='GameOfLife.OnClick(this)'><div class='hover'></div></td>"
      }
      html += "</tr>"
    }
    world.innerHTML = html;
    world.setAttribute("style","width:"+(GameOfLife.Width / GameOfLife.Height) * 100 + "vh");
  },
  GetWorld: function() {
    return document.querySelector('.world');
  },
  GetCell: function(x, y) {
    return document.querySelector("[data-x='"+x+"'][data-y='"+y+"']");
  },
  OnClick: function(elem) {
    if(GameOfLife.Phase == GameOfLife.Constants.SETUP || GameOfLife.Phase == GameOfLife.Constants.PAUSE)
    {
      var x = elem.getAttribute("data-x");
      var y = elem.getAttribute("data-y");
      GameOfLife.ToggleLive(x, y);
    }
  },
  Start: function() {
    if(GameOfLife.Phase == GameOfLife.Constants.SETUP){
      GameOfLife.SetPhase(GameOfLife.Constants.SIMULATING);
      GameOfLife.FirstGeneration = GameOfLife.Data;
      var start = document.querySelector('.start');
      setTimeout(GameOfLife.Step, GameOfLife.Speed);
    }
  },
  SetPhase: function(phase) {
    GameOfLife.Phase = phase;
    document.body.setAttribute("data-phase", phase);
  },
  Step: function() {
    if(GameOfLife.Extinguished()) {
      return GameOfLife.GameOver();
    }
    if(GameOfLife.Phase == GameOfLife.Constants.PAUSE){
      return;
    }
    GameOfLife.SetStep(GameOfLife.Steps + 1);
    var newData = [];
    for (var x = 0; x < GameOfLife.Width; x++) {
      newData[x] = [];
      for (var y = 0; y < GameOfLife.Height; y++) {
        var aliveNeighbours = GameOfLife.AliveNeighbours(x, y);
        var currentLive = GameOfLife.Data[x][y];
        newData[x][y] =currentLive;
        if(currentLive == GameOfLife.Constants.ALIVE) {
          if(aliveNeighbours < 2 || aliveNeighbours > 3){
            newData[x][y] = GameOfLife.Constants.DEAD;
          }
        } else if(aliveNeighbours == 3){
            newData[x][y] = GameOfLife.Constants.ALIVE;
        }
      }
    }
    GameOfLife.Data = newData;
    GameOfLife.UpdateWorld();
    setTimeout(GameOfLife.Step, GameOfLife.Speed);
  },
  ToggleLive: function(x, y) {
    var live = GameOfLife.Data[x][y];
    if(live == GameOfLife.Constants.ALIVE) {
      GameOfLife.Data[x][y] = GameOfLife.Constants.DEAD;
      GameOfLife.GetCell(x,y).setAttribute("data-live", GameOfLife.Constants.DEAD)
    }
    else {
      GameOfLife.Data[x][y] = GameOfLife.Constants.ALIVE;
      GameOfLife.GetCell(x,y).setAttribute("data-live", GameOfLife.Constants.ALIVE)
    }
  },
  AliveNeighbours: function(x, y) {
    return [GameOfLife.FetchLive(x-1, y-1), GameOfLife.FetchLive(x-1, y), GameOfLife.FetchLive(x-1, y+1),
     GameOfLife.FetchLive(x, y-1), GameOfLife.FetchLive(x, y+1),
     GameOfLife.FetchLive(x+1, y-1), GameOfLife.FetchLive(x+1, y), GameOfLife.FetchLive(x+1, y+1)].filter(function(live) {return live == GameOfLife.Constants.ALIVE}).length;
  },
  FetchLive: function(x, y){
    if(x < 0 || y < 0 ) {
      return GameOfLife.Constants.DEAD;
    }
    if(x >= GameOfLife.Width || y >= GameOfLife.Height ) {
      return GameOfLife.Constants.DEAD;
    }
    return GameOfLife.Data[x][y];
  },
  UpdateWorld: function() {
    for (var y = 0; y < GameOfLife.Height; y++) {
      for (var x = 0; x < GameOfLife.Width; x++) {
        GameOfLife.GetCell(x, y).setAttribute("data-live", GameOfLife.Data[x][y]);
      }
    }
  },
  Extinguished: function() {
    return !GameOfLife.Data.flatten().some(function(live){return live == GameOfLife.Constants.ALIVE});
  },
  GameOver: function() {
    GameOfLife.SetPhase(GameOfLife.Constants.PAUSE);
    if(confirm("Game over.\nWant to reload your first generation?")){
        GameOfLife.Data = GameOfLife.FirstGeneration;
        GameOfLife.UpdateWorld();
    }
    GameOfLife.SetStep(0);
    document.querySelector('.pause').innerHTML = "Continue";
  },
  SetStep: function(step) {
    GameOfLife.Steps = step;
    document.querySelector('.steps').innerHTML = "Generation: " + (step + 1);
  },
  PauseContinue: function() {
    if(GameOfLife.Phase == GameOfLife.Constants.SIMULATING) {
      GameOfLife.SetPhase(GameOfLife.Constants.PAUSE);
      document.querySelector('.pause').innerHTML = "Continue";
    } else if(GameOfLife.Phase == GameOfLife.Constants.PAUSE) {
      GameOfLife.SetPhase(GameOfLife.Constants.SIMULATING);
      document.querySelector('.pause').innerHTML = "Pause";
      GameOfLife.Step();
    }
  },
  Reset: function() {
    GameOfLife.Data = GameOfLife.FirstGeneration;
    GameOfLife.SetStep(0);
    GameOfLife.UpdateWorld();
  },
};
