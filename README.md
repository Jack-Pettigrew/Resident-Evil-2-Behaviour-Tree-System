![RE2 Reception FInal Cropped](https://github.com/Jack-Pettigrew/Resident-Evil-2-Behaviour-Tree-System/assets/36480371/0023a237-112b-407a-936d-c728aad8b350)
# Behaviour Tree AI Study: Mr X - Resident Evil 2 Remake
A technical exploration and recreation of the 'Mr X' AI system from Resident Evil 2 Remake (2019) using my own implementation of a Behaviour Tree system. This project serves as my first foray into studying video game AI, starting with creating a Behaviour Tree from the ground up.

Mr. X is an enemy AI that hunts the player through a large building; stalking the halls and searching rooms. I've attempted to mimic that behaviour while learning to create my own basic Behaviour Tree system.

## Features
- A project exploring video game AI, recreating RE2’s ‘Mr X’ with a **Behaviour Tree built from scratch** within Unity.
- **Composite and Action nodes** resembling patrol, search, chase and attack behaviours.
- **Room System** with AI Door handling for Room-to-Room navigation.
- **Object-Oriented architecture** using inheritance to define type-based and **abstract logic**.
- Fully playable with traditional gameplay systems to test AI implementation in a game setting.

## The AI of Mr X
### Behaviour Tree System
### Sensory Events

## Room System
When it comes to Rooms navigation, video game AI are basically Potatoes. Neither can do it, so we have to show them how. T-the AI, n-not the Potatoes...

### The Door Problem
[The Door Problem](https://lizengland.com/blog/2014/04/the-door-problem/) is an age-old game development pitfall where Doors quickly become the bain of existence. Though simple, they quickly introduce seemingly never-ending, recursive complexities that need accounting for.

Not only does Mr. X need to use doors to navigate between rooms, but he needs to know which door to use, reach the door, open the door, _go through_ the door... but not before knowing which door is the correct door to use in the first place. Easy for us, but hard for video game characters.

In light of this, I developed a Room System solution for this project to navigate this issue and implemented it as a branch within the Behaviour Tree as Action nodes. It's primitive but does the trick.

### Bustin' Down the Door
The Room system is composed of multiple sub-systems that work together to achieve the desired result.

First, in Unity, I defined a Room by all the Floor objects that make up its foundation. As a Room also has at least one door, you can add these to a collection within the Room class. Doors are what connect Rooms, with this in mind, each door links to two rooms (or one if it leads outside). Now we have a network of Rooms, we can start shoving Mr. X between them. (I eventually made a tool that automatically links Rooms and Doors together which made adding or removing rooms later on much quicker).

To traverse between Rooms, a Door has two entry points, one on either side. The AI can identify which entry point to use by requesting the Door to tell it which side it's on, returning that point. Continuing through the Behaviour Tree branch the AI navigates to that point, faces the Door, requests it to open, gracefully (not) bends over so as not to knock off his glorious hat, moves to the opposite entry point, and finds itself on the other side in another room ready to do the entire process again. And he doesn't even say thank you!

### Finding the right Door

It's all well and good groovin' through doorways but it doesn't matter if you end up crashing someone's birthday party because you've gone through the wrong door. How does he know which network of rooms to navigate?

_Breadth-first search (BFS)_. After getting the Player's current Room, using the network of Rooms, I start from the AI's Room and traverse each Room via each of their Doors (adding them to a queue) before then giving each Room a numeric value equal to that of the BFS iteration. Once the Player's Room has been reached, I create a collection of Rooms by walking back to the starting Room through each neighbouring Room with a lower number than the last. This gives us our string of Rooms to navigate.

It's a simple solution. There will always be a more elegant or performant solution, but for this project, it worked.

## Screenshots
<img src="https://github.com/Jack-Pettigrew/Resident-Evil-2-Behaviour-Tree-System/assets/36480371/8749b7df-8b43-4339-83db-11c641b21eb1" width="1000">
<img src="https://user-images.githubusercontent.com/36480371/234687024-6575be4c-a1e5-4218-a503-e924fd352fd2.png" width="1000">
<img src="https://user-images.githubusercontent.com/36480371/181758495-9e50acea-ed20-44f9-9d3a-646dfe399be5.jpg" width="1000">
<img src="https://user-images.githubusercontent.com/36480371/181758507-ceb21fde-157d-4287-97bc-9019e13e3b86.jpg" width="1000">
<img src="https://user-images.githubusercontent.com/36480371/181758513-c8170b2c-ab1e-4ffc-9ff8-d363d104388c.jpg" width="1000">
<img src="https://user-images.githubusercontent.com/36480371/234688986-258e5ff5-8179-40bd-a0fc-cd3af9686aee.png" width="1000">
<img src="https://user-images.githubusercontent.com/36480371/234688991-a3a6ce31-5f04-42fd-b686-68c62700f222.png" width="1000">
