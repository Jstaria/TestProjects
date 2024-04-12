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

std::vector<sf::Texture*> lTexture_ptrs;
std::vector<sf::Texture> levelTextures;
std::map<int, sf::Texture*> levelTexture_ptrs;
std::vector<sf::Texture*> iTexture_ptrs;
std::vector<sf::Texture> interactableTextures;
std::map<int, sf::Texture*> interactableTexture_ptrs;

GameManager* game;

sf::View* view_ptr;
sf::View view;

ViewControl vc;
sf::Font font;

LevelEditor* editor;

bool LoadTexture(std::string file, std::vector<sf::Texture>& textures) {
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

    LoadTexture("../Platformer/Images/grass1.png", levelTextures);
    LoadTexture("../Platformer/Images/grass2.png", levelTextures);
    LoadTexture("../Platformer/Images/grass3.png", levelTextures);
    LoadTexture("../Platformer/Images/grass4.png", levelTextures);
    LoadTexture("../Platformer/Images/grass5.png", levelTextures);
    LoadTexture("../Platformer/Images/grass6.png", levelTextures);
    LoadTexture("../Platformer/Images/grass7.png", levelTextures);
    LoadTexture("../Platformer/Images/grass8.png", levelTextures);
    LoadTexture("../Platformer/Images/grass9.png", levelTextures);
    LoadTexture("../Platformer/Images/grass10.png", levelTextures);
    LoadTexture("../Platformer/Images/grass11.png", levelTextures);
    LoadTexture("../Platformer/Images/grass12.png", levelTextures);
    LoadTexture("../Platformer/Images/grass13.png", levelTextures);
    LoadTexture("../Platformer/Images/grass14.png", levelTextures);
    LoadTexture("../Platformer/Images/grass15.png", levelTextures);
    LoadTexture("../Platformer/Images/grass16.png", levelTextures);
    LoadTexture("../Platformer/Images/grass17.png", levelTextures);
    LoadTexture("../Platformer/Images/grass18.png", levelTextures);
    LoadTexture("../Platformer/Images/dirt.png", levelTextures);
    LoadTexture("../Platformer/Images/prototypeBlock.png", levelTextures);
    LoadTexture("../Platformer/Images/protoGreen.png", levelTextures);
    LoadTexture("../Platformer/Images/protoRed.png", levelTextures);
    LoadTexture("../Platformer/Images/protoCyan.png", levelTextures);
    LoadTexture("../Platformer/Images/protoViolet.png", levelTextures);
    LoadTexture("../Platformer/Images/checkpoint_unlit_editor.png", interactableTextures);

    for (size_t i = 0; i < levelTextures.size(); i++)
    {
        lTexture_ptrs.push_back(&levelTextures[i]);
    }

    for (size_t i = 0; i < levelTextures.size(); i++)
    {
        levelTexture_ptrs.emplace(i, lTexture_ptrs[i]);
    }

    for (size_t i = 0; i < interactableTextures.size(); i++)
    {
        iTexture_ptrs.push_back(&interactableTextures[i]);
    }

    for (size_t i = 0; i < interactableTextures.size(); i++)
    {
        interactableTexture_ptrs.emplace(i, iTexture_ptrs[i]);
    }

    GlobalVariables::setTextureScaler(3);
    GlobalVariables::setTextures(levelTexture_ptrs, "level");
    GlobalVariables::setTextures(interactableTexture_ptrs, "interactable");

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
    desktop = sf::VideoMode(1920, 1080);
    sf::RenderWindow window(desktop, "Level Editor"/*, sf::Style::Fullscreen*/);
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





