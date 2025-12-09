#!/usr/bin/env python3
import sys
import re
from pathlib import Path

if len(sys.argv)<2:
    print('Usage: remove_duplicate_sections.py <file>')
    sys.exit(2)

p=Path(sys.argv[1])
if not p.exists():
    print('file not found',p)
    sys.exit(2)

text=p.read_text(encoding='utf-8')
lines=text.splitlines()
header_re=re.compile(r'^(#{1,6})\s*(.*)$')

# collect sections: list of (start_idx, level, title)
sections=[]
for i,l in enumerate(lines):
    m=header_re.match(l)
    if m:
        level=len(m.group(1))
        title=m.group(2).strip()
        sections.append((i,level,title))

# append end sentinel
sections_with_end=[]
for idx,(start,level,title) in enumerate(sections):
    end=sections[idx+1][0] if idx+1<len(sections) else len(lines)
    content='\n'.join(lines[start:end]).strip()
    norm_title=re.sub(r'\s+',' ',title).lower()
    sections_with_end.append({'start':start,'end':end,'level':level,'title':title,'norm':norm_title,'content':content})

# map norm_title -> list of sections
from collections import defaultdict
map_title=defaultdict(list)
for s in sections_with_end:
    map_title[s['norm']].append(s)

# find duplicates where content identical
to_remove=[]
for title,items in map_title.items():
    if len(items)>1:
        # compare content; keep first, remove later identicals
        base=items[0]['content']
        for other in items[1:]:
            if other['content']==base:
                to_remove.append((other['start'],other['end'],other['title']))

if not to_remove:
    print('No exact duplicate sections found')
    sys.exit(0)

# remove ranges in reverse order
to_remove_sorted=sorted(to_remove, key=lambda x:x[0], reverse=True)
for start,end,title in to_remove_sorted:
    print(f'Removing duplicate section: {title} (lines {start+1}-{end})')
    del lines[start:end]

p.write_text('\n'.join(lines), encoding='utf-8')
print('Updated file written:', p)
