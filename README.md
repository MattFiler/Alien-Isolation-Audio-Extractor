# Alien: Isolation Audio Extractor

### A tool to export and name sound files from Alien: Isolation.

Converted files will be placed in /DATA/SOUNDS_ORGANISED/ when the extraction process has completed. Any sounds which cannot be named will be placed in /DATA/SOUNDS_UNORGANISED/.

## How to use

1) [Click here](https://github.com/MattFiler/Alien-Isolation-Audio-Extractor/raw/master/Build/Audio%20Extractor.exe) to download the latest build. 
2) Once downloaded, copy the program into your Alien: Isolation game directory (where AI.exe is located).
3) Run the program and wait for it to finish.
4) Once finished, the tool will open the output folder for you.

## Note

Full voice line support is currently a **work in progress**. Voice files will be exported (and some named), however for the time being they will stay in WEM format within the unorganised folder. Conversion and full naming functionality for voices is intended to be implemented at a later date.

## Final mentions

 * BNK extraction is made possible by a modified version of [rickvg's BNKExtract](https://github.com/rickvg/Wwise-BNKExtract).
 * PCK conversion is made possible by [AlphaTwentyThree's wavescan](http://forum.xentax.com/viewtopic.php?f=17&t=4292) bms plugin, along with [QuickBMS](http://aluigi.altervista.org/quickbms.htm) itself.
 * WEM conversion is made possible by [hcs64's ww2ogg](https://github.com/hcs64/ww2ogg) and [Yirkha's REVORB](http://yirkha.fud.cz/progs/foobar2000/revorb.cpp).