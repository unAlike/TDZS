using Godot;
using System;

public class GameLoop : Node2D
{
	float time = 0;
	public int kills = 0;
	int lastLevelUp = 0;
	RandomNumberGenerator rng = new RandomNumberGenerator();
	PackedScene ZombieScene;
	PlayerMovement player;
	Navigation2D nav;
	int zomcount = 0;
	public int level = 1;
	int trackNum = 0;
	public override void _Ready()
	{
		ZombieScene = GD.Load<PackedScene>("res://Zombie.tscn");
		player = (PlayerMovement)GetNode<KinematicBody2D>("Player");
		nav = GetNode<Navigation2D>("Navigation2D");

	}
	public override void _Process(float delta)
	{
		if(GetNode<KinematicBody2D>("Player").GetNode<Camera2D>("Camera2D").Zoom <= new Vector2(2+level*.05f,2+level*.05f)){
			GetNode<KinematicBody2D>("Player").GetNode<Camera2D>("Camera2D").Zoom += new Vector2(.001f,.001f);
		}
		
		kills = player.player.GetKills();
		delta = delta * (1+(level*.1f));
		time += delta;
		if(Math.Floor(time)>zomcount){
			SpawnZombie();
			zomcount++;
			//GD.Print("Spawned Zombie");
			GetNode<AudioStreamPlayer2D>("LevelUpSound").Play();
		}
		if(kills%25 == 0){
			// GD.Print(kills);
			if(kills!=lastLevelUp){
				lastLevelUp=kills;
				level++;
				// GD.Print("Level Up");
			}
		}
		//GD.Print(GetNode<AudioStreamPlayer>("Track1").GetPlaybackPosition());
		if(level>=0&&level<3){
			if(trackNum<1){
				trackNum++;
				GetNode<AudioStreamPlayer>("Track1").Playing = true;
				GetNode<AudioStreamPlayer>("Track2").Playing = false;
				GetNode<AudioStreamPlayer>("Track3").Playing = false;
			}
		}
		else if(level>2&&level<5){
			if(trackNum<2){
				
				if(GetNode<AudioStreamPlayer>("Track1").GetPlaybackPosition() >= 4.7f){
					GetNode<AudioStreamPlayer>("Track1").Playing = false;
					GetNode<AudioStreamPlayer>("Track2").Playing = true;
					GetNode<AudioStreamPlayer>("Track3").Playing = false;
					trackNum++;
				}

			}
		}
		else if(level>4&&level<7){
			if(trackNum<3){
				if(GetNode<AudioStreamPlayer>("Track2").GetPlaybackPosition() >= 4.7f){
					GetNode<AudioStreamPlayer>("Track1").Playing = false;
					GetNode<AudioStreamPlayer>("Track2").Playing = false;
					GetNode<AudioStreamPlayer>("Track3").Playing = true;
					trackNum++;
				}
			}
		}
		else if(level>6&&level<9){
			if(trackNum<4){
				trackNum++;
			}
		}
	}

	public void SpawnZombie(){
		rng.Randomize();
		TileMap tile = GetNode<TileMap>("TileMap");
		bool spawned = false;
		Rect2 rect = tile.GetUsedRect();
		int x = rng.RandiRange((int)(rect.Position.x),(int)(rect.End.x));
		int y = rng.RandiRange((int)(rect.Position.y),(int)(rect.End.y));
		while(!spawned){
			rng.Randomize();
			if(tile.GetCell(x,y) == TileMap.InvalidCell || tile.GetCell(x,y) == 3){
				x = rng.RandiRange((int)(rect.Position.x),(int)(rect.End.x));
				y = rng.RandiRange((int)(rect.Position.y),(int)(rect.End.y));
				//GD.Print(x + " " + y + " Invalid Spawn");
			}
			else{

				Zombie zom = ZombieScene.Instance<Zombie>();
				
				zom.setTarget(player);
				zom.setNav(nav);
				GetParent().AddChild(zom);
				zom.GlobalPosition = new Vector2(tile.MapToWorld(new Vector2(x,y)));
				spawned = true;
				//GD.Print("Spanwed Zombie at: X: " + x + "  Y: "+y);
			}
		}
	}

}
