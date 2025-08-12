#!/usr/bin/env python3

# Import calls
import tcod
import colour


import setup_game

import exceptions
import input_handlers

import traceback

def save_game(handler: input_handlers.BaseEventHandler, filename: str) -> None:
    # If the current event handler has an active Engine then save it.
    if isinstance(handler, input_handlers.EventHandler):
        handler.engine.save_as(filename)
        print("Game saved.")


def main() -> None:
    # Determines the size of the playscreen
    screen_width = 100
    screen_height = 60

    tileset = tcod.tileset.load_tilesheet("Anno_16x16.png",16,16,tcod.tileset.CHARMAP_CP437)

    # A new event handler object is created to handle user inputs
    handler: input_handlers.BaseEventHandler = setup_game.MainMenu()

    # Creating the screen within the given parameters
    with tcod.context.new_terminal(
        screen_width,
        screen_height,
        tileset=tileset,
        title="The Endless Crypt",
        vsync=True,
    ) as context:
        # Creating the 'terminal' for the screen
        root_console = tcod.console.Console(screen_width,screen_height,order="F")
        try:
            while True:
                root_console.clear()
                handler.on_render(console=root_console)
                context.present(root_console)

                try:
                    for event in tcod.event.wait():
                        context.convert_event(event)
                        handler = handler.handle_events(event)
                except Exception:  # Handle exceptions in game.
                    traceback.print_exc()  # Print error to stderr.
                    # Then print the error to the message log.
                    if isinstance(handler, input_handlers.EventHandler):
                        handler.engine.message_log.add_message(
                            traceback.format_exc(), colour.error
                        )
        except exceptions.QuitWithoutSaving:
            raise
        except SystemExit:  # Save and quit.
            save_game(handler, "savegame.sav")
            raise
        except BaseException:  # Save on any other unexpected exception.
            save_game(handler, "savegame.sav")
            raise


if __name__ == "__main__":
    main()