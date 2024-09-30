#include "MouseHandler.h"

void MouseHandler::MouseButtonCallback(int button, int state, int x, int y)
{
	MouseHandler* handler = GetInstance();
	handler->HandleMouseButton(button, state, x, y);
}

void MouseHandler::MouseMotionCallback(int x, int y)
{
	MouseHandler* handler = GetInstance();
	handler->HandleMouseMotion(x, y);
}

void MouseHandler::PassiveMouseMotionCallback(int x, int y)
{
	MouseHandler* handler = GetInstance();
	handler->HandlePassiveMouseMotion(x, y);
}

void MouseHandler::HandleMouseButton(int button, int state, int x, int y)
{
	posX = x;
	posY = y;

	if (button == GLUT_LEFT_BUTTON)
		leftButtonDown = (state == GLUT_DOWN);

	// Not going to make else if just because I dont know if it would allow for both right and left to be pressed otherwise
	else if (button == GLUT_RIGHT_BUTTON)
		rightButtonDown = (state == GLUT_DOWN);
}

void MouseHandler::HandleMouseMotion(int x, int y)
{
	posX = x;
	posY = y;
}

void MouseHandler::HandlePassiveMouseMotion(int x, int y)
{
	posX = x;
	posY = y;
}

MouseHandler* MouseHandler::GetInstance()
{
	static MouseHandler instance;
	return &instance;
}
