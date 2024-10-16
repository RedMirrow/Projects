# A file to store any colours needed for
# later references/UI
from typing import Tuple

white = (0xFF, 0xFF, 0xFF)
black = (0x0, 0x0, 0x0)
red = (0xFF, 0x0, 0x0)
green = (0x0, 0xFF, 0x0)
blue = (0x0, 0x0, 0xFF)
purple = (0xFF, 0x0, 0xFF)

player_atk = (0xE0, 0xE0, 0xE0)
enemy_atk = (0xFF, 0xC0, 0xC0)
needs_target = (0x3F, 0xFF, 0xFF)
status_effect_applied = (0x3F, 0xFF, 0x3F)
descend = (0x9F, 0x3F, 0xFF)

invalid = (0xFF, 0xFF, 0x00)
impossible = (0x80, 0x80, 0x80)
error = (0xFF, 0x40, 0x40)

player_die = (0xFF, 0x30, 0x30)
enemy_die = (0xFF, 0xA0, 0x30)

welcome_text = (0x20, 0xA0, 0xFF)
health_recovered = (0x0, 0xFF, 0x0)

bar_text = white
bar_filled = (0x0, 0x60, 0x0)
bar_empty = (0x40, 0x10, 0x10)

menu_title = (255, 255, 63)
menu_text = white

# "The Crypt" tile colours
bg_crypt = (0, 0, 0)
wall_crypt = (195, 202, 214)
floor_crypt = (160, 166, 176)

# "The Dungeon" tile colours
bg_dung = (0, 0, 0)
wall_dung = (146, 116, 150)
floor_dung = (123, 89, 128)

# "The Oubliette" tile colours
bg_oubl = (0, 0, 0)
wall_oubl = (255, 255, 255)
floor_oubl = (153, 104, 104)

current_bg = bg_crypt
current_wall = wall_crypt
current_floor = floor_crypt


def gray_scale_color(color: Tuple[int, int, int]) -> Tuple[int, int, int]:
    """Convert a color to grayscale."""
    # Calculate the grayscale value
    gray_value = sum(color) // 3

    # Create a new tuple with the grayscale value repeated three times
    gray_color = (gray_value, gray_value, gray_value)

    # Return the grayscale color
    return gray_color

