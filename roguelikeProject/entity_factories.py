

from components import consumable, equippable
from components.ai import HostileEnemy, SpawnerEnemy, HazardSpawnerEnemy
from components.equipment import Equipment
from components.fighter import Fighter
from components.hazard import PoisonGas
from components.inventory import Inventory
from components.level import Level
from entity import Actor, Item, Hazard

#=====================================================================#
#                              Hazards                                #
#=====================================================================#

poisonGas = Hazard(
    char="▒",
    color=(204, 204, 0),
    name="Poison Bile",
    duration=10,
    is_permanent=False,
    hazard_component=PoisonGas,
    moves_around=True
)

#=====================================================================#
#                             Characters                              #
#=====================================================================#
player = Actor(
    char="@",
    color=(255, 255, 255),
    name="Player",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=30, base_defense=0, base_power=2),
    equipment=Equipment(),
    inventory=Inventory(capacity=26),
    level=Level(level_up_base=200),
)


rat = Actor(
    # The weakest opponent naturally available
    char="r",
    color=(222, 184, 135),
    name="Rat",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=10, base_defense=0, base_power=2),
    equipment=Equipment(),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=5),
)

hound = Actor(
    # A slightly stronger opponent than a rat
    char="h",
    color=(222, 184, 135),
    name="Hound",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=10, base_defense=0, base_power=3),
    equipment=Equipment(),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=10),
)

houndTindalos = Actor(
    # A hard hitting, but fragile opponent
    char="h",
    color=(255, 140, 120),
    name="Hound of Tindalos",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=20, base_defense=0, base_power=10),
    equipment=Equipment(),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=40),
)
skeleton = Actor(
    # Stronger than the dog and rat,
    # more common in deeper levels than aforementioned 2
    char="s",
    color=(255, 245, 238),
    name="Skeleton",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=15, base_defense=1, base_power=3),
    equipment=Equipment(),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=10),
)

skeletonKnight = Actor(
    # Armored version of the skeleton
    # To replace the skeleton further down
    char="S",
    color=(255, 245, 238),
    name="Skeleton Knight",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=45, base_defense=5, base_power=7),
    equipment=Equipment(),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=50),
)

orc = Actor(
    # The first major threat to the player, appearing on level 3 and further
    char="o",
    color=(63, 127, 63),
    name="Orc",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=30, base_defense=2, base_power=5),
    equipment=Equipment(),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=30),
)
troll = Actor(
    char="T",
    color=(0, 127, 0),
    name="Troll",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=60, base_defense=6, base_power=8),
    equipment=Equipment(),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=100),
)

armour_troll = Actor(
    char="T",
    color=(128, 128, 128),
    name="Armoured Troll",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=100, base_defense=12, base_power=11),
    equipment=Equipment(),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=175),
)

# Creatures that summon/get summoned by other creatures
slime = Actor(
    # An enemy to be spawned by mama_slime opponent
    char="m",
    color=(255, 80, 80),
    name="Slime ",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=8, base_defense=0,  base_power=3),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=5),
    equipment=Equipment(),
)


mama_slime = Actor(
    # An enemy that avoids conflict and spawns basic slimes
    char="M",
    color=(255, 80, 80),
    name="Mama Slime",
    ai_cls=SpawnerEnemy,
    fighter=Fighter(hp=40, base_defense=0,  base_power=0),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=40),
    equipment=Equipment(),
)
mama_slime.ai.setup(slime, 5)
# To set up the mama_slime with AI


slime_bile = Actor(
    # An enemy to be spawned by mama_slime opponent
    char="m",
    color=(20, 255, 80),
    name="Slime ",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=15, base_defense=2,  base_power=4),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=15),
    equipment=Equipment(),
)


mama_bile = Actor(
    # An enemy that avoids conflict and spawns basic slimes
    char="M",
    color=(20, 255, 80),
    name="Mama Bile",
    ai_cls=SpawnerEnemy,
    fighter=Fighter(hp=80, base_defense=2,  base_power=3),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=80),
    equipment=Equipment(),
)
mama_bile.ai.setup(slime_bile, 5)
# To set up the mama_slime with AI

bile_spew = Actor(
    # An attempt at an enemy that spawns a gas entity, does not work as of 16/10/24
    char="B",
    color=(20, 255, 80),
    name="Bile Gas Spew",
    ai_cls=HazardSpawnerEnemy,
    fighter=Fighter(hp=60, base_defense=0,  base_power=2),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=60),
    equipment=Equipment(),
)
bile_spew.ai.setup(poisonGas, 2)

#=====================================================================#
#                        Consumables - Potion                         #
#=====================================================================#
# Potions are to be templated into this area
health_potion = Item(
    char="!",
    color=(127, 0, 255),
    name="Health Potion",
    consumable=consumable.HealingConsumable(amount=4),
)
greater_health_potion = Item(
    char="!",
    color=(180, 0, 255),
    name="Great Health Potion",
    consumable=consumable.HealingConsumable(amount=12),
)

