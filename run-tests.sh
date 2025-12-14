#!/bin/bash
# Run Integrator Mobile unit tests

set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$SCRIPT_DIR"

echo "========================================="
echo "  Integrator Mobile - Unit Tests"
echo "========================================="
echo ""

# Restore dependencies
echo "Restoring dependencies..."
dotnet restore tests/IntegratorMobile.Tests/IntegratorMobile.Tests.csproj

# Run tests
echo ""
echo "Running tests..."
dotnet test tests/IntegratorMobile.Tests/IntegratorMobile.Tests.csproj \
    --verbosity normal \
    --logger "console;verbosity=detailed" \
    "$@"

echo ""
echo "========================================="
echo "  Tests completed!"
echo "========================================="
