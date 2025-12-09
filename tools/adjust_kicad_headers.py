#!/usr/bin/env python3
import sys
import re
from pathlib import Path

if len(sys.argv) < 2:
    print("Usage: adjust_kicad_headers.py <markdown-file>")
    sys.exit(2)

p = Path(sys.argv[1])
if not p.exists():
    print(f"File not found: {p}")
    sys.exit(2)

text = p.read_text(encoding='utf-8')
lines = text.splitlines()

# Keep the first H1 (# ) as-is (document title). For other headings that start with a numeric
# prefix like '1.', '9.3', '10.1.2' adjust their markdown level to depth+1 (so 1->##, 1.1->###, ...)

num_header_re = re.compile(r'^(#{1,6})\s+(\d+(?:\.\d+)*\.)\s*(.*)$')
new_lines = []
seen_first_h1 = False
for i, line in enumerate(lines):
    # detect the very first H1 (document title) and mark it
    if not seen_first_h1 and line.startswith('# '):
        seen_first_h1 = True
        new_lines.append(line)
        continue

    m = num_header_re.match(line)
    if m:
        # numeric header found
        num = m.group(2)  # includes trailing dot
        rest = m.group(3)
        parts = num.rstrip('.').split('.')
        depth = len(parts)  # number of segments
        desired_level = depth + 1  # as requested: 1 -> H2, 1.1 -> H3, etc.
        if desired_level < 2:
            desired_level = 2
        if desired_level > 6:
            desired_level = 6
        new_header = ('#' * desired_level) + ' ' + num
        if rest:
            new_header += ' ' + rest
        new_lines.append(new_header)
    else:
        new_lines.append(line)

# Write to a temporary file then overwrite original
bak = p.with_suffix(p.suffix + '.bak')
if not bak.exists():
    p.write_text('\n'.join(new_lines), encoding='utf-8')
    print(f"Updated file written: {p}")
else:
    # if bak exists (we created a repo backup earlier), write anyway but keep message
    p.write_text('\n'.join(new_lines), encoding='utf-8')
    print(f"Updated file written: {p} (backup already existed: {bak})")

print('Done')
