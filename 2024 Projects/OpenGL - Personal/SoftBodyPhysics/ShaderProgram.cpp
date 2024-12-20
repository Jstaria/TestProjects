#include "ShaderProgram.h"

ShaderProgram::ShaderProgram(void)
{
	id = 0;
}

ShaderProgram::~ShaderProgram(void)
{
}

void ShaderProgram::Create()
{
	id = glCreateProgram();
}

void ShaderProgram::link(ShaderClass shader)
{
	glAttachShader(id, shader.id);
	glLinkProgram(id);

	int status;
	glGetProgramiv(id, GL_LINK_STATUS, &status);

	if (status != GL_TRUE) {
		printProgramInfoLog(id);
		return;
	}
}

void ShaderProgram::setFloat(const char* name, float value)
{
	unsigned int loc = glGetUniformLocation(id, name);

	if (loc == -1) return;
	else glUniform1f(loc, value);
}

void ShaderProgram::setInt(const char* name, int value)
{
	unsigned int loc = glGetUniformLocation(id, name);

	if (loc == -1) return;
	else glUniform1i(loc, value);
}

void ShaderProgram::setFloat1V(const char* name, unsigned int count, const float* floatPtr)
{
	unsigned int loc = glGetUniformLocation(id, name);

	if (loc == -1) return;
	else glUniform1fv(loc, count, floatPtr);
}

void ShaderProgram::setFloat3V(const char* name, unsigned int count, const float* floatPtr)
{
	unsigned int loc = glGetUniformLocation(id, name);

	if (loc == -1) return;
	else glUniform3fv(loc, count, floatPtr);
}

void ShaderProgram::setMatric4v(const char* name, unsigned int count, const float* floatPtr)
{
	unsigned int loc = glGetUniformLocation(id, name);

	if (loc == -1) return;
	else glUniformMatrix4fv(loc, count, GL_FALSE, floatPtr);
}

void ShaderProgram::createDataTexture(unsigned int& pGLTexID, float* pData, unsigned int pMaxWidth, unsigned int pMaxHeight)
{
	glGenTextures(1, &pGLTexID);
	glActiveTexture(GL_TEXTURE_2D + pGLTexID);

	glEnable(GL_TEXTURE_2D);
	glBindTexture(GL_TEXTURE_2D, pGLTexID);

	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_CLAMP_TO_EDGE);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_CLAMP_TO_EDGE);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
	glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);

	glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA32F, pMaxWidth, pMaxHeight, 0, GL_RGBA, GL_FLOAT, pData);
	glDisable(GL_TEXTURE_2D);
}

void ShaderProgram::setSampler(const char* sampleName, GLuint textureUnit)
{
	GLint samplerLocation = glGetUniformLocation(id, sampleName);
	glUniform1i(samplerLocation, textureUnit);
}

void ShaderProgram::printProgramInfoLog(unsigned int prog_id)
{
	int infoLogLen = 0;
	int charsWritten = 0;
	char* infoLog;

	glGetProgramiv(prog_id, GL_INFO_LOG_LENGTH, &infoLogLen);

	if (infoLogLen > 0) {
		infoLog = new char[infoLogLen];
		glGetProgramInfoLog(prog_id, infoLogLen, &charsWritten, infoLog);
		cout << infoLog << endl;
		delete[] infoLog;
	}
}
