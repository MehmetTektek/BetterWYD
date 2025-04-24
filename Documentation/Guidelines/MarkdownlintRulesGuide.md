# Markdownlint Rules Guide

**Version:** 1.0  
**Date:** 24 Nisan 2025  
**Author:** Mehmet Tektek  
**Last Updated By:** GitHub Copilot

## Overview

This document serves as a comprehensive reference guide for all markdownlint rules used in the BetterWYD project. Following these rules ensures consistent, readable, and maintainable markdown documentation across the project.

## Table of Contents

1. [Introduction](#introduction)
2. [Rule Categories](#rule-categories)
3. [Rules Reference](#rules-reference)
4. [Common Issues and Fixes](#common-issues-and-fixes)
5. [Configuration](#configuration)
6. [Integration with VS Code](#integration-with-vs-code)
7. [References](#references)

## Introduction

Markdownlint is a static analysis tool that helps identify and fix common markdown style and formatting issues. The BetterWYD project uses markdownlint to maintain consistency across all documentation files. This guide documents each rule, explains why it's important, and provides examples of correct and incorrect usage.

## Rule Categories

Markdownlint rules cover several categories:

- **Document Structure** - Rules related to overall document organization
- **Headings** - Rules for heading formatting and hierarchy
- **Lists** - Rules for list formatting and consistency
- **Whitespace** - Rules for whitespace usage
- **Line Length** - Rules for line length and wrapping
- **Emphasis** - Rules for formatting bold, italic, and other emphasis formats
- **Links & Images** - Rules for formatting links and images
- **Code & HTML** - Rules for code blocks and HTML usage
- **Special Characters** - Rules for special character handling

## Rules Reference

### Document Structure

#### MD001 - Heading levels should only increment by one level at a time

Headings should not skip levels (e.g., H1 to H3).

✅ **Correct:**
```md
# Heading 1
## Heading 2
### Heading 3
```

❌ **Incorrect:**
```md
# Heading 1
### Heading 3 (skipped level 2)
```

**Why it matters:** Proper heading hierarchy ensures good document structure and accessibility.

[MD001 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md001)

#### MD002 - First heading should be a top-level heading

The first heading in a document should be a level 1 heading.

✅ **Correct:**
```md
# Document Title
## Section Heading
```

❌ **Incorrect:**
```md
## Section Heading (missing top-level heading)
```

**Why it matters:** A top-level heading provides a clear title for the document.

[MD002 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md002)

#### MD003 - Heading style

Headings should use consistent style (ATX-style for BetterWYD).

✅ **Correct:**
```md
# Heading 1
## Heading 2
```

❌ **Incorrect:**
```md
Heading 1
=========
```

**Why it matters:** Consistent heading styles improve readability and maintainability.

[MD003 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md003)

#### MD041 - First line in file should be a top-level heading

The first non-whitespace line in a file should be a level 1 heading.

✅ **Correct:**
```md
# Document Title
Introduction text...
```

❌ **Incorrect:**
```md
Introduction text...
# Document Title
```

**Why it matters:** Starting with a top-level heading provides immediate context.

[MD041 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md041)

### Headings

#### MD018 - No space after hash on atx style heading

Headings should have a space after the hash characters.

✅ **Correct:**
```md
# Heading 1
```

❌ **Incorrect:**
```md
#Heading 1
```

**Why it matters:** Consistent spacing improves readability.

[MD018 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md018)

#### MD019 - Multiple spaces after hash on atx style heading

Only one space should follow the hash characters.

✅ **Correct:**
```md
# Heading 1
```

❌ **Incorrect:**
```md
#  Heading 1
```

**Why it matters:** Consistent spacing improves readability and avoids inconsistencies.

[MD019 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md019)

#### MD023 - Headings must start at the beginning of the line

Headings should not be indented.

✅ **Correct:**
```md
# Heading 1
```

❌ **Incorrect:**
```md
  # Heading 1
```

**Why it matters:** Indented headings may not be properly recognized by Markdown parsers.

[MD023 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md023)

#### MD024 - Multiple headings with the same content

Headings with the same content should be unique to support linking.

✅ **Correct:**
```md
## Installation
...
## Configuration
```

❌ **Incorrect:**
```md
## Setup
...
## Setup
```

**Why it matters:** Duplicate headings create ambiguous anchor links.

[MD024 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md024)

### Lists

#### MD004 - Unordered list style

Unordered lists should use consistent markers (dash `-` for BetterWYD).

✅ **Correct:**
```md
- Item 1
- Item 2
- Item 3
```

❌ **Incorrect:**
```md
* Item 1
+ Item 2
- Item 3
```

**Why it matters:** Consistent list markers improve readability.

[MD004 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md004)

#### MD005 - Inconsistent indentation for list items at the same level

List items at the same level should be indented consistently.

✅ **Correct:**
```md
- Item 1
- Item 2
  - Nested item
  - Nested item
```

❌ **Incorrect:**
```md
- Item 1
- Item 2
    - Nested item
  - Nested item
```

**Why it matters:** Consistent indentation clearly shows list hierarchy.

[MD005 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md005)

#### MD007 - Unordered list indentation

Nested lists should use consistent indentation (2 spaces for BetterWYD).

✅ **Correct:**
```md
- Item 1
  - Nested item
    - Deeply nested item
```

❌ **Incorrect:**
```md
- Item 1
    - Nested item
        - Deeply nested item
```

**Why it matters:** Consistent indentation makes lists more readable.

[MD007 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md007)

#### MD029 - Ordered list item prefix

Ordered lists should use consistent numbering styles (incrementing for BetterWYD).

✅ **Correct:**
```md
1. First item
2. Second item
3. Third item
```

❌ **Incorrect:**
```md
1. First item
1. Second item
1. Third item
```

**Why it matters:** Sequential numbering improves readability and maintainability.

[MD029 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md029)

### Whitespace

#### MD009 - Trailing spaces

Avoid trailing whitespace at the end of lines.

✅ **Correct:**
```md
This line has no trailing spaces.
```

❌ **Incorrect:**
```md
This line has trailing spaces.   
```

**Why it matters:** Trailing spaces create invisible differences and unnecessary changes in version control.

[MD009 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md009)

#### MD010 - Hard tabs

Use spaces for indentation instead of tabs.

✅ **Correct:**
```md
- Item 1
  - Nested item (indented with spaces)
```

❌ **Incorrect:**
```md
- Item 1
	- Nested item (indented with tab)
```

**Why it matters:** Tabs can render differently across editors, leading to inconsistent formatting.

[MD010 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md010)

#### MD012 - Multiple consecutive blank lines

Avoid multiple consecutive blank lines.

✅ **Correct:**
```md
Paragraph 1.

Paragraph 2.
```

❌ **Incorrect:**
```md
Paragraph 1.


Paragraph 2.
```

**Why it matters:** Excessive whitespace makes documents harder to read and maintain.

[MD012 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md012)

### Line Length

#### MD013 - Line length

Lines should not exceed 100 characters for BetterWYD (configurable).

✅ **Correct:**
```md
This is a line that is fewer than 100 characters long.
```

❌ **Incorrect:**
```md
This is a very long line that exceeds 100 characters and makes the document harder to read in editors that don't wrap text automatically and requires horizontal scrolling.
```

**Why it matters:** Limited line length improves readability, especially for side-by-side diffs.

[MD013 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md013)

### Emphasis

#### MD036 - Emphasis used instead of a heading

Don't use bold or italic text to create headings.

✅ **Correct:**
```md
## Section Title

Regular paragraph with **bold** text.
```

❌ **Incorrect:**
```md
**Section Title**

Regular paragraph.
```

**Why it matters:** Proper headings provide structure and are recognized by navigation tools.

[MD036 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md036)

#### MD049 - Emphasis style should be consistent

Use consistent style for emphasis (**asterisks** for BetterWYD).

✅ **Correct:**
```md
This text has **bold** and *italic* formatting.
```

❌ **Incorrect:**
```md
This text has __bold__ and _italic_ formatting.
```

**Why it matters:** Consistent emphasis styles improve readability and maintainability.

[MD049 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md049)

### Links & Images

#### MD034 - Bare URL used

URLs should be enclosed in angle brackets or formatted as links.

✅ **Correct:**
```md
See [our website](https://example.com) or <https://example.com>
```

❌ **Incorrect:**
```md
See our website at https://example.com
```

**Why it matters:** Properly formatted links are more reliable across different markdown parsers.

[MD034 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md034)

#### MD040 - Fenced code blocks should have a language specified

Code blocks should specify the language for syntax highlighting.

✅ **Correct:**
```md
```csharp
public class Example {
    void Start() {
        Debug.Log("Hello World");
    }
}
```
```

❌ **Incorrect:**
```md
```
public class Example {
    void Start() {
        Debug.Log("Hello World");
    }
}
```
```

**Why it matters:** Language specification enables proper syntax highlighting.

[MD040 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md040)

### Code & HTML

#### MD031 - Fenced code blocks should be surrounded by blank lines

Code blocks should have blank lines before and after them.

✅ **Correct:**
```md
Text before code.

```csharp
Console.WriteLine("Hello World");
```

Text after code.
```

❌ **Incorrect:**
```md
Text before code.
```csharp
Console.WriteLine("Hello World");
```
Text after code.
```

**Why it matters:** Proper spacing improves readability and ensures code blocks render correctly.

[MD031 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md031)

#### MD033 - Inline HTML

Avoid inline HTML in markdown documents when possible.

✅ **Correct:**
```md
Use **bold** and *italic* for emphasis.
```

❌ **Incorrect:**
```md
Use <strong>bold</strong> and <em>italic</em> for emphasis.
```

**Why it matters:** Pure markdown is more portable and consistent across renderers.

[MD033 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md033)

### Special Characters

#### MD037 - Spaces inside emphasis markers

Don't include spaces inside emphasis markers.

✅ **Correct:**
```md
This is **bold** text.
```

❌ **Incorrect:**
```md
This is ** bold ** text.
```

**Why it matters:** Spaces inside markers prevent proper rendering of emphasis.

[MD037 Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md#md037)

## Common Issues and Fixes

Below are common markdownlint issues and how to fix them:

| Issue | Description | Fix |
|-------|-------------|-----|
| Line length | Lines exceed 100 characters | Break long paragraphs at natural points |
| Inconsistent lists | Mixed list markers or indentation | Standardize on dash (-) and 2-space indentation |
| Heading hierarchy | Skipped heading levels | Ensure heading levels increase by only one |
| Trailing whitespace | Extra spaces at line ends | Remove trailing spaces |
| Missing code language | No language specified for code blocks | Add language identifier after opening fences |

## Configuration

The BetterWYD project uses a custom markdownlint configuration defined in `.markdownlint.json` at the project root. This configuration enforces our project standards while allowing specific exceptions when necessary.

Key configuration settings:

- **Line length**: 100 characters
- **List style**: Dash (-) for unordered lists
- **Heading style**: ATX-style headings (#)
- **Indentation**: 2 spaces for nested lists

## Integration with VS Code

For Visual Studio Code users, install the "markdownlint" extension by David Anson to get real-time linting:

1. Open VS Code
2. Go to Extensions (Ctrl+Shift+X / Cmd+Shift+X)
3. Search for "markdownlint"
4. Install the extension by David Anson

The extension will automatically use the project's `.markdownlint.json` configuration.

## References

- [Official markdownlint Documentation](https://github.com/DavidAnson/markdownlint/blob/main/doc/Rules.md)
- [BetterWYD Markdown Style Guide](/Documentation/Guidelines/MarkdownStyleGuide.md)
- [BetterWYD Documentation Standards](/Documentation/Guidelines/DocumentationStandards.md)
- [VS Code Markdownlint Extension](https://marketplace.visualstudio.com/items?itemName=DavidAnson.vscode-markdownlint)
- [CommonMark Specification](https://spec.commonmark.org/)