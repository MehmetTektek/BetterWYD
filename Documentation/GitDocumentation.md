# Git Documentation for BetterWYD Project

This documentation provides guidelines and best practices for using Git version control with the BetterWYD Unity project.

## Table of Contents

1. [Introduction to Git](#introduction-to-git)
2. [Setup and Configuration](#setup-and-configuration)
3. [Basic Git Commands](#basic-git-commands)
4. [Git Workflow for BetterWYD](#git-workflow-for-betterwyd)
5. [Best Practices for Unity Projects](#best-practices-for-unity-projects)
6. [Troubleshooting Common Issues](#troubleshooting-common-issues)

## Introduction to Git

Git is a distributed version control system that allows multiple developers to work on the same project simultaneously. It tracks changes to files, enables reverting to previous states, and maintains a history of the project's development.

### Why Git for Unity Projects?

- **Version Control**: Track changes and revert to previous states if necessary
- **Collaboration**: Multiple team members can work on the project simultaneously
- **Backup**: The repository serves as a backup of your project
- **Branching**: Create separate branches for features, bug fixes, or experiments

## Setup and Configuration

### Installing Git

1. Download Git from [git-scm.com](https://git-scm.com/downloads)
2. Install with default settings
3. Verify installation by opening a terminal/command prompt and typing:
   ```
   git --version
   ```

### Initial Configuration

Set your identity for Git:

```bash
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

### Repository Setup

The BetterWYD project is hosted on GitHub at `https://github.com/MehmetTektek/BetterWYD.git`.

To clone the repository:

```bash
git clone https://github.com/MehmetTektek/BetterWYD.git
```

## Basic Git Commands

### Checking Status

To see the status of your working directory:

```bash
git status
```

### Staging Changes

To stage changes for commit:

```bash
git add <file>       # Stage a specific file
git add .            # Stage all changes
```

### Committing Changes

To commit staged changes:

```bash
git commit -m "Your descriptive commit message"
```

### Pushing Changes

To push commits to the remote repository:

```bash
git push origin main
```

### Pulling Changes

To pull changes from the remote repository:

```bash
git pull origin main
```

### Branching

```bash
git branch                  # List branches
git branch <branch-name>    # Create a new branch
git checkout <branch-name>  # Switch to a branch
git checkout -b <branch-name>  # Create and switch to a new branch
```

### Merging

```bash
git checkout main           # Switch to the target branch
git merge <source-branch>   # Merge the source branch into the current branch
```

## Git Workflow for BetterWYD

For the BetterWYD project, we recommend the following workflow:

1. **Pull the latest changes** before starting work:
   ```bash
   git pull origin main
   ```

2. **Create a feature branch** for your work:
   ```bash
   git checkout -b feature/your-feature-name
   ```

3. **Make changes** to the project

4. **Commit changes** with descriptive messages:
   ```bash
   git add .
   git commit -m "Add player movement system"
   ```

5. **Push your branch** to the remote repository:
   ```bash
   git push origin feature/your-feature-name
   ```

6. **Create a Pull Request** on GitHub for review

7. After review, **merge** the Pull Request into the main branch

## Best Practices for Unity Projects

### .gitignore

The project includes a .gitignore file configured for Unity projects. This prevents unnecessary files (e.g., Library, Temp, logs) from being tracked.

### Large Files

Avoid committing large binary files directly to Git. Consider using Git LFS (Large File Storage) for:
- Textures
- Models
- Audio files
- Video files

To set up Git LFS:
```bash
git lfs install
git lfs track "*.psd"  # Example for Photoshop files
git lfs track "*.fbx"  # Example for 3D models
```

### Scene Files

Be cautious when merging scene files as conflicts can be difficult to resolve. Consider:
- Dividing scenes into smaller, more manageable scenes
- Using prefabs for complex objects

### Commit Frequency

- Commit often with smaller, focused changes
- Ensure the project is in a working state before committing
- Use descriptive commit messages

## Troubleshooting Common Issues

### Resolving Merge Conflicts

If you encounter merge conflicts:

1. Use `git status` to identify conflicting files
2. Open the files and look for conflict markers (`<<<<<<<`, `=======`, `>>>>>>>`)
3. Resolve the conflicts manually
4. Stage the resolved files with `git add <file>`
5. Complete the merge with `git commit`

For Unity-specific merge conflicts, consider using Unity's Smart Merge tool.

### Handling Large Repositories

If the repository becomes too large:
- Use Git LFS for large binary files
- Consider cleaning up the repository with tools like BFG Repo-Cleaner
- Archive old branches that are no longer needed

### Common Git Errors

- **"fatal: refusing to merge unrelated histories"**
  ```bash
  git pull origin main --allow-unrelated-histories
  ```

- **"You have unstaged changes"**
  ```bash
  git stash
  # Perform your operation
  git stash pop  # To restore your changes
  ```

- **"Failed to push some refs"**
  ```bash
  git pull origin main
  git push origin main
  ```

## Additional Resources

- [Git Documentation](https://git-scm.com/doc)
- [GitHub Learning Lab](https://lab.github.com/)
- [Unity Version Control Best Practices](https://unity.com/how-to/unity-version-control-systems-best-practices)
- [Pro Git Book](https://git-scm.com/book/en/v2)