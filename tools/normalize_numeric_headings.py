#!/usr/bin/env python3
"""Normalize numeric section titles into Markdown headings and disambiguate duplicates.

Rules:
- Lines starting with a numeric section like "1.", "1.1" become headings: depth+1 hashes (1 -> H2).
- Keep the first H1 (# ) as the document title; convert other H1 to H2.
- Ensure duplicate heading texts are made unique by appending " (suite N)" to subsequent occurrences.
- Preserve existing hashes when already present, but adjust levels for numeric sections.
"""
import re
import sys
from pathlib import Path


def backup_path(p: Path) -> Path:
    return p.with_suffix(p.suffix + '.headbak')


def is_hash_heading(line):
    return bool(re.match(r'^(#{1,6})\s+', line))


def numeric_prefix_heading(line):
    m = re.match(r'^\s*(\d+(?:\.\d+)*)[)\.:\-\s]+(.+)$', line)
    if m:
        prefix = m.group(1)
        rest = m.group(2).strip()
        depth = prefix.count('.') + 1
        return depth, rest
    return None


def normalize(path: Path):
    text = path.read_text(encoding='utf-8')
    lines = text.splitlines()
    path_backup = backup_path(path)
    path_backup.write_text(text, encoding='utf-8')

    out = []
    seen_h1 = False
    heading_counts = {}

    for ln in lines:
        # convert numeric-prefixed headings to hashes
        np = numeric_prefix_heading(ln)
        if np:
            depth, title = np
            level = min(depth + 1, 6)
            newln = '#' * level + ' ' + title
            ln = newln
        # normalize H1s: keep only the first as H1
        if is_hash_heading(ln):
            m = re.match(r'^(#{1,6})\s+(.*)$', ln)
            hashes = m.group(1)
            title = m.group(2).strip()
            level = len(hashes)
            if level == 1:
                if not seen_h1:
                    seen_h1 = True
                else:
                    # demote to H2
                    level = 2
                    ln = '#' * level + ' ' + title
            # track duplicates
            key = title.lower()
            cnt = heading_counts.get(key, 0) + 1
            heading_counts[key] = cnt
            if cnt > 1:
                # append suite number to disambiguate
                ln = ('#' * level) + ' ' + title + f' (suite {cnt})'
        out.append(ln)

    new_text = '\n'.join(out) + '\n'
    path.write_text(new_text, encoding='utf-8')
    print(f"Normalized headings written: {path}\nBackup saved: {path_backup}")


def main():
    if len(sys.argv) < 2:
        print('Usage: normalize_numeric_headings.py file.md')
        sys.exit(2)
    p = Path(sys.argv[1])
    if not p.exists():
        print('File not found:', p)
        sys.exit(1)
    normalize(p)


if __name__ == '__main__':
    main()
