#include <SFML/Graphics.hpp>
#include <iostream>

#include "SceneManager.h"
#include "FileIO.h"
#include "Player.h"

std::vector<std::string> data;
sf::Texture texture;
sf::Texture texture1;

Player* player;

/// <summary>
/// Will load content so that it is useable in main
/// </summary>
void LoadContent() {
    data = FileIO::Instance()->ReadFromFile("File.txt");

    texture1.loadFromFile("Images/protoPlayer.png");
    player = new Player(sf::Sprite(texture1), sf::Vector2f(640, 360));

    texture.loadFromFile("Images/prototypeBlock.png");
}

void Draw(sf::RenderTexture& target) {
    for (size_t i = 0; i < data.size(); i++)
    {
        std::string line = data[i];

        std::vector<std::string> lineData = FileIO::Split(',', line);

        for (size_t j = 0; j < lineData.size(); j++)
        {
            if (lineData[j] == "o") {

                sf::Sprite shape(texture);
                shape.setScale(4, 4);
                shape.setPosition(j * shape.getLocalBounds().width * shape.getScale().x, i * shape.getLocalBounds().height * shape.getScale().y);
                target.draw(shape);
            }
        }
    }
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

    sf::RenderWindow window(sf::VideoMode(1280, 720), "SFML works!");

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





