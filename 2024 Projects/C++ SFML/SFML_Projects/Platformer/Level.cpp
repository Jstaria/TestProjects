#include "Level.h"
#include "BoundingBox.h"
#include "GlobalVariables.h"

Level::Level(std::string levelPath) :
	levelPath(levelPath)
{
    textureScaler = GlobalVariables::getTextureScaler();
    textures = GlobalVariables::getTextures();

    LoadTileData(levelPath);
    CreateBB(levelPath);
}

Level::~Level() {

    //for (int i = 0; i < arrayWidth; ++i) {
    //    delete[] tileArray[i];
    //}
    //delete[] tileArray;

    bbArray.clear();
}

void Level::LoadTileData(std::string filePath)
{
	std::vector<std::string> data = FileIO::ReadFromFile(filePath + ".txt");

    std::cout << "Loaded File" << std::endl;

    std::vector<std::string> dimensions = FileIO::Split(',',data[0]);

    arrayWidth = std::stoi(dimensions[0]);
    arrayHeight = std::stoi(dimensions[1]);
    
    sf::Vector2f scaler(textures[0].getSize().x * textureScaler, textures[0].getSize().y * textureScaler);

    std::vector<std::string> playerPos = FileIO::Split(',', data[1]);

    playerStartPos = sf::Vector2f(std::stoi(playerPos[0]) * scaler.x, std::stoi(playerPos[1]) * scaler.y);

    std::cout << "Loaded Level Dimensions" << std::endl;

    std::vector<std::vector<TileData>> tileArray(arrayHeight, std::vector<TileData>(arrayWidth));

    std::cout << "Set Array Width" << std::endl;

    std::cout << "Set Array height" << std::endl;

    for (size_t i = 2; i < data.size(); i++)
    {
        std::string line = data[i];

        std::vector<std::string> lineData = FileIO::Split(',', line);

        for (size_t j = 0; j < lineData.size(); j++)
        {
            std::cout << "Loaded {" << i << "," << j << "}" << std::endl;
            if (lineData[j] == "o") {
                
                sf::Texture* texture;

                texture = &textures[0];

                sf::Vector2f position(j * texture->getSize().x * textureScaler, i * texture->getSize().y * textureScaler);

                TileData tile(texture, position, 4, 0, sf::Vector2f(i, j));

                tileArray[i-2][j] = tile;
                std::cout << "Created Tile" << std::endl;
            }
        }
    }

    this->tileArray = tileArray;

    std::cout << "Loaded Tile Data" << std::endl;
}

void Level::CreateBB(std::string filePath)
{
    std::vector<std::string> data = FileIO::ReadFromFile(filePath + "BB.txt");

    std::cout << "Read File" << std::endl;

    for (size_t i = 0; i < data.size(); i++)
    {
        std::string line = data[i];

        std::vector<std::string> lineData = FileIO::Split(',', line);

        sf::Vector2f scaler(textures[0].getSize().x * textureScaler, textures[0].getSize().y * textureScaler);

        std::vector<std::string> pos1Data = FileIO::Split(':', lineData[0]);
        sf::Vector2f pos1(std::stoi(pos1Data[0]) * scaler.x, std::stoi(pos1Data[1]) * scaler.y);
        
        std::cout << "Position 1 Created" << std::endl;

        std::vector<std::string> pos2Data = FileIO::Split(':', lineData[1]);
        sf::Vector2f pos2(std::stoi(pos2Data[0]) * scaler.x, std::stoi(pos2Data[1]) * scaler.y);

        std::cout << "Position 2 Created" << std::endl;

        BoundingBox bb(pos1, pos2, sf::Color::Cyan);

        bbArray.push_back(bb);

        std::cout << "Bounding Box Created" << std::endl;
    }
}

void Level::Draw(sf::RenderWindow& window)
{
    //std::cout << "Got to draw" << std::endl;
    for (size_t i = 0; i < arrayWidth; i++)
    {
        for (size_t j = 0;  j < arrayHeight;  j++)
        {
            if (!tileArray[j][i].IsActive()) continue;

            tileArray[j][i].Draw(window);
            
            //std::cout << "Drew Tile: {" << i << "," << j << "}" << std::endl;
        }
    }

    for (auto it = bbArray.begin(); it != bbArray.end(); ++it) {
        it->Draw(window); // Assuming 'target' is your render target (like sf::RenderWindow)
    }
}

std::list<BoundingBox> Level::getBBArray()
{
    return bbArray;
}

sf::Vector2f Level::getPlayerPos()
{
    return playerStartPos;
}
