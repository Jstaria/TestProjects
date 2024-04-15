#include <SFML/Graphics.hpp>
#include <iostream>
#include <bitset>

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

std::vector<sf::Texture*> lTexture_ptrs;
std::vector<sf::Texture> levelTextures;
std::map<int, sf::Texture*> levelTexture_ptrs;

Player* player;
Input* input;

Level* testLevel;

sf::Shader outlineShader;
sf::Shader lightShader;

GameManager* game;

sf::View* view_ptr;
sf::View view;

sf::Clock clock2;

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

    outlineShader.loadFromFile("Shaders/Outline.frag", sf::Shader::Fragment);
    lightShader.loadFromFile("Shaders/Light.frag", sf::Shader::Fragment);

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

    LoadTexture("Images/grass1.png", levelTextures);
    LoadTexture("Images/grass2.png", levelTextures);
    LoadTexture("Images/grass3.png", levelTextures);
    LoadTexture("Images/grass4.png", levelTextures);
    LoadTexture("Images/grass5.png", levelTextures);
    LoadTexture("Images/grass6.png", levelTextures);
    LoadTexture("Images/grass7.png", levelTextures);
    LoadTexture("Images/grass8.png", levelTextures);
    LoadTexture("Images/grass9.png", levelTextures);
    LoadTexture("Images/grass10.png", levelTextures);
    LoadTexture("Images/grass11.png", levelTextures);
    LoadTexture("Images/grass12.png", levelTextures);
    LoadTexture("Images/grass13.png", levelTextures);
    LoadTexture("Images/grass14.png", levelTextures);
    LoadTexture("Images/grass15.png", levelTextures);
    LoadTexture("Images/grass16.png", levelTextures);
    LoadTexture("Images/grass17.png", levelTextures);
    LoadTexture("Images/grass18.png", levelTextures);
    LoadTexture("Images/dirt.png", levelTextures);
    LoadTexture("Images/prototypeBlock.png", levelTextures);
    LoadTexture("Images/protoGreen.png", levelTextures);
    LoadTexture("Images/protoRed.png", levelTextures);
    LoadTexture("Images/protoCyan.png", levelTextures);
    LoadTexture("Images/protoViolet.png", levelTextures);

    for (size_t i = 0; i < levelTextures.size(); i++)
    {
        lTexture_ptrs.push_back(&levelTextures[i]);
    }

    for (size_t i = 0; i < levelTextures.size(); i++)
    {
        levelTexture_ptrs.emplace(i, lTexture_ptrs[i]);
    }

    GlobalVariables::setTextureScaler(3);
    GlobalVariables::setTextures(levelTexture_ptrs, "level");
    GlobalVariables::setSprites(checkSprites_ptr, "interactableSprites");
    GlobalVariables::setSprites(playerSprites_ptr, "playerSprites");
    GlobalVariables::setInput(input);
    GlobalVariables::setShader("outline", &outlineShader);
    GlobalVariables::setShader("light", &lightShader);

    player = new Player(playerSprites_ptr, sf::Vector2f(640, 360), 6, input);
    
    game = new GameManager(player, input);
    game->SetLevel("Levels/EditorTest");

    //testPNGLevel = new Level("Levels/test.png", true);
}

void Draw(sf::RenderWindow& window) {

    game->Draw(window);
    //testPNGLevel->Draw(window);
}

void Update(sf::RenderWindow& window) {
    game->Update();

    if (sf::Keyboard::isKeyPressed(sf::Keyboard::Escape)) {
        window.close();
    }

    std::cout << clock2.getElapsedTime().asMilliseconds() << std::endl;
    clock2.restart();
}

int main()
{
    // Load our main content

    sf::VideoMode desktop = sf::VideoMode::getDesktopMode();
    desktop = sf::VideoMode(1280, 720);

    view.setCenter(desktop.width * .5f, desktop.height * .5f);

    sf::RenderTexture renderTexture;
    renderTexture.create(desktop.width, desktop.height);
    sf::Vector2f ogPos = view.getCenter();
    
    sf::RenderWindow window(desktop, "Level");
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

        renderTexture.clear();

        renderTexture.display();

        sf::Sprite renderSprite(renderTexture.getTexture());
        renderSprite.setPosition(view.getCenter());
        renderSprite.setOrigin(view.getSize().x / 2, view.getSize().y / 2);
        // Then draw that texture to the window
        window.clear(sf::Color::Color(140, 203, 215));
        //window.clear(sf::Color::Color(0,00,00));
        
        Draw(window);

        //int binary;

        sf::Vector2f difference = view.getCenter() - ogPos;

        float positionX = 300 - difference.x;
        float positionY = 300 - difference.y;

        float width = 300;
        float height = 300;

        //std::cout << positionX << "," << positionY << "," << width << "," << height << std::endl;

        /*binary += */

        lightShader.setUniform("position", sf::Vector2f(positionX, positionY));
        lightShader.setUniform("bounds", sf::Vector2f(width, height));
        window.draw(renderSprite, &lightShader);

        window.display();
}

    return 0;
}





