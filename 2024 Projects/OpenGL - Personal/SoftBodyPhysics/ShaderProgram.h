#pragma once

#include <GL/glew.h>
#include <string>
#include <fstream>
#include <iostream>
#include "GlobalVariables.h"
#include "ShaderClass.h"

using namespace std;

class ShaderProgram
{
public:
	unsigned int id;
public:
	ShaderProgram(void);
	~ShaderProgram(void);

	void Create();
	void link(ShaderClass shader);

	void setFloat(const char* name, float value);
	void setInt(const char* name, int value);
	void setFloat1V(const char* name, unsigned int count, const float* floatPtr);
	void setFloat3V(const char* name, unsigned int count, const float* floatPtr);
	void setMatric4v(const char* name, unsigned int count, const float* floatPtr);
	// Where to add more set functions for dif data types

	void createDataTexture(unsigned int& pGLTexID, float* pData, unsigned int pMaxWidth, unsigned int pMaxHeight);

	void setSampler(const char* sampleName, GLuint textureUnit);

private:
	void printProgramInfoLog(unsigned int prog_id);
};

