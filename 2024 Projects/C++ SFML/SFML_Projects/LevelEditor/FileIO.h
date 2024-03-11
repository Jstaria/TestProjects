#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <vector>

class FileIO {
private:
    static FileIO* s_instance;

    static std::ifstream inputFile;
    static std::ofstream outputFile;

public:
    FileIO();

    static FileIO* Instance();

    static std::vector<std::string> ReadFromFile(std::string fileDirectory);
    
    static void WriteToFile(std::string fileDirectory, std::vector<std::string> data);

    static std::vector<std::string> Split(char splitChar, std::string string);
};

