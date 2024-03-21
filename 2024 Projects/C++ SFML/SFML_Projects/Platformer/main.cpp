#include <SFML/Graphics.hpp>
#include <iostream>

#include "SceneManager.h"
#include "FileIO.h"
#include "Player.h"
#include "Level.h"
#include "GlobalVariables.h"
#include "ViewManager.h"

#include "GameManager.h"

std::vector<std::string> data;

std::map<std::string, sf::Texture> playerTextures;
std::map<int, sf::Texture> levelTextures;

std::map<std::string, sf::Sprite> playerSprites;
std::map<std::string, sf::Sprite>* playerSprites_ptr = &playerSprites;

sf::Texture texture;

Player* player;

Level* testLevel;
Level* testPNGLevel;

GameManager* game;

sf::View* view_ptr;
sf::View view;

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

    for (auto& pair : playerTextures) {
        sf::Sprite sprite;
        sprite.setTexture(pair.second);

        playerSprites.emplace(pair.first,sprite);
    }

    texture.loadFromFile("Images/prototypeBlock.png");

    levelTextures.emplace(1, texture);

    GlobalVariables::setTextureScaler(3);
    GlobalVariables::setTextures(levelTextures);

    player = new Player(playerSprites_ptr, sf::Vector2f(640, 360), 6);
    
    game = new GameManager(player);
    game->SetLevel("Levels/EditorTest");

    //testPNGLevel = new Level("Levels/test.png", true);
}

void Draw(sf::RenderWindow& window) {
    game->Draw(window);
    //testPNGLevel->Draw(window);
}

void Update() {
    game->Update();
    
    //if (sf::Keyboard::isKeyPressed(sf::Keyboard::Up)) {
    //    ViewManager::Instance()->SetCameraPosition(sf::Vector2f(640, -50));
    //}
    //else if (sf::Keyboard::isKeyPressed(sf::Keyboard::Down)) {
    //    ViewManager::Instance()->SetCameraPosition(sf::Vector2f(640, 50 + 720));
    //}
    //else if (sf::Keyboard::isKeyPressed(sf::Keyboard::Left)) {
    //    ViewManager::Instance()->SetCameraPosition(sf::Vector2f(-100, 360));
    //}
    //else if (sf::Keyboard::isKeyPressed(sf::Keyboard::Right)) {
    //    ViewManager::Instance()->SetCameraPosition(sf::Vector2f(100 + 1280, 360));
    //}
    //else {
    //    ViewManager::Instance()->SetCameraPosition(sf::Vector2f(1280 / 2, 720 / 2));
    //}
}

int main()
{
    // Load our main content
    sf::RenderTexture renderTexture;
    renderTexture.create(1280, 720);

    sf::RenderWindow window(sf::VideoMode(1920, 1080), "LevelLoading");
    //sf::RenderWindow window(sf::VideoMode(1280, 720), "LevelLoading");

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
        Update();
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





