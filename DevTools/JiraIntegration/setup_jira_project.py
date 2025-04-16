#!/usr/bin/env python3
# -*- coding: utf-8 -*-

"""
BetterWYD Jira Project Setup

This script creates all required Jira items based on the BetterWYD Development Roadmap.
It automatically detects valid issue types in your Jira project.

Usage:
1. Ensure your .env file is set up with Jira credentials
2. Run this script to populate your Jira project with items
"""

import os
import sys
import json
from pathlib import Path

# Get the script directory
SCRIPT_DIR = Path(os.path.dirname(os.path.abspath(__file__)))
PARENT_DIR = SCRIPT_DIR.parent

# Add the JiraIntegration directory to the Python path
sys.path.append(str(SCRIPT_DIR))

# Import the JiraAPI class from the jira_integration module
try:
    from jira_integration import JiraAPI, configure_urllib3
    print("Successfully imported JiraAPI")
except Exception as e:
    print(f"Error importing JiraAPI: {e}")
    sys.exit(1)

def get_valid_issue_types(jira):
    """
    Get valid issue types for the Jira project
    
    Args:
        jira: JiraAPI instance
        
    Returns:
        Dictionary mapping issue type categories to IDs and properties
    """
    print("Fetching valid issue types for your project...")
    
    # Fetch issue creation metadata for this specific project
    url = f"{jira.jira_url}/rest/api/3/issue/createmeta?projectKeys={jira.project_key}&expand=projects.issuetypes"
    
    try:
        import requests
        response = requests.get(url, headers=jira.headers)
        
        if response.status_code != 200:
            print(f"Failed to get project metadata. Status code: {response.status_code}")
            print(f"Response: {response.text}")
            return None
        
        metadata = response.json()
        projects = metadata.get('projects', [])
        
        if not projects:
            print(f"No project found with key: {jira.project_key}")
            return None
        
        project = projects[0]
        issue_types = project.get('issuetypes', [])
        
        print(f"Found {len(issue_types)} issue types available for this project:")
        
        # Build a map of issue type categories to IDs and properties
        type_map = {}
        regular_types = []
        subtask_types = []
        
        # First separate regular types and subtask types
        for issue_type in issue_types:
            type_id = issue_type.get('id')
            name = issue_type.get('name', '')
            is_subtask = issue_type.get('subtask', False)
            
            print(f"- {name} (ID: {type_id}, {'Sub-task' if is_subtask else 'Standard task'})")
            
            if is_subtask:
                subtask_types.append(issue_type)
            else:
                regular_types.append(issue_type)
        
        # Find the most appropriate issue types among regular types
        for issue_type in regular_types:
            type_id = issue_type.get('id')
            name = issue_type.get('name', '').lower()
            
            # Map common issue types to their IDs
            if 'epic' in name:
                type_map['epic'] = {'id': type_id, 'name': issue_type.get('name'), 'subtask': False}
            elif 'story' in name:
                type_map['story'] = {'id': type_id, 'name': issue_type.get('name'), 'subtask': False}
            elif 'task' in name and 'sub' not in name:
                type_map['task'] = {'id': type_id, 'name': issue_type.get('name'), 'subtask': False}
        
        # If we have only subtask types for task, use them
        if 'task' not in type_map and subtask_types:
            for issue_type in subtask_types:
                name = issue_type.get('name', '').lower()
                if 'task' in name or 'sub' in name:
                    type_map['task'] = {
                        'id': issue_type.get('id'), 
                        'name': issue_type.get('name'), 
                        'subtask': True
                    }
                    break
        
        # Provide fallbacks
        # If epic not found, use any regular type
        if 'epic' not in type_map and regular_types:
            type_map['epic'] = {
                'id': regular_types[0]['id'], 
                'name': regular_types[0]['name'], 
                'subtask': False
            }
            print(f"Epic type not found. Using {regular_types[0]['name']} as fallback.")
        
        # If story not found, use task or any regular type
        if 'story' not in type_map:
            if 'task' in type_map and not type_map['task']['subtask']:
                type_map['story'] = type_map['task']
                print(f"Story type not found. Using {type_map['task']['name']} as fallback.")
            elif regular_types:
                type_map['story'] = {
                    'id': regular_types[0]['id'], 
                    'name': regular_types[0]['name'], 
                    'subtask': False
                }
                print(f"Story type not found. Using {regular_types[0]['name']} as fallback.")
        
        # If task not found, use any type (even subtask)
        if 'task' not in type_map:
            if regular_types:
                type_map['task'] = {
                    'id': regular_types[0]['id'], 
                    'name': regular_types[0]['name'], 
                    'subtask': False
                }
                print(f"Task type not found. Using {regular_types[0]['name']} as fallback.")
            elif subtask_types:
                type_map['task'] = {
                    'id': subtask_types[0]['id'], 
                    'name': subtask_types[0]['name'], 
                    'subtask': True
                }
                print(f"Task type not found. Using {subtask_types[0]['name']} (sub-task) as fallback.")
        
        print(f"Using issue types:")
        print(f"  Epic: {type_map.get('epic', {}).get('name')} (ID: {type_map.get('epic', {}).get('id')})")
        print(f"  Story: {type_map.get('story', {}).get('name')} (ID: {type_map.get('story', {}).get('id')})")
        print(f"  Task: {type_map.get('task', {}).get('name')} (ID: {type_map.get('task', {}).get('id')}, {'Sub-task' if type_map.get('task', {}).get('subtask') else 'Standard task'})")
        
        return type_map
        
    except Exception as e:
        print(f"Error fetching issue types: {str(e)}")
        import traceback
        traceback.print_exc()
        return None

