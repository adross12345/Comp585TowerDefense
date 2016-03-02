# Smart Towers
Client: Vladimir Jojic {vjojic@cs.unc.edu}

Team: Franz Dominno, Stephen Yan, Thomas Allen, Andrew Ross

{dominno, sjyan, tjallen, adross}@live.unc.edu

# Design Documentation

### Concept

Tower defense games involve placements of simple defensive structures that pick off enemies arriving in waves following a winding path. Friendly units may move along the path taking out the enemies. Typically, towers destroy enemies and ignore friendly units. Rather than making the placement of towers the key challenge, the proposed game requires player to train towers to discriminate friend from foe. In order for the player to be successful, she would need to pick training examples (sprites) of enemies and friends, select structure of the network performing foe detection, and feed these to defense tower training algorithm. In the context of neural nets, a single node would provide a detector that picks up on a feature of enemy unit's appearance. More nodes in a single layer would lead to more features that can be detected. Consequently a tower might have ability to detect different units with lower certainty, or detect a particular unit with high certainty. Two layer network would provide ability to detect complex appearances, for example, if the enemy's parts are similar to friend's but their composition differs. Visualization of tower's decision to fire -- which nodes in the network activate and why -- would provide very strong cues about how neural networks work.

We aim to help people understand conceptually the mechanics and utility of machine learning through the decision-making mechanics of a simple tower defense game. We are developing in the Unity3D environment.

### Gameplay

+ **Objectives**: As a single-player game, the goal is to survive as many levels as possible and to keep your control tower safe and alive
+ **Progression**: The player succeeds in a level as long as the control tower's health is not fully depleted. The control tower's health is decremented every time an enemy unit successfully makes it all the way there. As the player proceeds through levels, waves become more and more dense and enemy units become more diverse, forcing the player to allocate resources and plan more strategically.

The distinguishing factor for this tower defense game is the training property of the towers. Every tower a player instantiates requires training - this is the machine learning component. There will be a separate view for when a user buys a tower that will display the tower and a simple wave - the player essentially plays a simulation as the tower to shoot and eliminate desired units - the way in which the player trains the tower will affect its automatic performance in the game view.

+ There will be an options window that will display status (health/money/resources) and options to buy resources
+ There will also be simple animations to visualize the machine learning decision-making process of the tower's choices to fire

### World
+ **Look & Feel**: Maps are subject to change in difficulty and complexity in accordance to the level, but the general layout of the world is minimalistic. There is unused space that is unwalkable by allies and enemies - this is mainly used as guidelines for the units to maneuver. The moveable territory is green and unmovable territory is black. Towers may be placed in either territories.
+ **Areas**:

### Characters
+ **Protagonists**: Tower units - the player's control tower and attacking towers, ally units
.+ There is only one main tower, but as levels progress, there will be more options for towers (changes in frequency and speed of fire commensurate with price)
+ **Antagonists**: Enemy units, boss units
..+ As player progresses, units get more diverse and inflict more damage - move faster and more unpredictably

Units are simple square sprites with different colors and patterns to distinguish themselves as friend or foe. The main tower is the largest and attacking towers are smaller, turret-like structures.

### Mechanics
+ Rules: Enemy and ally units spawn in waves and follow paths to the control tower. When enemy units reach the tower, the tower's health will be decremented. When ally units reach the tower, the player gains money, which can be spent on resources (attacking towers). Resource towers are not affected by units. In the training view, units reaching the tower will not affect anything but the efficacy of the attacking tower in real play. When the main tower's health is depleted, the player loses. A player's success is determined by how high the level and the amount of money earned.
+ Physics: Enemy units move semi-randomly along the moveable territory towards the control tower. 
+ Options: Through the main status/options window, the player will be able to buy resources - purchased towers may be placed anywhere on the map.
+ Replay/Saving:


