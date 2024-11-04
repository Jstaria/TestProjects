#ifdef __APPLE__
#include <GLUT/glut.h>
#else
#include <GL/freeglut.h>
#endif

#include <vector>

#include <ctime>
#include <cmath>

#include <iostream>

using namespace std;

float canvasSize[] = { 20.0f, 20.0f };
int rasterSize[] = { 600, 600 };

// tracking the game time - millisecond 
unsigned int curTime = 0;
unsigned int preTime = 0;
float deltaTime = 0;

float rotationAngle = 0.0f;
float rotateSpeed = 90.0f;
int vertNum = 30;
float radius = 2.0f;

vector<float> ballPoint = { 10.0f, 7.5f };
vector<float> penPoint = { 10.0f, 15.0f };

void DrawFilledCircle(float red, float green, float blue, float centerX, float centerY, float radius) {

	glColor3f(red, green, blue);
	glBegin(GL_POLYGON);

	float radians = 2 * (atan(1) * 4) / vertNum;

	for (int i = 0; i < vertNum; i++)
	{
		glVertex2f(centerX + radius * cos(radians * i), centerY + radius * sin(radians * i));
	}
	glEnd();
}

void display(void)
{
	glClearColor(0.9f, 0.9f, 0.7f, 1.0);
	glClear(GL_COLOR_BUFFER_BIT);

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	
	/*****************************/
	// write your code below
	
	float radians = rotateSpeed * 3.1415 / 180;

	cout << rotationAngle << endl;

	ballPoint[0] = ballPoint[0] - penPoint[0];
	ballPoint[1] = ballPoint[1] - penPoint[1];

	float tempX = (ballPoint[0] * cos(radians * deltaTime)) - (ballPoint[1] * sin(radians * deltaTime));
	float tempY = (ballPoint[0] * sin(radians * deltaTime)) + (ballPoint[1] * cos(radians * deltaTime));

	ballPoint[0] = tempX;
	ballPoint[1] = tempY;

	ballPoint[0] = ballPoint[0] + penPoint[0];
	ballPoint[1] = ballPoint[1] + penPoint[1];

	//cout << ballPoint[0] << "," << ballPoint[1] << endl;

	glColor3f(1.0, 1.0, 1.0);
	glLineWidth(10);
	glBegin(GL_POINTS);

	glVertex2f(penPoint[0], penPoint[1]);
	glEnd();

	glColor3f(1.0, 1.0, 1.0);
	glLineWidth(10);
	glBegin(GL_LINE_STRIP);

	glVertex2f(penPoint[0], penPoint[1]);
	glVertex2f(ballPoint[0], ballPoint[1]);
	glEnd();

	DrawFilledCircle(1.0, .5, .5, ballPoint[0], ballPoint[1], radius);

	// write your code above
	/*****************************/

	glutSwapBuffers();
}

void reshape(int w, int h)
{
	rasterSize[0] = w;
	rasterSize[1] = h;

	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluOrtho2D(0, canvasSize[0], 0, canvasSize[1]);
	glViewport(0, 0, rasterSize[0], rasterSize[1]);

	glutPostRedisplay();
}


void update() {
	curTime = glutGet(GLUT_ELAPSED_TIME);
	deltaTime = (float)(curTime - preTime) / 1000.0f; 


	if (rotationAngle > 45.0f) {
		rotateSpeed = -abs(rotateSpeed);
	}
	else if (rotationAngle < -45.0f) {
		rotateSpeed = abs(rotateSpeed);
	}
	rotationAngle += deltaTime * rotateSpeed;

	preTime = curTime;
	glutPostRedisplay();
}


int main(int argc, char* argv[])
{
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA);
	glutInitWindowSize(rasterSize[0], rasterSize[1]);
	glutCreateWindow("Simple Pendulum");

	glutReshapeFunc(reshape);
	glutDisplayFunc(display);
	glutIdleFunc(update);
	
	preTime = glutGet(GLUT_ELAPSED_TIME);
	glutMainLoop();
	return 0;
}