#!/bin/bash
# Build and run the Integrator Mobile prototype on macOS

set -e

echo "Building for macOS Catalyst..."
dotnet build -f net10.0-maccatalyst src/IntegratorMobile.Prototype/IntegratorMobile.Prototype.csproj

echo "Launching app..."
open "src/IntegratorMobile.Prototype/bin/Debug/net10.0-maccatalyst/maccatalyst-arm64/Integrator Mobile.app"
