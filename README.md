# Batman's Night Patrol in Gotham 🦇

This project is a simulator for Batman's night patrol in Gotham, developed using Unity. In this simulator, you can control Batman and the Batmobile's movement, switch between various Batman states (Normal, Stealth, Alert), and use special features like the Bat-Signal and Alert state.

## Features of the Project

- **Batman and Batmobile Movement**: Normal movement and speed boost (by holding the Shift key).
- **Batman States**:
  - **Normal**: Regular movement speed.
  - **Stealth**: Reduced speed and dimmed lights.
  - **Alert**: Special lights and alarm sound activated.
- **Bat-Signal**: A special light that can be toggled on/off with the B key and slowly rotates in the sky.
- **Alert State Lights and Sound**: Flashing lights (red and blue) and alarm sound when in Alert state.
- **Gotham City Model**: A simple model of Gotham City for Batman and Batmobile to patrol.

## Control Explanation

- **Batman and Batmobile Movement**:
  - Use W, S, A, D or arrow keys to move and turn.
  - Hold Shift for speed boost.
  
- **Switching States**:
  - Press **N** to switch to **Normal** state.
  - Press **C** to switch to **Stealth** state.
  - Press **Space** to switch to **Alert** state.
  
- **Bat-Signal**:
  - Press **B** to toggle the Bat-Signal on/off.

- **Alert State**:
  - In Alert mode, flashing red and blue lights are activated and the alarm sound plays.

## Summary of Work Done

- Implemented movement for Batman and Batmobile using keyboard inputs.
- Created and implemented three distinct Batman states (Normal, Stealth, Alert) using an enum and if/else statements.
- Added the Bat-Signal light which can be toggled on/off using the B key and rotates slowly in the sky.
- Implemented flashing lights and alarm sound in the Alert state.
