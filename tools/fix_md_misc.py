#!/usr/bin/env python3
"""Small fixer for common markdownlint issues in long converted docs.

Fixes applied:
- remove trailing single spaces (MD009)
- collapse multiple blank lines to a single blank line (MD012)
- ensure blank lines before/after list blocks (MD032)
- normalize table pipe spacing for table rows/headers (MD060)
- convert full-line emphasis to `##` headings (MD036) when the whole line is emphasized

Use: python tools/fix_md_misc.py path/to/file.md
"""
import re
import sys
from pathlib import Path


def backup_path(p: Path) -> Path:
    return p.with_suffix(p.suffix + '.fixbak')


def remove_trailing_spaces(lines):
    # Remove all trailing whitespace (spaces/tabs). We prefer no trailing spaces
    # (markdownlint accepts 0 or 2 for line break; this script chooses 0)
    return [ln.rstrip() for ln in lines]


def collapse_blank_lines(lines):
    out = []
    prev_blank = False
    for ln in lines:
        if ln.strip() == '':
            if not prev_blank:
                out.append('')
            prev_blank = True
        else:
            out.append(ln)
            prev_blank = False
    return out


def ensure_blank_around_lists(lines):
    i = 0
    out = list(lines)
    list_marker_re = re.compile(r'^(>\s*)?(?:[-*+]\s+|\d+\.\s+)')
    # Find contiguous list blocks and ensure blank line before and after
    while i < len(out):
        if list_marker_re.match(out[i]):
            # start of list block
            start = i
            j = i
            while j < len(out) and (list_marker_re.match(out[j]) or out[j].strip() == ''):
                # allow blank lines inside an indented/list block
                j += 1
            end = j - 1
            # ensure blank before start
            if start > 0 and out[start - 1].strip() != '':
                out.insert(start, '')
                i += 1
                end += 1
            # ensure blank after end
            if end + 1 < len(out) and out[end + 1].strip() != '':
                out.insert(end + 1, '')
            i = end + 2
        else:
            i += 1
    return out


def normalize_table_pipes(lines):
    out = []
    i = 0
    in_code = False
    while i < len(lines):
        ln = lines[i]
        # Track code fences to avoid changing code blocks
        if ln.strip().startswith('```'):
            in_code = not in_code
            out.append(ln)
            i += 1
            continue
        if in_code:
            out.append(ln)
            i += 1
            continue

        # Heuristic: treat as table row if it contains '|' and either previous or next line contains '|'
        prev_has = i > 0 and '|' in lines[i-1]
        next_has = i+1 < len(lines) and '|' in lines[i+1]
        if '|' in ln and (prev_has or next_has):
            # split on |, strip each cell, then join with ' | '
            cells = [c.strip() for c in ln.split('|')]
            out.append(' | '.join(cells))
        else:
            out.append(ln)
        i += 1
    return out


def convert_full_line_emphasis(lines):
    out = []
    # Matches lines that are entirely emphasized: optional blockquote, then *, **, or _ ... ending marker
    emph_re = re.compile(r'^(>\s*)?([*_]{1,3})\s*(.+?)\s*\2\s*$')
    for ln in lines:
        m = emph_re.match(ln)
        if m:
            # convert to a level-2 heading
            content = m.group(3).strip()
            out.append('## ' + content)
        else:
            out.append(ln)
    return out


def ensure_blank_around_headings(lines):
    out = []
    i = 0
    while i < len(lines):
        ln = lines[i]
        if ln.lstrip().startswith('#'):
            # ensure previous is blank
            if len(out) > 0 and out[-1].strip() != '':
                out.append('')
            out.append(ln)
            # ensure next is blank
            if i + 1 < len(lines) and lines[i+1].strip() != '':
                out.append('')
                i += 1
            i += 1
        else:
            out.append(ln)
            i += 1
    return out


def limit_heading_jumps(lines):
    out = []
    prev_level = 0
    for ln in lines:
        m = re.match(r'^(#+)\s+', ln)
        if m:
            level = len(m.group(1))
            # If prev_level is 0 (no previous heading), or level <= prev_level+1 it's fine
            if prev_level == 0:
                prev_level = level
                out.append(ln)
            else:
                if level > prev_level + 1:
                    # reduce to allowed level
                    new_level = prev_level + 1
                    content = ln.lstrip('#').lstrip()
                    out.append('#' * new_level + ' ' + content)
                    prev_level = new_level
                else:
                    out.append(ln)
                    prev_level = level
        else:
            out.append(ln)
    return out


def fix_blank_between_blockquotes(lines):
    out = list(lines)
    for i in range(1, len(out)-1):
        if out[i].strip() == '' and out[i-1].lstrip().startswith('>') and out[i+1].lstrip().startswith('>'):
            # replace plain blank with a blockquote-empty line
            out[i] = '>'
    return out


