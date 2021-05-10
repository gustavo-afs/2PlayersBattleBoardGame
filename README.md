# 2PlayersBattleBoardGame
 Unity project with board game mechanics for two players.

## Project Overview

-Title: Monsters Battle

-Plataform: PC Standalone

-Genre: 3D Strategy Board Game

-Game Engine: Unity 3D (2019.3.5f1)

## High Concept

Monsters Battle is a 3D strategy Board Game with 2 local players where you need to move your character through the tilemap to collect resources and get stronger to defeat your opponent using a Dice Battle;

## Synopsys

Both Characters are monsters that want to fight to see who's the strongest monster of all times, but to do that they need to think about the best stratagy.

## Game Objectives

The main objective is to defeat the opponent player in 'Dice Battles' using the resources collected at the level.

## Game Rules

Each characters have a quantity of moves to cross the map above the tiles. The player can only move over the available directions (ortogonal movement) and when the moves count reach 0, the turn player changes and the another player will restart the movement cycle.

Every tile has a collectable that will help the player to increase the Health and Attack Points, Extra Dices quantiy or add a new turn movement. When there's under 10% collectables available the game will fill the empty tiles with new collectables.

When the players are ortogonally next to each other the 'Battle System' starts and both players roll dices. Each player roll three dices and one extra dice if the player collected it during the same turn and the 3 greater dices values will be used to compare values between each other. Ex.: The 1st greater dice value of the player q will be compared to the 1st greater dice value of the player 2. The greater between each comparison wins a point, if the values are equal the turn player wins a point. After all comparisons, the battle winner will damage the Health points of the loser player with the winner player attack points.

When a characther Health Points reach 0 the oponnent is declared winner.

## Game Structure

The Structure is a turn based on movement system, with battles when the players are next to each other. It's 


[Main Menu] -> [Game Level] -> [Create the Tile Map] -> [Allocate the Player] ->

-> [Moviment Cycle]( [Player 1 Moves] > [Collects the Tile Item] > [Available Moves reach 0] > [Player 2 Moves] > ... ) -> [Players are next to each other] ->
-> [Battle Start] -> [Players Rolls Dicess] -> [Dices values are calculated] -> [winner attack points are subtracted from theloser health points] -> [Move or Win Condition]->
-> [Health points reach 0] -> [Winner is declared] -> [Replay or return to menu]


## Gameplay

The commands for this game are simple click to move. The player must move just 1 tile ortogonally at the Highlighted possible moves, the battle system happens automatically but it's displayed to both players so they can confirm the values and the battle winner of each dice.

## HUD and GUI

The HUD consists in two panels representing both players Stats, they will be updated after every movement with the Health Points, Attack Points and Extra Dices values, the game will display the available movements left information at the top of the screen.

The GUI elements are the Main Menu with Play and Exit Game options and the Panel after the end of the game where the player can choose between replay or return to main menu.

## Collectables 

ID | Name   | Color  | Description
03 | Move   |  Pink  | Adds another available movement for the turn player, it must be used at the same turn
04 | Attack | Yellow | Increase the Attack points
05 | Health |  Red   | Increase the Health points
06 | Dices  |  Blue  | Adds an extra dice, that can be used 1 per battle at the same turn.

## Art Aspects

This game was made using only free assets from Unity Asset store. The idea is to give a happy coloful look to the game.

The monsters and tiles: https://assetstore.unity.com/publishers/3867
Dices: https://assetstore.unity.com/packages/templates/packs/dice-pack-light-165

## Sound Design

The game sound until now it's just provided at the end of the game when the player defeats it's opponent. 
