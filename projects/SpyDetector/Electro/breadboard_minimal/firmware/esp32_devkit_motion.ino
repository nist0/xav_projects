// esp32_devkit_motion.ino
// Minimal motion detector for ESP32 Dev Kit + AM312 PIR

// Pin configuration - change to match your board/wiring
const int PIR_PIN = 15;      // PIR sensor digital output
const int LED_PIN = 2;       // On-board LED or external LED
const int BUZZER_PIN = 12;   // Active buzzer + on 3.3V

unsigned long lastTrigger = 0;
const unsigned long triggerCooldown = 5000; // milliseconds between triggers

void setup() {
  Serial.begin(115200);
  pinMode(PIR_PIN, INPUT);
  pinMode(LED_PIN, OUTPUT);
  pinMode(BUZZER_PIN, OUTPUT);
  digitalWrite(LED_PIN, LOW);
  digitalWrite(BUZZER_PIN, LOW);
  Serial.println("ESP32 Motion detector starting");
}

void loop() {
  int pir = digitalRead(PIR_PIN);
  if (pir == HIGH) {
    unsigned long now = millis();
    if (now - lastTrigger > triggerCooldown) {
      Serial.println("Motion detected!");
      // Visual feedback
      digitalWrite(LED_PIN, HIGH);
      // Sound buzzer for 300ms
      digitalWrite(BUZZER_PIN, HIGH);
      delay(300);
      digitalWrite(BUZZER_PIN, LOW);
      // Keep LED on for 2s
      delay(2000);
      digitalWrite(LED_PIN, LOW);
      lastTrigger = now;
    }
  }
  delay(50);
}
