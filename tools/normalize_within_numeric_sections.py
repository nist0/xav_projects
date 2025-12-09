#!/usr/bin/env python3
import sys
import re
from pathlib import Path

if len(sys.argv) < 2:
    print("Usage: normalize_within_numeric_sections.py <markdown-file>")
    sys.exit(2)

p = Path(sys.argv[1])
if not p.exists():
    print(f"File not found: {p}")
    sys.exit(2)

text = p.read_text(encoding='utf-8')
lines = text.splitlines()

num_re = re.compile(r'^(#{1,6})\s+(\d+(?:\.\d+)*)\.\s*(.*)$')
header_re = re.compile(r'^(#{1,6})\s*(.*)$')

# find numeric section headers
numeric_headers = []  # list of (index, level)
for i, line in enumerate(lines):
    m = num_re.match(line)
    if m:
        level = len(m.group(1))
        numeric_headers.append((i, level))

if not numeric_headers:
    print("No numeric headers found. Nothing to do.")
    sys.exit(0)

# process each numeric section range
for idx, (start_idx, start_level) in enumerate(numeric_headers):
    end_idx = numeric_headers[idx+1][0] if idx+1 < len(numeric_headers) else len(lines)
    # for lines between start_idx+1 and end_idx-1, increase heading level by 1
    for j in range(start_idx+1, end_idx):
        line = lines[j]
        mh = header_re.match(line)
        if mh:
            hashes = mh.group(1)
            body = mh.group(2)
            # if this header line is itself a numeric header (rare), skip
            if num_re.match(line):
                continue
            current_level = len(hashes)
            new_level = current_level + 1
            if new_level > 6:
                new_level = 6
            lines[j] = ('#' * new_level) + ' ' + body

bak = p.with_suffix(p.suffix + '.bak2')
# save backup only if not exists
if not bak.exists():
    p.write_text('\n'.join(lines), encoding='utf-8')
    print(f"Updated file written: {p}")
else:
    p.write_text('\n'.join(lines), encoding='utf-8')
    print(f"Updated file written: {p} (bak2 already existed: {bak})")

print('Done')
