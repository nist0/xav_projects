#!/usr/bin/env python3
import sys,re
from pathlib import Path
if len(sys.argv)<2:
    print('usage: detect_heading_jumps.py <mdfile>')
    sys.exit(2)
p=Path(sys.argv[1])
if not p.exists():
    print('file not found',p)
    sys.exit(2)
lines=p.read_text(encoding='utf-8').splitlines()
header_re=re.compile(r'^(#{1,6})\s*(.*)$')
prev_level=None
problems=[]
for i,l in enumerate(lines, start=1):
    m=header_re.match(l)
    if m:
        level=len(m.group(1))
        if prev_level is None:
            prev_level=level
            continue
        if level>prev_level+1:
            problems.append((i, prev_level, level, l.strip()))
        prev_level=level
if not problems:
    print('No heading jump problems found')
else:
    print('Found heading jumps (line, previous_level, current_level, text):')
    for pinfo in problems:
        print(pinfo)
