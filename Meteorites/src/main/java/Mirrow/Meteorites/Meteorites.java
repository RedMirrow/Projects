package Mirrow.Meteorites;

import java.util.ArrayList;

import javafx.animation.AnimationTimer;
import javafx.application.Application;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.input.KeyEvent;
import javafx.scene.layout.BorderPane;
import javafx.scene.layout.Pane;
import javafx.scene.paint.Color;
import javafx.scene.text.Font;
import javafx.stage.Stage;

public class Meteorites extends Application{
	BorderPane root = new BorderPane();
	Scene scene = new Scene(root);
	Canvas canvas;
	GraphicsContext gc;
	double tempvelo=20;
	int score=0;
	int lives = 3;
	private boolean gameStart;
	
	public static void main(String[] args) {
		try {launch(args);}
		catch(Exception error){error.printStackTrace();}
		finally {System.exit(0);}
		}

	@Override
	public void start(Stage primaryStage) throws Exception {
		
		primaryStage.setTitle("Meteorites"); //set the window app name
		
		//Sets up the area for the graphics.
		
		
		primaryStage.setScene(scene);
		canvas = new Canvas(800,600);
		gc = canvas.getGraphicsContext2D();
		root.setCenter(canvas);
		canvas.setOnMouseClicked(e->gameStart = true);
		
		//Arraylist for sprites 
		ArrayList<Sprite> rockList = new ArrayList<Sprite>();
		ArrayList<Sprite> laserList = new ArrayList<Sprite>();
		
		int rockCount = 7;
		for (int c=0;c < rockCount;c++) {
			Sprite rock=new Sprite("rock.png");
			double x = 500*Math.random()+300; //300-800
			double y = 400*Math.random()+100; //100-500
			double angle = 360*Math.random(); //0-360
			rock.position.set(x, y);
			rock.velocity.setLength(45);
			rock.velocity.setAngle(angle);
			rockList.add(rock);		
		}
		//Input ArrayList
		ArrayList<String> keyPressList = new ArrayList<String>(); //continuous
		ArrayList<String> discreetKeyPress = new ArrayList<String>();
		scene.setOnKeyPressed(
				(KeyEvent event)->{
					String keyName = event.getCode().toString();
					//avoiding duplicates
					if(!keyPressList.contains(keyName)) {keyPressList.add(keyName);discreetKeyPress.add(keyName);}
				}
				);
		scene.setOnKeyReleased(
				(KeyEvent event)->{
					String keyName = event.getCode().toString();
					//avoiding duplicates
					if(keyPressList.contains(keyName)) {keyPressList.remove(keyName);}
				}
				);
		//Event listener setup
		
		//Sprite generation
		Sprite background = new Sprite("space.png");
		background.position.set(400,300);
		
		//Player ship sprite
		Sprite player = new Sprite("ship.png");
		player.position.set(100, 300);
		//Game loop timer
		AnimationTimer gameLoop = new AnimationTimer() {
			public void handle(long nanotime) {
				background.render(gc);
				player.render(gc);
				//Game starts when the button is pressed
				if(gameStart) {
					
					if(lives>0) {
						//Handling inputs from the keyPressList array
						
						//Rotates the player by 3 
						if(keyPressList.contains("LEFT")) {player.rotation-=3;player.velocity.setAngle(player.rotation);} 
						if(keyPressList.contains("RIGHT")) {player.rotation+=3;player.velocity.setAngle(player.rotation);}
						if(keyPressList.contains("UP")) {
							if(tempvelo < 100) {
								if(tempvelo < 50) {tempvelo =tempvelo+1;}
								else {tempvelo =tempvelo+0.5;}
								}
							
							player.velocity.setLength(tempvelo);
							player.velocity.setAngle(player.rotation);
							}
						
						if(keyPressList.contains("DOWN")) {
							if(tempvelo < 100) {
								if(tempvelo < 50) {
									if(tempvelo>5) {tempvelo =tempvelo-0.5;}
									}
								else {tempvelo =tempvelo-1;}
								}
							
							player.velocity.setLength(tempvelo);
							player.velocity.setAngle(player.rotation);
							}
						if(discreetKeyPress.contains("SPACE")) {
							Sprite laser = new Sprite("plasmaball.png");
							laser.position.set(player.position.x, player.position.y);
							laser.velocity.setLength(200);
							laser.velocity.setAngle(player.rotation);
							laserList.add(laser);
							}
						discreetKeyPress.clear();
						
						//Deceleration when player does not press up arrow with a set minimum speed
						if(!keyPressList.contains("UP")&& tempvelo>5) {
							tempvelo=tempvelo-0.1;
							player.velocity.setLength(tempvelo);
							player.velocity.setAngle(player.rotation);
							}
						
						//Sprite graphics updates
						player.update(1/60.0);
						
						//Uses a for loop with a counter to be able to remove laser sprites that existed for longer than 2.5 sec
						for(int n=0; n<laserList.size(); n++) {
							Sprite laser = laserList.get(n);
							laser.update(1/60.0);
							if(laser.elapsedTime > 2.5) {laserList.remove(n);}
							
							}
						
						//Uses a for loop with a counter to generate and update rocks
						for(int n=0; n<rockList.size(); n++) {
							Sprite rock = rockList.get(n);
							rock.update(1/60.0);
							}
						
						for(Sprite laser : laserList) laser.render(gc);
						for(Sprite rock : rockList) rock.render(gc);
						//When laser overlaps a rock, remove both
						for(int laserNum=0; laserNum<laserList.size();laserNum++) {
							Sprite laser = laserList.get(laserNum);
							for(int rockNum=0; rockNum<rockList.size();rockNum++) {
								Sprite rock = rockList.get(rockNum);
								if(laser.overlaps(rock)) {
									laserList.remove(laserNum);
									//If the rock size is not original do not run this code
									if(rock.img.getWidth()>40) {
										//Create 3 more rocks after a larger rock is hit by a laser
										int newRockCount = 3;
										for (int c=0;c < newRockCount;c++) {
											Sprite newRock=new Sprite("tinyRock.png");
											double x = rock.position.x;
											double y = rock.position.y;
											double angle = 360*Math.random(); //0-360
											newRock.position.set(x, y);
											newRock.velocity.setLength(45);
											newRock.velocity.setAngle(angle);
											rockList.add(newRock);
										}
									}
									rockList.remove(rockNum);
									score=score+50;
									}
							}
						}
						//Generate more rocks if there are no asteroids to destroy
						if(rockList.size()==0) {
							int newRockCount = 7;
							for (int c=0;c < newRockCount;c++) {
								Sprite rock=new Sprite("rock.png");
								double x = 500*Math.random()+300; //300-800
								double y = 400*Math.random()+100; //100-500
								double angle = 360*Math.random(); //0-360
								rock.position.set(x, y);
								rock.velocity.setLength(45);
								rock.velocity.setAngle(angle);
								rockList.add(rock);		
							}
						}
						
						//If a rock hits the player ship, reduce score and remove a life
						//Game over at 0 lives
						for(int rockNum=0; rockNum<rockList.size();rockNum++) {
							Sprite rock = rockList.get(rockNum);
							if(player.overlaps(rock)) {
								rockList.remove(rockNum);
								score=score-200;
								lives = lives-1;
								if(lives==0||lives<0) {gameStart = false;}
								}
							}
						}
					//The player can try again with a fresh score and live count
					if(lives==0||lives<0) {
						//Reset position, lives and score
						player.position.set(100, 300);
						score = 0;
						lives = 3;
						//Empty the rockList from rocks to start anew
						for(int rockNum=0; rockNum<rockList.size();rockNum++) {rockList.remove(rockNum);}
						int newRockCount = 7;
						for (int c=0;c < newRockCount;c++) {
							Sprite rock=new Sprite("rock.png");
							double x = 500*Math.random()+300; //300-800
							double y = 400*Math.random()+100; //100-500
							double angle = 360*Math.random(); //0-360
							rock.position.set(x, y);
							rock.velocity.setLength(45);
							rock.velocity.setAngle(angle);
							rockList.add(rock);		
						}
						}
					}
				//Score text
				gc.setFill(Color.WHITE);
				gc.setStroke(Color.LIME);
				String text = "Score: "+score;
				int textX = 600;
				int textY = 80;
				gc.setFont(new Font("Arial Black",30));
				gc.setLineWidth(1);
				gc.fillText(text, textX, textY);
				gc.strokeText(text, textX, textY);
				//Lives Count
				text = "Lives: "+lives;
				textX = 600;
				textY = 110;
				gc.setFont(new Font("Arial Black",30));
				gc.setLineWidth(1);
				gc.fillText(text, textX, textY);
				gc.strokeText(text, textX, textY);
			}
		};
		gameLoop.start();

		primaryStage.show();
	}

}
