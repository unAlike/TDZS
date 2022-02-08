using Godot;
using System;

public class Menu : Node
{
    ColorRect Controls;
    ColorRect Main;
    bool InMenu = false;
    public override void _Ready()
    {
        Controls = GetNode<ColorRect>("Controls");
        Main = GetNode<ColorRect>("Main");
    }

    public void _on_Play_pressed() {
        GetTree().ChangeScene("res://Level.tscn");
    }

    public void _on_Controls_pressed() {
        Controls.Visible = true;
        Main.Visible = false;
        InMenu = true;
    }

    public void _on_Quit_pressed() {
        GetTree().Quit();
    }

    public void _on_Back_pressed() {
        Controls.Visible = false;
        Main.Visible = true;
    }

    public override void _Process(float delta)
  {
      if(Input.IsActionJustPressed("ui_cancel") & InMenu == true) {
          Controls.Visible = false;
          Main.Visible = true;
      }
  }
}