def wrap_paragraphs(lines, width=80):
    import textwrap
    out = []
    buf = []
    def flush_buf():
        if not buf:
            return
        paragraph = ' '.join([l.strip() for l in buf])
        wrapped = textwrap.fill(paragraph, width=width)
        out.extend(wrapped.split('\n'))
        buf.clear()

    skip_prefixes = ('#', '-', '*', '+', '```', '|','    ')
    i = 0
    while i < len(lines):
        ln = lines[i]
        if ln.strip() == '':
            flush_buf()
            out.append(ln)
        else:
            # handle blockquote paragraphs separately
            if ln.lstrip().startswith('>'):
                flush_buf()
                # collect consecutive blockquote lines
                bq_buf = []
                while i < len(lines) and lines[i].lstrip().startswith('>'):
                    bq_buf.append(lines[i].lstrip()[1:].lstrip())
                    i += 1
                # wrap the blockquote content and re-add '> '
                paragraph = ' '.join([l.strip() for l in bq_buf if l.strip() != ''])
                wrapped = textwrap.fill(paragraph, width=width)
                for wln in wrapped.split('\n'):
                    out.append('> ' + wln)
                continue
            else:
                buf.append(ln)
        i += 1
    flush_buf()
    return out


def normalize_ul_indent(lines):
    out = []
    for ln in lines:
        # reduce 3-space indent before list markers to 2 spaces
        newln = re.sub(r'^ {3}([-*+]\s+)', r'  \1', ln)
        out.append(newln)
    return out


def strip_trailing_punctuation_in_headings(lines):
    out = []
    for ln in lines:
        if ln.lstrip().startswith('#'):
            # remove trailing spaces and trailing colon/punctuation
            newln = re.sub(r"\s*[:\-–—]+\s*$", '', ln)
            out.append(newln)
        else:
            out.append(ln)
    return out


def normalize_ordered_list_prefixes(lines):
    out = []
    for ln in lines:
        m = re.match(r'^(\s*)\d+\.(\s+)', ln)
        if m:
            out.append(m.group(1) + '1.' + m.group(2) + ln[m.end():])
        else:
            out.append(ln)
    return out


def fix_space_in_emphasis(lines):
    # Remove spaces immediately inside emphasis markers like '* text *' -> '*text*'
    out = []
    for ln in lines:
        newln = re.sub(r'([*_])\s+(.*?)\s+\1', r"\1\2\1", ln)
        out.append(newln)
    return out


def aggressive_wrap_any(lines, width=80):
    import textwrap
    out = []
    in_code = False
    for ln in lines:
        if ln.strip().startswith('```'):
            in_code = not in_code
            out.append(ln)
            continue
        if in_code or '|' in ln:
            out.append(ln)
            continue
        if len(ln) <= width:
            out.append(ln)
            continue
        # try to detect list or blockquote prefix
        m = re.match(r'^(\s*(>\s*)?([-*+]\s+|\d+\.\s+))(.+)$', ln)
        if not m:
            # detect bare blockquote prefix like '> content'
            m2 = re.match(r'^(\s*>\s*)(.+)$', ln)
            if m2:
                prefix = m2.group(1)
                text = m2.group(2).strip()
                wrapped = textwrap.fill(text, width=width, subsequent_indent=' ' * len(prefix))
                for i, wln in enumerate(wrapped.split('\n')):
                    if i == 0:
                        out.append(prefix + wln)
                    else:
                        out.append(' ' * len(prefix) + wln)
                continue
        if m:
            prefix = m.group(1)
            text = m.group(4).strip()
            wrapped = textwrap.fill(text, width=width, subsequent_indent=' ' * len(prefix))
            for i, wln in enumerate(wrapped.split('\n')):
                if i == 0:
                    out.append(prefix + wln)
                else:
                    out.append(' ' * len(prefix) + wln)
        else:
            wrapped = textwrap.fill(ln.strip(), width=width)
            out.extend(wrapped.split('\n'))
    return out


def shorten_long_headings(lines, width=80):
    out = []
    for ln in lines:
        if ln.lstrip().startswith('#') and len(ln) > width:
            m = re.match(r'^(#{1,6})\s+(.*)$', ln)
            if m:
                prefix = m.group(1)
                title = m.group(2)
                # collapse numeric list markers found inside a heading
                new_title = re.sub(r"\s+\d+\.\s+", ', ', title)
                # collapse multiple spaces
                new_title = re.sub(r'\s{2,}', ' ', new_title).strip()
                ln = f"{prefix} {new_title}"
        out.append(ln)
    return out


