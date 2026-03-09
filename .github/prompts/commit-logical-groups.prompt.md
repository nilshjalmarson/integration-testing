---
description: "Commit current workspace changes in logical groups"
name: "Commit Logical Groups"
argument-hint: "Commit intent and optional commit style, e.g. 'use conventional commits'"
agent: "agent"
---
Create commits from the current uncommitted Git changes by grouping related edits together.

Requirements:
- Inspect all unstaged and staged changes first.
- Propose commit groups by concern (feature, tests, docs, refactor, chore).
- Stage only files and hunks that belong to each group.
- Use partial-file staging when a single file contains unrelated changes.
- Never include unrelated edits in the same commit.
- Keep commit messages concise and meaningful; use the user's requested style if provided.
- After each commit, show a short summary of included files and rationale.
- Leave the working tree clean unless the user explicitly asks to keep leftovers.

Quality checks:
- Run the most relevant tests or build command before finalizing commits when feasible.
- If validation fails, report failures clearly and avoid committing broken changes unless explicitly requested.

Output format:
1. Planned commit groups.
2. Executed commits with commit SHA and message.
3. Remaining changes (if any) and why.
