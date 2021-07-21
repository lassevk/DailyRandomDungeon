# DailyRandomDungeon

This is a very simple program for Windows for us that plays Elder Scrolls Online on a console.

Every time you do a random daily dungeon or some other daily random activity, you get a 20 hour countdown timer. Until that timer has elapsed, you get less rewards.
Only when the full 20 hours have elapsed do you again get the bigger daily reward from that activity.

Sadly, this 20 hour countdown is not displayed in the UI. The only thing you can use to keep track of this on the console is to check the daily random reward information.
If the reward is written in purple text, you can get the bigger daily reward. If it is written in blue then the 20 hour countdown is still counting.

On PC you can install addons that keep track of this, but on consoles this is not possible, hence this program.

# How to install it

1. Download the program from the Releases, latest releast is https://github.com/lassevk/DailyRandomDungeon/releases/tag/0.1-beta
2. Right-click the zip file and select Properties
3. Check the "Unblock" checkbox at the bottom of this dialog and click OK
4. Unzip the zip file into a folder of your choice
5. Make a shortcut somewhere, desktop or start menu, to the DailyRandomDungeon.exe executable

# How to use it

Start the program.

Then, to add a character, type in the character name in the textbox at the bottom of the window and hit Enter to add a new character.
You can add as many characters as you'd like.

Then, whenever a character completes the daily random dungeon, and you get the "Activity completed" message on your screen, click the Completed button for that character.
This will start the 20 hour countdown timer for the character, in the program.

The status of a character will be displayed as:

* Green background means the character have access to the daily reward bonus
* Yellow background means the character is still on countdown, but it is less than 1 hour until it completes
* Red background means the character is still on countdown, and it's more than 1 hour

As long as there is a countdown, the time left will also be displayed to the left of the character name.

If you mis-click on a character and start the countdown for it you can click the Undo button that pops up to restore it back to its previous state.

If you wish to remove a character use the X button to the far right of the character name.
