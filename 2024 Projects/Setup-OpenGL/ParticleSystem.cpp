#include <GL/glew.h>
#include <GL/freeglut.h>
#include "ParticleSystem.h"
#include <vector>

ParticleSystem::ParticleSystem(int _numParticles)
{
	numParticles = _numParticles;	// Set the number of particles

	// Allocate memory for positions, velocities, colors, and lifetimes.
	positions = new float[numParticles * 3];
	velocities = new float[numParticles * 3];
	colors = new float[numParticles * 4];
	lifeTimes = new float[numParticles];

	indices = new unsigned short[numParticles];

	for (int i = 0; i < numParticles; i++) {
		// Initialize the life times
		lifeTimes[i] = maxLifeTime - maxLifeTime * i / numParticles;

		/***************************/
		// Write your code below
		// Please add initializations for other arrays as you see appropriate.

		// Set default positions
		positions[i * 3] = 0;
		positions[i * 3 + 1] = 0;
		positions[i * 3 + 2] = 0;

		float x = 5;
		float y = 20;

		// I want to normalize this \/

		// Set random velocities
		velocities[i * 3] = getRandomValue(-x, x);
		velocities[i * 3 + 1] = getRandomValue(y - 1, y);
		velocities[i * 3 + 2] = getRandomValue(-x, x);

		vector<float> color = hueToRGB(hue);

		// Set Colors
		colors[i * 3] = color[0];
		colors[i * 3 + 1] = color[1];
		colors[i * 3 + 2] = color[2];
		colors[i * 3 + 3] = 1;

		hue += .01f;
		hue = fmodf(hue, 1);

		indices[i] = i;

		/***************************/
	}
}

void ParticleSystem::update(float deltaTime)
{
	for (int i = 0; i < numParticles; i++) {
		/***************************/
		// Write your code below
		// Update lifetime, velocity, position, and color.
		// Reset particle states (positions, velocities, colors, and lifetimes) when the lifetime reaches the maxLifeTime

		float gravity = 9.8f;

		//velocities[i * 3] = getRandomValue(0, 1);
		velocities[i * 3 + 1] -= gravity * deltaTime;
		//velocities[i * 3 + 2] = getRandomValue(0, 1);

		positions[i * 3] += velocities[i * 3] * deltaTime;
		positions[i * 3 + 1] += velocities[i * 3 + 1] * deltaTime;
		positions[i * 3 + 2] += velocities[i * 3 + 2] * deltaTime;

		// Write your code above
		/***************************/
	}
}

void ParticleSystem::draw()
{
	/***************************/
	// Write your code below
	// Use GL_POINTS for rendering

	glEnableClientState(GL_COLOR_ARRAY);
	glEnableClientState(GL_VERTEX_ARRAY);
	glColorPointer(4, GL_FLOAT, 0, colors);
	glVertexPointer(3, GL_FLOAT, 0, positions);

	glPointSize(3);
	glDrawElements(GL_POINTS, numParticles, GL_UNSIGNED_SHORT, indices);

	glDisableClientState(GL_VERTEX_ARRAY);
	glDisableClientState(GL_COLOR_ARRAY);
	// Write your code above
	/***************************/
}

float ParticleSystem::getRandomValue(float min_value, float max_value)
{
	return min_value + (std::rand()) / (RAND_MAX / (max_value - min_value));;
}

float min3(float a, float b, float c) {
	return min(min(a, b), c);
}

vector<float> hueToRGB(float h)
{
	float kr = fmodf(5 + h * 6, 6);
	float kg = fmodf(3 + h * 6, 6);
	float kb = fmodf(1 + h * 6, 6);

	float r = 1 - max(min3(kr, 4 - kr, 1), 0.0f);
	float g = 1 - max(min3(kg, 4 - kg, 1), 0.0f);
	float b = 1 - max(min3(kb, 4 - kb, 1), 0.0f);

	return { r, g, b };
}
