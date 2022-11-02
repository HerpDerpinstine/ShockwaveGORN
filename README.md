# ShockwaveGORN
Mod for GORN using MelonLoader  

- Allows you to use your Shockwave Suit to feel feedback when playing GORN.
- Mod Settings are found in ``UserData\Shockwave``
- Discord: https://discord.gg/JDw423Wskf

| Features |
| - |
| Wobble |
| Surprise |
| HeartBeat |
| Reactive Player Damage |
| Velocity Based Haptic Scaling |
| Customizable Feedback Configs | 
| Weapon Feedback for all Variations of Damage and Usage |

---

### REQUIREMENTS:

- [MelonLoader](https://github.com/LavaGang/MelonLoader/releases) v0.5.7 or higher.

---

### INSTALLATION:

1) Install [MelonLoader](https://github.com/LavaGang/MelonLoader/releases) v0.5.7 or higher.
2) Download [ShockwaveGORN](https://github.com/HerpDerpinstine/ShockwaveGORN/releases) from Releases.
3) Place ``ShockwaveGORN.dll`` in the ``Mods`` folder of your Game's Install Folder.
4) Place ``ShockWaveIMU.dll`` in your Game's Install Folder.
4) Start the Game.

---

### VELOCITY SCALING:

- The Algorithm for Velocity Scaling works as follows:  
``PatternIntensity * ( ClampMinMax( ( CurrentVelocity.magnitude * VelocityScale ), VelocityScaleMin, VelocityScaleMax  ) * IntensityScale )``

- For example if:  
1) ``PatternIntensity`` equals ``100``  
2) ``CurrentVelocity.magnitude`` equals ``256``  
3) ``VelocityScale`` equals ``0.001``  
4) ``VelocityScaleMin`` equals ``0``  
5) ``VelocityScaleMax`` equals ``2``  
6) ``IntensityScale`` equals ``1.2`` 

- The  Algorithm would read as:  
``100 * ( ClampMinMax( ( 256 * 0.001 ), 0, 2  ) * 1.2 )``

1) ``256`` multiplied by ``0.001`` equals ``0.256``
2) ``0.256`` clamped between ``0`` to ``2`` equals ``0.256``
3) ``0.256`` multiplied by ``1.2`` equals ``0.3072``
4) ``100`` multiplied by ``0.3072`` equals ``30.72``

- Which means in this example the Haptic Pattern would play at an Intensity of ``30``.


---

### LICENSING & CREDITS:

ShockwaveGORN is licensed under the Apache License, Version 2.0. See [LICENSE](https://github.com/HerpDerpinstine/ShockwaveGORN/blob/master/LICENSE.md) for the full License.