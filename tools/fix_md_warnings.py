#!/usr/bin/env python3
import re
import sys
from pathlib import Path

if len(sys.argv)<2:
    print('Usage: fix_md_warnings.py <markdown-file>')
    sys.exit(2)

p=Path(sys.argv[1])
if not p.exists():
    print('File not found',p)
    sys.exit(2)

text=p.read_text(encoding='utf-8')
lines=text.splitlines()

# backup
bak=p.with_suffix(p.suffix+'.lintbak')
if not bak.exists():
    bak.write_text(text, encoding='utf-8')

changed=False

# 1) Convert full-line emphasis/bold used as headings -> make H3
emph_re=re.compile(r'^\s*(\*\*|__)(.+?)\1\s*$')
for i,l in enumerate(lines):
    m=emph_re.match(l)
    if m:
        new='### '+m.group(2).strip()
        lines[i]=new
        changed=True

# 2) Normalize headings spacing: ensure blank line before and after headings
# we'll build new_lines iteratively to insert blanks as needed
new_lines=[]
i=0
while i<len(lines):
    l=lines[i]
    if re.match(r'^#{1,6}\s', l):
        # ensure previous line blank unless start of file
        if len(new_lines)>0 and new_lines[-1].strip()!='':
            new_lines.append('')
            changed=True
        new_lines.append(l)
        # ensure next line blank: look ahead
        nxt = lines[i+1] if i+1<len(lines) else ''
        if nxt.strip()!='':
            new_lines.append('')
            changed=True
        i+=1
        # skip original next because we've added blank; continue
    else:
        new_lines.append(l)
        i+=1
lines=new_lines

# 3) Ensure lists surrounded by blank lines (MD032)
new_lines=[]
in_list=False
for i,l in enumerate(lines):
    if re.match(r'^\s*([-+*]|\d+\.)\s+', l):
        if not in_list:
            # start of list: ensure previous line blank
            if len(new_lines)>0 and new_lines[-1].strip()!='':
                new_lines.append('')
                changed=True
            in_list=True
        new_lines.append(l)
    else:
        if in_list:
            # end of list: ensure a blank line after
            if l.strip()!='':
                new_lines.append('')
                changed=True
        in_list=False
        new_lines.append(l)
lines=new_lines

# 4) Detect duplicate headings (MD024) and disambiguate subsequent ones by appending (suite)
seen={}
for i,l in enumerate(lines):
    m=re.match(r'^(#{1,6})\s*(.*)$', l)
    if m:
        text=m.group(2).strip()
        key=re.sub(r'\s+',' ', text).lower()
        if key in seen:
            seen[key]+=1
            # append marker
            suffix=f" (suite {seen[key]})"
            lines[i]=m.group(1)+' '+text+suffix
            changed=True
        else:
            seen[key]=1

if changed:
    p.write_text('\n'.join(lines), encoding='utf-8')
    print('File updated:', p)
else:
    print('No changes needed')

print('Backup at', bak)