#=====================================================================#
#                        Consumables - Scroll                         #
#=====================================================================#
# Scrolls/spells are to be templated into this area
lightning_scroll = Item( # Longer ranged spell with notable damage
    char="~",
    color=(255, 255, 0),
    name="Lightning Scroll",
    consumable=consumable.LightningDamageConsumable(damage=20, maximum_range=5),
)
smite_scroll = Item( # Close ranged spell with massive damage
    char="~",
    color=(255, 215, 0),
    name="Smite Scroll",
    consumable=consumable.LightningDamageConsumable(damage=60, maximum_range=2),
)
confusion_scroll = Item( # Scrambles movement of a target
    char="~",
    color=(207, 63, 255),
    name="Confusion Scroll",
    consumable=consumable.ConfusionConsumable(number_of_turns=8),
)
weaken_scroll = Item( # Lowers target's attack by 1 permamently
    char="~",
    color=(255, 0, 255),
    name="Weaken Scroll",
    consumable=consumable.WeakenConsumable(number_of_turns=1),
)
strength_scroll = Item( # Increases target's attack by 1 permamently
    char="+",
    color=(255, 215, 0),
    name="Strength Scroll",
    consumable=consumable.StrenghtenConsumable(number_of_turns=1),
)
fireball_scroll = Item( # Area of effect damage spell
    char="~",
    color=(200, 0, 0),
    name="Fireball Scroll",
    consumable=consumable.FireballDamageConsumable(damage=15, radius=3),
)
fireblast_scroll = Item( # Large area of effect damage spell, likely to hit the caster
    char="~",
    color=(255, 0, 0),
    name="Fireblast Scroll",
    consumable=consumable.FireballDamageConsumable(damage=30, radius=4),
)
#=====================================================================#
#                             Equipment                               #
#=====================================================================#
# Various types of equipment are templated below


#==============================WEAPONS================================#
# Symbols: /, }
dagger = Item(
    char="/", color=(180, 189, 186),
    name="Dagger", equippable=equippable.Dagger())
hatchet = Item(
    char="/", color=(190, 199, 196),
    name="Hatchet", equippable=equippable.Hatchet())
sword = Item(
    char="/", color=(202, 209, 206),
    name="Sword", equippable=equippable.Sword())
swordAndShield = Item(
    char="}", color=(249, 169, 48),
    name="Shortsword and Shield", equippable=equippable.ShortSwordAndShield())
axe = Item(
    char="/", color=(212, 219, 216),
    name="Axe", equippable=equippable.Axe())
greatsword = Item(
    char="/", color=(232, 219, 216),
    name="Greatsword", equippable=equippable.Greatsword())
swordAndBigShield = Item(
    char="}", color=(202, 209, 206),
    name="Sword and Greatshield", equippable=equippable.SwordAndGreatShield())
towershield = Item(
    char="}", color=(232, 219, 216),
    name="Towershield", equippable=equippable.GreaterShield())

#==============================ARMOUR================================#
# Symbols: [
cloth_armour = Item(
    char="[", color=(255,255,255),
    name="Clothing", equippable=equippable.cloths())

leather_armour = Item(
    char="[", color=(171, 88, 46),
    name="Leather Armour", equippable=equippable.LeatherArmour())

chain_mail = Item(
    char="[", color=(128, 128, 128),
    name="Chain Mail", equippable=equippable.ChainMail())

strong_leather_armour = Item(
    char="[", color=(227, 118, 64),
    name="Strong Leather Armour", equippable=equippable.StrongLeatherArmour())

strong_chain_mail = Item(
    char="[", color=(171, 169, 169),
    name="Chain Mail of Strength", equippable=equippable.StrongChainMail())

#==============================HELMETS===============================#
# Symbols: ^
leather_cap = Item(
    char="^", color=(171, 88, 46),
    name="Leather Cap", equippable=equippable.LeatherCap())

agileCap = Item(
    char="^", color=(227, 118, 64),
    name="Agility Cap", equippable=equippable.AgileCap())

featherCap = Item(
    char="^", color=(227, 118, 64),
    name="Feather Cap", equippable=equippable.FeatherCap())

chainCoif = Item(
    char="^", color=(128, 128, 128),
    name="Chain Coif", equippable=equippable.ChainCoif())

knightHelm = Item(
    char="^", color=(171, 169, 169),
    name="Knight Helm", equippable=equippable.FullPlateHelmet())

#===============================RINGS================================#
# Symbols: c

strengthRing = Item(
    char="c", color=(255, 151, 0),
    name="Ring of Strength", equippable=equippable.StrengthRing())
hardRing = Item(
    char="c", color=(52, 192, 235),
    name="Hardened Ring", equippable=equippable.HardenedRing())
powerRing = Item(
    char="c", color=(200, 50, 46),
    name="Ring of Great Strength", equippable=equippable.PowerRing())
shieldRing = Item(
    char="c", color=(52, 156, 235),
    name="Ring of Shields", equippable=equippable.ShieldingRing())
recklessRing = Item(
    char="c", color=(255, 23, 0),
    name="Ring of Reckless Strength", equippable=equippable.RecklessRing())
turtleRing = Item(
    char="c", color=(52, 92, 235),
    name="Ring of the Turtle", equippable=equippable.TurtleRing())

#==============================AMULETS===============================#
# Symbols: u
strengthAmulet = Item(
    char="u", color=(255, 151, 0),
    name="Amulet of Strength", equippable=equippable.StrengthAmulet())
agileAmulet = Item(
    char="u", color=(52, 192, 235),
    name="Amulet of Agility", equippable=equippable.AgileAmulet())
powerAmulet = Item(
    char="u", color=(200, 50, 46),
    name="Amulet of Greater Strength", equippable=equippable.PowerAmulet())
steelAmulet = Item(
    char="u", color=(52, 156, 235),
    name="Amulet of Steelskin", equippable=equippable.SteelAmulet())
berserkAmulet = Item(
    char="u", color=(255, 23, 0),
    name="Amulet of Berserk Strength", equippable=equippable.BerserkAmulet())
knightsAmulet = Item(
    char="u", color=(52, 92, 235),
    name="Amulet of the Knight", equippable=equippable.KnightsAmulet())
titansAmulet = Item(
    char="u", color=(0, 255, 23),
    name="Titan's Amulet", equippable=equippable.TitansAmulet())