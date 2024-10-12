

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
    name="Poison",
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
    fighter=Fighter(hp=30, base_defense=1, base_power=2),
    equipment=Equipment(),
    inventory=Inventory(capacity=26),
    level=Level(level_up_base=200),
)


bat = Actor(
    char="^",
    color=(222, 184, 135),
    name="bat",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=10, base_defense=0, base_power=2),
    equipment=Equipment(),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=2),
)
skeleton = Actor(
    char="s",
    color=(255, 245, 238),
    name="Skeleton",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=15, base_defense=1, base_power=3),
    equipment=Equipment(),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=10),
)

orc = Actor(
    char="o",
    color=(63, 127, 63),
    name="Orc",
    ai_cls=HostileEnemy,
    fighter=Fighter(hp=30, base_defense=3, base_power=6),
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

slime = Actor(
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
bile_spew = Actor(
    char="B",
    color=(20, 255, 80),
    name="Bile Gas Spew",
    ai_cls=HazardSpawnerEnemy,
    fighter=Fighter(hp=60, base_defense=0,  base_power=2),
    inventory=Inventory(capacity=0),
    level=Level(xp_given=60),
    equipment=Equipment(),
)
bile_spew.ai.setup(poisonGas, 4)

#=====================================================================#
#                        Consumables - Potion                         #
#=====================================================================#

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

lightning_scroll = Item(
    char="~",
    color=(255, 255, 0),
    name="Lightning Scroll",
    consumable=consumable.LightningDamageConsumable(damage=20, maximum_range=5),
)
confusion_scroll = Item(
    char="~",
    color=(207, 63, 255),
    name="Confusion Scroll",
    consumable=consumable.ConfusionConsumable(number_of_turns=8),
)
weaken_scroll = Item(
    char="~",
    color=(255, 0, 255),
    name="Weaken Scroll",
    consumable=consumable.WeakenConsumable(number_of_turns=1),
)
strength_scroll = Item(
    char="+",
    color=(255, 215, 0),
    name="Strength Scroll",
    consumable=consumable.StrenghtenConsumable(number_of_turns=1),
)
fireball_scroll = Item(
    char="~",
    color=(255, 0, 0),
    name="Fireball Scroll",
    consumable=consumable.FireballDamageConsumable(damage=12, radius=3),
)
#=====================================================================#
#                             Equipment                               #
#=====================================================================#
dagger = Item(
    char="/", color=(0, 191, 255), name="Dagger", equippable=equippable.Dagger()
)

sword = Item(char="/", color=(0, 191, 255), name="Sword", equippable=equippable.Sword())

leather_armour = Item(
    char="[",
    color=(139, 69, 19),
    name="Leather Armour",
    equippable=equippable.LeatherArmour(),
)

chain_mail = Item(
    char="[", color=(139, 69, 19), name="Chain Mail", equippable=equippable.ChainMail()
)