def remove_stray_asterisks(lines):
    out = []
    for ln in lines:
        # remove asterisks that are adjacent to spaces between words (likely stray separators)
        ln = re.sub(r'([A-Za-z0-9])\*(\s+)', r'\1\2', ln)
        ln = re.sub(r'(\s+)\*(?=[A-Za-z0-9])', r'\1', ln)
        out.append(ln)
    return out


def wrap_blockquotes(lines, width=80):
    import textwrap
    out = []
    i = 0
    while i < len(lines):
        ln = lines[i]
        if ln.lstrip().startswith('>'):
            # collect contiguous blockquote lines
            bq = []
            while i < len(lines) and lines[i].lstrip().startswith('>'):
                bq.append(lines[i].lstrip()[1:].lstrip())
                i += 1
            paragraph = ' '.join([b for b in bq if b.strip() != ''])
            wrapped = textwrap.fill(paragraph, width=width)
            for w in wrapped.split('\n'):
                out.append('> ' + w)
        else:
            out.append(ln)
            i += 1
    return out


def run_fix(path: Path):
    text = path.read_text(encoding='utf-8')
    lines = text.splitlines()
    path_backup = backup_path(path)
    path_backup.write_text(text, encoding='utf-8')
    # First pass
    lines = remove_trailing_spaces(lines)
    lines = collapse_blank_lines(lines)
    lines = ensure_blank_around_lists(lines)
    lines = normalize_table_pipes(lines)
    lines = convert_full_line_emphasis(lines)

    # Second pass: clean up any duplicates introduced and normalize blockquote blanks
    lines = remove_trailing_spaces(lines)
    lines = collapse_blank_lines(lines)
    # Ensure headings are surrounded by blank lines and limit heading jumps
    lines = ensure_blank_around_headings(lines)
    lines = limit_heading_jumps(lines)
    # Remove blockquote-only blank lines (these cause MD028). If a line is just '>' or '>  ' remove it.
    out = []
    for ln in lines:
        if ln.strip().startswith('>') and ln.strip().strip('> ').strip() == '':
            # skip blockquote-only empty lines
            continue
        out.append(ln)

    # Convert plain blank lines that appear between blockquote lines into a blockquote-empty
    out = fix_blank_between_blockquotes(out)

    # Normalize ordered list prefixes
    out = normalize_ordered_list_prefixes(out)

    # Strip trailing punctuation from headings (remove final ':' and similar)
    out = strip_trailing_punctuation_in_headings(out)

    # Fix spaces inside emphasis markers
    out = fix_space_in_emphasis(out)

    # Wrap plain paragraphs to the target line length
    out = wrap_paragraphs(out, width=80)
    # Final cleanup: ensure headings have blank lines and disambiguate duplicate headings
    out = ensure_blank_around_headings(out)
    out = collapse_blank_lines(out)

    # Disambiguate duplicate headings by appending (suite N)
    def disambiguate_duplicate_headings(lines):
        counts = {}
        res = []
        for l in lines:
            m = re.match(r'^(#{1,6})\s+(.*)$', l)
            if m:
                prefix = m.group(1)
                title = m.group(2).strip()
                # remove any existing "(suite N)" suffix for counting
                title_key = re.sub(r"\s*\(suite\s+\d+\)\s*$", '', title, flags=re.I).lower()
                cnt = counts.get(title_key, 0) + 1
                counts[title_key] = cnt
                if cnt > 1:
                    l = f"{prefix} {title} (suite {cnt})"
            res.append(l)
        return res
    out = disambiguate_duplicate_headings(out)

    # First, wrap blockquote blocks explicitly (use slightly smaller width to stay under 80)
    out = wrap_blockquotes(out, width=78)

    # Aggressively wrap any remaining long lines (preserving lists/quotes)
    out = aggressive_wrap_any(out, width=78)

    # Shorten long headings and remove stray asterisks
    out = shorten_long_headings(out, width=80)
    out = remove_stray_asterisks(out)

    # Ensure lists and headings have surrounding blank lines (final normalization)
    out = ensure_blank_around_lists(out)
    out = ensure_blank_around_headings(out)
    out = collapse_blank_lines(out)

    # Replace plain blank lines that are inside blockquote sequences with a blockquote-empty marker
    out = fix_blank_between_blockquotes(out)

    new_text = '\n'.join(out) + '\n'
    path.write_text(new_text, encoding='utf-8')
    print(f"File updated: {path}\nBackup saved: {path_backup}")


def main():
    if len(sys.argv) < 2:
        print('Usage: fix_md_misc.py file.md')
        sys.exit(2)
    p = Path(sys.argv[1])
    if not p.exists():
        print('File not found:', p)
        sys.exit(1)
    run_fix(p)


if __name__ == '__main__':
    main()
