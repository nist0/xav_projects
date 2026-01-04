# Mathematiques (LaTeX) — regles

Ce fichier est une extraction de `.github/copilot-instructions.md`.

==================================================
LATEX / MATH RULES
==================================================

12. Display math
    - Use ONLY $$ ... $$ for block equations
    - NEVER use \[ \], \boxed{}, align, cases, or environments
    - Display equations MUST be surrounded by blank lines (MD022)
        - NEVER put a blank line inside a $$ ... $$ block (including whitespace-only lines)
            - Keep lines consecutive if you break an equation across multiple lines
            - Prefer a single-line expression for fractions and nested denominators
            - Ensure braces { } are balanced inside the block

13. Inline math
    - Allowed ONLY in normal text using $...$
    - Keep inline math minimal

14. Math inside tables
    - NEVER put LaTeX ($...$ or $$...$$) inside tables
    - Use plain text only:
      Examples: "ω << ωc", "ω = ω0", "1/√2"