def setup_jira_project():
    """
    Set up the Jira project with all required items based on the Development Roadmap
    """
    print("Starting Jira project setup...")
    
    # Configure urllib3 to suppress LibreSSL warnings
    configure_urllib3()
    
    # Check if .env file exists
    env_path = SCRIPT_DIR / '.env'
    if not os.path.exists(env_path):
        print(f"Error: .env file not found at {env_path}")
        print("Please create a .env file with your Jira credentials.")
        return
    else:
        print(f"Found .env file at {env_path}")
    
    # Create Jira API instance
    try:
        print("Initializing Jira API...")
        jira = JiraAPI()
        print("Jira API initialized successfully")
    except ValueError as e:
        print(f"Error initializing Jira API: {str(e)}")
        return
    except Exception as e:
        print(f"Unexpected error initializing Jira API: {str(e)}")
        return
    
    # Test connection
    print("Testing connection to Jira...")
    if not jira.test_connection():
        print("Failed to connect to Jira. Please check your credentials.")
        return
    
    print("Connected to Jira successfully.")
    
    # Get valid issue types for this project
    issue_types = get_valid_issue_types(jira)
    if not issue_types:
        print("Could not determine valid issue types. Aborting setup.")
        return
    
    # Use the retrieved issue types
    epic_type = issue_types.get('epic')
    story_type = issue_types.get('story')
    task_type = issue_types.get('task')
    
    # Create Phase Epics
    try:
        print("Creating Phase 1 Epic...")
        phase1_epic = jira.create_issue(
            summary="Phase 1: Foundation & Core Systems",
            description="This phase focuses on setting up the foundation and core systems of the BetterWYD game. Timeline: April 15 - May 15, 2025",
            issue_type=epic_type
        )
        
        print("Creating Phase 2 Epic...")
        phase2_epic = jira.create_issue(
            summary="Phase 2: Gameplay Implementation",
            description="This phase focuses on implementing the core gameplay elements of BetterWYD. Timeline: May 16 - June 15, 2025",
            issue_type=epic_type
        )
        
        print("Creating Phase 3 Epic...")
        phase3_epic = jira.create_issue(
            summary="Phase 3: Polishing & Testing",
            description="This phase focuses on polishing the game, implementing multiplayer features, and preparing for testing. Timeline: June 16 - July 15, 2025",
            issue_type=epic_type
        )
        
        # Store Epic keys for linking stories
        epic_keys = {
            "phase1": phase1_epic.get("key") if phase1_epic else None,
            "phase2": phase2_epic.get("key") if phase2_epic else None,
            "phase3": phase3_epic.get("key") if phase3_epic else None
        }
        
        print(f"Created Phase Epics: {epic_keys}")
        
        # Create Phase 1 Stories and Tasks
        print("Creating Phase 1 items...")
        create_phase1_items(jira, epic_keys["phase1"], story_type, task_type)
        
        # Create Phase 2 Stories and Tasks
        print("Creating Phase 2 items...")
        create_phase2_items(jira, epic_keys["phase2"], story_type, task_type)
        
        # Create Phase 3 Stories and Tasks
        print("Creating Phase 3 items...")
        create_phase3_items(jira, epic_keys["phase3"], story_type, task_type)
        
        print("Successfully created all Jira items based on the Development Roadmap.")
    except Exception as e:
        print(f"Error creating Jira items: {str(e)}")
        import traceback
        traceback.print_exc()

