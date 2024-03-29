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

std::map<std::string, sf::Texture> textures;

std::vector<sf::Texture*> texture_ptrs;
std::vector<sf::Texture> levelTextures;
std::map<int, sf::Texture*> levelTexture_ptrs;

Checkpoint* testPoint;
Checkpoint* testPoint2;
Player* player;
Input* input;

Level* testLevel;


GameManager* game;

sf::View* view_ptr;
sf::View view;

bool loadTexture(const std::string& id, const std::string& filename) {
    sf::Texture texture;
    if (!texture.loadFromFile(filename)) {
        return false;
    }
    textures[id] = texture;
    return true;
}

// Get texture by ID
sf::Texture& getTexture(const std::string& id) {
    return textures.at(id);
}

// Load sprite from texture
sf::Sprite createSprite(const std::string& textureId) {
    sf::Sprite sprite;
    sprite.setTexture(getTexture(textureId));
    return sprite;
}

bool LoadTexture(std::string file, std::vector<sf::Texture>& textures) {
    bool s;

    sf::Texture texture;
    s = texture.loadFromFile(file);
    textures.push_back(texture);

    return s;
}

bool LoadSprite(std::string mapName, std::string file, std::map<std::string, sf::Sprite>& sprites, std::vector<sf::Texture>& textures) {
    
    bool s;

    s = LoadTexture(file, textures);

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

    loadTexture("idle", "Images/character_idle.png");
    loadTexture("walk", "Images/character_walk.png");
    loadTexture("jump", "Images/character_jump.png");

    playerSprites["idle"] = createSprite("idle");
    playerSprites["walk"] = createSprite("walk");
    playerSprites["jump"] = createSprite("jump");

    loadTexture("unlit", "Images/checkpoint_unlit.png");
    loadTexture("lit", "Images/checkpoint_lit.png");
    loadTexture("light", "Images/checkpoint_light.png");
    loadTexture("extinguish", "Images/checkpoint_extinguish.png");

    checkSprites["extinguish"] = createSprite("extinguish");
    checkSprites["light"] = createSprite("light");
    checkSprites["unlit"] = createSprite("unlit");
    checkSprites["lit"] = createSprite("lit");

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

    LoadTexture("Images/prototypeBlock.png", levelTextures);
    LoadTexture("Images/protoGreen.png", levelTextures);
    LoadTexture("Images/protoRed.png", levelTextures);
    LoadTexture("Images/protoCyan.png", levelTextures);
    LoadTexture("Images/protoViolet.png", levelTextures);

    for (size_t i = 0; i < levelTextures.size(); i++)
    {
        texture_ptrs.push_back(&levelTextures[i]);
    }

    for (size_t i = 0; i < levelTextures.size(); i++)
    {
        levelTexture_ptrs.emplace(i, texture_ptrs[i]);
    }

    GlobalVariables::setTextureScaler(3);
    GlobalVariables::setTextures(levelTexture_ptrs, "level");
    GlobalVariables::setSprites(checkSprites_ptr, "interactableSprites");
    GlobalVariables::setSprites(playerSprites_ptr, "playerSprites");
    GlobalVariables::setInput(input);

    testPoint = new Checkpoint(GlobalVariables::getSprites("interactableSprites"), sf::Vector2f(800, 300), 4);
    //testPoint2 = new Checkpoint(GlobalVariables::getSprites("interactableSprites"), sf::Vector2f(1200, 300), 4);
    player = new Player(playerSprites_ptr, sf::Vector2f(640, 360), 6, input);
    
    game = new GameManager(player, input);
    game->SetLevel("Levels/EditorTest");

    //testPNGLevel = new Level("Levels/test.png", true);
}

void Draw(sf::RenderWindow& window) {
    testPoint->Draw(window);
    //testPoint2->Draw(window);
    game->Draw(window);
    //testPNGLevel->Draw(window);
}

void Update(sf::RenderWindow& window) {
    game->Update();
    testPoint->Update();
    //testPoint2->Update();

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
    sf::RenderWindow window(desktop, "Level", sf::Style::Fullscreen);
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





