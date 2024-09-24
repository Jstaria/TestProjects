// Course:			IGME 309
// Student Name:	Joseph Staria
// Friday Exercise:	04

#ifdef __APPLE__
#include <GLUT/glut.h>
#else
#include <GL/freeglut.h>
#endif

#include <cstdlib>
#include <ctime>
#include <chrono>

#include "Shape.h"
#include "MouseHandler.h"
#include "GlobalVariables.h"

using namespace std;
using namespace chrono;

//GLint fbo, textureColorBuffer;
//GLuint PPShader, vShader, fShader;

float canvasSize[] = { 16.0f, 9.0f };
int rasterSize[] = { 1280, 720 };

// global parameters defining the ball
int vertNum = 30; // total number of vertices for the circle
vector<float> ballPos = vector<float> { 5.0f, 8.0f }; // center position of the circle
vector<float> ballColor = vector<float>{ 1.0f, .5f, 1.0f };
float radius = 0.3f; // circle's radius

float velocity_x = 0.0f;
float velocity_y = 0.0f;

// bar
float barPos[] = { 5.0f, 2.0f };	// center position of the bar
float barSize = 3.0f;
float speed = 400.0f;

// tracking the game time - millisecond 
unsigned int curTime = 0;
unsigned int preTime = 0;
float deltaTime = 0;
high_resolution_clock::time_point lastTime;

int score = 0;
bool ballIsActive = false;
float prepareTime = 2.0f;

Shape ball;

void SpawnBall() {
	ball = Shape(ballPos, ballColor, .25, 40, 200, .25, true, true);
}

void CreateFrameBuffer() {
	
}

void init(void)
{
	lastTime = high_resolution_clock::now();
	std::srand(std::time(0));
	SpawnBall();
}

void display(void)
{
	glClearColor(0.3, 0.6, 0.3, 0.0);
	glClear(GL_COLOR_BUFFER_BIT);

	ball.Draw();

	glMatrixMode(GL_MODELVIEW);
	glLoadIdentity();

	glutSwapBuffers();
}

void reshape(int w, int h)
{
	rasterSize[0] = w;
	rasterSize[1] = h;

	glMatrixMode(GL_PROJECTION);
	glLoadIdentity();
	gluOrtho2D(0.0, canvasSize[0], 0.0, canvasSize[1]);
	glViewport(0, 0, rasterSize[0], rasterSize[1]);

	glutPostRedisplay();
}

void keyboard(unsigned char key, int x, int y)
{
	switch (key) {
	case 27:
		exit(0);
		break;
	case 'a':
		if (barPos[0] - speed * deltaTime - barSize / 2.0f > 0)
			barPos[0] -= speed * deltaTime;
		break;
	case 'd':
		if (barPos[0] + speed * deltaTime + barSize / 2.0f < canvasSize[0])
			barPos[0] += speed * deltaTime;
		break;	
	}
}

vector<float> GetMousePosition() {

	vector<float> position = { 0,0 };

	position[0] = (float)(MouseHandler::GetInstance()->getX()) / 1280 * 16;
	position[1] = (720 - (float)(MouseHandler::GetInstance()->getY())) / 720 * 9;

	return position;
}

void mouse() {

	vector<float> mousePosition = GetMousePosition();
	vector<float> ballPosition = ball.GetPosition();

	vector<float> force = { mousePosition[0] - ballPosition[0], mousePosition[1] - ballPosition[1] };

	float forceMag = sqrtf(force[0] * force[0] + force[1] * force[1]);

	float strength = 10;

	if (forceMag != 0)
		force = { force[0] / forceMag * strength, force[1] / forceMag * strength};

	if (MouseHandler::GetInstance()->isLeftButtonDown())
		ball.ApplyForce(force);

	if (MouseHandler::GetInstance()->isRightButtonDown())
		ball.ApplyForce({ 10,0 });
}

void update() {
	//curTime = glutGet(GLUT_ELAPSED_TIME); // returns the number of milliseconds since glutInit() was called.
	
	auto currentTime = high_resolution_clock::now();
	
	//deltaTime = (float)(curTime - preTime) / 1000; // frame-different time in seconds 
	deltaTime = duration_cast<duration<float>>(currentTime - lastTime).count() * 100;
	//printf("%f\n", deltaTime);
	GlobalVariables::GetInstance()->setDeltaTime(deltaTime);

	lastTime = currentTime;

	mouse();
	ball.Update([]() -> vector<float> {return ball.GetPhysicsObjPos(); });

	

	preTime = curTime; // the curTime become the preTime for the next frame
	glutPostRedisplay();
}

int main(int argc, char* argv[])
{
	init();
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA);
	glutInitWindowSize(rasterSize[0], rasterSize[1]);
	glutCreateWindow("Pong!");

	// Register mouse input callbacks with GLUT
	glutMouseFunc(MouseHandler::MouseButtonCallback);
	glutMotionFunc(MouseHandler::MouseMotionCallback);
	glutPassiveMotionFunc(MouseHandler::PassiveMouseMotionCallback);

	glutReshapeFunc(reshape);
	glutDisplayFunc(display);
	glutKeyboardFunc(keyboard);
	glutIdleFunc(update);

	preTime = glutGet(GLUT_ELAPSED_TIME);

	glutMainLoop();
	return 0;
}