# MetalGearRedirector

MelonLoader mod for the METAL GEAR & METAL GEAR 2: Solid Snake (Master Collection) launcher that opens the game manuals from another site, by default the Polish manuals on metalgear.grzyb.app.

Table of Contents
-----------------
- [Game Info](#game-info)
- [Legal notes](#legal-notes)
- [Features](#features)
- [Requirements](#requirements)
- [Installing](#installing)
- [Configuration](#configuration)
- [Usage](#usage)
- [Development](#development)
- [Credits](#credits)

Game Info
---------
![METAL GEAR & METAL GEAR 2: Solid Snake](https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/2131680/library_600x900.jpg "METAL GEAR & METAL GEAR 2: Solid Snake")

|         Type | Value                                                                   |
|-------------:|:------------------------------------------------------------------------|
| Developer(s) | Konami                                                                  |
| Publisher(s) | Konami                                                                  |
|  Director(s) | Hideo Kojima (original games)                                           |
|  Producer(s) | Noriaki Okamura (Master Collection)                                     |
|       Engine | Proprietary (games), Unity (launcher)                                   |
|  Platform(s) | Windows, PlayStation 4, PlayStation 5, Xbox Series X/S, Nintendo Switch |
|     Genre(s) | Action-adventure, stealth                                               |
|      Mode(s) | Single-player                                                           |

Legal notes
-----------

- The project contains no original assets or code from the game.
- To use this project you need to have an original copy of the game (bought from [Steam](https://store.steampowered.com/app/2131680/METAL_GEAR__METAL_GEAR_2_Solid_Snake/)), the project doesn't make piracy easier and doesn't break any of the DRM included in-game.
- The project is not affiliated with or endorsed by Konami.

Features
--------

- Opens the launcher's manuals of both games from another site, by default the Polish manuals on metalgear.grzyb.app
- Replaces the English manual with the manual in the configured language; manuals in the launcher's other languages keep their language and only move to the other site
- Site and language set in MelonLoader's settings file
- Leaves the game's files unchanged: the launcher's code is patched in memory when it starts

Requirements
------------

- [MelonLoader](https://github.com/LavaGang/MelonLoader) 0.7 or later, installed for `launcher.exe`

Installing
----------

1. Open the game folder (in Steam: right-click the game, then Manage, Browse local files)
2. Install MelonLoader with its installer, selecting `launcher.exe` from that folder, or by extracting `MelonLoader.x64.zip` there
3. Rename MelonLoader's `version.dll` in the game folder to `winhttp.dll`: the launcher gets Windows's own `version.dll` before it reaches MelonLoader's, so under that name MelonLoader does not start (the game itself does not load `winhttp.dll`)
4. Copy `MetalGearRedirector.dll`, either from the latest build in [GitHub Releases](https://github.com/GrzybDev/MetalGearRedirector/releases) (the DLL is code-signed) or from `src\bin\Release\net472` if you compiled it yourself, to the `Mods` folder in the game folder, creating it if it does not exist yet

To uninstall, delete `Mods\MetalGearRedirector.dll`; to remove MelonLoader as well, delete `winhttp.dll` and the `MelonLoader`, `Mods`, `Plugins`, `UserData` and `UserLibs` folders.

Configuration
-------------

MelonLoader keeps the mod's settings in `UserData\MelonPreferences.cfg` in the game folder, in the `[MetalGearRedirector]` section, which it writes with the defaults once the launcher has run with the mod. Edit the file while the launcher is closed; MelonLoader reads it when the launcher starts.

| Key        | Description                                                         | Allowed values                     | Default               |
|:----------:|:--------------------------------------------------------------------|:----------------------------------:|:---------------------:|
| `Host`     | The site that serves the manuals in place of `metalgear.konami.net` | A host name                        | `metalgear.grzyb.app` |
| `Language` | The language of the manual opened in place of the English one      | A language code the site serves    | `pl`                  |

```toml
[MetalGearRedirector]
Host = "metalgear.grzyb.app"
Language = "pl"
```

The site has to serve the manuals at the paths the original site uses, such as `/manual/mc1/mg2/pc/pl/index.html` for the METAL GEAR 2 manual in Polish.

Usage
-----

1. Install the mod as described in [Installing](#installing)
2. Start the game from Steam as usual; the launcher opens
3. Open the manual from the launcher

The launcher builds the link to the manual of the selected game in its own language, such as `https://metalgear.konami.net/manual/mc1/mg1_msx/pc/en/index.html`. The mod changes that link before the launcher opens it in the browser, here to `https://metalgear.grzyb.app/manual/mc1/mg1_msx/pc/pl/index.html`. The mod finds the launcher's method that builds the link by its name, so a launcher update that renames it stops the redirect until the mod is updated.

Development
-----------

The mod builds without the game: MelonLoader and Harmony come from NuGet, and the patch names the launcher's method instead of referencing it. It needs the .NET SDK that `global.json` names, and Windows for the tests, which run on .NET Framework.

```sh
dotnet build
dotnet test
dotnet format --verify-no-changes
```

The mod lands in `src\bin\<configuration>\net472\MetalGearRedirector.dll`. Set `ModsFolder` (an environment variable or `-p:ModsFolder=path\to\Mods`) to copy each build to the game's `Mods` folder. A build can change the defaults of the settings with `-p:ManualHost=...` and `-p:ManualLanguage=...`.

Set `METALGEAR_GAME_FOLDER` to the game folder to also check, against your copy of the launcher, that it still builds the links the mod expects; without it, that test is skipped.

Credits
-------
- [GrzybDev](https://grzyb.dev)

Thanks to:
- [MelonLoader](https://github.com/LavaGang/MelonLoader) and [HarmonyX](https://github.com/BepInEx/HarmonyX), which load the mod and patch the launcher

Special thanks to:
- Konami (for making the game)