def create_phase1_items(jira, epic_key, story_type, task_type):
    """Create Jira items for Phase 1: Foundation & Core Systems"""
    if not epic_key:
        print("Warning: Epic key for Phase 1 is missing. Stories will not be linked to an Epic.")
    
    # Week 1-2: Project Setup & Architecture
    architecture_story = jira.create_issue(
        summary="Project Setup & Architecture",
        description="Complete the initial project setup and architecture design for BetterWYD. This includes setting up the core systems framework.",
        issue_type=story_type
    )
    
    if architecture_story:
        story_key = architecture_story.get('key')
        # Link the story to the epic
        if epic_key:
            # In Jira REST API v3, this would normally require a separate call to link the story to the epic
            # For simplicity, we're just printing this out
            print(f"Linked Story {story_key} to Epic {epic_key}")
        
        # Create tasks for the story - handle if task type is a sub-task
        is_subtask = task_type.get('subtask', False) if isinstance(task_type, dict) else False
        parent_key = story_key if is_subtask else None
        
        print(f"Creating tasks for '{architecture_story.get('fields', {}).get('summary')}' (Parent: {parent_key if parent_key else 'None'})")
        
        jira.create_issue(
            summary="Complete project architecture design",
            description="Design the overall architecture of the game, including core systems, modules, and their interactions.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Set up core systems framework",
            description="Implement the basic framework for core game systems, including state management, event system, and object pooling.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Implement character controller and basic movement",
            description="Create a character controller with basic movement functionality, including walking, running, and jumping.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Design and create database schema for character data",
            description="Design the database schema for storing character data and implement the data access layer.",
            issue_type=task_type,
            parent_key=parent_key
        )
    
    # Week 3-4: Core Game Systems
    core_systems_story = jira.create_issue(
        summary="Core Game Systems Implementation",
        description="Implement the core game systems, including inventory, combat, character progression, and UI framework.",
        issue_type=story_type
    )
    
    if core_systems_story:
        story_key = core_systems_story.get('key')
        # Link the story to the epic
        if epic_key:
            print(f"Linked Story {story_key} to Epic {epic_key}")
        
        # Create tasks for the story - handle if task type is a sub-task
        is_subtask = task_type.get('subtask', False) if isinstance(task_type, dict) else False
        parent_key = story_key if is_subtask else None
        
        print(f"Creating tasks for '{core_systems_story.get('fields', {}).get('summary')}' (Parent: {parent_key if parent_key else 'None'})")
        
        jira.create_issue(
            summary="Implement inventory system",
            description="Develop a flexible inventory system that supports different item types, stacking, and management operations.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Develop basic combat system framework",
            description="Create the framework for the combat system, including attack mechanics, damage calculation, and hit detection.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Create character progression system",
            description="Implement character progression mechanics, including leveling, attributes (Strength, Dexterity, Intelligence, Constitution), and experience points.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Set up basic UI framework and main menus",
            description="Design and implement the UI framework and create the main game menus, including character selection and options.",
            issue_type=task_type,
            parent_key=parent_key
        )

def create_phase2_items(jira, epic_key, story_type, task_type):
    """Create Jira items for Phase 2: Gameplay Implementation"""
    if not epic_key:
        print("Warning: Epic key for Phase 2 is missing. Stories will not be linked to an Epic.")
    
    # Week 5-6: Class System & Combat
    class_system_story = jira.create_issue(
        summary="Class System & Combat",
        description="Implement the character class system and enhance the combat mechanics with abilities and skills.",
        issue_type=story_type
    )
    
    if class_system_story:
        story_key = class_system_story.get('key')
        # Link the story to the epic
        if epic_key:
            print(f"Linked Story {story_key} to Epic {epic_key}")
        
        # Create tasks for the story - handle if task type is a sub-task
        is_subtask = task_type.get('subtask', False) if isinstance(task_type, dict) else False
        parent_key = story_key if is_subtask else None
        
        print(f"Creating tasks for '{class_system_story.get('fields', {}).get('summary')}' (Parent: {parent_key if parent_key else 'None'})")
        
        jira.create_issue(
            summary="Implement Transknight class",
            description="Create the Transknight class with melee combat specialization, including unique abilities and attributes.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Implement Hunter class",
            description="Create the Hunter class with ranged damage dealing capabilities, including unique abilities and attributes.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Implement Foema class",
            description="Create the Foema class with magical abilities and spells, including unique abilities and attributes.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Implement Beastmaster class",
            description="Create the Beastmaster class with creature summoning and control abilities, including unique abilities and attributes.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Develop class-specific abilities and skills",
            description="Implement the various abilities and skills for each character class, including visual effects and animations.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Enhance combat system with attacks, skills, and effects",
            description="Expand the combat system to include special attacks, skill usage, and visual/audio effects for combat actions.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Create skill tree and progression system",
            description="Implement a skill tree system allowing players to unlock and upgrade their class-specific abilities.",
            issue_type=task_type,
            parent_key=parent_key
        )
    
    # Week 7-8: World Building & Environment
    world_building_story = jira.create_issue(
        summary="World Building & Environment",
        description="Create the game world, including terrain, maps, environmental effects, and day/night cycle.",
        issue_type=story_type
    )
    
    if world_building_story:
        story_key = world_building_story.get('key')
        # Link the story to the epic
        if epic_key:
            print(f"Linked Story {story_key} to Epic {epic_key}")
        
        # Create tasks for the story - handle if task type is a sub-task
        is_subtask = task_type.get('subtask', False) if isinstance(task_type, dict) else False
        parent_key = story_key if is_subtask else None
        
        print(f"Creating tasks for '{world_building_story.get('fields', {}).get('summary')}' (Parent: {parent_key if parent_key else 'None'})")
        
        jira.create_issue(
            summary="Develop terrain generation system",
            description="Create a system for generating and rendering terrain with various biomes and features.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Create the first playable map based on Kersef continent",
            description="Design and implement the first playable map based on the Kersef continent from the original game.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Implement day/night cycle",
            description="Create a day/night cycle system with appropriate lighting and environmental changes.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Add environmental effects and ambiance",
            description="Implement environmental effects such as weather, particles, and ambient sounds to enhance immersion.",
            issue_type=task_type,
            parent_key=parent_key
        )

