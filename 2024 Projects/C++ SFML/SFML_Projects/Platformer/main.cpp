#include <SFML/Graphics.hpp>
#include <iostream>

#include "SceneManager.h"
#include "FileIO.h"
#include "Player.h"
#include "Level.h"
#include "GlobalVariables.h"
#include "ViewManager.h"
#include "Checkpoint.h"
#include "GameManager.h"

std::vector<std::string> data;

std::map<std::string, sf::Texture> playerTextures;
std::map<std::string, sf::Texture> checkTextures;

std::map<std::string, sf::Sprite> playerSprites;
std::map<std::string, sf::Sprite>* playerSprites_ptr = &playerSprites;
std::map<std::string, sf::Sprite> checkSprites;
std::map<std::string, sf::Sprite>* checkSprites_ptr = &checkSprites;

std::vector<sf::Texture*> texture_ptrs;
std::vector<sf::Texture> textures;
std::map<int, sf::Texture*> levelTextures;

Checkpoint* testPoint;
Player* player;
Input* input;

Level* testLevel;
Level* testPNGLevel;

GameManager* game;

sf::View* view_ptr;
sf::View view;

bool LoadTexture(std::string file, std::vector<sf::Texture>& textures) {
    bool s;

    sf::Texture texture;
    s = texture.loadFromFile(file);
    textures.push_back(texture);

    return s;
}

bool LoadSprite(std::string mapName, std::string file, std::map<std::string, sf::Sprite>& sprites, std::vector<sf::Texture>& textures) {
    
    bool s;

    LoadTexture(file, textures);

    sprites.emplace(mapName, sf::Sprite(textures.back()));

    return s;
}

/// <summary>
/// Will load content so that it is useable in main
/// </summary>
void LoadContent(sf::RenderWindow& window) {
    //data = FileIO::Instance()->ReadFromFile("Levels/File.txt");

    view = window.getDefaultView();
    view_ptr = &view;

    ViewManager::Instance()->SetWindowView(view_ptr);

    sf::Texture idle;
    idle.loadFromFile("Images/character_idle.png");
    playerTextures.emplace("idle", idle);
    sf::Texture walk;
    walk.loadFromFile("Images/character_walk.png");
    playerTextures.emplace("walk", walk);
    sf::Texture jump;
    jump.loadFromFile("Images/character_jump.png");
    playerTextures.emplace("jump", jump);

    sf::Texture unlit;
    unlit.loadFromFile("Images/checkpoint_unlit.png");
    checkTextures.emplace("unlit", unlit);
    sf::Texture lit;
    lit.loadFromFile("Images/checkpoint_lit.png");
    checkTextures.emplace("lit", lit);

    for (auto& pair : playerTextures) {
        sf::Sprite sprite;
        sprite.setTexture(pair.second);

        playerSprites.emplace(pair.first,sprite);
    }

    for (auto& pair : checkTextures) {
        sf::Sprite sprite;
        sprite.setTexture(pair.second);

        checkSprites.emplace(pair.first, sprite);
    }

    input = new Input("Input/Controls");

    LoadTexture("Images/prototypeBlock.png", textures);
    LoadTexture("Images/protoGreen.png", textures);
    LoadTexture("Images/protoRed.png", textures);
    LoadTexture("Images/protoCyan.png", textures);
    LoadTexture("Images/protoViolet.png", textures);

    for (size_t i = 0; i < textures.size(); i++)
    {
        texture_ptrs.push_back(&textures[i]);
    }

    for (size_t i = 0; i < textures.size(); i++)
    {
        levelTextures.emplace(i, texture_ptrs[i]);
    }

    GlobalVariables::setTextureScaler(3);
    GlobalVariables::setTextures(levelTextures, "level");
    //GlobalVariables::setTextures(levelTextures, "level");
    GlobalVariables::setInput(input);

    testPoint = new Checkpoint(checkSprites_ptr, sf::Vector2f(800, 300), 1);
    player = new Player(playerSprites_ptr, sf::Vector2f(640, 360), 6, input);
    
    game = new GameManager(player, input);
    game->SetLevel("Levels/EditorTest");

    //testPNGLevel = new Level("Levels/test.png", true);
}

void Draw(sf::RenderWindow& window) {
    testPoint->Draw(window);
    game->Draw(window);
    //testPNGLevel->Draw(window);
}

void Update(sf::RenderWindow& window) {
    game->Update();
    testPoint->Update();

    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Escape)) {
        window.close();
    }
}

int main()
{
    // Load our main content
    sf::RenderTexture renderTexture;
    renderTexture.create(1280, 720);

    sf::VideoMode desktop = sf::VideoMode::getDesktopMode();
    desktop = sf::VideoMode(1920, 1080);
    sf::RenderWindow window(desktop, "Level"/*, sf::Style::Fullscreen*/);
    //sf::RenderWindow window(sf::VideoMode(1280, 720), "LevelLoading");
    window.setVerticalSyncEnabled(true);

    LoadContent(window);

    window.setFramerateLimit(60);


    while (window.isOpen())
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed)
                window.close();
        }

        // Update our main gameloop
        Update(window);
        window.setView(ViewManager::Instance()->GetWindowView());
        //// Draw everything to a render texture
        //renderTexture.clear(sf::Color::Blue);

        //

        //renderTexture.display();

        //sf::Sprite renderSprite(renderTexture.getTexture());

        // Then draw that texture to the window
        window.clear(sf::Color::Color(180, 243, 255));
        //window.draw(renderSprite);

        Draw(window);

        window.display();
}

    return 0;
}





