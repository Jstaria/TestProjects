#include <SFML/Graphics.hpp>
#include <iostream>
#include <../../SFML_Projects/Player.hpp>

Texture texture;
Sprite sprite;

Player* player;

/// <summary>
/// Will load content so that it is useable in main
/// </summary>
void LoadContent() {
    texture.loadFromFile("../Images/playerScaled.png");
    sprite.setTexture(texture);
}

void Draw(RenderTarget& target) {
    player->Draw(target);
}

int main()
{
    sf::RenderWindow window(sf::VideoMode(1280, 720), "SFML works!");

    player = new Player(sprite, Vector2f(720,360));

    while (window.isOpen())
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed)
                window.close();
        }



        window.clear();
        
        window.display();
    }  

    return 0;
}




