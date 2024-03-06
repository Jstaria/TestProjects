#include "Player.h"
#include <iostream>
#include "GlobalVariables.h"

Player::Player(std::map<std::string, sf::Sprite>* sprites, sf::Vector2f position, int maxFrames)
	: Entity(sprites, position, maxFrames) {

	for (const auto& pair : *Player::sprites) {
		sf::Sprite& sprite = (*Player::sprites)[pair.first];

		sprite.setScale(GlobalVariables::getTextureScaler(), GlobalVariables::getTextureScaler());
		sprite.setPosition(position);
	}

	std::string key = "walk";
	this->frameWidth = (*this->sprites)[key].getLocalBounds().width / maxFrames;
	this->frameHeight = (*this->sprites)[key].getLocalBounds().height;

	this->speedMultiplier = 10;
	this->velocity = sf::Vector2f(0, 0);
	this->maxVelocity = sf::Vector2f(speedMultiplier, 60 / GlobalVariables::getTextureScaler());

	this->gravity = 1.f;
	this->jumpVelocity = -maxVelocity.y;
	this->isGrounded = false;

	this->acceleration = .5f;
	this->deceleration = .75f;

	this->clock = sf::Clock();
	this->coyoteTime = sf::seconds(.5f);

	lastFacedDirectionX = 1;
	currentSprite = (*this->sprites)[key];
	GetCurrentSprite();

	CreateBB();

	currentState = PlayerState::Idle;
}

void Player::Update() {

	currentState = PlayerState::Idle;

	bool canMoveRight = true;
	bool canMoveLeft = true;
	bool isMoving = false;

	bool anyCollision = false;

	for (auto& bb : *currentLevel->getBBArray()) {
		sf::FloatRect intersection;

		if (bb.getRect().getPosition().y + bb.getRect().height < position.y - drawnSprite.getTextureRect().height / 2 && bb.CheckCollision(GetFutureRect(true,true))) {
			velocity.y = 0;
		}

		if (bb.CheckCollision(boundingBoxes["GroundBox"])) {
			velocity.y = 0;

			//std::cout << velocity.y << std::endl;
			position.y = bb.getRect().top - drawnSprite.getLocalBounds().height / 2 * GlobalVariables::getTextureScaler();
			isGrounded = true;
			timeOfGrounded = clock.getElapsedTime();
			anyCollision = true;
			canJump = true;
			// std::cout << "In collision" << std::endl;
		}
	}
	

	if (!anyCollision) {
		isGrounded = false;
		currentState = PlayerState::Jump;
	}

	if (!isGrounded && !anyCollision) {
		velocity.y += gravity;
		velocity.y = velocity.y > maxVelocity.y ? maxVelocity.y : velocity.y;
	}

	sf::Time timeSinceBeingGrounded = clock.getElapsedTime() - timeOfGrounded;

	bool wasGrounded = timeSinceBeingGrounded < coyoteTime && !isGrounded;

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Space) && ((isGrounded && canJump) || (canJump && wasGrounded))) {
		velocity.y = 0;
		velocity.y += jumpVelocity;
		isGrounded = false;
		canJump = false;
	}

	/*	bool isPressingKey = false;

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::Space)) {
		isPressingKey = true;
		timeOfJumpPress = clock.getElapsedTime();
	}

	if (wasPressingKey && !isPressingKey && ((isGrounded && canJump) || (canJump && wasGrounded))) {
		velocity.y = 0;
		velocity.y += jumpVelocity * (clock.getElapsedTime() / timeOfJumpPress);
		isGrounded = false;
		canJump = false;
	}*/

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::A)) {
		if (velocity.x > 0) {
			velocity.x -= deceleration * 2;
		}

		if (canMoveLeft) {
			velocity.x -= acceleration;
			velocity.x = velocity.x < -maxVelocity.x ? -maxVelocity.x : velocity.x;
		}
		isMoving = true;
	}

	if (sf::Keyboard::isKeyPressed(sf::Keyboard::D)) {
		if (velocity.x < 0) {
			velocity.x += deceleration * 2;
		}

		if (canMoveRight) {
			velocity.x += acceleration;
			velocity.x = velocity.x > maxVelocity.x ? maxVelocity.x : velocity.x;
		}
		isMoving = true;
	}

	for (auto& bb : *currentLevel->getBBArray()) {
		sf::FloatRect intersection;

		//boundingBoxes["futureHitbox"].setRect(GetFutureRect())

		if (bb.CheckCollision(GetFutureRect(true, true))) {

			//std::cout << "isInCollision" << std::endl;

			// Adjust position based on intersection
			if (velocity.x > 0) {
				position.x -= intersection.width;
			}
			else if (velocity.x < 0) {
				position.x += intersection.width;
			}

			// Stop horizontal movement
			velocity.x = 0;
		}
	}

	if (!isMoving) {
		if (velocity.x > 0) {
			velocity.x -= deceleration;
			velocity.x = velocity.x < 0 ? 0 : velocity.x;
		}
		else if (velocity.x < 0) {
			velocity.x += deceleration;
			velocity.x = velocity.x > 0 ? 0 : velocity.x;
		}
	}

	if (isMoving && isGrounded) {
		currentState = PlayerState::Walk;
	}

	//Player::direction = lerp(Player::direction, direction, lerpSpeed);

	Move(velocity);

	Player::GetCurrentSprite();

	if (velocity.x != 0) {
		lastFacedDirectionX = sign(velocity.x);
	}
}

