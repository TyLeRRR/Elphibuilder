# Elphibuilder



#### Build Elbphilarmonie in a classic Tetris style.

The user is an architect who has to build the [Elbphilharmonie](https://en.wikipedia.org/wiki/Elbphilharmonie) with the help of different blocks. He is given some amount of money and building blocks in three various forms. The blocks spawn randomly.

The internal algorithm calculates the construction costs based on the following criteria:
- how fast has the block landed.
-  the shape of the block.
-  how many floors were fully constructed with the help of this block

**You lose**  - 1) if the value in the construction costs bar is negative. 2) put a block in the wrong place. The animation with an angry investor will be triggered. 

**You win** -  if the user has placed all blocks correctly and he needed less investment than initially received. That will trigger the animation with happy visitors going towards the entrance.

What is left from the building costs will be added to the highscore board.

![Gameplay](elphibuilder.gif "Elphibuilder Gameplay")

