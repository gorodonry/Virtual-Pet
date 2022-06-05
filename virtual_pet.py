##
# virtual_pet.py
# Date: 06/03/2022
# Author: Ryan Gordon
# The virtual pet is dead. Long live the virtual pet!

import random
from dataclasses import dataclass, field
from math import inf


def check_s(string: str) -> str:
    """
    Check's whether to add an s after an apostrophe.

    Returns formatted string.
    """
    if list(string.lower())[-1] == "s":
        return string + "'"
    else:
        return string + "'s"


def join_with_and(iterable: list) -> str:
    """Joins a list of strings together with commas and an and."""
    if len(iterable) == 0:
        return ""
    elif len(iterable) == 1:
        return iterable[0]
    elif len(iterable) == 2:
        return f"{iterable[0]} and {iterable[1]}"
    else:
        return f"{', '.join(iterable[0:len(iterable)-1])} and {iterable[-1]}"


@dataclass
class Pet:
    """
    Contains all information on a user's virtual pets.

    Only the name should be specified upon instantiation, unless you know what
    you're doing.
    """

    _name: str
    _boredom: int=0
    _hunger: int=0
    _sounds: list=field(default_factory=lambda: [])
    _health: int=100
    _boredom_limit: int=field(default_factory=lambda: random.randint(50, 90))
    _hunger_limit: int=80

    _standard_max_health: int=100
    _boredom_rate: int=4
    _hunger_rate: int=4

    _strength: str="normal"

    @property
    def strength(self) -> str:
        """Returns the strength of a pet relative to other pets."""
        return self._strength

    @property
    def name(self) -> str:
        """Returns the name of a pet."""
        return self._name

    @property
    def boredom(self) -> int:
        """Returns the boredom level of a pet."""
        return self._boredom

    @boredom.setter
    def boredom(self, value: int) -> None:
        """Increases/decreases the boredom level of a pet."""
        # Boredom cannot exceed 100, or drop below 0
        self._boredom = max(min(100, value), 0)

    @property
    def hunger(self) -> int:
        """Returns the hunger level of a pet."""
        return self._hunger

    @hunger.setter
    def hunger(self, value: int) -> None:
        """Increases/decreases the hunger level of a pet."""
        # Hunger cannot exceed 100, or drop below 0
        self._hunger = max(min(100, value), 0)

    @property
    def sounds(self) -> list:
        """Returns a list of all the sounds a pet can make."""
        return self._sounds

    @sounds.setter
    def sounds(self, sound: str) -> None:
        """Adds a sound to the list of sounds a pet can make."""
        # Number of sounds taught cannot exceed 5
        if len(self._sounds) < 5:
            self._sounds.append(sound)

    @property
    def health(self) -> int:
        """Returns the health of a pet."""
        return self._health

    @health.setter
    def health(self, value: int) -> None:
        """Increases/decreases the health of a pet."""
        # Health cannot exceed max health (can vary), or drop below 0
        self._health = max(min(self.max_health, value), 0)

    @property
    def max_health(self) -> int:
        """Returns the max health of a pet."""
        return self._standard_max_health

    @property
    def boredom_limit(self) -> int:
        """Returns the boredom limit of a pet, cannot be changed."""
        return self._boredom_limit

    @property
    def hunger_limit(self) -> int:
        """Returns the hunger limit of a pet, cannot be changed."""
        return self._hunger_limit

    @property
    def boredom_rate(self) -> int:
        """Returns the boredom rate of a pet (the rate they get bored at)."""
        # Boredom rate increases if the pet is already bored
        if self.boredom > self.boredom_limit:
            return self._boredom_rate * 2
        else:
            return self._boredom_rate

    @property
    def hunger_rate(self) -> int:
        """Returns the hunger rate of a pet (the rate they get hungry at)."""
        # Hunger rate is halved if the pet is close to death
        if self.health <= int(self.max_health / 4):
            rate = int(self._hunger_rate / 2)
        else:
            rate = self._hunger_rate
        if self.strength == "weak":
            # Hunger rate quadruples if boredom is at/above the boredom limit
            if self.boredom >= self.boredom_limit:
                rate *= 4
        else:
            # Hunger rate is doubled per boredom threshold the pet is over
            if self.boredom > self.boredom_limit:
                rate *= 2
            if self.boredom > 90:
                rate *= 2
        return rate

    def tick(self) -> None:
        """Apply the consequences of advancing time by a tick."""
        self.boredom += self.boredom_rate
        self.hunger += self.hunger_rate
        if self.hunger > self.hunger_limit:
            self.health -= int(self.hunger_rate / 2)

    def feed(self, cake) -> None:
        """Feeds a pet a cake."""
        # Feed pet
        self.hunger -= CAKES[cake].hunger
        self.health += CAKES[cake].health
        # Provide relevant feedback
        print(f"Fed {self.name} the {cake}cake.", end=' ')
        if CAKES[cake].hunger != 0 and CAKES[cake].health != 0:
            print(f"{self.name} gained {CAKES[cake].health} health", end=' ')
            print(f"and replenished {CAKES[cake].hunger} hunger!\n")
        elif CAKES[cake].hunger != 0:
            print(f"{self.name} replenished {CAKES[cake].hunger} hunger!\n")
        else:
            print(f"{self.name} gained {CAKES[cake].health} health!\n")

    def train(self, sound: str) -> None:
        """Trains a user entered sound to a pet."""
        if len(self.sounds) == 5:
            print(f"{check_s(self.name)} memory is full!\n")
        else:
            self.sounds.append(sound)
            self.boredom -= 50
            self.hunger += 25
            print(f"Taught {self.name} {sound}.", end=' ')
            print(f"{self.name} lost 50 boredom and gained 25 hunger!\n")

    def mood(self) -> list:
        """Returns a list of strings indicating a pet's mood and vitals."""
        if self.health >= int(0.8 * self.max_health):
            health_message = "fighting fit"
        elif self.health > int(0.2 * self.max_health):
            health_message = "ok"
        elif self.health >= 1:
            health_message = "sick"
        else:
            health_message = "dead"
        if self.hunger < 50:
            hunger_message = "full"
        elif self.hunger <= self.hunger_limit:
            hunger_message = "hungry"
        else:
            hunger_message = "starving"
        if self.boredom < self.boredom_limit:
            boredom_message = "happy"
        elif self.boredom < 90 and self.strength != "weak":
            boredom_message = "bored"
        else:
            boredom_message = "angry"
        return [health_message, hunger_message, boredom_message]

    def status(self) -> None:
        """Prints out current information/status of a pet."""
        print(f"""--{self.name.upper()}--
Health  | {self.health}/{self.max_health} => {self.mood()[0]}
Hunger  | {self.hunger} (limit: {self.hunger_limit}) => {self.mood()[1]}
Boredom | {self.boredom} (limit: {self.boredom_limit}) => {self.mood()[2]}
--{'-' * len(self.name)}--\n""")


