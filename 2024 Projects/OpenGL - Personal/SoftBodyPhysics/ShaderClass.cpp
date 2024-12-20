#include "ShaderClass.h"

using namespace std;

ShaderClass::ShaderClass(void)
{
	id = 0;
}

ShaderClass::~ShaderClass(void)
{
}

void ShaderClass::Create(const char* shaderFileName, GLenum targetType)
{
	if (GLEW_ARB_vertex_shader && GLEW_ARB_fragment_shader)
		cout << "Ready for GLSL\n";
	else {
		cout << "No GLSL support\n";
		exit(1);
	}

	char* source = NULL;
	int status;

	if (shaderFileName)
	{
		source = loadShaderFile(shaderFileName);

		if (source == NULL) {
			cout << "The shader of '" << shaderFileName << "' isn't created!" << endl;
			return;
		}

		id = glCreateShader(targetType);
		glShaderSource(id, 1, const_cast<const GLchar* *>(&source), NULL);
		glCompileShader(id);
		
		glGetShaderiv(id, GL_COMPILE_STATUS, &status);

		if (status != GL_TRUE) {
			cout << "Error: The shader of '" << shaderFileName << "' is created but incorrect, see the shader info log below!" << endl; 
			printShaderInfoLog(id);
		}

		delete[] source;

	}
}

void ShaderClass::Destroy()
{
	glDeleteShader(id);
}

char* ShaderClass::loadShaderFile(const char* fn)
{
	ifstream file;
	char* content = NULL;
	int count = 0;

	file.open(fn, ios::in, ifstream::binary);

	if (!file) {
		cout << fn << "doesn't exist!" << endl; 
		return NULL;
	}
	if (!file.good()) {
		cout << fn << "is empty!" << endl; 
		return NULL;
	}

	file.seekg(0, ios::end);

	unsigned long len = file.tellg();

	file.seekg(ios::beg);

	if (len == 0) {
		cout << fn << "is empty!" << endl; 
		return NULL;
	}

	content = new char[len + 1];
	content[len] = '\0';

	unsigned int i = 0;

	while (!file.eof())
	{
		content[i] = file.get();
		i++;
	}

	content[i - 1] = '\0';
	file.close();
	return content;
}

void ShaderClass::printShaderInfoLog(unsigned int shader_id)
{
	int infoLogLen = 0;
	int charsWritten = 0;
	char* infoLog;

	glGetShaderiv(shader_id, GL_INFO_LOG_LENGTH, &infoLogLen);

	if (infoLogLen > 0) {
		infoLog = new char[infoLogLen];
		glGetShaderInfoLog(shader_id, infoLogLen, &charsWritten, infoLog);
		cout << infoLog << endl;
		delete[] infoLog;
	}
}
