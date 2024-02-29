#include "FileIO.h"

FileIO* FileIO::s_instance = 0;

FileIO::FileIO() {
    s_instance = this;
}

FileIO* FileIO::Instance() {
    if (s_instance == nullptr) {
        s_instance = new FileIO();
    }

    return s_instance;
}

std::list<std::string> FileIO::ReadFromFile(std::string fileDirectory) {
    inputFile.open(fileDirectory);

    if (!inputFile.is_open()) {
        outputFile.open(fileDirectory);
        outputFile << "";
    }

    std::list<std::string> data;
    std::string line;

    while (std::getline(inputFile, line)) {
        data.push_back(line);
    }

    if (inputFile.is_open()) {
        inputFile.close();
    }

    return data;
}
