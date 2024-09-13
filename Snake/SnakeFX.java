package Mirrow.Snake;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

import javafx.animation.AnimationTimer;
import javafx.application.Application;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.input.KeyCode;
import javafx.scene.input.KeyEvent;
import javafx.scene.layout.BorderPane;
import javafx.scene.layout.VBox;
import javafx.scene.paint.Color;
import javafx.scene.text.Font;
import javafx.stage.Stage;

public class SnakeFX extends Application{
	
	//Declaring variables
	static int foodColor=0;
	static int speed = 5;
	static int width = 20;
	static int height = 20;
	static int foodXpos = 0;
	static int foodYpos = 0;
	static int cornerSize = 25;
	static List<Corner> snake = new ArrayList<>();
	//Dictates the direction the snake is going in
	static Dir direction = Dir.left;
	public enum Dir{
		left,right,up,down;
	}
	//Corner is what makes up the snake
	
	public static class Corner {
		int x, y;
		public Corner(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}
	static boolean gameOver = false;
	static Random rand = new Random();
	public static void main(String[] args) {launch(args);}
	
	@Override
	public void start(Stage primaryStage) throws Exception {
		
		VBox root = new VBox();
		Canvas canvas = new Canvas(width*cornerSize,height*cornerSize);
		GraphicsContext gc = canvas.getGraphicsContext2D();
		root.getChildren().add(canvas);
		
		new AnimationTimer() {
			long lastTick=0;
			@Override
			public void handle(long now) {
				if(lastTick==0) {
					lastTick=now;
					tick(gc);
					return;
					}
				if(now-lastTick>1000000000/speed) {
					lastTick = now;
					tick(gc);
				}
			}
			}.start();
		
		Scene scene = new Scene(root,width*cornerSize,height*cornerSize);
		//Player controls
		scene.addEventFilter(KeyEvent.KEY_PRESSED, key->{
			if(key.getCode()==KeyCode.W) {direction = Dir.up;}
			if(key.getCode()==KeyCode.S) {direction = Dir.down;}
			if(key.getCode()==KeyCode.A) {direction = Dir.left;}
			if(key.getCode()==KeyCode.D) {direction = Dir.right;}
		});
		//Builds up the start snake
		snake.add(new Corner(width/2,height/2));
		snake.add(new Corner(width/2,height/2));
		snake.add(new Corner(width/2,height/2));
		primaryStage.setScene(scene);
		primaryStage.setTitle("Snake Game in Java");
		primaryStage.show();
	}
	
	//Game tick
	public static void tick(GraphicsContext gc) {
		if(gameOver) {
			gc.setFill(Color.RED);
			gc.setFont(new Font("",50));
			gc.fillText("GAME OVER", 100, 250);
			return;
		}
		else {
			for(int i = snake.size()-1;i>=1;i--) {
				snake.get(i).x=snake.get(i-1).x;
				snake.get(i).y=snake.get(i-1).y;
			}
		//switch cases used to handle snake direction
		switch(direction) {
		case up:
			snake.get(0).y--;
			if(snake.get(0).y<0) {gameOver=true;}
			break;
		case down:
			snake.get(0).y++;
			if(snake.get(0).y>height) {gameOver=true;}
			break;
		case left:
			snake.get(0).x--;
			if(snake.get(0).x<0) {gameOver=true;}
			break;
		case right:
			snake.get(0).x++;
			if(snake.get(0).x>width) {gameOver=true;}
			break;
		}
		//When the snake gets to the food
		if(foodXpos==snake.get(0).x && foodYpos == snake.get(0).y) {
			snake.add(new Corner(-1,-1));
			//spawns a new food
			newFood();
		}
		//When the snake hits itself the game is over
		for(int i = 1; i<snake.size(); i++) {
			if(snake.get(0).x == snake.get(i).x && snake.get(0).y == snake.get(i).y) {
				gameOver=true;
			}
		}
		//Filling
		gc.setFill(Color.BLACK);
		gc.fillRect(0, 0, width*cornerSize, height*cornerSize);
		
		//Scoring
		gc.setFill(Color.WHITE);
		gc.setFont(new Font("",30));
		gc.fillText("Score: "+(speed-5), 10, 30);
		}
		
		//Random food colours
		Color cc = Color.WHITE;
		switch(foodColor) {
		case 0:
			cc=Color.PURPLE;
			break;
		case 1:
			cc=Color.YELLOW;
			break;
		case 2:
			cc=Color.CORAL;
			break;
		case 3:
			cc=Color.WHITE;
			break;
		case 4:
			cc=Color.BLUE;
			break;
		}
		gc.setFill(cc);
		gc.fillOval(foodXpos*cornerSize, foodYpos*cornerSize, cornerSize, cornerSize);
		
		//Snake
		for(Corner c:snake) {
			gc.setFill(Color.LIGHTGREEN);
			gc.fillRect(c.x*cornerSize, c.y*cornerSize, cornerSize-1, cornerSize-1);
			gc.setFill(Color.GREEN);
			gc.fillRect(c.x*cornerSize, c.y*cornerSize, cornerSize-2, cornerSize-2);
		}
	}
	//Foods
	public static void newFood() {
		start:while(true) {
			foodXpos=rand.nextInt(width);
			foodYpos=rand.nextInt(height);
			for(Corner c:snake) {
				if(c.x ==foodXpos && c.y ==foodYpos) {
					continue start;
				}
			}
			//Randomises food colour and speeds up the game by 1 unit
			foodColor = rand.nextInt(5);
			speed++;
			break;
		}
		
		
	}
	
}
