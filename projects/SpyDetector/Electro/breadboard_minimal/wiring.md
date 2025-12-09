# Wiring and Quick Setup

Power
- Connect the `ESP32 Dev Kit` to your PC via USB. The board provides 3.3V power for the MCU.
- Power the `AM312` PIR module from the `3.3V` pin and `GND` on the dev board.

Pin mapping (recommended defaults)
- `PIR OUT` -> `GPIO15` (changeable; set `PIR_PIN` in sketch)
- `LED` -> `GPIO2` (on many boards this is `LED_BUILTIN`)
- `BUZZER` -> `GPIO12`

Example wiring steps
1. Place the ESP32 Dev Kit on the breadboard so pins straddle the center gap.
2. Insert the AM312 module onto the breadboard.
3. Connect AM312 `VCC` to `3.3V` on ESP32, `GND` to `GND`, and `OUT` to `GPIO15`.
4. Connect LED anode (long leg) to `GPIO2` through `220Ω` resistor; LED cathode to `GND`.
5. Connect an active buzzer `+` to `GPIO12` and `-` to `GND`. If the buzzer requires more current, drive it via an NPN transistor (base via 1k resistor) and add a flyback diode if needed.

Software setup (Arduino IDE)
1. In Arduino IDE, add the ESP32 boards URL: `https://raw.githubusercontent.com/espressif/arduino-esp32/gh-pages/package_esp32_index.json` (Preferences -> Additional Boards Manager URLs).
2. Open Boards Manager and install `esp32` by Espressif.
3. Select your `ESP32 Dev Module` board and the appropriate COM port.

Testing tips
- Before uploading motion sketch, open Serial Monitor at `115200` to see boot messages.
- To quickly test the PIR, upload a blink-style sketch that reads the PIR pin and prints its state.
