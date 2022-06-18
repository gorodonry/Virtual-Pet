# Virtual-Pet
A simple space-invaders type game with a GUI. Space-invaders in the sense that it's only a matter of time before you lose. Instructions can be found here: https://onslowcollege.github.io/13dtc/programming/virtual-pet.

**How to play**

The game works on the basis of ticks, an arbitrary measurement of time. The aim of the game is to keep your pets alive for as long as possible. The controls and stats of your pets are very simple - a pet has health, hunger, and boredom levels. If health drops to 0 the pet dies. If hunger increases beyond a certain point the pet starts to starve (causes health to go down). If boredom increases beyond certain thresholds the pet's hunger goes up at an accelerated rate.

Each tick you can take one of three actions:
	- Feed a pet (reduces hunger and/or health of the pet)
	- Teach a pet a sound (reduces boredom of the pet at the expense of increasing hunger)
	- Pass (do nothing)

Pets are fed cakes, which cost money. Money is earnt by keeping pets happy. Pets are kept happy by teaching them sounds, but each pet can learn a maximum of five sounds over the course of the game.

There is an option which users can choose to enable in the name selection view called 'The Hannah extension'. This extension allows users to feed pets to other pets (i.e. pets can also function as 'cakes' if this option is enabled).

The user has three pets. Each tick the action taken only applies to the pet selected by the user.

This GUI uses the WPF.
