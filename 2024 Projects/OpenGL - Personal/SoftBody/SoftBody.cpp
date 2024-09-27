#include "SoftBody.h"

void SoftBody::CreateSoftBody(SoftBodyShape shape, int size, vector<float> position, float restlength)
{
	vector<float> color = { 1.0,1.0,1.0 };

	switch (shape)
	{
		case Square:

			for (int i = 0; i < size; i++)
			{
				softBody.push_back(vector<Shape> {});
				softBodyPos.push_back(vector<vector<float>> {});

				for (int j = 0; j < size; j++)
				{
					vector<float> newPos = { position[0] + i * restlength, position[1] + j * restlength };

					softBodyPos[i].push_back(newPos);
					softBody[i].push_back(Shape(softBodyPos[i][j], color, .05, 15, 40, .5, true, true));
				}
			}

			printf("SoftBody Created\n");

			break;
		case Circle:

			int radius = size - 1;
			int centerX = size;
			int centerY = size;

			for (int i = 0; i < size * 2; i++)
			{
				softBody.push_back(vector<Shape> {});
				softBodyPos.push_back(vector<vector<float>> {});

				for (int j = 0; j < size * 2; j++)
				{
					vector<float> newPos = { position[0] + i * restlength, position[1] + j * restlength };

					softBodyPos[i].push_back(newPos);
					softBody[i].push_back(Shape());

					int distSquared = (i - centerX) * (i - centerX) + (j - centerY) * (j - centerY);
					if (distSquared <= radius * radius)
					{
						softBody[i][j] = (Shape(softBodyPos[i][j], color, .05, 15, 40, .5, true, true));
					}
				}
			}

			break;
	}
}

void SoftBody::ApplyForce(vector<float> force)
{
	for (int i = 0; i < softBody.size(); i++)
	{
		for (int j = 0; j < softBody[i].size(); j++)
		{
			softBody[i][j].ApplyForce(force);
		}
	}
}

void SoftBody::ApplySpringForce(vector<int> arrayPos, vector<float> center, float springConstant, float restLength)
{
	for (int i = 0; i < softBody.size(); i++)
	{
		for (int j = 0; j < softBody[i].size(); j++)
		{
			/*if (arrayPos[0] == 0 || arrayPos[1] == 0 || arrayPos[0] == softBody.size() - 1 || arrayPos[1] == softBody[0].size() - 1) {
				if (arrayPos[0] == 0 && i == -1) continue;
				if (arrayPos[1] == 0 && j == -1) continue;
				if (arrayPos[0] == softBody.size() - 1 && i == 1) continue;
				if (arrayPos[1] == softBody[0].size() - 1 && j == 1) continue;
			}

			if (i == 0 && j == 0) continue;
			if (!softBody[i + arrayPos[0]][j + arrayPos[1]].isActive) continue;*/
			if (i == arrayPos[0] && j == arrayPos[1]) continue;
			
			vector<float> position = softBody[i][j].GetPosition();
			float distance = sqrtf(pow(i - arrayPos[0], 2) + pow(j - arrayPos[1], 2));

			if (distance > 2.0f) continue;

			softBody[i][j].GetPhysicsObj().ApplySpringForce(springConstant, center, restLength);
		}
	}
}

SoftBody::SoftBody(int size, SoftBodyShape shape, vector<float> position,
	float restLength, float springConstant) : 
	restLength(restLength), springConstant(springConstant), size(size)
{
	CreateSoftBody(shape, size, position, restLength);
}

void SoftBody::Update()
{
	for (int i = 0; i < softBody.size(); i++)
	{
		for (int j = 0; j < softBody[i].size(); j++)
		{
			if (!softBody[i][j].isActive) continue;
			ApplySpringForce({ i,j }, softBody[i][j].GetPosition(), springConstant, restLength);
			softBody[i][j].Update();
		}
	}
}

void SoftBody::Draw()
{
	glColor3f(1.0, .5, 1.0);
	glBegin(GL_TRIANGLES);

	glVertex2d(softBody[0][0].GetPosition()[0], softBody[0][0].GetPosition()[1]);
	glVertex2d(softBody[1][0].GetPosition()[0], softBody[1][0].GetPosition()[1]);
	glVertex2d(softBody[0][1].GetPosition()[0], softBody[0][1].GetPosition()[1]);

	glVertex2d(softBody[0][1].GetPosition()[0], softBody[0][1].GetPosition()[1]);
	glVertex2d(softBody[1][1].GetPosition()[0], softBody[1][1].GetPosition()[1]);
	glVertex2d(softBody[1][0].GetPosition()[0], softBody[1][0].GetPosition()[1]);

	glEnd();

	for (int i = 0; i < softBody.size(); i++)
	{
		for (int j = 0; j < softBody[i].size(); j++)
		{
			if (!softBody[i][j].isActive) continue;

			/*glLineWidth(10);
			glColor3f(.5, .5, .5);
			glBegin(GL_LINE_STRIP);
			glVertex2d(softBody[i][j].GetPosition()[0], softBody[i][j].GetPosition()[1]);

			if (i != size - 1)
				glVertex2d(softBody[i + 1][j].GetPosition()[0], softBody[i + 1][j].GetPosition()[1]);

			glVertex2d(softBody[i][j].GetPosition()[0], softBody[i][j].GetPosition()[1]);
			if (j != size - 1)
				glVertex2d(softBody[i][j + 1].GetPosition()[0], softBody[i][j + 1].GetPosition()[1]);

			glEnd();*/

			softBody[i][j].Draw();
		}
	}
}

Shape& SoftBody::GetFirstShape()
{
	for (int i = 0; i < softBody.size(); i++)
	{
		for (int j = 0; j < softBody[i].size(); j++)
		{
			if (!softBody[i][j].isActive) continue;

			return softBody[i][j];
		}
	}
}
