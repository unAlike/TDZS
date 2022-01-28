using Godot;
using System;

public class Menu : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_Play_pressed() {
        GetTree().ChangeScene("res://Player.tscn");
    }

    public void _on_Controls_pressed() {
        GD.Print("Not implemented yet");
    }

    public void _on_Quit_pressed() {
        GD.Print("Not implemented yet");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
