#include "FileIO.h"

FileIO* FileIO::s_instance = 0;

std::ifstream FileIO::inputFile;
std::ofstream FileIO::outputFile;

FileIO::FileIO() {
    s_instance = this;
}

FileIO* FileIO::Instance() {
    if (s_instance == nullptr) {
        s_instance = new FileIO();
    }

    return s_instance;
}

std::vector<std::string> FileIO::ReadFromFile(std::string fileDirectory) {
    inputFile.open(fileDirectory);

    if (!inputFile.is_open()) {
        outputFile.open(fileDirectory);
        outputFile << "";
    }

    std::vector<std::string> data;
    std::string line;

    while (std::getline(inputFile, line)) {
        data.push_back(line);
    }

    if (inputFile.is_open()) {
        inputFile.close();
    }

    return data;
}

void FileIO::WriteToFile(std::string fileDirectory, std::vector<std::string> data)
{
    
}

std::vector<std::string> FileIO::Split(char splitChar, std::string string)
{
    std::vector<std::string> dataList;
    std::string data;

    for (size_t i = 0; i < string.size(); i++)
    {
        if (string[i] == splitChar) {
            dataList.push_back(data);
            data.clear();

            continue;
        }

        data.push_back(string[i]);
    }

    return dataList;
}
