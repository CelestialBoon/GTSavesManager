HOW GOLDEN TREASURE FUNCTIONS
The game state is mainly stored in two files: savedGame.gt and globalSave.gt.
The savedGame keeps track of the current story run, including shinies, artifacts, stats, events, and whatnot.
The globalSave collects what remains between runs, like completed masteries, tarot cards, gallery unlocks, and the like.

HOW THE SAVE MANAGER WORKS
In order to properly "reset" the game and eg. start over with no advantages, it is necessary to replace the globalSave with a new, fresh one.
In order to do so, one can click the New button in the upper left of the program window, allowing the creation of a new globalSave (eg. gs02) alongside the existing one (eg. gs01). The switch between one and the other can be done through the Go button.

Additionally, the program allows for the backup of story points for later revisiting, which is what the larger buttons are about, allowing one to create new save points, overwrite them, and restore them in order to play them again. For example, this could allow the quick in-game exploration of all narrative branches of a particular event.
(Save points are tied to globalSaves in order to avoid contamination.)

The program requires knowledge of where the save folder and the GT launcher are located. Usually the default ones will be already there, but in case of weird installations, they will have to be provided manually through the Settings window.

The game needs to be closed in order for successful overwriting of save data to take place. You will be prompted in case this condition is required for the save manager's functioning.