# LLM Agents - Integrator Mobile

This folder contains AI agent configurations for development assistance on the Integrator Mobile project.

## Available Agents

| Agent | Role | Specialization |
|-------|------|----------------|
| [BA Agent](ba-agent/) | Business Analyst | Requirements, user stories, acceptance criteria |
| [UX Agent](ux-agent/) | UX Designer | UI/UX patterns, design system, accessibility |
| [MAUI Agent](maui-agent/) | MAUI Specialist | .NET MAUI development, cross-platform mobile |

## Agent Structure

Each agent folder contains:

```
agent-name/
├── AGENT.md          # Agent persona and capabilities
├── context/          # Project context files
│   └── project.md    # Current project understanding
├── memory/           # Persistent memory
│   └── decisions.md  # Key decisions and rationale
└── prompts/          # Reusable prompt templates
    └── templates.md  # Common prompts
```

## Usage

### Activating an Agent

When working with an AI assistant, reference the agent configuration:

```
@llm-agents/maui-agent/AGENT.md

Help me implement the new photo gallery feature following the established patterns.
```

### Providing Context

Include relevant context files for better assistance:

```
@llm-agents/maui-agent/context/project.md
@docs/requirements/functional-requirements.md

What's the best approach for implementing offline photo sync?
```

### Updating Memory

After significant decisions or implementations, update the agent's memory:

```
Update @llm-agents/maui-agent/memory/decisions.md with:
- Decision: Use SQLite for offline storage
- Rationale: Better performance than LiteDB, wider community support
- Date: 2024-12-15
```

## Agent Collaboration

Agents are designed to work together:

1. **BA Agent** defines requirements and acceptance criteria
2. **UX Agent** designs the user experience and interface
3. **MAUI Agent** implements the technical solution

### Example Workflow

```
Step 1: BA Agent
"Define user stories for the photo capture feature"

Step 2: UX Agent
"Design the photo capture UI following our design system"

Step 3: MAUI Agent
"Implement the photo capture page using MAUI Essentials"
```

## Creating New Agents

To create a new specialized agent:

1. Create agent folder: `mkdir llm-agents/new-agent`
2. Copy template: `cp llm-agents/_template/* llm-agents/new-agent/`
3. Customize `AGENT.md` with persona and capabilities
4. Add relevant context to `context/` folder
5. Initialize empty memory in `memory/` folder

## Key Documentation References

| Document | Path | Purpose |
|----------|------|---------|
| Requirements | `@docs/requirements/functional-requirements.md` | Feature specifications |
| Feature Matrix | `@docs/requirements/feature-matrix.md` | Vue-MAUI parity tracking |
| Design System | `@docs/design-system/design-tokens.md` | Colors, typography, spacing |
| Architecture | `@docs/architecture/maui-architecture.md` | MAUI app structure |
| Testing Strategy | `@docs/testing/testing-strategy.md` | Test coverage roadmap |
| MAUI Status | `@docs/progress/maui-prototype-status.md` | Implementation status |

## Testing Context

Current test coverage: **~55% (100+ tests)**

| Test Category | Tests | Agent Responsible |
|---------------|-------|-------------------|
| Model Tests | 25 | MAUI Agent |
| Service Tests | 18 | MAUI Agent |
| Logic Tests | 45 | MAUI Agent |
| Integration | 8 | MAUI Agent |
| Feature Parity | 7 | BA Agent (verification) |

### Testing Resources
- Strategy document: `@docs/testing/testing-strategy.md`
- BA verification: `@llm-agents/ba-agent/memory/verification-2024-12-15.md`
- Technical decisions: `@llm-agents/maui-agent/memory/decisions.md`

## Best Practices

1. **Keep Context Current** - Update context files as the project evolves
2. **Document Decisions** - Record important decisions in memory files
3. **Use Appropriate Agent** - Match the task to the agent's specialization
4. **Provide Examples** - Include code examples when requesting implementations
5. **Reference Patterns** - Point to existing code for consistency
6. **Include Test Coverage** - Consider testability when designing features
7. **Update Memory** - Log key decisions for future agent sessions
