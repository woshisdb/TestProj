#!/bin/bash -i

# Environment variables
export PLANUTILS_PREFIX="~/.planutils"
export PATH="$PATH:$PLANUTILS_PREFIX/bin"

# Add a blue coloured shell prompt prefix (planutils)
export PS1="(\[\e[1;34m\]planutils\[\e[0m\]) $PS1"

echo
echo "   Entering planutils environment..."
echo

# Enter the shell
#$SHELL --init-file <(echo "export PS1=\"$PS1\"")

lpg-td p10-domain.pddl p10-s45-n3-l5-f30.pddl