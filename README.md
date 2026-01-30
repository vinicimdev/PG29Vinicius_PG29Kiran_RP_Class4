# PG29Vinicius\_PG29Kiran\_RP\_Class4



\## Project Structure



This repository contains 2 Unity projects:



\### 1. Minigame Project (Cooking Gameplay)



Contains:



Minigame Scene - Main cooking gameplay

Win Screen - Displayed when plate is completed

Share Screen - Share results after winning



\### 2. Menu Project (UI \& Navigation)



Contains:



Main Menu - Start screen

Shop - Buy ingredients/items

Ingredient Preparation Screen - Select ingredients before starting the minigame



\##How the Minigame Works



Drag and Drop System



Tap/Click on an ingredient and drag it to the plate

Ingredient becomes semi-transparent while dragging

If released over the plate: ingredient disappears and plate fills up

If released outside plate: ingredient returns to original position

Works on both mouse and touch (mobile ready)



Plate Fill System



Plate uses a Filled Image that fills from bottom to top

Each ingredient adds +33% fill

After 3 ingredients (100% fill), the plate is complete

When complete: automatically loads the next scene (Win Screen)

