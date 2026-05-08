# LoopBound

A 2D top-down puzzle game built in Unity 6 using C#.

You are trapped in a repeating time loop. Each run gives you two minutes. Press Space to record your path and commit it as a ghost; the ghost replays that route and holds its final position on a pressure plate. Your job is to stand on the other plate yourself. Both plates active at once unlocks the exit door.

The twist: pressing E cycles through three emotional states, Neutral, Joy, and Sad, each with a different speed multiplier. Because the ghost replays position data from the original recording, the emotion you chose during recording changes how the ghost moves. Fast recordings produce fast ghosts; slow recordings produce precise ones. Emotion becomes a puzzle variable rather than a cosmetic detail.

---

## Requirements

- Unity 6 (6000.x LTS) — [Download](https://unity.com/releases/editor/archive)
- Universal Render Pipeline (included via Package Manager)
- Unity Input System package (included)

---

## Setup

1. Clone or download the repo
2. Open **Unity Hub** → **Add project from disk** → select the repo folder
3. Open the project in Unity 6
4. In the Project window: `Assets/Scenes/` → double-click **SampleScene**
5. Press **Play**

All dependencies are stored in the project. No additional packages need installing.

---

## Controls

| Key | Action |
|-----|--------|
| W / Up Arrow | Move up |
| S / Down Arrow | Move down |
| A / Left Arrow | Move left |
| D / Right Arrow | Move right |
| E | Cycle emotion (Neutral > Joy > Sad) |
| Space | Start or commit a ghost path recording |

---

## How to Play

1. Press **Space** to begin tracing a path
2. Move to where you want the ghost to end up, then press **Space** again to commit
3. The ghost spawns, replays your path, and freezes on its final position
4. Stand on the second pressure plate while the ghost holds the first
5. Both plates active simultaneously unlocks the door
6. Walk through the door before the timer hits zero

If time runs out, everything resets. There is no permanent game-over state.