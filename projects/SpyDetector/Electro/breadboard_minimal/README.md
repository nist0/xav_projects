# SpyDetector — Minimal Breadboard Version (ESP32 Dev Kit)

This folder contains a minimal breadboard-friendly detector design using a generic `ESP32 Dev Kit` (WROOM) and a low-power PIR motion sensor. The goal is to provide a quick, testable prototype: BOM, wiring instructions and an example Arduino sketch.

Overview
- MCU: `ESP32 Dev Kit` (WROOM) — common Arduino-compatible dev boards.
- Sensor: small PIR motion sensor (AM312 recommended for 3.3V logic compatibility).
- Outputs: LED (visual) and active buzzer (audible).

Files added
- `BOM.md` — minimal parts list and substitution notes
- `wiring.md` — step-by-step breadboard wiring and power notes
- `firmware/esp32_devkit_motion.ino` — Arduino-style sketch for quick testing

Before you begin
- Install ESP32 support in Arduino IDE or PlatformIO. See `wiring.md` for a short install note.
- Verify your dev board pins and on-board LED pin mapping (some boards use `LED_BUILTIN` on GPIO2).

Next steps
- Test the PIR sensor output with a simple blink sketch to confirm wiring.
- Upload `esp32_devkit_motion.ino` and adjust pin defines if needed.
