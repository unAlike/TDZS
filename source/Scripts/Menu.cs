using Godot;
using System;

public class Menu : Node
{
    Panel Control;
    bool InMenu = false;
    public override void _Ready()
    {
        Control = GetNode<Panel>("Controls");
    }

    public void _on_Play_pressed() {
        GetTree().ChangeScene("res://Level.tscn");
    }

    public void _on_Controls_pressed() {
        Control.Visible = true;
        InMenu = true;
    }

    public void _on_Quit_pressed() {
        GetTree().Quit();
    }

    public void _on_Back_pressed() {
        Control.Visible = false;
    }

    public override void _Process(float delta)
  {
      if(Input.IsActionJustPressed("ui_cancel") & InMenu == true) {
          Control.Visible = false;
      }
  }
}
