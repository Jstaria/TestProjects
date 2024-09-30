#pragma once

#include <GL/glew.h>
#include <string>
#include <fstream>
#include <iostream>
#include "GlobalVariables.h"

using namespace std;

class ShaderClass
{
public:
	unsigned int id;

public:
	ShaderClass(void);
	~ShaderClass(void);

	void Create(const char* shaderFileName, GLenum targetType);
	void Destroy();

private:
	char* loadShaderFile(const char* fn);
	void printShaderInfoLog(unsigned int shader_id);
};

