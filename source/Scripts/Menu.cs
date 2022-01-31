using Godot;
using System;

public class Menu : Node
{
    Panel Control;
    public override void _Ready()
    {
        Control = GetNode<Panel>("Controls");
    }

    public void _on_Play_pressed() {
        GetTree().ChangeScene("res://Player.tscn");
    }

    public void _on_Controls_pressed() {
        Control.Visible = true;
    }

    public void _on_Quit_pressed() {
        GetTree().Quit();
    }

    public void _on_Back_pressed() {
        Control.Visible = false;
    }
}
