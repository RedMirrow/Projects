package Mirrow.Pong;


import java.util.Random;

import javafx.animation.KeyFrame;
import javafx.animation.Timeline;
import javafx.application.Application;
import javafx.scene.Scene;
import javafx.scene.canvas.Canvas;
import javafx.scene.canvas.GraphicsContext;
import javafx.scene.layout.Pane;
import javafx.scene.layout.StackPane;
import javafx.scene.paint.Color;
import javafx.scene.text.Font;
import javafx.scene.text.TextAlignment;
import javafx.stage.Stage;
import javafx.util.Duration;


public class PongFX extends Application{
	//			Variables needed for the game
	private static final double width = 700;
	private static final double height = 400;
	private static final double Player_Width = 15;
	private static final double Player_Height = 70;
	private static final double Ball_Radius = 15;
	//Player 1 position
	private double player1Xpos = 15;
	private double player1Ypos = height/2;
	//Player 2/CPU Position
	private double player2Xpos = width-(Player_Width*2);
	private double player2Ypos = height/2;
	//Ball position
	private double ballXpos = width/2;
	private double ballYpos = height/2;
	
	private int ball_speedX = 1;
	private int ball_speedY = 1;
	private int scorePl1 =0;
	private int scorePl2 =0;
	private boolean gameStart;
	private boolean CPUran = true;
	
	//Sets up the window for the pong game
	Pane root = new Pane();
	Canvas canvas;
	GraphicsContext gc;

	public static void main(String[] args) {launch(args);}

	@Override
	public void start(Stage stage) throws Exception {
		stage.setTitle("Pong Game in Java");
		canvas = new Canvas(width,height);
		gc = canvas.getGraphicsContext2D();
		
		Timeline tl = new Timeline(new KeyFrame(Duration.millis(10),e ->run(gc)));
		tl.setCycleCount(Timeline.INDEFINITE);
		//Mouse Controls for Player 1
		canvas.setOnMouseMoved(e-> player1Ypos=e.getY());
		canvas.setOnMouseClicked(e->gameStart = true);
		stage.setScene(new Scene(new StackPane(canvas)));
		stage.show();
		tl.play();
		
	}

	//The main game logic method of the program
	private void run(GraphicsContext gc) {
		//Set background to black
		gc.setFill(Color.BLACK);
		gc.fillRect(0, 0, width, height);
		//Set text to white and size 25
		gc.setFill(Color.WHITE);
		gc.setFont(Font.font(25));
		
		//Game Logic
		if(gameStart) {
			//Set ball movement
			ballXpos+=ball_speedX;
			ballYpos+=ball_speedY;
			
			//Simple CPU logic - will follow the ball
			if(CPUran) {if(ballXpos<width-width/4) {player2Ypos = ballYpos-(Player_Height/2);}
			else {
				player2Ypos = ballYpos > player2Ypos + Player_Height/2 ?player2Ypos+=1:player2Ypos-1;}}
			
			
			
			//Draw the ball 
			gc.fillOval(ballXpos, ballYpos, Ball_Radius, Ball_Radius);
		}
		else {
			gc.setStroke(Color.WHITE);
			gc.setTextAlign(TextAlignment.CENTER);
			
			//reset the position of the ball
			ballXpos = width/2;
			ballYpos = height/2;
			//reset ball speed and direction
			ball_speedX = new Random().nextInt(2)==0?1:-1;
			ball_speedY = new Random().nextInt(2)==0?1:-1;;
			}
		
		//Preventing the ball from moving out of canvas
		if(ballYpos>height||ballYpos<0) {ball_speedY *=-1;}
		
		//Scoring
		//CPU
		if(ballXpos < 0) {
			scorePl2++;
			gameStart = false;
		}
		//Player
		if(ballXpos > 700) {
					scorePl1++;
					gameStart = false;
		}
		//Ball speeds up over time whilst in play and within the canvas boundaries
		//and between the 2 players 
		if((ballXpos+Ball_Radius>player2Xpos)&& ballYpos>=player2Ypos 
				&& ballYpos<=player2Ypos+Player_Height || (ballXpos+Ball_Radius<player1Xpos+Player_Width)&&ballYpos>=player1Ypos && ballYpos <= player1Ypos+Player_Height) 
		{
			ball_speedX +=1 * Math.signum(ball_speedY);
			ball_speedY +=1 * Math.signum(ball_speedX);
			ball_speedX *= -1;
			ball_speedY *= -1;
		}
		//Drawing the score
		gc.fillText(scorePl1 + "\t\t\t\t\t\t\t\t\t\t" + scorePl2, width/2, 20);
		//Drawing the players
		gc.fillRect(player1Xpos, player1Ypos, Player_Width, Player_Height);
		gc.fillRect(player2Xpos, player2Ypos, Player_Width, Player_Height);
	}
	

}
