using Godot;
using System;

public class Pause : Node
{
    ColorRect PauseMenu, ControlsMenu;
public override void _Ready()
    {
        PauseMenu = GetNode<ColorRect>("PauseMenu");
        ControlsMenu = GetNode<ColorRect>("Controls");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
      if(Input.IsActionJustPressed("ui_cancel")) {
          GetTree().Paused = !GetTree().Paused;
          PauseMenu.Visible = true;
      }
  }

  public void _on_Resume_pressed() {
      GetTree().Paused = !GetTree().Paused;
      PauseMenu.Visible = false;
  }

  public void _on_Controls_pressed() {
      ControlsMenu.Visible = true;
      PauseMenu.Visible = false;
  }

  public void _on_MainMenu_pressed() {
      GetTree().Paused = !GetTree().Paused;
      GetTree().ChangeScene("res://MainMenu.tscn");
  }

  public void _on_Back_pressed() {
        ControlsMenu.Visible = false;
        PauseMenu.Visible = true;
    }

}