def create_phase3_items(jira, epic_key, story_type, task_type):
    """Create Jira items for Phase 3: Polishing & Testing"""
    if not epic_key:
        print("Warning: Epic key for Phase 3 is missing. Stories will not be linked to an Epic.")
    
    # Week 9-10: Multiplayer Framework & Social Features
    multiplayer_story = jira.create_issue(
        summary="Multiplayer Framework & Social Features",
        description="Implement the multiplayer functionality and social features for player interaction.",
        issue_type=story_type
    )
    
    if multiplayer_story:
        story_key = multiplayer_story.get('key')
        # Link the story to the epic
        if epic_key:
            print(f"Linked Story {story_key} to Epic {epic_key}")
        
        # Create tasks for the story - handle if task type is a sub-task
        is_subtask = task_type.get('subtask', False) if isinstance(task_type, dict) else False
        parent_key = story_key if is_subtask else None
        
        print(f"Creating tasks for '{multiplayer_story.get('fields', {}).get('summary')}' (Parent: {parent_key if parent_key else 'None'})")
        
        jira.create_issue(
            summary="Implement basic client-server architecture",
            description="Create the client-server architecture for multiplayer functionality, handling connections and data synchronization.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Add player-to-player interaction",
            description="Implement mechanics for players to interact with each other, including proximity detection and interaction options.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Develop chat system",
            description="Create a chat system with different channels (global, local, private) for player communication.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Create basic guild framework",
            description="Implement a basic guild system allowing players to form groups with shared identity and chat.",
            issue_type=task_type,
            parent_key=parent_key
        )
    
    # Week 11-12: Testing & Optimization
    testing_story = jira.create_issue(
        summary="Testing & Optimization",
        description="Focus on testing, optimization, and bug fixing to prepare for alpha release.",
        issue_type=story_type
    )
    
    if testing_story:
        story_key = testing_story.get('key')
        # Link the story to the epic
        if epic_key:
            print(f"Linked Story {story_key} to Epic {epic_key}")
        
        # Create tasks for the story - handle if task type is a sub-task
        is_subtask = task_type.get('subtask', False) if isinstance(task_type, dict) else False
        parent_key = story_key if is_subtask else None
        
        print(f"Creating tasks for '{testing_story.get('fields', {}).get('summary')}' (Parent: {parent_key if parent_key else 'None'})")
        
        jira.create_issue(
            summary="Performance optimization",
            description="Identify and address performance bottlenecks, optimize resource usage, and improve frame rates.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Bug fixing",
            description="Identify and fix bugs throughout the game, focusing on critical issues that affect gameplay.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Balance adjustments",
            description="Review and adjust game balance, including character classes, skills, progression, and combat mechanics.",
            issue_type=task_type,
            parent_key=parent_key
        )
        
        jira.create_issue(
            summary="Prepare for alpha testing",
            description="Set up the infrastructure and processes for alpha testing, including test plans and feedback collection.",
            issue_type=task_type,
            parent_key=parent_key
        )

if __name__ == "__main__":
    setup_jira_project()