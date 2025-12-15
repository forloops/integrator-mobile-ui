# Integrator Mobile Documentation

This folder contains all project documentation for the Integrator Mobile field service application.

## Documentation Structure

```
docs/
├── README.md                    # This file
├── requirements/               # Functional requirements & specifications
│   ├── functional-requirements.md
│   └── feature-matrix.md
├── design-system/              # UI components and patterns
│   ├── design-tokens.md
│   └── components.md
├── progress/                   # Development progress tracking
│   ├── maui-prototype-status.md
│   └── vue-prototype-comparison.md
└── architecture/               # Technical architecture
    └── maui-architecture.md
```

## Quick Links

- [MAUI Prototype Status](progress/maui-prototype-status.md) - Current implementation status
- [Vue-MAUI Feature Comparison](progress/vue-prototype-comparison.md) - Feature parity tracking
- [Functional Requirements](requirements/functional-requirements.md) - Full requirements specification
- [Design System](design-system/design-tokens.md) - Color tokens and typography
- [Components](design-system/components.md) - Reusable UI components

## Project Overview

The Integrator Mobile project consists of two parallel prototype implementations:

1. **Vue/Vite Prototype** - Web-based clickable prototype for rapid UI iteration
2. **MAUI Prototype** - Native mobile implementation for iOS/Android/macOS

Both prototypes share the same design system and functional requirements, allowing the team to validate UX patterns before production implementation in the ServiceTech application.

## Related Resources

- [Main README](../README.md) - Project setup and build instructions
- [LLM Agents](../llm-agents/README.md) - AI agent configurations for development assistance
