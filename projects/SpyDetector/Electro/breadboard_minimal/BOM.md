# Bill of Materials — Minimal

Core
- `1x ESP32 Dev Kit (WROOM)` — any generic ESP32 dev board with USB serial and 3.3V regulator
- `1x AM312 PIR sensor` — small 3.3V-compatible PIR module (preferred) OR `HC-SR501` (note: often 5V)

Outputs
- `1x LED (any color)` + `1x 220Ω resistor`
- `1x Active buzzer (3.3V)` OR passive buzzer + transistor (e.g. 2N2222) + diode

Breadboard & wiring
- `1x small solderless breadboard`
- `Jumper wires (male-to-female and male-to-male)`
- `USB A to micro/USB-C cable` (depending on board)

Optional
- `1x pushbutton` for mute/test
- `1x MOSFET or transistor` if driving a larger buzzer or relay

Notes
- AM312 is recommended because it runs at 3.3V and its digital output matches the ESP32 logic level.
- If using HC-SR501, power it at 3.3V if possible, or use a level shifter/voltage divider on the PIR output.
- Use an active buzzer rated for 3.3V when possible to simplify wiring.
