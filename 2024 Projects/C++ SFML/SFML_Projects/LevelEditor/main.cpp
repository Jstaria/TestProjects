#include <SFML/Graphics.hpp>
#include <iostream>

#include "SceneManager.h"
#include "FileIO.h"
#include "Player.h"
#include "Level.h"
#include "GlobalVariables.h"
#include "ViewManager.h"
#include "ViewControl.h"
#include "GameManager.h"
#include "LevelEditor.h"

std::vector<std::string> data;

sf::Texture texture;
std::map<int, sf::Texture> levelTextures;

GameManager* game;

sf::View* view_ptr;
sf::View view;

ViewControl vc;
sf::Font font;

LevelEditor editor;

/// <summary>
/// Will load content so that it is useable in main
/// </summary>
void LoadContent(sf::RenderWindow& window) {

    font.loadFromFile("arial.ttf");

    view = window.getDefaultView();
    view_ptr = &view;
    ViewManager::Instance()->SetWindowView(view_ptr);

    vc = ViewControl();

    texture.loadFromFile("Images/prototypeBlock.png");

    levelTextures.emplace(0, texture);

    GlobalVariables::setTextureScaler(10);
    GlobalVariables::setTextures(levelTextures);

    editor = LevelEditor();
}

void Draw(sf::RenderWindow& window) {

    editor.Draw(window);
    sf::Sprite sprite(texture);
    sprite.setScale(10, 10);
    window.draw(sprite);
}

void Update(sf::RenderWindow& window) {

    vc.Update();
    editor.Update(window);
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





