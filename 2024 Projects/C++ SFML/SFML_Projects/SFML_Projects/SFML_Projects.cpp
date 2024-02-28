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

void Draw(RenderTexture& target) {
    player->Draw(target);
}

int main()
{
    LoadContent();

    sf::RenderTexture renderTexture;
    renderTexture.create(1280, 720);

    sf::RenderWindow window(sf::VideoMode(1280, 720), "SFML works!");

    player = new Player(texture, Vector2f(640,360));

    while (window.isOpen())
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed)
                window.close();
        }
        
        renderTexture.clear(Color::White);

        Draw(renderTexture);

        renderTexture.display();

        Sprite renderSprite(renderTexture.getTexture());

        window.clear(Color::White);
        window.draw(renderSprite);
        window.display();
    }  

    return 0;
}




