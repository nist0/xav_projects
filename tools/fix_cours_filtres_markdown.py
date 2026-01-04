#!/usr/bin/env python3
"""Fix common MarkdownLint issues in cours_filtres/*.md.

Scope is intentionally narrow and conservative:
- Ensures blank line before/after headings (MD022) outside fenced blocks.
- Ensures blank line before/after lists (MD032) outside fenced blocks.
- Collapses 3+ consecutive blank lines down to 1 blank line (MD060-ish) outside fenced blocks.
- Removes UTF-8 BOM if present.
- Replaces tab characters with two spaces everywhere (MD010).

It avoids rewriting inside fenced blocks except for tab replacement.
"""

from __future__ import annotations

from dataclasses import dataclass
from pathlib import Path
import re
import sys


FENCE_RE = re.compile(r"^(\s*)(```|~~~)")
HEADING_RE = re.compile(r"^#{1,6}\s")
LIST_RE = re.compile(r"^\s*(?:[-+*]|\d+\.)\s+")


@dataclass(frozen=True)
class FixResult:
    path: Path
    changed: bool
    notes: list[str]


def split_lines_keepends(text: str) -> list[str]:
    # Normalize to \n for processing; we'll write back with \n.
    return text.replace("\r\n", "\n").replace("\r", "\n").split("\n")


def fix_markdown(text: str) -> tuple[str, list[str]]:
    notes: list[str] = []

    # Remove BOM if present.
    if text.startswith("\ufeff"):
        text = text.lstrip("\ufeff")
        notes.append("Removed UTF-8 BOM")

    # Tabs -> two spaces everywhere (including fences).
    if "\t" in text:
        text = text.replace("\t", "  ")
        notes.append("Replaced TAB with two spaces")

    lines = split_lines_keepends(text)

    out: list[str] = []
    in_fence = False
    fence_token: str | None = None

    def ensure_blank_before() -> None:
        if out and out[-1].strip() != "":
            out.append("")

    def ensure_blank_after(next_line: str | None) -> None:
        if next_line is not None and next_line.strip() != "":
            out.append("")

    i = 0
    while i < len(lines):
        line = lines[i]

        m_fence = FENCE_RE.match(line)
        if m_fence:
            token = m_fence.group(2)
            if not in_fence:
                in_fence = True
                fence_token = token
            else:
                # Close only if same fence token.
                if fence_token == token:
                    in_fence = False
                    fence_token = None
            out.append(line)
            i += 1
            continue

        if in_fence:
            out.append(line)
            i += 1
            continue

        # Headings spacing (MD022)
        if HEADING_RE.match(line):
            ensure_blank_before()
            out.append(line)
            next_line = lines[i + 1] if i + 1 < len(lines) else None
            ensure_blank_after(next_line)
            i += 1
            continue

        # Lists spacing (MD032)
        if LIST_RE.match(line):
            # Ensure blank line before list block.
            ensure_blank_before()

            # Copy the whole list block as-is.
            while i < len(lines) and LIST_RE.match(lines[i]):
                out.append(lines[i])
                i += 1

            # Ensure blank line after list block if next non-list line is not blank.
            next_line = lines[i] if i < len(lines) else None
            ensure_blank_after(next_line)
            continue

        out.append(line)
        i += 1

    # Collapse 3+ consecutive blank lines to a single blank line.
    collapsed: list[str] = []
    blank_run = 0
    for line in out:
        if line.strip() == "":
            blank_run += 1
            if blank_run <= 2:
                collapsed.append("")
        else:
            blank_run = 0
            collapsed.append(line)

    if collapsed != out:
        notes.append("Collapsed excessive blank lines")

    # Strip trailing spaces on blank lines.
    final_lines = ["" if l.strip() == "" else l for l in collapsed]

    return "\n".join(final_lines).rstrip("\n") + "\n", notes


def process_file(path: Path) -> FixResult:
    original = path.read_text(encoding="utf-8")
    fixed, notes = fix_markdown(original)

    changed = fixed != original
    if changed:
        path.write_text(fixed, encoding="utf-8", newline="\n")

    return FixResult(path=path, changed=changed, notes=notes)


def main(argv: list[str]) -> int:
    if len(argv) != 2:
        print("Usage: fix_cours_filtres_markdown.py <cours_filtres_dir>")
        return 2

    root = Path(argv[1])
    if not root.exists() or not root.is_dir():
        print(f"Directory not found: {root}")
        return 2

    md_files = sorted(root.glob("*.md"))
    if not md_files:
        print(f"No *.md files found in: {root}")
        return 2

    changed = 0
    for p in md_files:
        res = process_file(p)
        if res.changed:
            changed += 1
            print(f"UPDATED: {p.name} ({', '.join(res.notes) if res.notes else 'formatting'})")

    print(f"DONE: {changed}/{len(md_files)} files updated")
    return 0


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
