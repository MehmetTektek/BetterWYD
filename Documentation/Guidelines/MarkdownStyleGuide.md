# Markdown Style Guide

## Overview

This document outlines the Markdown formatting guidelines for the BetterWYD project documentation.
Consistent formatting ensures readability and maintainability of our documentation.

## Heading Structure

- Use ATX-style headings with hash symbols (#)
- Always include a space after the hash symbols
- Use proper heading hierarchy (don't skip levels)
- Leave one blank line before each heading

```markdown
# Level 1 Heading

## Level 2 Heading

### Level 3 Heading
```

## Line Length

- Keep lines under 100 characters
- Break long paragraphs at natural points
- For lists of links or code, one item per line is acceptable

## Lists

- Use dash (-) for unordered lists
- Use numbers (1. 2. 3.) for ordered lists
- No blank lines between list items
- Use 2 spaces for nested list indentation

```markdown
- First item
- Second item
  - Nested item
  - Another nested item
- Third item
```

## Code Formatting

- Use backticks (`) for inline code
- Use triple backticks (```) for code blocks
- Always specify the language for code blocks

```markdown
Use `GameObject.Find()` sparingly.

```csharp
public class Example : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Hello World");
    }
}
```
```

## Links

- Use [text](URL) format for links
- Use descriptive link text
- For repeated links, use reference-style links

```markdown
[BetterWYD Documentation](./Documentation)

[Unity Documentation][unity-docs]

[unity-docs]: https://docs.unity3d.com/
```

## Images

- Use the format: ![Alt text](path/to/image)
- Include meaningful alt text
- Store images in a consistent location

## Tables

- Use tables for structured data
- Align table dividers for readability
- Minimize table width when possible

```markdown
| Name      | Type        | Description           |
|-----------|-------------|-----------------------|
| speed     | float       | Movement speed        |
| health    | int         | Character health      |
| inventory | GameObject  | Inventory reference   |
```

## Emphasis

- Use *asterisks* for italic text
- Use **double asterisks** for bold text
- Use ***triple asterisks*** for bold italic text
- Use sparingly for emphasis, not for decoration

## Horizontal Rules

- Use three hyphens (---) for horizontal rules
- Leave blank lines before and after

## File Organization

- Use descriptive filenames in kebab-case (my-file-name.md)
- Group related documents in subdirectories
- Include a README.md in each directory
- Link between related documents

## Common Issues to Avoid

- Trailing whitespace
- Mixed indentation
- Missing language indicators in code blocks
- Skipped heading levels
- Excessive use of HTML in Markdown