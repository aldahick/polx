using SFML.Graphics;
using SFML.System;
using SFML.Window;

var window = new RenderWindow(
  new VideoMode(new Vector2u(800, 600)),
  "POLX");

window.Closed += (sender, evt) => {
  window.Close();
};

var shape = new CircleShape(100f) {
  FillColor = Color.Green
};

while (window.IsOpen) {
  window.DispatchEvents();
  window.Clear();
  window.Draw(shape);
  window.Display();
}
