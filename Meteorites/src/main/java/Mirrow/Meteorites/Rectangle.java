package Mirrow.Meteorites;

public class Rectangle {
	// x,y, top left corner
	double x, y;
	double width,height;
	public Rectangle() { // default constructor
		this.setPos(0, 0);
		this.setSize(1, 1);
	}
	public Rectangle(double x, double y, double width, double height) {
		this.setPos(x, y);
		this.setSize(width,height);
	}
	
	public void setPos (double x, double y) {this.x=x;this.y=y;}
	public void setSize (double w, double h) {this.width=w;this.height=h;}
	public boolean overlaps(Rectangle other) {
		// If the rectangle is on the left||right||above||below the other
		// 4 cases of no overlap
		boolean noOverlap =
				this.x + this.width < other.x|| 
				other.x+other.width < this.x||
				this.y+this.height < other.y||
				other.y+other.y < this.y;
		
		return !noOverlap;//returns the inverse of the method
		
	}

}