@dataclass
class WeakPet(Pet):
    """
    Subclass of Pet with a lower health and a higher boredom limit.

    Only the name should be specified upon instantiation, unless you know what
    you're doing.
    """

    _health: int=50

    _strength: str="weak"

    @property
    def max_health(self) -> int:
        """Returns the max health of a pet."""
        return int(self._standard_max_health / 2)

    @property
    def boredom_limit(self) -> int:
        """Returns the boredom limit of a weak pet, always 100."""
        return 100


@dataclass
class StrongPet(Pet):
    """
    Subclass of Pet with a higher health and hunger rate.

    Only the name should be specified upon instantiation, unless you know what
    you're doing.
    """

    _health: int=200

    _hunger_rate: int=8

    _strength: str="strong"

    @property
    def max_health(self) -> int:
        """Returns the max health of a pet."""
        return self._standard_max_health * 2


@dataclass
class Cake:
    """Contains all information on a type of cake that can be fed to pets."""

    _hunger: int
    _health: int
    _cost: int

    @property
    def hunger(self) -> int:
        """Returns the hunger a cake restores."""
        if self._hunger is inf:
            return "∞"
        else:
            return self._hunger

    @property
    def health(self) -> int:
        """Returns the health a cake restores."""
        if self._health is inf:
            return "∞"
        else:
            return self._health

    @property
    def cost(self) -> int:
        """Returns the cost of a cake."""
        return self._cost

# Define constants
DIFFICULTIES = {"easy": "e", "medium": "m", "hard": "h", "god": "g"}
CAKES = {"cake": Cake(2, 0, 0),
         "berry": Cake(10, 5, 5),
         "banana": Cake(15, 2, 10),
         "peach": Cake(20, 0, 20),
         "pea": Cake(5, 10, 10),
         "bean": Cake(2, 15, 25),
         "pod": Cake(0, 20, 40),
         "ambrosia": Cake(inf, inf, 200)}

