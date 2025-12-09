#!/usr/bin/env python3
import sys
import re
from pathlib import Path

if len(sys.argv) < 2:
    print("Usage: normalize_set_relative_levels.py <markdown-file>")
    sys.exit(2)

p = Path(sys.argv[1])
if not p.exists():
    print(f"File not found: {p}")
    sys.exit(2)

lines = p.read_text(encoding='utf-8').splitlines()
num_re = re.compile(r'^(#{1,6})\s+(\d+(?:\.\d+)*)\.\s*(.*)$')
header_re = re.compile(r'^(#{1,6})\s*(.*)$')

# find numeric headers
numeric = []
for i,l in enumerate(lines):
    m = num_re.match(l)
    if m:
        numeric.append((i, len(m.group(1))))

if not numeric:
    print('No numeric headers, nothing to do')
    sys.exit(0)

# process
for idx,(start, S) in enumerate(numeric):
    end = numeric[idx+1][0] if idx+1 < len(numeric) else len(lines)
    for j in range(start+1, end):
        mh = header_re.match(lines[j])
        if mh:
            h = len(mh.group(1))
            body = mh.group(2)
            rel = h - S
            new_level = S + 1 + max(rel, 0)
            if new_level > 6:
                new_level = 6
            lines[j] = ('#'*new_level) + ' ' + body

p.write_text('\n'.join(lines), encoding='utf-8')
print(f'Updated: {p}')
