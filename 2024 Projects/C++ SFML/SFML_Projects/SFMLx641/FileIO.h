#pragma once

#include <iostream>
#include <fstream>
#include <string>
#include <list>

class FileIO {
private:
    static FileIO* s_instance;

    std::ifstream inputFile;
    std::ofstream outputFile;

public:
    FileIO();

    static FileIO* Instance();

    std::list<std::string> ReadFromFile(std::string fileDirectory);
};

