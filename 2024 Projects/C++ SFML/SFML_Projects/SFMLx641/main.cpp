#include <SFML/Graphics.hpp>
#include <iostream>

#include "SceneManager.h"
#include "FileIO.h"


/// <summary>
/// Will load content so that it is useable in main
/// </summary>
void LoadContent() {
    FileIO::Instance()->ReadFromFile("..//..//File.txt");
}

void Draw(sf::RenderTexture& target) {

}

void Update() {

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
        renderTexture.clear(sf::Color::White);

        Draw(renderTexture);

        renderTexture.display();

        sf::Sprite renderSprite(renderTexture.getTexture());

        // Then draw that texture to the window
        window.clear(sf::Color::White);
        window.draw(renderSprite);
        window.display();
    }

    return 0;
}





