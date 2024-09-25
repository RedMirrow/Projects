package Mirrow.Meteorites;

public class Vector {
	double x;
	double y;

	public Vector() {double x,y;}//Default
	public Vector(double x, double y) {this.set(x, y);}
	
	//Methods
	public void set(double x, double y) {this.x=x;this.y=y;}
	public void add(double dx, double dy) {this.x=this.x+dx;this.y=this.y+dy;}
	//adds to x and y
	public void multi(double m) {this.x=this.x*m;this.y=this.y*m;} 
	// multiplies x and y
	
	//Lengths
	public double getLength() {return Math.sqrt(this.x*this.x+this.y*this.y);} 
	// uses pythagorean theorem to get the length
	public void setLength(double l) {
		double currLength = this.getLength();
		if (currLength == 0) {
			this.set(l, 0);
			
			}
		else {
			// scaling the vector to length 1
			this.multi(1/currLength);
			// multiplies to length l
			this.multi(l);
		}
		
	}
	
	// Angles
	public double getAngle() {return Math.toDegrees(Math.atan2(this.y, this.x));}
	public void setAngle(double degrees) {
		double length = this.getLength();
		double angleRadian = Math.toRadians(degrees);
		this.x= length * Math.cos(angleRadian);
		this.y= length * Math.sin(angleRadian);
	}
}
