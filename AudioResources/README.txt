This extra project is simply here to store the XACT and audio files needed to play the audio of the game.
We decided to store this in a separate project as VS tries to rebuild these files on every compile and that
takes up to a minute, which is way too long.

I don't know if build settings get transfered over git so, if your compile is taking too long, make sure
you go to Build > Configuration Manager and untick AudioResources in the Build column.

Also note: you'll need to build the AudioResources project at least once, otherwise you'll get runtime
errors because of missing files.