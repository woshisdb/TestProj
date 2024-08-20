#!/bin/bash -i
# Environment variables
export PLANUTILS_PREFIX="~/.planutils"
export PATH="$PATH:$PLANUTILS_PREFIX/bin"
export PS1="(\[\e[1;34m\]planutils\[\e[0m\]) $PS1"
echo "Entering planutils environment..."

lpg-td $1 $2