#include <SFML/Graphics.hpp>
#include <iostream>

#include "SceneManager.h"
#include "FileIO.h"
#include "Player.h"
#include "Level.h"

std::vector<std::string> data;

std::map<std::string, sf::Texture> playerTextures;
std::map<int, sf::Texture> levelTextures;

std::map<std::string, sf::Sprite> playerSprites;
std::map<std::string, sf::Sprite>* playerSprites_ptr = &playerSprites;

sf::Texture texture;

Player* player;

Level* testLevel;

/// <summary>
/// Will load content so that it is useable in main
/// </summary>
void LoadContent() {
    //data = FileIO::Instance()->ReadFromFile("Levels/File.txt");

    sf::Texture idle;
    idle.loadFromFile("Images/character_idle.png");
    playerTextures.emplace("idle", idle);
    sf::Texture walk;
    walk.loadFromFile("Images/character_walk.png");
    playerTextures.emplace("walk", walk);

    for (auto& pair : playerTextures) {
        sf::Sprite sprite;
        sprite.setTexture(pair.second);

        playerSprites.emplace(pair.first,sprite);
    }

    player = new Player(playerSprites_ptr, sf::Vector2f(640, 360), 6);

    texture.loadFromFile("Images/prototypeBlock.png");

    levelTextures.emplace(0, texture);
    testLevel = new Level("Levels/File", 4, levelTextures);
}

void Draw(sf::RenderTexture& target) {
    testLevel->Draw(target);
}

void Update() {
    player->Update();
}

int main()
{
    // Load our main content
    LoadContent();

    sf::RenderTexture renderTexture;
    renderTexture.create(1280, 720);

    sf::RenderWindow window(sf::VideoMode(1280, 720), "LevelLoading");

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

        // Draw everything to a render texture
        renderTexture.clear(sf::Color::Blue);

        Draw(renderTexture);

        renderTexture.display();

        sf::Sprite renderSprite(renderTexture.getTexture());

        // Then draw that texture to the window
        window.clear(sf::Color::White);
        window.draw(renderSprite);
        player->Draw(window);
        window.display();
    }

    return 0;
}