if __name__ == "__main__":
    # Determine the difficulty the user wishes to play
    print(f"""{'---VIRTUAL PET---'.center(80)}\n
A quote from the developer:\n{('_' * 31).center(80)}\n
{'"How long can you keep your creations alive?! Muahaha!"'.center(80)}
{('_' * 31).center(80)}\n\n""")
    print("You can choose from: easy, medium, hard, and god.")
    difficulty = ""
    while difficulty not in DIFFICULTIES.values():
        difficulty = input("What difficulty would you like to play at? ")
        difficulty = difficulty.strip().lower()
        if difficulty in DIFFICULTIES:
            difficulty = DIFFICULTIES[difficulty]
        elif difficulty not in DIFFICULTIES.values():
            print("I don't recognise that as a difficulty you can play at.")
    print()

    # Number of ticks incremented between actions
    ticks_in = 1 if difficulty in ["e", "m"] else 5

    tick_count, wallet = 0, 100

    # Get the names of the user's pets, loop until valid
    names = []
    for i in range(1, 4):
        name = ""
        while name == "" or name.lower() in (x.lower() for x in names):
            name = input(f"What is the name of pet {i}? ").strip()
            if name == "":
                print("That's not really a name...")
            elif name.lower() in (x.lower() for x in names):
                print("That'll just get confusing...")
        names.append(name)
    print()

    # Create virtual pets
    pets, cake_hist = {}, {}
    for name in names:
        pets[name.lower()] = random.choice([Pet, WeakPet, StrongPet])(name)
        cake_hist[name.lower()] = []

    # Keep playing until all the pets die!
    cemetery = {}
    while len(pets) > 0:
        for i in range(ticks_in):
            # Print and increment the tick count
            if i == ticks_in - 1:
                print(f"Tick {tick_count}:\n")
            else:
                print(f"Processing tick {tick_count}...")
            tick_count += 1
            # Print out status of each pet after tick applied, record happiness
            n_happy, dead = 0, []
            for pet in pets:
                pets[pet].tick()
                if i == ticks_in - 1:
                    pets[pet].status()
                if pets[pet].mood()[0] == "dead":
                    print(f"{pets[pet].name} has died. RIP\n")
                    dead.append(pet)
                    cemetery[pets[pet].name] = [tick_count, pets[pet].sounds]
                elif pets[pet].mood()[2] == "happy":
                    n_happy += 1
            for pet in dead:
                del pets[pet]
            if len(pets) == 0:
                break
            # Determine coins based off happiness
            if n_happy == 0:
                print("None of your pets are happy (0 coins gained) :(")
            else:
                print(f"{n_happy} pet{'' if n_happy == 1 else 's'}", end=' ')
                print(f"{'was' if n_happy == 1 else 'were'} happy", end=' ')
                print(f"({n_happy} coin{'' if n_happy == 1 else 's'} gained)!")
                wallet += n_happy
            print()
        print(f"You have {wallet} coin{'' if wallet == 1 else 's'}.\n")
        # Get action to take
        if len(pets) > 0:
            action = input(">>> ").strip().lower()
        else:
            action = ""
        print()
        # Advance to the next tick if the user does nothing
        if action != "":
            # Break down command into its components and execute them
            action = action.split(" ")
            if action[0] == "feed":
                try:
                    pet = action[1]
                except IndexError:
                    print("I understood you as far as wanting to", end=' ')
                    print("feed a pet.\n")
                    continue
                try:
                    if pet in pets:
                        if len(action) == 3:
                            cake = action[2]
                        else:
                            print("The cake specified doesn't exist.\n")
                            continue
                    else:
                        print("The pet specified doesn't exist.\n")
                        continue
                except IndexError:
                    print("I understood you as far as wanting to", end=' ')
                    print(f"feed {pets[pet].name}.\n")
                    continue
                # Cake can be added to the end of an input, remove here
                if cake == "cake" or cake == "cakecake":
                    cake = "cake"
                elif ''.join(cake.split("cake")) in CAKES:
                    cake = ''.join(cake.split("cake"))
                # Feed pet the cake if the user can afford it
                if cake in CAKES:
                    if wallet - CAKES[cake].cost >= 0:
                        if difficulty not in ["m", "g"]:
                            pets[pet].feed(cake)
                            wallet -= CAKES[cake].cost
                        else:
                            # Pets cannot eat twice in a row for m/g
                            try:
                                if cake_hist[pet][-1] != tick_count - ticks_in:
                                    pets[pet].feed(cake)
                                    wallet -= CAKES[cake].cost
                                    cake_hist[pet].append(tick_count)
                                else:
                                    print(f"{pets[pet].name} is", end=' ')
                                    print("still full from their", end=' ')
                                    print("last cake!\n")
                            # Exception occurs if no cakes eaten by pet yet
                            except IndexError:
                                pets[pet].feed(cake)
                                wallet -= CAKES[cake].cost
                                cake_hist[pet].append(tick_count)
                    else:
                        print(f"You can't afford {cake}cake.\n")
                # Feed a pet to another pet (blame Hannah and Stirling)
                elif cake in list(pets.keys()):
                    regen = max(pets[cake].hunger_limit - pets[cake].hunger, 0)
                    pets[pet].hunger -= regen
                    pets[pet].boredom = 0
                    print(f"{pets[pet].name} has eaten", end=' ')
                    print(f"{pets[cake].name} (restored all", end=' ')
                    print(f"boredom and {regen} hunger)!\n")
                    print(f"{pets[cake].name} has died. RIP\n")
                    cemetery[pets[cake].name] = [tick_count, pets[cake].sounds]
                    del pets[cake]
                else:
                    print("The cake specified doesn't exist.\n")
            elif action[0] == "teach":
                try:
                    pet = action[1]
                except IndexError:
                    print("I understood you as far as wanting to", end=' ')
                    print("teach a pet a sound.\n")
                    continue
                if pet in pets:
                    try:
                        # action[2:] works with no data at index 2, prevent
                        test = isinstance(action[2], str)
                    except IndexError:
                        print("I understood you as far as wanting to", end=' ')
                        print(f"teach {pets[pet].name}.\n")
                        continue
                    # Pets cannot be taught the same sound twice
                    if ' '.join(action[2:]) not in pets[pet].sounds:
                        pets[pet].train(' '.join(action[2:]))
                    else:
                        print(f"{pets[pet].name} knows that already.\n")
                else:
                    print("The pet specified doesn't exist.\n")
            elif action[0] == "sounds":
                try:
                    pet = action[1]
                    if pet in pets and len(action) == 2:
                        # Print out all the sounds the pet has learnt
                        sounds, name = pets[pet].sounds, pets[pet].name
                        if len(sounds) > 0:
                            print(f"{name} knows how to say:", end=' ')
                            print(f"{join_with_and(sounds)}.\n")
                        else:
                            print(f"{name} hasn't learnt any sounds yet :(\n")
                    elif pet in pets:
                        print("I am confused as to why you wrote", end=' ')
                        print(f"'{' '.join(action[2:])}'.\n")
                    else:
                        print("The specified pet doesn't exist.\n")
                except IndexError:
                    print("I understood you as far as wanting to", end=' ')
                    print("print a list of sounds a pet can make.\n")
            elif action[0] == "cakes":
                if len(action) == 1:
                    # Print out all cakes and their info
                    print("List of available cakes:")
                    for cake in CAKES:
                        print(f"{cake.capitalize()}cake: costs ", end='')
                        print(f"{CAKES[cake].cost}, restores", end=' ')
                        print(f"{CAKES[cake].hunger} hunger and", end=' ')
                        print(f"{CAKES[cake].health} health.")
                    print()
                else:
                    print("Cakes doesn't support secondary commands.\n")
            elif action[0] == "help":
                if len(action) == 1:
                    # Print out all available commands
                    print("""List of available commands:
teach *pet name* *sound* - teaches specified pet the specified sound
feed *pet name* *cake* - feeds specified pet the specified cake (see cakes)
sounds *pet name* - prints a list of sounds the specified pet can make
cakes - prints a list of all the cakes that can be fed to pets
help - prints this text :). Incidentally this is the only place instructions to
    find these instructions are...
[enter] - advances to the next tick without taking an action\n""")
                else:
                    print("Help doesn't support secondary commands.\n")
            else:
                # If the user enters an invalid command, what do I care?
                print("I don't recognise that command.\n")

    # Game over
    print(f"""All your pets have died :(

Well done on lasting {tick_count} ticks!

{names[0]} died on tick {cemetery[names[0]][0]}
    {names[0]} knew how to say {join_with_and(cemetery[names[0]][1])}
{names[1]} died on tick {cemetery[names[1]][0]}
    {names[1]} knew how to say {join_with_and(cemetery[names[1]][1])}
{names[2]} died on tick {cemetery[names[2]][0]}
    {names[2]} knew how to say {join_with_and(cemetery[names[2]][1])}

May they be forever at peace.
""")
