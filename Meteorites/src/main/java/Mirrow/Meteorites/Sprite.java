package Mirrow.Meteorites;

import javafx.scene.canvas.GraphicsContext;
import javafx.scene.image.Image;

public class Sprite {

	public Vector position;
	public Vector velocity;
	public double rotation; // will be in degrees
	public Rectangle bounds;
	public Image img;
	public double elapsedTime; //in sec

	//default
	public Sprite() {
		this.position = new Vector();
		this.velocity = new Vector();
		this.bounds = new Rectangle();
		this.rotation=0;
		this.elapsedTime=0;
	}
	//calls upon the default constructor and uses setImage method
	public Sprite(String filename) {
		this();
		
		this.setImage(filename);
	}
	
	//sets the image to the given file and adjust the boundary size
	public void setImage(String filename) {
		this.img=new Image(this.getClass().getResourceAsStream(filename));
		this.bounds.setSize(this.img.getWidth(), this.img.getHeight());
		}
	
	public Rectangle getBounds() {
		this.bounds.setPos(this.position.x, this.position.y); // sets the boundary box when called
		return this.bounds;
	}
	//calls upon rectangle's overlap method whilst using the other sprite's bounds
	public boolean overlaps(Sprite other) {
		return this.getBounds().overlaps(other.getBounds());
	}
	// update position based on position, velocity and time taken
	public void update(double deltaTime) {
		this.elapsedTime+= deltaTime;
		this.position.add(this.velocity.x*deltaTime, this.velocity.y*deltaTime);
		this.wrap(800, 600);
	}
	
	//render the sprite
	//gc save and restore ensures that changes happen only to this sprite
	public void render(GraphicsContext gc) {
		gc.save();
		gc.translate(this.position.x, this.position.y);
		gc.rotate(this.rotation);
		//img.getWidth()/2 and height()/2 is to ensure that rotation happens in the centre
		gc.translate(-this.img.getWidth()/2, -this.img.getHeight()/2);
		gc.drawImage(img, 0, 0);
		gc.restore();
	}
	
	//wrap the object i.e. if the object leaves the screen, pop it out from the opposite end
		public void wrap(double screenWidth,double screenHeight){
			if(this.position.x + this.img.getWidth() <0){this.position.x = screenWidth;}
			if(this.position.x > screenWidth){this.position.x = -img.getWidth();}
			if(this.position.y + this.img.getHeight() <0){this.position.y = screenHeight;}
			if(this.position.y > screenWidth){this.position.y = -this.img.getHeight();}
		}
}
