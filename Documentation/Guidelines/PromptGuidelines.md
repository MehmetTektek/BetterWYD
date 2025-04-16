# BetterWYD Prompt Guidelines

This document provides guidelines for creating effective prompts when working with AI assistants for the BetterWYD project.

## Prompt Structure

For best results, structure your prompts using these sections:

### 1. Context

Begin by providing relevant context:
- What are you working on?
- Which part of the BetterWYD project is involved?
- What's the current state?

Example:
```
I'm working on the Character Controller for the BetterWYD game. I've implemented basic movement but need help with adding jump mechanics.
```

### 2. Goal

Clearly state what you want to achieve:
- What is the end result you're looking for?
- Are there specific requirements or constraints?

Example:
```
I need to implement a variable-height jump system where holding the button longer results in a higher jump, up to a maximum height.
```

### 3. Details

Include any specific details that might be helpful:
- Technical specifications
- Design requirements
- References to existing code
- Links to documentation

Example:
```
The jump should have:
- Initial impulse force of 8f
- Maximum additional force of 4f when holding the button
- Gravity multiplier of 2.5f when falling
- Current player speed should influence jump distance
```

### 4. Request Type

Specify what kind of help you need:
- Code implementation
- Debugging help
- Design suggestions
- Documentation creation
- Best practices advice

Example:
```
I need help implementing this jump mechanic in my existing PlayerController.cs script.
```

## Best Practices

1. **Be specific**: The more specific your prompt, the better the response will be.

2. **Provide context**: Include information about your project structure, the tools you're using, and the problem you're trying to solve.

3. **Use examples**: When appropriate, include examples of what you're looking for.

4. **Break down complex tasks**: For complex problems, break them down into smaller, more manageable parts.

5. **Reference existing work**: If you're extending existing functionality, provide context about the current implementation.

6. **Specify format**: If you need the response in a specific format (code snippet, design document, etc.), make that clear.

7. **Indicate priority**: If you have multiple questions, indicate which ones are most important.

## Example Full Prompt

```
Context:
I'm working on the inventory system for BetterWYD. The basic data structure is in place, but I need to implement item stacking functionality.

Goal:
Create a robust item stacking system that allows items of the same type to stack up to a configurable maximum stack size, which can vary by item type.

Details:
- Each item has an ItemData scriptable object with properties like ID, Name, Description, and MaxStackSize
- The Inventory is a List<InventorySlot> where InventorySlot contains an ItemData and a quantity
- Some items should never stack (MaxStackSize = 1)
- When adding items, we should first try to fill existing stacks before using new slots

Request Type:
I need help implementing the AddItem method in my InventoryManager.cs that handles all these stacking rules properly.
```

## Templates for Common Requests

### Bug Fix Request

```
Context: [Describe where and when the bug occurs]
Issue: [Describe the bug and its symptoms]
Expected Behavior: [What should happen instead]
Code/Component: [Relevant code or component information]
Request: Help me identify and fix this bug
```

### Feature Implementation

```
Context: [Current project state]
Feature Needed: [Description of the feature]
Requirements: [Specific requirements or constraints]
Existing Components: [Related existing code/systems]
Request: Help me implement this feature
```

### Code Review

```
Context: [Project and code purpose]
Code to Review: [The code or a description of where to find it]
Concerns: [Any specific areas of concern]
Request: Please review this code for [performance/security/readability/etc.]
```

## Using These Guidelines

Copy and adapt the appropriate sections when creating prompts for AI assistants. Providing structured, detailed information will result in more accurate and helpful responses.

Remember that these are guidelines, not strict rules. Adapt them as needed based on your specific situation and needs.