void Player::setCurrentLevel(Level* level)
{
	currentLevel = level;
	MoveTo(level->getPlayerPos());
}

void Player::Draw(sf::RenderWindow& window) {
	window.draw(drawnSprite);

	for (auto& pair : boundingBoxes) {
		pair.second.Draw(window);
	}
}

void Player::GetCurrentSprite() {

	switch (currentState) {
	case PlayerState::Idle: {

		drawnSprite = CreateSprite((*sprites)["idle"].getTexture());

		IncrementFrameNum(6);
	}
						  break;

	case PlayerState::Walk: {
		drawnSprite = CreateSprite((*sprites)["walk"].getTexture());

		IncrementFrameNum(6);
	}
						  break;

	case PlayerState::Jump: {
		frameNum = 1;

		if (velocity.y > 10) {
			frameNum = 2;
		}
		else if (velocity.y < -5) {
			frameNum = 0;
		}

		drawnSprite = CreateSprite((*sprites)["jump"].getTexture());
	}
						  break;
	}

	count++;
}

void Player::IncrementFrameNum(int modulus)
{
	if (count % (modulus) == 0) {
		frameNum = (frameNum + 1) % maxFrames;
		count = 0;
	}
}

sf::Sprite Player::CreateSprite(const sf::Texture *texture)
{

	sf::Sprite newSprite(*texture, sf::IntRect(frameWidth * frameNum, 0, frameWidth, frameHeight));
	newSprite.setScale(GlobalVariables::getTextureScaler() * sign(lastFacedDirectionX), GlobalVariables::getTextureScaler());
	newSprite.setOrigin(newSprite.getLocalBounds().width / 2, newSprite.getLocalBounds().height / 2);
	newSprite.setPosition(position);

	return newSprite;
}

void Player::Move(sf::Vector2f speed) {
	position += speed;
	//std::cout << position.x << "," << position.y << std::endl;
	currentSprite.setPosition(position);

	for (auto& pair : boundingBoxes) {
		BoundingBox bb = pair.second;

		if (pair.first == "futureHitbox") continue;

		bb.Move(speed);

		boundingBoxes[pair.first] = bb;
	}
}

void Player::MoveTo(sf::Vector2f pos)
{
	position = pos;
	//std::cout << position.x << "," << position.y << std::endl;
	currentSprite.setPosition(position);

	for (auto& pair : boundingBoxes) {
		BoundingBox bb = pair.second;

		//sf::Vector2f offset(bb.GetRect().width / 2, bb.GetRect().height / 2);

		bb.MoveTo(position);

		boundingBoxes[pair.first] = bb;
	}
}

sf::FloatRect Player::GetFutureRect(bool useX, bool useY)
{
	sf::Vector2f tempVelocity(useX ? velocity.x : 0, useY ? velocity.y : 0);
	return sf::FloatRect(position + tempVelocity + boundingBoxes["Hitbox"].getOffset(), boundingBoxes["Hitbox"].getRect().getSize());
}

void Player::CreateBB()
{
	int scaler = GlobalVariables::getTextureScaler();
	BoundingBox box(
		position - sf::Vector2f(drawnSprite.getLocalBounds().width / 6 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler),
		position + sf::Vector2f(drawnSprite.getLocalBounds().width / 6 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler - 40),
		sf::Color::Yellow,
		-sf::Vector2f(drawnSprite.getLocalBounds().width / 6 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler - 30));

	boundingBoxes.emplace("Hitbox", box);

	float boxWidth = box.getRect().width / 2;

	BoundingBox ground(
		position - sf::Vector2f(boxWidth, 0),
		position + sf::Vector2f(boxWidth, 10),
		sf::Color::Magenta,
		sf::Vector2f(-boxWidth, drawnSprite.getLocalBounds().height / 2 * scaler));

	boundingBoxes.emplace("GroundBox", ground);

	//BoundingBox fbox(
	//	position - sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler),
	//	position + sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler - 50),
	//	sf::Color::Red,
	//	-sf::Vector2f(drawnSprite.getLocalBounds().width / 4 * scaler, drawnSprite.getLocalBounds().height / 2 * scaler));

	//boundingBoxes.emplace("futureHitbox", fbox);
}
