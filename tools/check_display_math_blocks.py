#!/usr/bin/env python3
"""Scan Markdown files for common KaTeX/MathJax display-math pitfalls.

Focus: $$ ... $$ blocks.
Flags:
- Unbalanced $$ delimiters (odd count).
- Blank line inside the math block (often breaks render in some viewers).
- Unbalanced braces { } inside the block (simple heuristic).
- Suspicious split fraction: \frac{...}{ on one line and denominator starts later.

This is a heuristic checker, not a full LaTeX parser.

Exit codes:
- 0: no issues
- 1: issues found
- 2: usage / IO error
"""

from __future__ import annotations

from dataclasses import dataclass
from pathlib import Path
import re
import sys


@dataclass
class Issue:
    path: Path
    kind: str
    detail: str


DOLLAR_RE = re.compile(r"(?m)^\$\$\s*$")


def find_display_blocks(text: str) -> list[tuple[int, int]]:
    """Return list of (start_idx, end_idx) ranges for $$ blocks (content only)."""
    matches = list(DOLLAR_RE.finditer(text))
    if len(matches) % 2 != 0:
        return []
    blocks: list[tuple[int, int]] = []
    for i in range(0, len(matches), 2):
        start = matches[i].end()
        end = matches[i + 1].start()
        blocks.append((start, end))
    return blocks


def brace_balance(s: str) -> int:
    bal = 0
    esc = False
    for ch in s:
        if esc:
            esc = False
            continue
        if ch == "\\":
            esc = True
            continue
        if ch == "{":
            bal += 1
        elif ch == "}":
            bal -= 1
    return bal


def scan_file(path: Path) -> list[Issue]:
    issues: list[Issue] = []
    text = path.read_text(encoding="utf-8")

    dollar_lines = list(DOLLAR_RE.finditer(text))
    if len(dollar_lines) % 2 != 0:
        issues.append(Issue(path, "UNBALANCED_DOLLAR_FENCE", f"count={len(dollar_lines)}"))
        return issues

    for start, end in find_display_blocks(text):
        block = text[start:end]
        if "\n\n" in block:
            issues.append(Issue(path, "BLANK_LINE_IN_DISPLAY_MATH", "contains empty line"))

        bal = brace_balance(block)
        if bal != 0:
            issues.append(Issue(path, "UNBALANCED_BRACES_IN_DISPLAY_MATH", f"balance={bal}"))

        # Fraction split heuristic
        if re.search(r"(?m)\\frac\{[^\n}]*\}\{\s*$", block):
            issues.append(Issue(path, "SPLIT_FRAC", "\\frac{...}{ split across lines"))

    return issues


def main(argv: list[str]) -> int:
    if len(argv) != 2:
        print("Usage: check_display_math_blocks.py <dir-or-file>")
        return 2

    target = Path(argv[1])
    if not target.exists():
        print(f"Not found: {target}")
        return 2

    files: list[Path]
    if target.is_dir():
        files = sorted(target.rglob("*.md"))
    else:
        files = [target]

    all_issues: list[Issue] = []
    for f in files:
        all_issues.extend(scan_file(f))

    if not all_issues:
        print("OK: no display-math issues found")
        return 0

    for iss in all_issues:
        rel = iss.path.as_posix()
        print(f"{iss.kind}: {rel} ({iss.detail})")

    print(f"FOUND: {len(all_issues)} issues")
    return 1


if __name__ == "__main__":
    raise SystemExit(main(sys.argv))
