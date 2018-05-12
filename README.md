# Alien: Isolation Audio Extractor (WIP)

### A tool to export and name sound files from Alien: Isolation.

This tool will export all sound files within the game Alien: Isolation and name as many as possible. 

Not all final game files are listed in the soundbank I managed to get hold of, so some files aren't able to be named - but they will still be converted to OGG to be playable, just placed in a separate directory.

Converted files will be placed in /DATA/SOUNDS_ORGANISED/ and /DATA/SOUNDS_UNORGANISED/ respectively.

**Voice line support is currently a work in progress. Although all will be exported, only main character files will be named.**

## How to use

<img align="right" src="https://i.imgur.com/vMyXwYE.png" width="50%">

Once you've downloaded, copy the executable from the "Build" folder into your Alien: Isolation game directory (where AI.exe is located).

Run the program and wait for the it to finish. It will prompt you to press a key before starting and finishing. Do not prematurely close the program!

**Exporting and renaming is a very long process and will likely take around an hour and a half to complete.**

The conversion process will use around 10GB of storage space while in operation - make sure to have this available before starting.

## Maximising output

Just running the program is fine - you'll get back the sounds present in the game. If you want to go even further however, PS3 and Xbox 360 versions of the game do include some extra sounds.

Install the game on your console to a USB and follow the extraction processes to get the game files on your PC (I won't cover the specifics of this obviously), copy the SOUND directory and paste it into your Alien: Isolation DATA folder on PC.

Merge the two folders, and repeat this process for both the PS3 and Xbox 360 builds of the game. You'll find you'll get a few more files than just the standard PC build.

## Final mentions

 * BNK extraction is made possible by a modified version of [rickvg's BNKExtract](https://github.com/rickvg/Wwise-BNKExtract).
 * PCK conversion is made possible by [AlphaTwentyThree's wavescan](http://forum.xentax.com/viewtopic.php?f=17&t=4292) bms plugin, along with [QuickBMS](http://aluigi.altervista.org/quickbms.htm) itself.
 * WEM conversion is made possible by [hcs64's ww2ogg](https://github.com/hcs64/ww2ogg) and [Yirkha's REVORB](http://yirkha.fud.cz/progs/foobar2000/revorb.cpp).