#!/usr/bin/env python3
import sys
import re
from pathlib import Path

if len(sys.argv) < 2:
    print("Usage: limit_heading_jumps.py <markdown-file>")
    sys.exit(2)

p = Path(sys.argv[1])
if not p.exists():
    print(f"File not found: {p}")
    sys.exit(2)

lines = p.read_text(encoding='utf-8').splitlines()
header_re = re.compile(r'^(#{1,6})\s*(.*)$')

out = []
first_h1_seen = False
prev_level = 1  # treat as top-level H1 already seen

for i, line in enumerate(lines):
    m = header_re.match(line)
    if not m:
        out.append(line)
        continue
    hashes, rest = m.group(1), m.group(2)
    level = len(hashes)
    # preserve first H1 at the very top as document title
    if not first_h1_seen and level == 1:
        first_h1_seen = True
        prev_level = 1
        out.append(line)
        continue
    # If level jumps up by more than one, reduce it
    if level > prev_level + 1:
        new_level = prev_level + 1
        if new_level > 6:
            new_level = 6
        out.append('#' * new_level + ' ' + rest)
        prev_level = new_level
    else:
        # allowed jump (down or up by 1 or equal)
        out.append(line)
        prev_level = level

p.write_text('\n'.join(out), encoding='utf-8')
print(f'Updated file written: {p}')
print('Done')
