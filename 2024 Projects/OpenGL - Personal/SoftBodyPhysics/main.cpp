// Course:			IGME 309
// Student Name:	Joseph Staria
// Friday Exercise:	04

#include "GL/glew.h"
#include "GL/GL.h"

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
#include "SoftBody.h"

#include <iostream>
#include <fstream>
#include <sstream>
#include <string>

#include "PostProcessingClass.h"

using namespace std;
using namespace chrono;


float canvasSize[] = { 16.0f, 9.0f };
int rasterSize[] = { 1280, 720 };

// global parameters defining the ball
int vertNum = 30; // total number of vertices for the circle
vector<float> ballPos = vector<float>{ 8, 2.5f }; // center position of the circle
vector<float> anchorPos = vector<float>{ 8, 9 };
vector<float> ballColor = vector<float>{ 1.0f, .5f, 1.0f };
float radius = 0.3f; // circle's radius

glm::vec3 cameraPos = glm::vec3(0.0f, 0.0f, 3.0f);
glm::vec3 cameraTarget = glm::vec3(0.0f, 0.0f, 0.0f);
glm::vec3 cameraUp = glm::vec3(0.0f, 1.0f, 0.0f);

// Create view matrix
glm::mat4 viewMatrix = glm::lookAt(cameraPos, cameraTarget, cameraUp);

float fov = glm::radians(45.0f); // Field of view in radians
float aspectRatio = rasterSize[0] / rasterSize[1]; // Aspect ratio of your window
float nearPlane = 0.1f; // Near clipping plane
float farPlane = 100.0f; // Far clipping plane

// Create projection matrix
glm::mat4 projectionMatrix = glm::perspective(fov, aspectRatio, nearPlane, farPlane);

// bar
float barPos[] = { 5.0f, 2.0f };	// center position of the bar
float barSize = 3.0f;
float speed = 400.0f;

// tracking the game time - millisecond 
float deltaTime = 0;

int score = 0;
bool ballIsActive = false;
float prepareTime = 2.0f;

float oldTimeSinceStart = 0;


PostProcessingClass ppc;

//Shape ball;
//Shape anchor;
SoftBody softBody;

void SpawnBall() {
	//ball = Shape(ballPos, ballColor, .25, 40, 100, .55, true, true);
	//anchor = Shape(anchorPos, ballColor, .1f, 4, 1, 1, false, false);
	softBody = SoftBody(2, SoftBodyShape::Square, ballPos, 2.0f, 1000);
}

char v_shader_file[] =
"./shaders/basic.vert";

char f_shader_file[] =
"./shaders/basic.frag";

void init(void)
{
	std::srand(std::time(0));
	SpawnBall();

	ppc.Create(v_shader_file, f_shader_file);
}

void drawScene() {
	
	glUseProgram(0);
	glClearColor(0.3, 0.6, 0.3, 0.0);
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

	softBody.Draw();
}

void display(void)
{
	glDisable(GL_LIGHTING);
	//glEnable(GL_DEPTH_TEST);
	glEnable(GL_MULTISAMPLE);

	// Drawing the post-processing mesh will draw the scene inside it when rendering to the fbo
	ppc.Draw(drawScene, viewMatrix, projectionMatrix);

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

void mouse(Shape& shape) {

	vector<float> mousePosition = GetMousePosition();
	vector<float> ballPosition = shape.GetPosition();

	vector<float> force = { mousePosition[0] - ballPosition[0], mousePosition[1] - ballPosition[1] };

	float forceMag = sqrtf(force[0] * force[0] + force[1] * force[1]);

	float strength = 200;

	if (forceMag != 0)
		force = { force[0] / forceMag * strength, force[1] / forceMag * strength };

	if (MouseHandler::GetInstance()->isLeftButtonDown())
	{
		shape.ApplyForce(force);
		softBody.ApplyForce({ force[0] * .1f, force[1] * .1f });
	}

	if (MouseHandler::GetInstance()->isRightButtonDown())
		shape.ApplyForce(shape.GetPhysicsObj().GetDirection());
}

void update() {
	//curTime = glutGet(GLUT_ELAPSED_TIME); // returns the number of milliseconds since glutInit() was called.

	auto currentTime = high_resolution_clock::now();

	//deltaTime = (float)(curTime - preTime) / 1000; // frame-different time in seconds 
	float timeSinceStart = glutGet(GLUT_ELAPSED_TIME);
	deltaTime = (timeSinceStart - oldTimeSinceStart) / 100;
	GlobalVariables::GetInstance()->setDeltaTime(deltaTime);
	oldTimeSinceStart = timeSinceStart;

	//lastTime = currentTime;

	mouse(softBody.GetFirstShape());

	//printf("%f\n", deltaTime);
	//printf("{%f,%f}\n", softBody.GetFirstShape().GetPosition()[0], softBody.GetFirstShape().GetPosition()[1]);
	if (deltaTime < .5f)
		softBody.Update();
	//ball.GetPhysicsObj().ApplySpringForce(50, { 8,9.0f }, 2);
	//ball.Update();

	//preTime = curTime; // the curTime become the preTime for the next frame
	glutPostRedisplay();
}

int main(int argc, char* argv[])
{
	glutInit(&argc, argv);
	glutInitDisplayMode(GLUT_DOUBLE | GLUT_RGBA | GLUT_MULTISAMPLE);
	glutInitWindowSize(rasterSize[0], rasterSize[1]);
	glutCreateWindow("Pong!");

	glewInit();

	// Register mouse input callbacks with GLUT
	glutMouseFunc(MouseHandler::MouseButtonCallback);
	glutMotionFunc(MouseHandler::MouseMotionCallback);
	glutPassiveMotionFunc(MouseHandler::PassiveMouseMotionCallback);

	glutReshapeFunc(reshape);
	glutDisplayFunc(display);
	glutKeyboardFunc(keyboard);
	glutIdleFunc(update);

	init();

	glutMainLoop();
	return 0;
}
