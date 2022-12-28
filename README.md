# TF2 Weapon Specific Crosshairs by HbiVnm
TF2WSC (Team Fortress 2 Weapon Specific Crosshairs) is a client-side solution to automating the installation and make customization of weapon specific crosshairs easier for Team Fortress 2.

Preview image:
![Preview image of TF2WSC.](https://i.imgur.com/VtGxozZ.png)

## What does TF2WSC specifically do?
When installing a config through TF2WSC, a new folder in `tf\custom` called `TF2WeaponSpecificCrosshairs` will be created. This folder will contain crosshair materials and script files necessary for your chosen weapon specific crosshairs.

## Download
Check out the releases and download [here](https://github.com/hbivnm/TF2WeaponSpecificCrosshairs/releases).

## Tutorial / Installation
**NOTE:** If you already have an old custom/weapon specific crosshair folder in `tf\custom` delete it or rename it to `TF2WeaponSpecificCrosshairs`

1. Download the latest release found [here](https://github.com/hbivnm/TF2WeaponSpecificCrosshairs/releases).
2. Extract the folder named `TF2WeaponSpecificCrosshairs` to a directory of your liking.
3. Run TF2WSC and customize your weapon specific crosshair config.
4. Hit Install/Update.
5. Set `cl_crosshair_file ""` in Team Fortress 2.

**NOTE:** Malwarebytes will report the executable as `MachineLearning/Anomalous.X%`, you will have to manually exclude the `TF2WeaponSpecificCrosshairs` folder to be able to use TF2WSC alongside Malwarebytes. A more detailed explanation to this issue can be found [here](https://forums.malwarebytes.com/topic/271784-machinelearninganomalous100-all-my-c-projects/) and [also here](https://forums.malwarebytes.com/topic/238670-machinelearninganomalous-detections-and-explanation/), explained by a Staff member on Malwarebytes forum.

## Help / FAQ
See the [wiki](https://github.com/hbivnm/TF2WeaponSpecificCrosshairs/wiki).

## The future of TF2WSC
### Planned features
- [ ] Add checkbox to change "ALL" checkboxes into (ex.) "ALL primary weapons for _CLASS_".
- [ ] Add "Update TF2WSC" notification when new release is available.
- [ ] Suggestion by depi: Double-clicking a weapon in the ListView should select it as "current weapon". (Note to self: add class listviewentry)
- [ ] Detect if non-TF2WSC config is present and rename folder. (This will probably mark the official 1.0 release)

### Implemented features
- [x] Add current version label.
- [x] Add a "Help" button.
- [x] Read current TF2WSC config.
- [x] Add a wiki to this repo.
- [x] Add a button to sync crosshairs from public repo (eliminates having to wait for new update for newly added crosshairs)
- [x] Show/Hide console ~~(state saved inbetween session)~~.
- [x] Easier to read/understand state of installation/update.
- [x] Suggestion by Pijukazz: Add no explosion to scripts for explosive weapons. (this will also solve crosshair issues related to explosive weapons, big thanks to shinso)
- [x] ~~Rename "Add to all" button.~~ Removed button instead.
- [x] TF2 Path input field saved inbetween sessions.
- [x] Suggestion by Pijukazz: Add a button for adding a crosshair ONLY to selected class ("Add to *CLASS*").
- [x] Suggestion by Kanga: Buttons for "Add to all PRIMARY/SECONDARY/MELEE.
- [x] Make console read-only (thanks depi LOL)

# Original idea
Credit where it's due, the original idea of weapon specific crosshairs in Team Fortress 2 (as far as I know) comes from a [teamfortress.tv thread](https://www.teamfortress.tv/30866/guide-weapon-specific-custom-crosshairs) by [joshuawn](https://www.teamfortress.tv/user/joshuawn).

# Icons (Fugue Icons 3.5.6)
Most icons used was provided by [Yusuke Kamiyamane](http://p.yusukekamiyamane.com/). Licensed under a [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/).
