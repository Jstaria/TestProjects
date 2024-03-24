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

std::vector<sf::Texture*> texture_ptrs;
std::vector<sf::Texture> textures;
std::map<int, sf::Texture*> levelTextures;

GameManager* game;

sf::View* view_ptr;
sf::View view;

ViewControl vc;
sf::Font font;

LevelEditor* editor;

bool LoadTexture(std::string file) {
    bool s;

    sf::Texture texture; 
    s = texture.loadFromFile(file);
    textures.push_back(texture);

    return s;
}

/// <summary>
/// Will load content so that it is useable in main
/// </summary>
void LoadContent(sf::RenderWindow& window) {

    font.loadFromFile("arial.ttf");

    view = window.getDefaultView();
    view_ptr = &view;
    ViewManager::Instance()->SetWindowView(view_ptr);

    vc = ViewControl();
    LoadTexture("Images/prototypeBlock.png");
    LoadTexture("Images/protoGreen.png");
    LoadTexture("Images/protoRed.png");
    LoadTexture("Images/protoCyan.png");
    LoadTexture("Images/protoViolet.png");

    for (size_t i = 0; i < textures.size(); i++)
    {
        texture_ptrs.push_back(&textures[i]);
    }

    for (size_t i = 0; i < textures.size(); i++)
    {
        levelTextures.emplace(i, texture_ptrs[i]);
    }

    GlobalVariables::setTextureScaler(3);
    GlobalVariables::setTextures(levelTextures);

    editor = new LevelEditor();
}

void Draw(sf::RenderWindow& window) {

    editor->Draw(window);
}

void Update(sf::RenderWindow& window) {

    vc.Update();
    editor->Update(window);
}

int main()
{
    // Load our main content
    sf::RenderTexture renderTexture;
    renderTexture.create(1280, 720);

    sf::VideoMode desktop = sf::VideoMode::getDesktopMode();
    //desktop = sf::VideoMode(1920, 1080);
    sf::RenderWindow window(desktop, "Level Editor", sf::Style::Fullscreen);
    //sf::RenderWindow window(sf::VideoMode(1280, 720), "LevelLoading");
    window.setVerticalSyncEnabled(true);

    LoadContent(window);

    window.setFramerateLimit(60);

    while (window.isOpen())
    {
        sf::Event event;
        while (window.pollEvent(event))
        {
            if (event.type == sf::Event::Closed || sf::Keyboard::isKeyPressed(sf::Keyboard::Escape))
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





